"""
McKinsey / Hedge-Fund Grade Forex Macro Dashboard
================================================

Features:
- QuantConnect-ready data layer
- Gold (XAU/USD)
- FRED macro data
- Retry-safe fetching
- Technical indicators
- Multiple timeframes (Daily, 4H, 1H)
- Enhanced Trading Ideas with multi-timeframe analysis

Run:
pip install streamlit plotly pandas numpy yfinance ta fredapi tenacity requests
streamlit run forex_macro_dashboard_pro.py
"""

import os
import streamlit as st
import plotly.graph_objects as go
import plotly.express as px
import pandas as pd
import numpy as np
import yfinance as yf
import requests
from datetime import datetime, timedelta
import ta
from fredapi import Fred
from tenacity import retry, stop_after_attempt, wait_fixed

# ─────────────────────────────────────────────────────────────
# CONFIG
# ─────────────────────────────────────────────────────────────

st.set_page_config(page_title="Macro Dashboard Pro", layout="wide")

ASSETS = {
    "EUR/USD": "EURUSD=X",
    "GBP/USD": "GBPUSD=X",
    "USD/JPY": "JPY=X",
    "USD/ZAR": "USDZAR=X",
    "AUD/USD": "AUDUSD=X",
    "USD/CAD": "CAD=X",
    "USD/CHF": "CHF=X",
    "XAU/USD": "XAUUSD=X",
}

TIMEFRAME_MAPPING = {
    "Daily": "1d",
    "4 Hour": "4h",
    "Hourly": "1h"
}

PERIOD_MAPPING = {
    "Daily": "1mo",
    "4 Hour": "1mo",
    "Hourly": "1mo"
}

# ─────────────────────────────────────────────────────────────
# DATA PROVIDERS
# ─────────────────────────────────────────────────────────────

class MarketDataProvider:
    def get_data(self, symbol: str, interval: str, period: str):
        raise NotImplementedError


class QuantConnectProvider(MarketDataProvider):
    BASE_URL = "http://localhost:5000/api/marketdata"

    def get_data(self, symbol, interval, period):
        url = f"{self.BASE_URL}/{symbol}?interval={interval}&period={period}"
        r = requests.get(url, timeout=5)
        r.raise_for_status()
        df = pd.DataFrame(r.json())
        df['time'] = pd.to_datetime(df['time'])
        df.set_index('time', inplace=True)
        return df


class FallbackProvider(MarketDataProvider):
    def get_data(self, symbol, interval, period):
        ticker = yf.Ticker(symbol)
        
        # Handle different intervals for yfinance
        yf_interval = interval
        if interval == "4h":
            yf_interval = "60m"  # yfinance doesn't have 4h, use 1h and resample later
        
        df = ticker.history(period=period, interval=yf_interval)
        
        # Resample to 4h if needed
        if interval == "4h" and not df.empty:
            df = df.resample('4H').agg({
                'Open': 'first',
                'High': 'max',
                'Low': 'min',
                'Close': 'last',
                'Volume': 'sum'
            }).dropna()
        
        return df


def get_provider():
    try:
        provider = QuantConnectProvider()
        test = provider.get_data("EURUSD=X", "1d", "1mo")
        if not test.empty:
            return provider
    except:
        pass
    return FallbackProvider()


provider = get_provider()

# ─────────────────────────────────────────────────────────────
# RETRY
# ─────────────────────────────────────────────────────────────

@retry(stop=stop_after_attempt(3), wait=wait_fixed(2))
def safe_fetch(symbol, interval, period):
    return provider.get_data(symbol, interval, period)

# ─────────────────────────────────────────────────────────────
# MACRO (FRED)
# ─────────────────────────────────────────────────────────────

fred = Fred(api_key="3497b1f39dfba433a617ab52919f63ef")

