"""
McKinsey / Hedge-Fund Grade Forex Macro Dashboard
================================================
CLOUD-READY VERSION

Features:
- No local dependencies required
- Uses cloud-compatible data sources
- Works on Streamlit Cloud, Railway, Render
- Multi-timeframe analysis (Daily, 4H, 1H)
- FRED macro data with fallbacks
- Trading ideas engine

Deploy on Streamlit Cloud:
1. Push to GitHub
2. Connect at share.streamlit.io
3. Add secrets for FRED API key
"""

import streamlit as st
import plotly.graph_objects as go
import plotly.express as px
import pandas as pd
import numpy as np
import yfinance as yf
from datetime import datetime, timedelta
import ta
from tenacity import retry, stop_after_attempt, wait_fixed
import warnings
warnings.filterwarnings('ignore')

# ─────────────────────────────────────────────────────────────
# PAGE CONFIG
# ─────────────────────────────────────────────────────────────

st.set_page_config(
    page_title="Forex Macro Dashboard Pro",
    page_icon="💹",
    layout="wide",
    initial_sidebar_state="expanded"
)

# ─────────────────────────────────────────────────────────────
# CONSTANTS
# ─────────────────────────────────────────────────────────────

ASSETS = {
    "EUR/USD": "EURUSD=X",
    "GBP/USD": "GBPUSD=X",
    "USD/JPY": "JPY=X",
    "USD/ZAR": "ZAR=X",  # Fixed symbol
    "AUD/USD": "AUDUSD=X",
    "USD/CAD": "CAD=X",
    "USD/CHF": "CHF=X",
    "XAU/USD": "GC=F",  # Gold futures
    "DXY": "DX-Y.NYB"
}

TIMEFRAMES = {
    "Daily": {"interval": "1d", "period": "3mo", "yf_interval": "1d"},
    "4 Hour": {"interval": "4h", "period": "1mo", "yf_interval": "60m"},
    "Hourly": {"interval": "1h", "period": "1mo", "yf_interval": "60m"}
}

# FRED API key from secrets (cloud) or environment variable
try:
    FRED_API_KEY = st.secrets.get("FRED_API_KEY", "")
except:
    FRED_API_KEY = ""

# ─────────────────────────────────────────────────────────────
# DATA FETCHING WITH RETRY
# ─────────────────────────────────────────────────────────────

@retry(stop=stop_after_attempt(3), wait=wait_fixed(2))
@st.cache_data(ttl=300, show_spinner=False)
def fetch_data(symbol, interval, period, yf_interval):
    """Fetch data from Yahoo Finance with retry logic"""
    try:
        ticker = yf.Ticker(symbol)
        
        # Handle 4H interval by resampling
        if interval == "4h":
            df = ticker.history(period=period, interval=yf_interval)
            if not df.empty:
                # Resample to 4 hours
                df = df.resample('4H').agg({
                    'Open': 'first',
                    'High': 'max',
                    'Low': 'min',
                    'Close': 'last',
                    'Volume': 'sum'
                }).dropna()
        else:
            df = ticker.history(period=period, interval=yf_interval)
        
        if df.empty:
            return pd.DataFrame()
        
        # Clean data
        df = df.dropna()
        df.columns = [col[0].upper() + col[1:] for col in df.columns]  # Capitalize columns
        
        return df
    
    except Exception as e:
        st.warning(f"Error fetching {symbol}: {str(e)}")
        return pd.DataFrame()

@st.cache_data(ttl=3600, show_spinner=False)
def fetch_macro_data():
    """Fetch macro data with FRED fallback to mock data"""
    macro_data = {
        "GDP": 2.8,
        "Inflation": 3.2,
        "Rates": 5.25,
        "Unemployment": 3.8
    }
    
    # Try FRED if API key is available
    if FRED_API_KEY:
        try:
            from fredapi import Fred
            fred = Fred(api_key=FRED_API_KEY)
            
            # GDP
            try:
                gdp = fred.get_series_latest_release("GDP")
                if not gdp.empty:
                    macro_data["GDP"] = gdp.iloc[-1]
            except:
                pass
            
            # Inflation (CPI)
            try:
                cpi = fred.get_series_latest_release("CPIAUCSL")
                if len(cpi) > 1:
                    macro_data["Inflation"] = cpi.pct_change().iloc[-1] * 100
            except:
                pass
            
            # Fed Funds Rate
            try:
                rates = fred.get_series_latest_release("DFF")
                if not rates.empty:
                    macro_data["Rates"] = rates.iloc[-1]
            except:
                pass
            
            # Unemployment
            try:
                unemp = fred.get_series_latest_release("UNRATE")
                if not unemp.empty:
                    macro_data["Unemployment"] = unemp.iloc[-1]
            except:
                pass
                
        except Exception as e:
            st.warning(f"FRED API error: {str(e)}")
    
    return macro_data