@st.cache_data(ttl=3600)
def get_macro_data():
    try:
        return {
            "USD": {
                "GDP": fred.get_series_latest_release("GDP").iloc[-1],
                "Inflation": fred.get_series_latest_release("CPIAUCSL").pct_change().iloc[-1] * 100,
                "Rates": fred.get_series_latest_release("DFF").iloc[-1],
                "Unemployment": fred.get_series_latest_release("UNRATE").iloc[-1]
            }
        }
    except:
        return {"USD": {"GDP": 0, "Inflation": 0, "Rates": 0, "Unemployment": 0}}

# ─────────────────────────────────────────────────────────────
# TECHNICALS
# ─────────────────────────────────────────────────────────────

def add_indicators(df):
    if df.empty or len(df) < 20:
        return df

    df = df.copy()
    
    # Momentum
    df['RSI'] = ta.momentum.RSIIndicator(df['Close'], window=14).rsi()
    
    # MACD
    macd = ta.trend.MACD(df['Close'])
    df['MACD'] = macd.macd()
    df['MACD_Signal'] = macd.macd_signal()
    df['MACD_Histogram'] = macd.macd_diff()
    
    # Moving Averages
    df['SMA_20'] = ta.trend.sma_indicator(df['Close'], window=20)
    df['SMA_50'] = ta.trend.sma_indicator(df['Close'], window=50)
    df['EMA_9'] = ta.trend.ema_indicator(df['Close'], window=9)
    
    # Bollinger Bands
    bb = ta.volatility.BollingerBands(df['Close'], window=20)
    df['BB_Upper'] = bb.bollinger_hband()
    df['BB_Lower'] = bb.bollinger_lband()
    df['BB_Width'] = (df['BB_Upper'] - df['BB_Lower']) / df['Close']
    
    # ATR for volatility
    df['ATR'] = ta.volatility.AverageTrueRange(df['High'], df['Low'], df['Close'], window=14).average_true_range()
    
    # Support and Resistance (using rolling highs/lows)
    df['Resistance_20'] = df['High'].rolling(window=20).max()
    df['Support_20'] = df['Low'].rolling(window=20).min()
    
    return df

# ─────────────────────────────────────────────────────────────
# GOLD SIGNAL
# ─────────────────────────────────────────────────────────────

def gold_signal(dxy_df, rates):
    if dxy_df.empty:
        return "Neutral"

    trend = dxy_df['Close'].pct_change().rolling(5).mean().iloc[-1]

    if trend > 0.002 and rates > 3:
        return "Bearish ❌"
    elif trend < -0.002 and rates < 3:
        return "Bullish ✅"
    return "Neutral ⚖️"

# ─────────────────────────────────────────────────────────────
# ENHANCED TRADING IDEAS ENGINE
# ─────────────────────────────────────────────────────────────

def analyze_multi_timeframe(df_daily, df_4h, df_1h, name, macro, dxy):
    """Analyze multi-timeframe data for a single pair"""
    
    if df_daily.empty or df_4h.empty or df_1h.empty:
        return None
    
    # Add indicators to all timeframes
    df_daily = add_indicators(df_daily)
    df_4h = add_indicators(df_4h)
    df_1h = add_indicators(df_1h)
    
    # Get latest values
    daily = df_daily.iloc[-1]
    hourly = df_1h.iloc[-1]
    four_hour = df_4h.iloc[-1]
    
    signals = {
        'daily': {'bias': None, 'strength': 0, 'reasons': []},
        '4h': {'bias': None, 'strength': 0, 'reasons': []},
        '1h': {'bias': None, 'strength': 0, 'reasons': []}
    }
    
    # Analyze each timeframe
    for tf, df, last in [('daily', df_daily, daily), ('4h', df_4h, four_hour), ('1h', df_1h, hourly)]:
        bias = None
        strength = 0
        reasons = []
        
        # RSI Analysis
        rsi = last.get('RSI', 50)
        if rsi < 30:
            bias = 'Long'
            strength += 2 if rsi < 25 else 1
            reasons.append(f"Oversold RSI ({rsi:.1f})")
        elif rsi > 70:
            bias = 'Short'
            strength += 2 if rsi > 75 else 1
            reasons.append(f"Overbought RSI ({rsi:.1f})")
        
        # Trend Analysis (SMA)
        price = last['Close']
        sma20 = last.get('SMA_20', price)
        sma50 = last.get('SMA_50', price)
        
        if price > sma20 and sma20 > sma50:
            if bias != 'Short':
                bias = 'Long'
                strength += 1
                reasons.append("Bullish SMA alignment")
        elif price < sma20 and sma20 < sma50:
            if bias != 'Long':
                bias = 'Short'
                strength += 1
                reasons.append("Bearish SMA alignment")
        
        # MACD Analysis
        macd = last.get('MACD', 0)
        macd_signal = last.get('MACD_Signal', 0)
        if macd > macd_signal:
            if bias != 'Short':
                bias = 'Long'
                strength += 1
                reasons.append("MACD bullish crossover")
        else:
            if bias != 'Long':
                bias = 'Short'
                strength += 1
                reasons.append("MACD bearish crossover")
        
        # Bollinger Bands
        bb_lower = last.get('BB_Lower', price * 0.98)
        bb_upper = last.get('BB_Upper', price * 1.02)
        if price <= bb_lower:
            if bias != 'Short':
                bias = 'Long'
                strength += 1
                reasons.append("At lower Bollinger Band")
        elif price >= bb_upper:
            if bias != 'Long':
                bias = 'Short'
                strength += 1
                reasons.append("At upper Bollinger Band")
        
        signals[tf] = {'bias': bias, 'strength': strength, 'reasons': reasons}
    
    # Combine signals with weighted scoring
    weights = {'daily': 3, '4h': 2, '1h': 1}
    total_long_strength = 0
    total_short_strength = 0
    
    for tf, signal in signals.items():
        if signal['bias'] == 'Long':
            total_long_strength += signal['strength'] * weights[tf]
        elif signal['bias'] == 'Short':
            total_short_strength += signal['strength'] * weights[tf]
    
    # Determine final bias
    if total_long_strength > total_short_strength:
        final_bias = 'Long'
        final_strength = total_long_strength
    elif total_short_strength > total_long_strength:
        final_bias = 'Short'
        final_strength = total_short_strength
    else:
        final_bias = 'Neutral'
        final_strength = 0
    
    # Conviction level
    if final_strength >= 8:
        conviction = "High"
    elif final_strength >= 4:
        conviction = "Medium"
    else:
        conviction = "Low"
    
    # Generate thesis
    thesis_parts = []
    for tf, signal in signals.items():
        if signal['reasons']:
            thesis_parts.append(f"{tf.upper()}: {', '.join(signal['reasons'][:2])}")
    
    thesis = " | ".join(thesis_parts)
    
    # Add macro context for Gold
    if name == "XAU/USD":
        macro_bias = gold_signal(dxy, macro["USD"]["Rates"])
        thesis += f" | Macro: {macro_bias}"
        if "Bullish" in macro_bias and final_bias != 'Short':
            final_bias = 'Long'
            conviction = "High"
        elif "Bearish" in macro_bias and final_bias != 'Long':
            final_bias = 'Short'
            conviction = "High"
    
    # Calculate price levels based on ATR from 4h timeframe
    atr = four_hour.get('ATR', four_hour['Close'] * 0.01)
    current_price = four_hour['Close']
    
    if final_bias == 'Long':
        entry = current_price
        stop_loss = current_price - (atr * 0.75)
        take_profit_1 = current_price + (atr * 1.0)
        take_profit_2 = current_price + (atr * 1.5)
        risk_reward = (take_profit_1 - entry) / (entry - stop_loss) if (entry - stop_loss) > 0 else 0
    elif final_bias == 'Short':
        entry = current_price
        stop_loss = current_price + (atr * 0.75)
        take_profit_1 = current_price - (atr * 1.0)
        take_profit_2 = current_price - (atr * 1.5)
        risk_reward = (entry - take_profit_1) / (stop_loss - entry) if (stop_loss - entry) > 0 else 0
    else:
        return None
    
    return {
        "pair": name,
        "bias": final_bias,
        "conviction": conviction,
        "strength_score": final_strength,
        "thesis": thesis,
        "entry": entry,
        "take_profit_1": take_profit_1,
        "take_profit_2": take_profit_2,
        "stop_loss": stop_loss,
        "risk_reward": risk_reward,
        "timeframe_signals": signals
    }