# ─────────────────────────────────────────────────────────────
# TECHNICAL INDICATORS
# ─────────────────────────────────────────────────────────────

def calculate_indicators(df):
    """Calculate comprehensive technical indicators"""
    if df.empty or len(df) < 20:
        return df
    
    df = df.copy()
    
    # Trend Indicators
    df['SMA_20'] = ta.trend.sma_indicator(df['Close'], window=20)
    df['SMA_50'] = ta.trend.sma_indicator(df['Close'], window=50)
    df['EMA_9'] = ta.trend.ema_indicator(df['Close'], window=9)
    df['EMA_21'] = ta.trend.ema_indicator(df['Close'], window=21)
    
    # Momentum Indicators
    df['RSI'] = ta.momentum.RSIIndicator(df['Close'], window=14).rsi()
    
    # MACD
    macd = ta.trend.MACD(df['Close'])
    df['MACD'] = macd.macd()
    df['MACD_Signal'] = macd.macd_signal()
    df['MACD_Histogram'] = macd.macd_diff()
    
    # Bollinger Bands
    bb = ta.volatility.BollingerBands(df['Close'], window=20)
    df['BB_Upper'] = bb.bollinger_hband()
    df['BB_Lower'] = bb.bollinger_lband()
    df['BB_Middle'] = bb.bollinger_mavg()
    df['BB_Width'] = (df['BB_Upper'] - df['BB_Lower']) / df['Close']
    
    # Volatility
    df['ATR'] = ta.volatility.AverageTrueRange(df['High'], df['Low'], df['Close'], window=14).average_true_range()
    
    # Support & Resistance
    df['Resistance'] = df['High'].rolling(window=20).max()
    df['Support'] = df['Low'].rolling(window=20).min()
    
    # Volume (if available)
    if 'Volume' in df.columns:
        df['Volume_SMA'] = ta.trend.sma_indicator(df['Volume'], window=20)
    
    return df

# ─────────────────────────────────────────────────────────────
# SIGNAL GENERATION
# ─────────────────────────────────────────────────────────────