def generate_trading_ideas(data_by_timeframe, macro, dxy_by_timeframe):
    """Generate trading ideas using multi-timeframe analysis"""
    ideas = []
    
    for pair_name in ASSETS.keys():
        # Get data for all timeframes
        df_daily = data_by_timeframe['Daily'].get(pair_name)
        df_4h = data_by_timeframe['4 Hour'].get(pair_name)
        df_1h = data_by_timeframe['Hourly'].get(pair_name)
        
        if df_daily is None or df_4h is None or df_1h is None:
            continue
        
        # Get DXY data for the same timeframes
        dxy_daily = dxy_by_timeframe['Daily']
        dxy_4h = dxy_by_timeframe['4 Hour']
        dxy_1h = dxy_by_timeframe['Hourly']
        
        # Use daily DXY for macro analysis
        idea = analyze_multi_timeframe(df_daily, df_4h, df_1h, pair_name, macro, dxy_daily)
        
        if idea and idea['bias'] != 'Neutral':
            ideas.append(idea)
    
    # Sort by conviction and strength
    ideas.sort(key=lambda x: (x['conviction'] == 'High', x['strength_score']), reverse=True)
    
    return ideas

# ─────────────────────────────────────────────────────────────
# SIDEBAR
# ─────────────────────────────────────────────────────────────

with st.sidebar:
    st.header("⚙️ Dashboard Settings")
    selected_timeframe = st.selectbox("Default Chart Timeframe", ["Daily", "4 Hour", "Hourly"])
    st.divider()
    st.header("📊 About")
    st.info("""
    **Multi-Timeframe Analysis**
    - Daily: Long-term trend
    - 4 Hour: Medium-term momentum
    - Hourly: Entry timing
    
    **Trading Ideas combine:**
    - Technical indicators across all timeframes
    - Macroeconomic data (FRED)
    - Gold-DXY correlation
    - Risk/Reward optimization
    """)

# ─────────────────────────────────────────────────────────────
# LOAD DATA FOR ALL TIMEFRAMES
# ─────────────────────────────────────────────────────────────

@st.cache_data(ttl=300)
def load_all_timeframes():
    """Load data for all timeframes"""
    data_by_timeframe = {
        'Daily': {},
        '4 Hour': {},
        'Hourly': {}
    }
    
    dxy_by_timeframe = {}
    
    for timeframe_name, interval in TIMEFRAME_MAPPING.items():
        period = PERIOD_MAPPING[timeframe_name]
        
        # Load forex pairs
        for name, symbol in ASSETS.items():
            try:
                df = safe_fetch(symbol, interval, period)
                if not df.empty:
                    data_by_timeframe[timeframe_name][name] = df
            except Exception as e:
                st.warning(f"Failed to load {name} ({timeframe_name}): {str(e)}")
                continue
        
        # Load DXY
        try:
            dxy_df = safe_fetch("DX-Y.NYB", interval, period)
            if not dxy_df.empty:
                dxy_by_timeframe[timeframe_name] = dxy_df
        except:
            dxy_by_timeframe[timeframe_name] = pd.DataFrame()
    
    return data_by_timeframe, dxy_by_timeframe

# Load data with progress indicator
with st.spinner("Loading market data for all timeframes..."):
    data_by_timeframe, dxy_by_timeframe = load_all_timeframes()

macro = get_macro_data()

# ─────────────────────────────────────────────────────────────
# HEADER
# ─────────────────────────────────────────────────────────────

st.title("💹 Macro Dashboard Pro - Multi-Timeframe Edition")
st.caption("Daily | 4 Hour | 1 Hour Analysis")

# ─────────────────────────────────────────────────────────────
# KPIs (Using Daily data for consistency)
# ─────────────────────────────────────────────────────────────

cols = st.columns(5)
pairs = ["EUR/USD", "GBP/USD", "USD/JPY", "USD/ZAR", "XAU/USD"]

daily_data = data_by_timeframe['Daily']

for i, pair in enumerate(pairs):
    with cols[i]:
        df = daily_data.get(pair)
        if df is not None and not df.empty:
            price = df['Close'].iloc[-1]
            change = df['Close'].pct_change().iloc[-1] * 100
            st.metric(pair, f"{price:.4f}", f"{change:.2f}%", delta_color="normal")

# ─────────────────────────────────────────────────────────────
# TABS
# ─────────────────────────────────────────────────────────────

tab1, tab2, tab3, tab4, tab5 = st.tabs([
    "📊 Overview",
    "📈 Technicals",
    "⏰ Multi-Timeframe",
    "🥇 Gold",
    "🎯 Trading Ideas"
])

# OVERVIEW
with tab1:
    st.subheader("Performance Overview")
    perf = []
    for name, df in daily_data.items():
        if len(df) > 1:
            ret = (df['Close'].iloc[-1] / df['Close'].iloc[0] - 1) * 100
            perf.append({"Asset": name, "Return %": ret})
    
    st.plotly_chart(px.bar(pd.DataFrame(perf), x="Asset", y="Return %", 
                            title="Period Returns (Daily)"), use_container_width=True)
    
    col1, col2, col3 = st.columns(3)
    with col1:
        st.metric("US GDP Growth", f"{macro['USD']['GDP']:.1f}%", delta=None)
    with col2:
        st.metric("US Inflation (CPI)", f"{macro['USD']['Inflation']:.1f}%", delta=None)
    with col3:
        st.metric("Fed Funds Rate", f"{macro['USD']['Rates']:.2f}%", delta=None)

# TECHNICALS
with tab2:
    pair = st.selectbox("Select Pair", list(daily_data.keys()), key="tech_pair")
    timeframe_tech = st.selectbox("Timeframe", ["Daily", "4 Hour", "Hourly"], key="tech_tf")
    
    df = data_by_timeframe[timeframe_tech].get(pair)
    if df is not None and not df.empty:
        df = add_indicators(df)
        
        fig = go.Figure()
        fig.add_trace(go.Candlestick(
            x=df.index,
            open=df['Open'],
            high=df['High'],
            low=df['Low'],
            close=df['Close'],
            name="Price"
        ))
        
        # Add moving averages
        fig.add_trace(go.Scatter(x=df.index, y=df['SMA_20'], name="SMA 20", line=dict(color='orange', width=1)))
        fig.add_trace(go.Scatter(x=df.index, y=df['SMA_50'], name="SMA 50", line=dict(color='blue', width=1)))
        
        # Add Bollinger Bands
        fig.add_trace(go.Scatter(x=df.index, y=df['BB_Upper'], name="BB Upper", line=dict(color='gray', dash='dash')))
        fig.add_trace(go.Scatter(x=df.index, y=df['BB_Lower'], name="BB Lower", line=dict(color='gray', dash='dash')))
        
        fig.update_layout(title=f"{pair} - {timeframe_tech} Chart", xaxis_title="Date", yaxis_title="Price")
        st.plotly_chart(fig, use_container_width=True)
        
        # RSI subplot
        fig_rsi = go.Figure()
        fig_rsi.add_trace(go.Scatter(x=df.index, y=df['RSI'], name="RSI"))
        fig_rsi.add_hline(y=70, line_dash="dash", line_color="red")
        fig_rsi.add_hline(y=30, line_dash="dash", line_color="green")
        fig_rsi.update_layout(title="RSI", height=200)
        st.plotly_chart(fig_rsi, use_container_width=True)