def generate_signals(df, pair_name, macro_data, dxy_df=None):
    """Generate trading signals from technical and macro data"""
    if df.empty or len(df) < 20:
        return None
    
    df = calculate_indicators(df)
    latest = df.iloc[-1]
    
    signals = {
        'bullish': 0,
        'bearish': 0,
        'reasons': []
    }
    
    # RSI Signals
    rsi = latest.get('RSI', 50)
    if rsi < 30:
        signals['bullish'] += 2
        signals['reasons'].append(f"Oversold RSI ({rsi:.1f})")
    elif rsi > 70:
        signals['bearish'] += 2
        signals['reasons'].append(f"Overbought RSI ({rsi:.1f})")
    
    # Trend Signals
    price = latest['Close']
    sma20 = latest.get('SMA_20', price)
    sma50 = latest.get('SMA_50', price)
    
    if price > sma20 and sma20 > sma50:
        signals['bullish'] += 1
        signals['reasons'].append("Bullish trend structure")
    elif price < sma20 and sma20 < sma50:
        signals['bearish'] += 1
        signals['reasons'].append("Bearish trend structure")
    
    # MACD Signals
    macd = latest.get('MACD', 0)
    macd_signal = latest.get('MACD_Signal', 0)
    if macd > macd_signal:
        signals['bullish'] += 1
        signals['reasons'].append("MACD bullish crossover")
    else:
        signals['bearish'] += 1
        signals['reasons'].append("MACD bearish crossover")
    
    # Bollinger Bands
    bb_lower = latest.get('BB_Lower', price * 0.98)
    bb_upper = latest.get('BB_Upper', price * 1.02)
    if price <= bb_lower:
        signals['bullish'] += 1
        signals['reasons'].append("At lower Bollinger Band")
    elif price >= bb_upper:
        signals['bearish'] += 1
        signals['reasons'].append("At upper Bollinger Band")
    
    # Gold Macro Signals
    if pair_name == "XAU/USD" and dxy_df is not None and not dxy_df.empty:
        dxy_change = dxy_df['Close'].pct_change().rolling(5).mean().iloc[-1]
        if dxy_change < -0.002 and macro_data['Rates'] < 4:
            signals['bullish'] += 2
            signals['reasons'].append("Gold macro bullish (DXY falling, low rates)")
        elif dxy_change > 0.002 and macro_data['Rates'] > 4:
            signals['bearish'] += 2
            signals['reasons'].append("Gold macro bearish (DXY rising, high rates)")
    
    # Determine bias
    if signals['bullish'] > signals['bearish']:
        bias = "Long"
        strength = signals['bullish']
    elif signals['bearish'] > signals['bullish']:
        bias = "Short"
        strength = signals['bearish']
    else:
        bias = "Neutral"
        strength = 0
    
    # Conviction
    if strength >= 4:
        conviction = "High"
    elif strength >= 2:
        conviction = "Medium"
    else:
        conviction = "Low"
    
    # Price levels based on ATR
    atr = latest.get('ATR', price * 0.01)
    
    if bias == "Long":
        entry = price
        stop_loss = price - (atr * 0.75)
        take_profit_1 = price + (atr * 1.0)
        take_profit_2 = price + (atr * 1.5)
        risk_reward = (take_profit_1 - entry) / (entry - stop_loss) if (entry - stop_loss) > 0 else 0
    elif bias == "Short":
        entry = price
        stop_loss = price + (atr * 0.75)
        take_profit_1 = price - (atr * 1.0)
        take_profit_2 = price - (atr * 1.5)
        risk_reward = (entry - take_profit_1) / (stop_loss - entry) if (stop_loss - entry) > 0 else 0
    else:
        return None
    
    return {
        'pair': pair_name,
        'bias': bias,
        'conviction': conviction,
        'strength': strength,
        'thesis': " | ".join(signals['reasons'][:4]),
        'entry': entry,
        'stop_loss': stop_loss,
        'take_profit_1': take_profit_1,
        'take_profit_2': take_profit_2,
        'risk_reward': risk_reward,
        'rsi': rsi,
        'atr': atr,
        'price': price
    }

def multi_timeframe_analysis(data_dict, pair_name, macro_data):
    """Combine signals from multiple timeframes"""
    timeframes_data = {}
    
    for tf_name, tf_config in TIMEFRAMES.items():
        df = data_dict.get(tf_name, {}).get(pair_name, pd.DataFrame())
        if not df.empty:
            dxy_df = data_dict.get(tf_name, {}).get("DXY", pd.DataFrame())
            signal = generate_signals(df, pair_name, macro_data, dxy_df)
            if signal:
                timeframes_data[tf_name] = signal
    
    if not timeframes_data:
        return None
    
    # Weighted combination
    weights = {'Daily': 3, '4 Hour': 2, 'Hourly': 1}
    total_bullish = 0
    total_bearish = 0
    combined_reasons = []
    
    for tf, signal in timeframes_data.items():
        weight = weights.get(tf, 1)
        if signal['bias'] == 'Long':
            total_bullish += signal['strength'] * weight
        elif signal['bias'] == 'Short':
            total_bearish += signal['strength'] * weight
        combined_reasons.append(f"{tf}: {signal['thesis'][:50]}")
    
    # Final bias
    if total_bullish > total_bearish:
        bias = "Long"
        strength = total_bullish
    elif total_bearish > total_bullish:
        bias = "Short"
        strength = total_bearish
    else:
        return None
    
    # Conviction
    if strength >= 10:
        conviction = "High"
    elif strength >= 5:
        conviction = "Medium"
    else:
        conviction = "Low"
    
    # Use daily data for price levels
    daily_signal = timeframes_data.get('Daily', list(timeframes_data.values())[0])
    
    return {
        'pair': pair_name,
        'bias': bias,
        'conviction': conviction,
        'strength': strength,
        'thesis': " | ".join(combined_reasons[:3]),
        'entry': daily_signal['entry'],
        'stop_loss': daily_signal['stop_loss'],
        'take_profit_1': daily_signal['take_profit_1'],
        'take_profit_2': daily_signal['take_profit_2'],
        'risk_reward': daily_signal['risk_reward'],
        'timeframes': len(timeframes_data)
    }