# MULTI-TIMEFRAME
with tab3:
    st.subheader("Multi-Timeframe Analysis")
    pair_mtf = st.selectbox("Select Pair", list(daily_data.keys()), key="mtf_pair")
    
    col1, col2, col3 = st.columns(3)
    
    for idx, (tf_name, data_dict) in enumerate([("Daily", daily_data), ("4 Hour", data_by_timeframe['4 Hour']), ("Hourly", data_by_timeframe['Hourly'])]):
        df = data_dict.get(pair_mtf)
        if df is not None and not df.empty:
            df = add_indicators(df)
            last = df.iloc[-1]
            
            with [col1, col2, col3][idx]:
                st.markdown(f"**{tf_name}**")
                st.metric("Price", f"{last['Close']:.4f}")
                st.metric("RSI", f"{last['RSI']:.1f}", 
                         delta="Oversold" if last['RSI'] < 30 else "Overbought" if last['RSI'] > 70 else None)
                st.metric("SMA 20", f"{last['SMA_20']:.4f}")
                st.metric("ATR", f"{last['ATR']:.4f}")
                
                # Trend indicator
                if last['Close'] > last['SMA_20']:
                    st.success("↑ Bullish")
                else:
                    st.error("↓ Bearish")
    
    # Multi-timeframe chart
    st.subheader("Multi-Timeframe Price Comparison")
    fig_mtf = go.Figure()
    
    for tf_name, data_dict in [("Daily", daily_data), ("4 Hour", data_by_timeframe['4 Hour']), ("Hourly", data_by_timeframe['Hourly'])]:
        df = data_dict.get(pair_mtf)
        if df is not None and not df.empty:
            # Normalize prices for comparison
            norm_price = df['Close'] / df['Close'].iloc[0] * 100
            fig_mtf.add_trace(go.Scatter(x=df.index, y=norm_price, name=tf_name))
    
    fig_mtf.update_layout(title=f"{pair_mtf} - Normalized Price Comparison (Base 100)", 
                          xaxis_title="Date", yaxis_title="Price Index")
    st.plotly_chart(fig_mtf, use_container_width=True)

# GOLD
with tab4:
    gold = daily_data.get("XAU/USD")
    if gold is not None:
        signal = gold_signal(dxy_by_timeframe['Daily'], macro["USD"]["Rates"])
        
        col1, col2 = st.columns(2)
        with col1:
            st.metric("Gold Signal (Daily)", signal)
            st.metric("Gold Price", f"{gold['Close'].iloc[-1]:.2f}", 
                     f"{gold['Close'].pct_change().iloc[-1]*100:.2f}%")
        
        with col2:
            st.metric("DXY Correlation", "Inverse", delta=None)
            st.metric("Fed Funds Rate", f"{macro['USD']['Rates']:.2f}%", delta=None)
        
        # Gold chart with technicals
        gold_tech = add_indicators(gold)
        fig_gold = go.Figure()
        fig_gold.add_trace(go.Scatter(x=gold_tech.index, y=gold_tech['Close'], name="Gold Price"))
        fig_gold.add_trace(go.Scatter(x=gold_tech.index, y=gold_tech['SMA_50'], name="SMA 50"))
        fig_gold.update_layout(title="XAU/USD - Daily Chart")
        st.plotly_chart(fig_gold, use_container_width=True)
        
        # Gold vs DXY
        dxy = dxy_by_timeframe['Daily']
        if not dxy.empty:
            fig_corr = go.Figure()
            fig_corr.add_trace(go.Scatter(x=gold_tech.index, y=gold_tech['Close'] / gold_tech['Close'].iloc[0] * 100, 
                                          name="Gold (Normalized)"))
            fig_corr.add_trace(go.Scatter(x=dxy.index, y=dxy['Close'] / dxy['Close'].iloc[0] * 100, 
                                          name="DXY (Normalized)"))
            fig_corr.update_layout(title="Gold vs DXY - Inverse Correlation")
            st.plotly_chart(fig_corr, use_container_width=True)