# ─────────────────────────────────────────────────────────────
# UI COMPONENTS
# ─────────────────────────────────────────────────────────────

def render_header(macro_data):
    """Render dashboard header with metrics"""
    st.title("💹 Forex Macro Dashboard Pro")
    st.caption("Multi-Timeframe Analysis | Cloud-Ready | Hedge Fund Grade")
    
    col1, col2, col3, col4 = st.columns(4)
    with col1:
        st.metric("🇺🇸 GDP Growth", f"{macro_data['GDP']:.1f}%")
    with col2:
        st.metric("📈 Inflation (CPI)", f"{macro_data['Inflation']:.1f}%", 
                 delta="↑" if macro_data['Inflation'] > 2 else "↓")
    with col3:
        st.metric("🏦 Fed Funds Rate", f"{macro_data['Rates']:.2f}%")
    with col4:
        st.metric("👥 Unemployment", f"{macro_data['Unemployment']:.1f}%")
    st.divider()

def render_price_grid(data_dict):
    """Display price grid for major pairs"""
    st.subheader("📊 Live Prices")
    
    cols = st.columns(4)
    major_pairs = ["EUR/USD", "GBP/USD", "USD/JPY", "XAU/USD"]
    
    daily_data = data_dict.get('Daily', {})
    
    for idx, pair in enumerate(major_pairs):
        df = daily_data.get(pair, pd.DataFrame())
        if not df.empty and len(df) > 1:
            price = df['Close'].iloc[-1]
            change = ((df['Close'].iloc[-1] / df['Close'].iloc[-2]) - 1) * 100
            delta_color = "normal" if change >= 0 else "inverse"
            with cols[idx]:
                st.metric(pair, f"{price:.4f}", f"{change:.2f}%", delta_color=delta_color)

def render_trading_ideas(ideas):
    """Display trading ideas"""
    st.subheader("🎯 Trading Ideas")
    
    if not ideas:
        st.info("No strong signals detected at this time. Check back when markets are more active.")
        return
    
    # Filters
    col_filter1, col_filter2 = st.columns(2)
    with col_filter1:
        bias_filter = st.multiselect("Filter by Bias", ["Long", "Short"], default=["Long", "Short"])
    with col_filter2:
        conviction_filter = st.multiselect("Filter by Conviction", ["High", "Medium", "Low"], default=["High", "Medium"])
    
    filtered_ideas = [
        i for i in ideas 
        if i['bias'] in bias_filter and i['conviction'] in conviction_filter
    ]
    
    if not filtered_ideas:
        st.warning("No ideas match your filters")
        return
    
    # Summary stats
    col_a, col_b, col_c = st.columns(3)
    with col_a:
        st.metric("Total Ideas", len(filtered_ideas))
    with col_b:
        long_count = sum(1 for i in filtered_ideas if i['bias'] == "Long")
        st.metric("Long Signals", long_count)
    with col_c:
        short_count = sum(1 for i in filtered_ideas if i['bias'] == "Short")
        st.metric("Short Signals", short_count)
    
    st.divider()
    
    # Display ideas
    for idea in filtered_ideas:
        with st.expander(f"**{idea['pair']} - {idea['bias']}** | {idea['conviction']} Conviction | {idea['timeframes']} Timeframes"):
            col_left, col_right = st.columns([2, 1])
            
            with col_left:
                st.markdown(f"**Thesis:** {idea['thesis']}")
                st.markdown("**Price Levels:**")
                
                level_cols = st.columns(4)
                with level_cols[0]:
                    st.metric("Entry", f"{idea['entry']:.4f}")
                with level_cols[1]:
                    st.metric("TP1", f"{idea['take_profit_1']:.4f}")
                with level_cols[2]:
                    st.metric("TP2", f"{idea['take_profit_2']:.4f}")
                with level_cols[3]:
                    st.metric("Stop", f"{idea['stop_loss']:.4f}")
                
                st.caption(f"**Risk/Reward:** 1:{idea['risk_reward']:.2f}")
            
            with col_right:
                st.metric("Signal Strength", f"{idea['strength']:.0f}/15")
                if idea['bias'] == "Long":
                    st.success("📈 Bullish Setup")
                else:
                    st.error("📉 Bearish Setup")
    
    # Visualization
    st.subheader("Signal Distribution")
    col_viz1, col_viz2 = st.columns(2)
    
    with col_viz1:
        bias_counts = pd.DataFrame([
            {"Bias": i['bias'], "Count": 1} for i in filtered_ideas
        ]).groupby("Bias").count().reset_index()
        if not bias_counts.empty:
            fig = px.bar(bias_counts, x="Bias", y="Count", title="Bias Distribution", color="Bias")
            st.plotly_chart(fig, use_container_width=True)
    
    with col_viz2:
        conviction_counts = pd.DataFrame([
            {"Conviction": i['conviction'], "Count": 1} for i in filtered_ideas
        ]).groupby("Conviction").count().reset_index()
        if not conviction_counts.empty:
            fig = px.pie(conviction_counts, values="Count", names="Conviction", title="Conviction Levels")
            st.plotly_chart(fig, use_container_width=True)

def render_chart(df, pair_name, timeframe):
    """Render interactive price chart"""
    if df.empty:
        st.warning(f"No data available for {pair_name}")
        return
    
    df = calculate_indicators(df)
    
    fig = go.Figure()
    
    # Candlestick chart
    fig.add_trace(go.Candlestick(
        x=df.index,
        open=df['Open'],
        high=df['High'],
        low=df['Low'],
        close=df['Close'],
        name="Price"
    ))
    
    # Moving averages
    if 'SMA_20' in df.columns:
        fig.add_trace(go.Scatter(x=df.index, y=df['SMA_20'], name="SMA 20", line=dict(color='orange', width=1)))
    if 'SMA_50' in df.columns:
        fig.add_trace(go.Scatter(x=df.index, y=df['SMA_50'], name="SMA 50", line=dict(color='blue', width=1)))
    
    # Bollinger Bands
    if 'BB_Upper' in df.columns and 'BB_Lower' in df.columns:
        fig.add_trace(go.Scatter(x=df.index, y=df['BB_Upper'], name="BB Upper", line=dict(color='gray', dash='dash')))
        fig.add_trace(go.Scatter(x=df.index, y=df['BB_Lower'], name="BB Lower", line=dict(color='gray', dash='dash')))
    
    fig.update_layout(
        title=f"{pair_name} - {timeframe} Chart",
        xaxis_title="Date",
        yaxis_title="Price",
        height=500,
        template="plotly_dark"
    )
    
    st.plotly_chart(fig, use_container_width=True)
    
    # RSI subplot
    if 'RSI' in df.columns:
        fig_rsi = go.Figure()
        fig_rsi.add_trace(go.Scatter(x=df.index, y=df['RSI'], name="RSI", line=dict(color='purple')))
        fig_rsi.add_hline(y=70, line_dash="dash", line_color="red", annotation_text="Overbought")
        fig_rsi.add_hline(y=30, line_dash="dash", line_color="green", annotation_text="Oversold")
        fig_rsi.update_layout(title="RSI (14)", height=200, template="plotly_dark")
        st.plotly_chart(fig_rsi, use_container_width=True)

# ─────────────────────────────────────────────────────────────
# MAIN APP
# ─────────────────────────────────────────────────────────────