# TRADING IDEAS
with tab5:
    st.subheader("🎯 Multi-Timeframe Trading Ideas")
    st.caption("Combined analysis from Daily, 4 Hour, and 1 Hour timeframes")
    
    with st.spinner("Generating trading ideas from all timeframes..."):
        ideas = generate_trading_ideas(data_by_timeframe, macro, dxy_by_timeframe)
    
    if ideas:
        # Summary statistics
        col1, col2, col3 = st.columns(3)
        with col1:
            st.metric("Total Ideas", len(ideas))
        with col2:
            long_count = sum(1 for i in ideas if i["bias"] == "Long")
            st.metric("Long Signals", long_count)
        with col3:
            short_count = sum(1 for i in ideas if i["bias"] == "Short")
            st.metric("Short Signals", short_count)
        
        st.divider()
        
        # Display ideas
        for idea in ideas:
            with st.container():
                col_left, col_right = st.columns([2, 1])
                
                with col_left:
                    # Bias with appropriate coloring
                    if idea["bias"] == "Long":
                        st.success(f"### {idea['pair']} - {idea['bias']} 📈")
                    else:
                        st.error(f"### {idea['pair']} - {idea['bias']} 📉")
                    
                    # Conviction and strength
                    st.caption(f"**Conviction:** {idea['conviction']} | **Strength Score:** {idea['strength_score']}/10")
                    
                    # Thesis
                    st.markdown(f"**Thesis:** {idea['thesis']}")
                    
                    # Price levels
                    st.markdown("**Price Levels:**")
                    cols_levels = st.columns(4)
                    with cols_levels[0]:
                        st.metric("Entry", f"{idea['entry']:.4f}")
                    with cols_levels[1]:
                        st.metric("TP1", f"{idea['take_profit_1']:.4f}")
                    with cols_levels[2]:
                        st.metric("TP2", f"{idea['take_profit_2']:.4f}")
                    with cols_levels[3]:
                        st.metric("Stop Loss", f"{idea['stop_loss']:.4f}")
                    
                    st.caption(f"**Risk/Reward:** 1:{idea['risk_reward']:.2f}")
                
                with col_right:
                    # Timeframe breakdown
                    st.markdown("**Timeframe Analysis:**")
                    for tf in ['daily', '4h', '1h']:
                        tf_data = idea['timeframe_signals'][tf]
                        if tf_data['bias']:
                            st.markdown(f"- **{tf.upper()}:** {tf_data['bias']} (Strength: {tf_data['strength']})")
                            for reason in tf_data['reasons'][:2]:
                                st.markdown(f"  • {reason}")
                        else:
                            st.markdown(f"- **{tf.upper()}:** Neutral")
                
                st.divider()
        
        # Visualization
        st.subheader("Signal Distribution")
        col1, col2 = st.columns(2)
        with col1:
            fig_bias = go.Figure(go.Bar(
                x=["Long", "Short"],
                y=[sum(1 for i in ideas if i["bias"] == "Long"), 
                   sum(1 for i in ideas if i["bias"] == "Short")]
            ))
            fig_bias.update_layout(title="Bias Distribution")
            st.plotly_chart(fig_bias, use_container_width=True)
        
        with col2:
            conviction_data = pd.DataFrame([
                {"Conviction": i["conviction"], "Count": 1} for i in ideas
            ]).groupby("Conviction").count().reset_index()
            
            if not conviction_data.empty:
                fig_conv = px.pie(conviction_data, values="Count", names="Conviction", title="Conviction Levels")
                st.plotly_chart(fig_conv, use_container_width=True)
    
    else:
        st.warning("No trading ideas generated. Try refreshing data or check market conditions.")

# FOOTER
st.divider()
st.caption(f"Last updated: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
st.caption("Data sources: Yahoo Finance (fallback) | FRED Economic Data | Multi-Timeframe Analysis")