def main():
    # Sidebar
    with st.sidebar:
        st.header("⚙️ Settings")
        
        selected_timeframe = st.selectbox("Chart Timeframe", list(TIMEFRAMES.keys()))
        selected_pair = st.selectbox("Select Pair", list(ASSETS.keys()))
        
        st.divider()
        st.header("ℹ️ About")
        st.markdown("""
        **Features:**
        - Daily, 4H, and 1H analysis
        - Technical indicators (RSI, MACD, BB)
        - Trading ideas with R/R calculation
        - Macroeconomic context
        
        **Data Sources:**
        - Yahoo Finance (real-time)
        - FRED (macro, with API key)
        
        **Best for:**
        - Swing trading
        - Position trading
        - Macro hedging
        """)
        
        st.divider()
        if st.button("🔄 Refresh Data"):
            st.cache_data.clear()
            st.rerun()
    
    # Load data
    with st.spinner("Loading market data..."):
        macro_data = fetch_macro_data()
        
        data_by_timeframe = {}
        for tf_name, tf_config in TIMEFRAMES.items():
            tf_data = {}
            for pair_name, symbol in ASSETS.items():
                df = fetch_data(
                    symbol, 
                    tf_config['interval'], 
                    tf_config['period'], 
                    tf_config['yf_interval']
                )
                if not df.empty:
                    tf_data[pair_name] = df
            data_by_timeframe[tf_name] = tf_data
    
    # Header
    render_header(macro_data)
    
    # Price grid
    render_price_grid(data_by_timeframe)
    
    # Main content tabs
    tab1, tab2, tab3 = st.tabs(["📈 Charts", "🎯 Trading Ideas", "📊 Multi-Timeframe"])
    
    with tab1:
        col_chart, col_info = st.columns([3, 1])
        
        with col_chart:
            current_data = data_by_timeframe.get(selected_timeframe, {}).get(selected_pair, pd.DataFrame())
            render_chart(current_data, selected_pair, selected_timeframe)
        
        with col_info:
            st.subheader("Technical Snapshot")
            df = data_by_timeframe.get(selected_timeframe, {}).get(selected_pair, pd.DataFrame())
            if not df.empty and len(df) > 20:
                df = calculate_indicators(df)
                latest = df.iloc[-1]
                
                st.metric("RSI (14)", f"{latest.get('RSI', 50):.1f}")
                st.metric("ATR", f"{latest.get('ATR', 0):.4f}")
                st.metric("SMA 20", f"{latest.get('SMA_20', 0):.4f}")
                st.metric("SMA 50", f"{latest.get('SMA_50', 0):.4f}")
                
                # Current signal
                dxy_df = data_by_timeframe.get(selected_timeframe, {}).get("DXY", pd.DataFrame())
                signal = generate_signals(df, selected_pair, macro_data, dxy_df)
                if signal:
                    st.success(f"**Current Bias:** {signal['bias']}")
                    st.caption(f"Strength: {signal['strength']} | {signal['conviction']} conviction")
    
    with tab2:
        # Generate multi-timeframe ideas
        all_ideas = []
        for pair_name in ASSETS.keys():
            if pair_name != "DXY":  # Skip DXY for trading ideas
                idea = multi_timeframe_analysis(data_by_timeframe, pair_name, macro_data)
                if idea:
                    all_ideas.append(idea)
        
        all_ideas.sort(key=lambda x: (x['conviction'] == 'High', x['strength']), reverse=True)
        render_trading_ideas(all_ideas)
    
    with tab3:
        st.subheader("Multi-Timeframe Comparison")
        mtf_pair = st.selectbox("Select Pair for Analysis", [p for p in ASSETS.keys() if p != "DXY"])
        
        cols = st.columns(3)
        for idx, (tf_name, tf_data) in enumerate(data_by_timeframe.items()):
            df = tf_data.get(mtf_pair, pd.DataFrame())
            if not df.empty and len(df) > 20:
                df = calculate_indicators(df)
                latest = df.iloc[-1]
                
                with cols[idx]:
                    st.markdown(f"**{tf_name}**")
                    st.metric("Price", f"{latest['Close']:.4f}")
                    st.metric("RSI", f"{latest.get('RSI', 50):.1f}")
                    st.metric("Trend", "Bullish" if latest['Close'] > latest.get('SMA_20', latest['Close']) else "Bearish")
        
        # Normalized comparison chart
        st.subheader("Normalized Price Comparison")
        fig_compare = go.Figure()
        
        for tf_name, tf_data in data_by_timeframe.items():
            df = tf_data.get(mtf_pair, pd.DataFrame())
            if not df.empty:
                normalized = df['Close'] / df['Close'].iloc[0] * 100
                fig_compare.add_trace(go.Scatter(
                    x=df.index, 
                    y=normalized, 
                    name=tf_name,
                    mode='lines'
                ))
        
        fig_compare.update_layout(
            title=f"{mtf_pair} - Price Performance (Base 100)",
            xaxis_title="Date",
            yaxis_title="Index Value",
            height=400,
            template="plotly_dark"
        )
        st.plotly_chart(fig_compare, use_container_width=True)
    
    # Footer
    st.divider()
    st.caption(f"🔄 Last updated: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')} UTC")
    st.caption("📊 Data: Yahoo Finance | Macro: FRED (if API key configured) | Disclaimer: For educational purposes only")

if __name__ == "__main__":
    main()