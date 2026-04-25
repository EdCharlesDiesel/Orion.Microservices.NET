

import os
import streamlit as st
import plotly.graph_objects as go
import plotly.express as px
from plotly.subplots import make_subplots
import pandas as pd
import numpy as np
import yfinance as yf
import requests
from datetime import datetime, timedelta
import ta
from fredapi import Fred
from tenacity import retry, stop_after_attempt, wait_fixed
import warnings
warnings.filterwarnings('ignore')

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
    "Weekly": {"interval": "1wk", "needs_resample": False, "resample_rule": None},
    "Daily": {"interval": "1d", "needs_resample": False, "resample_rule": None},
    "4 Hour": {"interval": "1h", "needs_resample": True, "resample_rule": "4H"},
    "Hourly": {"interval": "1h", "needs_resample": False, "resample_rule": None},
    "15 Minute": {"interval": "5m", "needs_resample": True, "resample_rule": "15T"}
}

PERIOD_MAPPING = {
    "Weekly": "3mo",
    "Daily": "3mo",
    "4 Hour": "1mo",
    "Hourly": "1mo",
    "15 Minute": "5d"  # 5 days of 5-minute data for 15m resampling
}

# DXY symbol
DXY_SYMBOL = "DX-Y.NYB"

# Strategy parameters
RISK_PER_TRADE = 0.02
ATR_SL_MULT = 1.5
MIN_RR = 2.0
ADX_TREND_MIN = 25
RSI_OS = 40
RSI_OB = 60
STOCH_OS = 25
STOCH_OB = 75

# Session windows (UTC)
LONDON_START = 9
LONDON_END = 12
NY_START = 13
NY_END = 16

# ─────────────────────────────────────────────────────────────
# DATA PROVIDERS
# ─────────────────────────────────────────────────────────────

class MarketDataProvider:
    def get_data(self, symbol: str, interval: str, period: str):
        raise NotImplementedError

class QuantConnectProvider(MarketDataProvider):
    BASE_URL = "http://localhost:5000/api/marketdata"

    def get_data(self, symbol, interval, period):
        try:
            url = f"{self.BASE_URL}/{symbol}?interval={interval}&period={period}"
            r = requests.get(url, timeout=5)
            r.raise_for_status()
            df = pd.DataFrame(r.json())
            if not df.empty and 'time' in df.columns:
                df['time'] = pd.to_datetime(df['time'])
                df.set_index('time', inplace=True)
            return df
        except:
            return pd.DataFrame()

class FallbackProvider(MarketDataProvider):
    def get_data(self, symbol, interval, period):
        try:
            ticker = yf.Ticker(symbol)
            
            # Handle different intervals for yfinance
            yf_interval = interval
            if interval == "4h":
                yf_interval = "1h"
            elif interval == "15m":
                yf_interval = "5m"  # Fetch 5m and resample to 15m
            
            df = ticker.history(period=period, interval=yf_interval)
            
            if df.empty:
                return df
            
            # Resample to weekly if needed
            if interval == "1wk":
                df = df.resample('W').agg({
                    'Open': 'first',
                    'High': 'max',
                    'Low': 'min',
                    'Close': 'last',
                    'Volume': 'sum'
                }).dropna()
            
            # Resample to 4h if needed
            elif interval == "4h":
                df = df.resample('4H').agg({
                    'Open': 'first',
                    'High': 'max',
                    'Low': 'min',
                    'Close': 'last',
                    'Volume': 'sum'
                }).dropna()
            
            # Resample to 15m if needed
            elif interval == "15m":
                df = df.resample('15T').agg({
                    'Open': 'first',
                    'High': 'max',
                    'Low': 'min',
                    'Close': 'last',
                    'Volume': 'sum'
                }).dropna()
            
            return df
        except Exception as e:
            st.warning(f"Error fetching {symbol}: {str(e)}")
            return pd.DataFrame()

def get_provider():
    try:
        provider = QuantConnectProvider()
        test = provider.get_data("EURUSD=X", "1d", "1mo")
        if test is not None and not test.empty:
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

# Get FRED API key from environment variable
FRED_API_KEY  = Fred("3497b1f39dfba433a617ab52919f63ef")

try:
    if FRED_API_KEY:
        fred = Fred("3497b1f39dfba433a617ab52919f63ef")
    else:
        fred = None
        st.warning("FRED API key not set. Using default macro values.")
except:
    fred = None
    st.warning("Could not initialize FRED. Using default macro values.")

@st.cache_data(ttl=3600)
def get_macro_data():
    """Fetch macro data from FRED"""
    default_data = {
        "USD": {"GDP": 2.5, "Inflation": 3.2, "Rates": 5.5, "Unemployment": 3.8},
        "ZAR": {"GDP": 1.2, "Inflation": 5.0, "Rates": 8.25, "Unemployment": 32.1},
        "JPY": {"GDP": 1.1, "Inflation": 2.8, "Rates": -0.1, "Unemployment": 2.6},
        "AUD": {"GDP": 2.0, "Inflation": 4.1, "Rates": 4.35, "Unemployment": 3.9},
        "CAD": {"GDP": 1.5, "Inflation": 3.4, "Rates": 5.0, "Unemployment": 5.1},
        "EUR": {"GDP": 0.8, "Inflation": 2.9, "Rates": 4.5, "Unemployment": 6.5},
        "GBP": {"GDP": 0.6, "Inflation": 3.4, "Rates": 5.25, "Unemployment": 4.2},
        "CHF": {"GDP": 0.9, "Inflation": 2.1, "Rates": 1.75, "Unemployment": 2.0}
    }
    
    if fred is None:
        return default_data
    
    try:
        macro_data = {}
        for currency in ["USD", "ZAR", "JPY", "AUD", "CAD", "EUR", "GBP", "CHF"]:
            macro_data[currency] = default_data[currency].copy()
            
            try:
                # GDP (using US GDP as proxy for all)
                gdp = fred.get_series_latest_release("GDP")
                if not gdp.empty:
                    macro_data[currency]["GDP"] = gdp.iloc[-1]
            except:
                pass
                
            try:
                # Inflation
                cpi = fred.get_series_latest_release("CPIAUCSL")
                if not cpi.empty and len(cpi) > 1:
                    macro_data[currency]["Inflation"] = cpi.pct_change().iloc[-1] * 100
            except:
                pass
                
            try:
                # Fed Funds Rate (US)
                fed_rate = fred.get_series_latest_release("DFF")
                if not fed_rate.empty:
                    macro_data[currency]["Rates"] = fed_rate.iloc[-1]
            except:
                pass
                
            try:
                # Unemployment
                unrate = fred.get_series_latest_release("UNRATE")
                if not unrate.empty:
                    macro_data[currency]["Unemployment"] = unrate.iloc[-1]
            except:
                pass
        
        return macro_data
    except Exception as e:
        st.warning(f"Error fetching FRED data: {e}")
        return default_data

# ─────────────────────────────────────────────────────────────
# TECHNICAL INDICATORS
# ─────────────────────────────────────────────────────────────

def add_indicators(df):
    """Add comprehensive technical indicators to dataframe"""
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
    df['EMA_20'] = ta.trend.ema_indicator(df['Close'], window=20)
    df['EMA_50'] = ta.trend.ema_indicator(df['Close'], window=50)
    df['EMA_200'] = ta.trend.ema_indicator(df['Close'], window=200)
    
    # Bollinger Bands
    bb = ta.volatility.BollingerBands(df['Close'], window=20, window_dev=2)
    df['BB_Upper'] = bb.bollinger_hband()
    df['BB_Middle'] = bb.bollinger_mavg()
    df['BB_Lower'] = bb.bollinger_lband()
    df['BB_Width'] = (df['BB_Upper'] - df['BB_Lower']) / df['Close']
    
    # ATR for volatility
    atr = ta.volatility.AverageTrueRange(df['High'], df['Low'], df['Close'], window=14)
    df['ATR'] = atr.average_true_range()
    
    # Stochastic Oscillator
    stoch = ta.momentum.StochasticOscillator(df['High'], df['Low'], df['Close'], window=14, smooth_window=3)
    df['Stoch_K'] = stoch.stoch()
    df['Stoch_D'] = stoch.stoch_signal()
    
    # ADX for trend strength
    adx = ta.trend.ADXIndicator(df['High'], df['Low'], df['Close'], window=14)
    df['ADX'] = adx.adx()
    df['ADX_Pos'] = adx.adx_pos()
    df['ADX_Neg'] = adx.adx_neg()
    
    # Support and Resistance (using rolling highs/lows)
    df['Resistance_20'] = df['High'].rolling(window=20).max()
    df['Support_20'] = df['Low'].rolling(window=20).min()
    df['Pivot_Point'] = (df['High'] + df['Low'] + df['Close']) / 3
    
    return df

# ─────────────────────────────────────────────────────────────
# 15-MINUTE ENTRY SIGNALS
# ─────────────────────────────────────────────────────────────

def get_15m_entry_signal(df_15m, bias):
    """
    Generate precise entry signals from 15-minute timeframe
    
    Long Entry Conditions:
    - Stochastic %K crosses above %D from below 25 (oversold)
    - Price above lower Bollinger Band
    - RSI > 30 (momentum turning up)
    
    Short Entry Conditions:
    - Stochastic %K crosses below %D from above 75 (overbought)
    - Price below upper Bollinger Band
    - RSI < 70 (momentum turning down)
    """
    if df_15m.empty or len(df_15m) < 5:
        return {'signal': 0, 'confidence': 0, 'reason': 'Insufficient data'}
    
    df_15m = add_indicators(df_15m)
    last = df_15m.iloc[-1]
    prev = df_15m.iloc[-2] if len(df_15m) > 1 else last
    
    # Get latest values
    k = last.get('Stoch_K', 50)
    d = last.get('Stoch_D', 50)
    prev_k = prev.get('Stoch_K', 50)
    prev_d = prev.get('Stoch_D', 50)
    rsi = last.get('RSI', 50)
    price = last['Close']
    bb_lower = last.get('BB_Lower', price * 0.99)
    bb_upper = last.get('BB_Upper', price * 1.01)
    
    signal = 0
    confidence = 0
    reasons = []
    
    if bias == 'Long':
        # Stochastic oversold crossover
        if prev_k <= prev_d and k > d and k < STOCH_OS:
            signal = 1
            confidence += 2
            reasons.append(f"Stochastic bullish crossover (K={k:.1f})")
        
        # RSI oversold bounce
        if rsi < 35:
            confidence += 1
            reasons.append(f"RSI oversold ({rsi:.1f})")
        
        # Price at lower BB support
        if price <= bb_lower * 1.002:
            confidence += 1
            reasons.append("Price at lower Bollinger Band")
        
        # Check trading session
        current_hour = datetime.now().hour
        if LONDON_START <= current_hour <= LONDON_END or NY_START <= current_hour <= NY_END:
            confidence += 1
            reasons.append("Active trading session")
    
    elif bias == 'Short':
        # Stochastic overbought crossunder
        if prev_k >= prev_d and k < d and k > STOCH_OB:
            signal = -1
            confidence += 2
            reasons.append(f"Stochastic bearish crossover (K={k:.1f})")
        
        # RSI overbought rejection
        if rsi > 65:
            confidence += 1
            reasons.append(f"RSI overbought ({rsi:.1f})")
        
        # Price at upper BB resistance
        if price >= bb_upper * 0.998:
            confidence += 1
            reasons.append("Price at upper Bollinger Band")
        
        # Check trading session
        current_hour = datetime.now().hour
        if LONDON_START <= current_hour <= LONDON_END or NY_START <= current_hour <= NY_END:
            confidence += 1
            reasons.append("Active trading session")
    
    return {
        'signal': signal,
        'confidence': min(confidence, 5),
        'reasons': reasons,
        'stoch_k': k,
        'stoch_d': d,
        'rsi': rsi,
        'price': price
    }

# ─────────────────────────────────────────────────────────────
# GOLD SIGNAL
# ─────────────────────────────────────────────────────────────

def gold_signal(dxy_df, rates):
    if dxy_df.empty:
        return "Neutral ⚖️"

    trend = dxy_df['Close'].pct_change().rolling(5).mean().iloc[-1]

    if trend > 0.002 and rates > 3:
        return "Bearish ❌"
    elif trend < -0.002 and rates < 3:
        return "Bullish ✅"
    return "Neutral ⚖️"

# ─────────────────────────────────────────────────────────────
# ENHANCED TRADING IDEAS ENGINE
# ─────────────────────────────────────────────────────────────

def analyze_multi_timeframe(df_weekly, df_daily, df_4h, df_1h, df_15m, name, macro, dxy_df):
    """Analyze multi-timeframe data with 15-minute entry signals"""
    
    # Check if we have enough data
    if df_daily.empty or df_4h.empty or df_1h.empty or df_15m.empty:
        return None
    
    # Add indicators to all timeframes
    df_weekly = add_indicators(df_weekly) if not df_weekly.empty else df_weekly
    df_daily = add_indicators(df_daily)
    df_4h = add_indicators(df_4h)
    df_1h = add_indicators(df_1h)
    df_15m = add_indicators(df_15m)
    
    # Get latest values
    daily = df_daily.iloc[-1]
    four_hour = df_4h.iloc[-1]
    hourly = df_1h.iloc[-1]
    fifteen_min = df_15m.iloc[-1]
    weekly = df_weekly.iloc[-1] if not df_weekly.empty else None
    
    signals = {
        'weekly': {'bias': None, 'strength': 0, 'reasons': []},
        'daily': {'bias': None, 'strength': 0, 'reasons': []},
        '4h': {'bias': None, 'strength': 0, 'reasons': []},
        '1h': {'bias': None, 'strength': 0, 'reasons': []}
    }
    
    # Analyze each timeframe
    for tf, df, last in [('weekly', df_weekly, weekly), 
                         ('daily', df_daily, daily), 
                         ('4h', df_4h, four_hour), 
                         ('1h', df_1h, hourly)]:
        if last is None or df.empty:
            continue
            
        bias = None
        strength = 0
        reasons = []
        
        # ADX trend strength check
        adx = last.get('ADX', 0)
        if not pd.isna(adx) and adx > ADX_TREND_MIN:
            strength += 1
            reasons.append(f"Strong trend (ADX={adx:.1f})")
        
        # RSI Analysis
        rsi = last.get('RSI', 50)
        if not pd.isna(rsi):
            if rsi < 30:
                bias = 'Long'
                strength += 2 if rsi < 25 else 1
                reasons.append(f"Oversold RSI ({rsi:.1f})")
            elif rsi > 70:
                bias = 'Short'
                strength += 2 if rsi > 75 else 1
                reasons.append(f"Overbought RSI ({rsi:.1f})")
        
        # EMA Trend Analysis
        price = last['Close']
        ema20 = last.get('EMA_20', price)
        ema50 = last.get('EMA_50', price)
        ema200 = last.get('EMA_200', price)
        
        if not pd.isna(ema20) and not pd.isna(ema50):
            if price > ema20 and ema20 > ema50:
                if bias != 'Short':
                    bias = 'Long'
                    strength += 1
                    reasons.append("Bullish EMA alignment")
            elif price < ema20 and ema20 < ema50:
                if bias != 'Long':
                    bias = 'Short'
                    strength += 1
                    reasons.append("Bearish EMA alignment")
        
        # MACD Analysis
        macd = last.get('MACD', 0)
        macd_signal = last.get('MACD_Signal', 0)
        if not pd.isna(macd) and not pd.isna(macd_signal):
            if macd > macd_signal:
                if bias != 'Short':
                    bias = 'Long'
                    strength += 1
                    reasons.append("MACD bullish crossover")
            elif macd < macd_signal:
                if bias != 'Long':
                    bias = 'Short'
                    strength += 1
                    reasons.append("MACD bearish crossover")
        
        signals[tf] = {'bias': bias, 'strength': strength, 'reasons': reasons}
    
    # Combine signals with weighted scoring
    weights = {'weekly': 4, 'daily': 3, '4h': 2, '1h': 1}
    total_long_strength = 0
    total_short_strength = 0
    
    for tf, signal in signals.items():
        if signal['bias'] == 'Long':
            total_long_strength += signal['strength'] * weights.get(tf, 1)
        elif signal['bias'] == 'Short':
            total_short_strength += signal['strength'] * weights.get(tf, 1)
    
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
    
    # Get 15-minute entry signal
    entry_signal = get_15m_entry_signal(df_15m, final_bias) if final_bias != 'Neutral' else None
    
    # Conviction level
    if final_strength >= 8:
        conviction = "High"
    elif final_strength >= 4:
        conviction = "Medium"
    else:
        conviction = "Low"
    
    # Enhance conviction with entry signal confidence
    if entry_signal and entry_signal['confidence'] >= 3:
        conviction = "High" if conviction == "Medium" else conviction
    
    # Generate thesis
    thesis_parts = []
    for tf, signal in signals.items():
        if signal['reasons']:
            thesis_parts.append(f"{tf.upper()}: {', '.join(signal['reasons'][:2])}")
    
    # Add entry signal to thesis
    if entry_signal and entry_signal['signal'] != 0:
        thesis_parts.append(f"15M Entry: {', '.join(entry_signal['reasons'][:2])}")
        thesis_parts.append(f"Entry Confidence: {entry_signal['confidence']}/5")
    
    thesis = " | ".join(thesis_parts) if thesis_parts else "No clear signals"
    
    # Add macro context for Gold
    if name == "XAU/USD" and not dxy_df.empty:
        macro_bias = gold_signal(dxy_df, macro.get("USD", {}).get("Rates", 5.5))
        thesis += f" | Macro: {macro_bias}"
        if "Bullish" in macro_bias and final_bias != 'Short':
            final_bias = 'Long'
            conviction = "High"
        elif "Bearish" in macro_bias and final_bias != 'Long':
            final_bias = 'Short'
            conviction = "High"
    
    # Calculate price levels based on ATR from 15m timeframe
    atr = fifteen_min.get('ATR', fifteen_min['Close'] * 0.005)
    current_price = fifteen_min['Close']
    
    if final_bias == 'Long':
        entry = current_price
        stop_loss = current_price - (atr * ATR_SL_MULT)
        take_profit_1 = current_price + (atr * 1.0)
        take_profit_2 = current_price + (atr * 2.0)
        risk_reward = (take_profit_1 - entry) / (entry - stop_loss) if (entry - stop_loss) > 0 else 0
    elif final_bias == 'Short':
        entry = current_price
        stop_loss = current_price + (atr * ATR_SL_MULT)
        take_profit_1 = current_price - (atr * 1.0)
        take_profit_2 = current_price - (atr * 2.0)
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
        "entry_signal": entry_signal,
        "timeframe_signals": signals
    }

def generate_trading_ideas(data_by_timeframe, macro, dxy_by_timeframe):
    """Generate trading ideas using multi-timeframe analysis with 15-minute entries"""
    ideas = []
    
    for pair_name in ASSETS.keys():
        # Get data for all timeframes
        df_weekly = data_by_timeframe['Weekly'].get(pair_name, pd.DataFrame())
        df_daily = data_by_timeframe['Daily'].get(pair_name, pd.DataFrame())
        df_4h = data_by_timeframe['4 Hour'].get(pair_name, pd.DataFrame())
        df_1h = data_by_timeframe['Hourly'].get(pair_name, pd.DataFrame())
        df_15m = data_by_timeframe['15 Minute'].get(pair_name, pd.DataFrame())
        
        if df_daily.empty or df_4h.empty or df_1h.empty or df_15m.empty:
            continue
        
        # Get DXY data
        dxy_df = dxy_by_timeframe.get('Daily', pd.DataFrame())
        
        idea = analyze_multi_timeframe(df_weekly, df_daily, df_4h, df_1h, df_15m, pair_name, macro, dxy_df)
        
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
    selected_timeframe = st.selectbox("Default Chart Timeframe", ["Daily", "4 Hour", "Hourly", "15 Minute"])
    st.divider()
    st.header("🎯 Entry Strategy Parameters")
    st.info(f"""
    **15-Minute Entry Conditions:**
    - Stochastic crossover (K > D from oversold/overbought)
    - RSI confirmation
    - Bollinger Band touch
    - Active trading session (London/NY)
    
    **Risk Parameters:**
    - Risk per trade: {RISK_PER_TRADE*100}%
    - ATR Stop Loss: {ATR_SL_MULT}x
    - Min R/R: {MIN_RR}:1
    """)
    st.divider()
    st.header("📊 About")
    st.info("""
    **Multi-Timeframe Analysis**
    - Weekly: Long-term trend
    - Daily: Medium-term trend
    - 4 Hour: Short-term momentum
    - Hourly: Setup confirmation
    - 15 Minute: Entry timing
    
    **Trading Ideas combine:**
    - Technical indicators across all timeframes
    - Macroeconomic data (FRED)
    - Gold-DXY correlation
    - Precise 15-minute entry signals
    """)
    st.divider()
    st.caption(f"Last updated: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")

# ─────────────────────────────────────────────────────────────
# LOAD DATA FOR ALL TIMEFRAMES
# ─────────────────────────────────────────────────────────────

@st.cache_data(ttl=300)
def load_all_timeframes():
    """Load data for all timeframes"""
    data_by_timeframe = {
        'Weekly': {},
        'Daily': {},
        '4 Hour': {},
        'Hourly': {},
        '15 Minute': {}
    }
    
    dxy_by_timeframe = {}
    
    # Load DXY data for all timeframes
    for timeframe_name, interval in TIMEFRAME_MAPPING.items():
        period = PERIOD_MAPPING.get(timeframe_name, "1mo")
        try:
            dxy_df = safe_fetch(DXY_SYMBOL, interval, period)
            if not dxy_df.empty:
                dxy_by_timeframe[timeframe_name] = dxy_df
        except Exception as e:
            st.warning(f"Failed to load DXY ({timeframe_name}): {str(e)}")
            dxy_by_timeframe[timeframe_name] = pd.DataFrame()
    
    # Load forex pairs
    for timeframe_name, interval in TIMEFRAME_MAPPING.items():
        period = PERIOD_MAPPING.get(timeframe_name, "1mo")
        
        for name, symbol in ASSETS.items():
            try:
                df = safe_fetch(symbol, interval, period)
                if not df.empty:
                    data_by_timeframe[timeframe_name][name] = df
            except Exception as e:
                st.warning(f"Failed to load {name} ({timeframe_name}): {str(e)}")
                continue
    
    return data_by_timeframe, dxy_by_timeframe

# Load data with progress indicator
with st.spinner("Loading market data for all timeframes (including 15-minute)..."):
    data_by_timeframe, dxy_by_timeframe = load_all_timeframes()

macro = get_macro_data()

# ─────────────────────────────────────────────────────────────
# HEADER
# ─────────────────────────────────────────────────────────────

st.title("💹 Macro Pro - Multi-Timeframe Edition with 15-Minute Entry")
st.caption("Weekly | Daily | 4 Hour | Hourly | 15 Minute Analysis")

# ─────────────────────────────────────────────────────────────
# KPIs (Using Daily data for consistency)
# ─────────────────────────────────────────────────────────────

cols = st.columns(4)
pairs = ["EUR/USD", "GBP/USD", "USD/JPY", "XAU/USD"]

daily_data = data_by_timeframe['Daily']

for i, pair in enumerate(pairs):
    if i < len(cols):
        with cols[i]:
            df = daily_data.get(pair)
            if df is not None and not df.empty:
                price = df['Close'].iloc[-1]
                change = df['Close'].pct_change().iloc[-1] * 100
                st.metric(pair, f"{price:.4f}", f"{change:+.2f}%", delta_color="normal")

# ─────────────────────────────────────────────────────────────
# TABS
# ─────────────────────────────────────────────────────────────

tab1, tab2, tab3, tab4, tab5, tab6 = st.tabs([
    "📊 Overview",
    "📈 Technicals",
    "⏰ Multi-Timeframe",
    "⏱️ 15-Minute Entry",
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
    
    if perf:
        st.plotly_chart(px.bar(pd.DataFrame(perf), x="Asset", y="Return %", 
                                title="Period Returns (Daily)"), use_container_width=True)
    else:
        st.warning("No performance data available")
    
    col1, col2, col3 = st.columns(3)
    usd_macro = macro.get("USD", {})
    with col1:
        st.metric("US GDP Growth", f"{usd_macro.get('GDP', 0):.1f}%", delta=None)
    with col2:
        st.metric("US Inflation (CPI)", f"{usd_macro.get('Inflation', 0):.1f}%", delta=None)
    with col3:
        st.metric("Fed Funds Rate", f"{usd_macro.get('Rates', 0):.2f}%", delta=None)
    
    # Trading session indicator
    st.subheader("Trading Sessions")
    current_hour = datetime.now().hour
    col1, col2 = st.columns(2)
    with col1:
        if LONDON_START <= current_hour <= LONDON_END:
            st.success("🔴 London Session: ACTIVE")
        else:
            st.info("⚫ London Session: Closed")
    with col2:
        if NY_START <= current_hour <= NY_END:
            st.success("🟢 New York Session: ACTIVE")
        else:
            st.info("⚫ New York Session: Closed")

# TECHNICALS
with tab2:
    available_pairs = [p for p in daily_data.keys() if not daily_data[p].empty]
    if available_pairs:
        pair = st.selectbox("Select Pair", available_pairs, key="tech_pair")
        timeframe_tech = st.selectbox("Timeframe", ["Weekly", "Daily", "4 Hour", "Hourly", "15 Minute"], key="tech_tf")
        
        df = data_by_timeframe[timeframe_tech].get(pair, pd.DataFrame())
        if not df.empty:
            df = add_indicators(df)
            
            # Create subplots for price and indicators
            fig = make_subplots(
                rows=3, cols=1,
                shared_xaxes=True,
                vertical_spacing=0.05,
                row_heights=[0.5, 0.25, 0.25],
                subplot_titles=(f'{pair} - {timeframe_tech}', 'RSI', 'Stochastic')
            )
            
            # Candlestick chart
            fig.add_trace(go.Candlestick(
                x=df.index,
                open=df['Open'],
                high=df['High'],
                low=df['Low'],
                close=df['Close'],
                name="Price"
            ), row=1, col=1)
            
            # Add moving averages
            if 'EMA_20' in df.columns and not df['EMA_20'].isna().all():
                fig.add_trace(go.Scatter(x=df.index, y=df['EMA_20'], name="EMA 20", line=dict(color='orange', width=1)), row=1, col=1)
            if 'EMA_50' in df.columns and not df['EMA_50'].isna().all():
                fig.add_trace(go.Scatter(x=df.index, y=df['EMA_50'], name="EMA 50", line=dict(color='blue', width=1)), row=1, col=1)
            
            # Add Bollinger Bands
            if 'BB_Upper' in df.columns and not df['BB_Upper'].isna().all():
                fig.add_trace(go.Scatter(x=df.index, y=df['BB_Upper'], name="BB Upper", line=dict(color='gray', dash='dash')), row=1, col=1)
                fig.add_trace(go.Scatter(x=df.index, y=df['BB_Lower'], name="BB Lower", line=dict(color='gray', dash='dash')), row=1, col=1)
            
            # RSI
            if 'RSI' in df.columns and not df['RSI'].isna().all():
                fig.add_trace(go.Scatter(x=df.index, y=df['RSI'], name="RSI", line=dict(color='purple', width=1)), row=2, col=1)
                fig.add_hline(y=70, line_dash="dash", line_color="red", row=2, col=1)
                fig.add_hline(y=30, line_dash="dash", line_color="green", row=2, col=1)
            
            # Stochastic
            if 'Stoch_K' in df.columns and not df['Stoch_K'].isna().all():
                fig.add_trace(go.Scatter(x=df.index, y=df['Stoch_K'], name="Stoch %K", line=dict(color='blue', width=1)), row=3, col=1)
                fig.add_trace(go.Scatter(x=df.index, y=df['Stoch_D'], name="Stoch %D", line=dict(color='red', width=1)), row=3, col=1)
                fig.add_hline(y=80, line_dash="dash", line_color="red", row=3, col=1)
                fig.add_hline(y=20, line_dash="dash", line_color="green", row=3, col=1)
            
            fig.update_layout(title=f"{pair} - {timeframe_tech} Technical Analysis", height=800, showlegend=True)
            st.plotly_chart(fig, use_container_width=True)
        else:
            st.warning(f"No data available for {pair} on {timeframe_tech} timeframe")
    else:
        st.warning("No data available")

# MULTI-TIMEFRAME
with tab3:
    st.subheader("Multi-Timeframe Analysis")
    available_pairs = [p for p in daily_data.keys() if not daily_data[p].empty]
    if available_pairs:
        pair_mtf = st.selectbox("Select Pair", available_pairs, key="mtf_pair")
        
        timeframes = ["Weekly", "Daily", "4 Hour", "Hourly", "15 Minute"]
        cols = st.columns(len(timeframes))
        
        for idx, tf_name in enumerate(timeframes):
            df = data_by_timeframe[tf_name].get(pair_mtf, pd.DataFrame())
            if not df.empty:
                df = add_indicators(df)
                last = df.iloc[-1]
                
                with cols[idx]:
                    st.markdown(f"**{tf_name}**")
                    st.metric("Price", f"{last['Close']:.4f}")
                    if 'RSI' in last and not pd.isna(last['RSI']):
                        rsi_val = last['RSI']
                        st.metric("RSI", f"{rsi_val:.1f}", 
                                 delta="Oversold" if rsi_val < 30 else "Overbought" if rsi_val > 70 else None)
                    if 'Stoch_K' in last and not pd.isna(last['Stoch_K']):
                        st.metric("Stoch K", f"{last['Stoch_K']:.1f}")
                    if 'ADX' in last and not pd.isna(last['ADX']):
                        st.metric("ADX", f"{last['ADX']:.1f}")
                    if 'ATR' in last and not pd.isna(last['ATR']):
                        st.metric("ATR", f"{last['ATR']:.4f}")
                    
                    # Trend indicator
                    if 'EMA_20' in last and not pd.isna(last['EMA_20']):
                        if last['Close'] > last['EMA_20']:
                            st.success("↑ Bullish")
                        else:
                            st.error("↓ Bearish")
        
        # Multi-timeframe chart
        st.subheader("Multi-Timeframe Price Comparison")
        fig_mtf = go.Figure()
        
        for tf_name in ["Daily", "4 Hour", "Hourly", "15 Minute"]:
            df = data_by_timeframe[tf_name].get(pair_mtf, pd.DataFrame())
            if not df.empty:
                # Normalize prices for comparison
                norm_price = df['Close'] / df['Close'].iloc[0] * 100
                fig_mtf.add_trace(go.Scatter(x=df.index, y=norm_price, name=tf_name))
        
        fig_mtf.update_layout(title=f"{pair_mtf} - Normalized Price Comparison (Base 100)", 
                              xaxis_title="Date", yaxis_title="Price Index", height=500)
        st.plotly_chart(fig_mtf, use_container_width=True)
    else:
        st.warning("No data available")

# 15-MINUTE ENTRY
with tab4:
    st.subheader("⏱️ 15-Minute Entry Signals")
    st.caption("Real-time entry signals based on 15-minute timeframe analysis")
    
    available_pairs = [p for p in daily_data.keys() if not daily_data[p].empty]
    if available_pairs:
        pair_entry = st.selectbox("Select Pair for Entry Analysis", available_pairs, key="entry_pair")
        
        # Get 15-minute data
        df_15m = data_by_timeframe['15 Minute'].get(pair_entry, pd.DataFrame())
        
        if not df_15m.empty:
            # Get trend bias from higher timeframes
            df_daily = data_by_timeframe['Daily'].get(pair_entry, pd.DataFrame())
            df_4h = data_by_timeframe['4 Hour'].get(pair_entry, pd.DataFrame())
            df_1h = data_by_timeframe['Hourly'].get(pair_entry, pd.DataFrame())
            
            # Determine trend bias
            trend_bias = 'Neutral'
            if not df_daily.empty:
                daily = add_indicators(df_daily).iloc[-1]
                if daily.get('ADX', 0) > ADX_TREND_MIN:
                    if daily['Close'] > daily.get('EMA_20', daily['Close']):
                        trend_bias = 'Long'
                    else:
                        trend_bias = 'Short'
            
            # Get entry signal
            entry_signal = get_15m_entry_signal(df_15m, trend_bias)
            
            # Display entry signal
            col1, col2, col3 = st.columns(3)
            with col1:
                if entry_signal['signal'] == 1:
                    st.success(f"### 🟢 LONG ENTRY SIGNAL")
                elif entry_signal['signal'] == -1:
                    st.error(f"### 🔴 SHORT ENTRY SIGNAL")
                else:
                    st.info(f"### ⚪ No Entry Signal")
            
            with col2:
                st.metric("Signal Confidence", f"{entry_signal['confidence']}/5")
            
            with col3:
                st.metric("Current Price", f"{entry_signal['price']:.5f}")
            
            # Display reasons
            if entry_signal['reasons']:
                st.subheader("Entry Conditions Met:")
                for reason in entry_signal['reasons']:
                    st.success(f"✅ {reason}")
            
            # Display current indicators
            st.subheader("Current 15-Minute Indicators")
            col1, col2, col3, col4 = st.columns(4)
            with col1:
                st.metric("Stochastic %K", f"{entry_signal['stoch_k']:.1f}")
            with col2:
                st.metric("Stochastic %D", f"{entry_signal['stoch_d']:.1f}")
            with col3:
                st.metric("RSI", f"{entry_signal['rsi']:.1f}")
            with col4:
                st.metric("Trend Bias", trend_bias)
            
            # Create 15-minute chart with entry signals
            df_15m_indicators = add_indicators(df_15m.tail(100))
            
            fig_entry = make_subplots(
                rows=3, cols=1,
                shared_xaxes=True,
                vertical_spacing=0.05,
                row_heights=[0.5, 0.25, 0.25],
                subplot_titles=(f'{pair_entry} - 15-Minute Chart', 'Stochastic', 'Entry Signals')
            )
            
            # Price chart
            fig_entry.add_trace(go.Candlestick(
                x=df_15m_indicators.index,
                open=df_15m_indicators['Open'],
                high=df_15m_indicators['High'],
                low=df_15m_indicators['Low'],
                close=df_15m_indicators['Close'],
                name="Price"
            ), row=1, col=1)
            
            # Add Bollinger Bands
            if 'BB_Upper' in df_15m_indicators.columns:
                fig_entry.add_trace(go.Scatter(x=df_15m_indicators.index, y=df_15m_indicators['BB_Upper'], 
                                              name="BB Upper", line=dict(color='gray', dash='dash')), row=1, col=1)
                fig_entry.add_trace(go.Scatter(x=df_15m_indicators.index, y=df_15m_indicators['BB_Lower'], 
                                              name="BB Lower", line=dict(color='gray', dash='dash')), row=1, col=1)
            
            # Stochastic
            fig_entry.add_trace(go.Scatter(x=df_15m_indicators.index, y=df_15m_indicators['Stoch_K'], 
                                          name="Stoch %K", line=dict(color='blue')), row=2, col=1)
            fig_entry.add_trace(go.Scatter(x=df_15m_indicators.index, y=df_15m_indicators['Stoch_D'], 
                                          name="Stoch %D", line=dict(color='red')), row=2, col=1)
            fig_entry.add_hline(y=80, line_dash="dash", line_color="red", row=2, col=1)
            fig_entry.add_hline(y=20, line_dash="dash", line_color="green", row=2, col=1)
            
            # Entry signals indicator
            entry_signals = []
            for i in range(len(df_15m_indicators)):
                if i > 0:
                    prev_k = df_15m_indicators['Stoch_K'].iloc[i-1]
                    prev_d = df_15m_indicators['Stoch_D'].iloc[i-1]
                    curr_k = df_15m_indicators['Stoch_K'].iloc[i]
                    curr_d = df_15m_indicators['Stoch_D'].iloc[i]
                    
                    if prev_k <= prev_d and curr_k > curr_d and curr_k < STOCH_OS:
                        entry_signals.append((df_15m_indicators.index[i], df_15m_indicators['Low'].iloc[i], 'Long'))
                    elif prev_k >= prev_d and curr_k < curr_d and curr_k > STOCH_OB:
                        entry_signals.append((df_15m_indicators.index[i], df_15m_indicators['High'].iloc[i], 'Short'))
            
            # Add entry signals to chart
            for timestamp, price, signal_type in entry_signals[-20:]:  # Last 20 signals
                color = 'green' if signal_type == 'Long' else 'red'
                symbol = '▲' if signal_type == 'Long' else '▼'
                fig_entry.add_annotation(x=timestamp, y=price, text=symbol, 
                                        showarrow=True, arrowhead=1, 
                                        arrowcolor=color, font=dict(color=color, size=20),
                                        row=1, col=1)
            
            fig_entry.update_layout(height=800, title=f"{pair_entry} - 15-Minute Entry Signals")
            st.plotly_chart(fig_entry, use_container_width=True)
        else:
            st.warning("No 15-minute data available")
    else:
        st.warning("No data available")

# GOLD
with tab5:
    gold = daily_data.get("XAU/USD", pd.DataFrame())
    if not gold.empty:
        dxy = dxy_by_timeframe.get('Daily', pd.DataFrame())
        signal = gold_signal(dxy, macro.get("USD", {}).get("Rates", 5.5))
        
        col1, col2 = st.columns(2)
        with col1:
            st.metric("Gold Signal (Daily)", signal)
            st.metric("Gold Price", f"{gold['Close'].iloc[-1]:.2f}", 
                     f"{gold['Close'].pct_change().iloc[-1]*100:+.2f}%")
        
        with col2:
            st.metric("DXY Correlation", "Inverse", delta=None)
            st.metric("Fed Funds Rate", f"{macro.get('USD', {}).get('Rates', 0):.2f}%", delta=None)
        
        # Gold chart with technicals
        gold_tech = add_indicators(gold)
        fig_gold = go.Figure()
        fig_gold.add_trace(go.Scatter(x=gold_tech.index, y=gold_tech['Close'], name="Gold Price"))
        if 'SMA_50' in gold_tech.columns and not gold_tech['SMA_50'].isna().all():
            fig_gold.add_trace(go.Scatter(x=gold_tech.index, y=gold_tech['SMA_50'], name="SMA 50"))
        fig_gold.update_layout(title="XAU/USD - Daily Chart", height=500)
        st.plotly_chart(fig_gold, use_container_width=True)
        
        # Gold vs DXY
        if not dxy.empty:
            fig_corr = go.Figure()
            fig_corr.add_trace(go.Scatter(x=gold_tech.index, y=gold_tech['Close'] / gold_tech['Close'].iloc[0] * 100, 
                                          name="Gold (Normalized)"))
            fig_corr.add_trace(go.Scatter(x=dxy.index, y=dxy['Close'] / dxy['Close'].iloc[0] * 100, 
                                          name="DXY (Normalized)"))
            fig_corr.update_layout(title="Gold vs DXY - Inverse Correlation", height=500)
            st.plotly_chart(fig_corr, use_container_width=True)
    else:
        st.warning("No Gold data available")

# TRADING IDEAS
with tab6:
    st.subheader("🎯 Multi-Timeframe Trading Ideas with 15-Minute Entry")
    st.caption("Combined analysis from Weekly, Daily, 4 Hour, Hourly, and 15-Minute timeframes")
    
    with st.spinner("Generating trading ideas from all timeframes (including 15-minute entries)..."):
        ideas = generate_trading_ideas(data_by_timeframe, macro, dxy_by_timeframe)
    
    if ideas:
        # Summary statistics
        col1, col2, col3, col4 = st.columns(4)
        with col1:
            st.metric("Total Ideas", len(ideas))
        with col2:
            long_count = sum(1 for i in ideas if i["bias"] == "Long")
            st.metric("Long Signals", long_count)
        with col3:
            short_count = sum(1 for i in ideas if i["bias"] == "Short")
            st.metric("Short Signals", short_count)
        with col4:
            high_conviction = sum(1 for i in ideas if i["conviction"] == "High")
            st.metric("High Conviction", high_conviction)
        
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
                        st.metric("Entry", f"{idea['entry']:.5f}")
                    with cols_levels[1]:
                        st.metric("TP1", f"{idea['take_profit_1']:.5f}")
                    with cols_levels[2]:
                        st.metric("TP2", f"{idea['take_profit_2']:.5f}")
                    with cols_levels[3]:
                        st.metric("Stop Loss", f"{idea['stop_loss']:.5f}")
                    
                    st.caption(f"**Risk/Reward:** 1:{idea['risk_reward']:.2f}")
                
                with col_right:
                    # Entry signal summary
                    if idea['entry_signal'] and idea['entry_signal']['signal'] != 0:
                        st.markdown("**⚡ Entry Signal Ready!**")
                        st.metric("Confidence", f"{idea['entry_signal']['confidence']}/5")
                        for reason in idea['entry_signal']['reasons'][:2]:
                            st.caption(f"✓ {reason}")
                    
                    # Timeframe breakdown
                    st.markdown("**Timeframe Analysis:**")
                    for tf in ['weekly', 'daily', '4h', '1h']:
                        tf_data = idea['timeframe_signals'].get(tf, {'bias': None})
                        if tf_data and tf_data.get('bias'):
                            st.markdown(f"- **{tf.upper()}:** {tf_data['bias']} (Strength: {tf_data['strength']})")
                
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
            conviction_counts = {}
            for i in ideas:
                conviction_counts[i["conviction"]] = conviction_counts.get(i["conviction"], 0) + 1
            
            if conviction_counts:
                conviction_df = pd.DataFrame({
                    "Conviction": list(conviction_counts.keys()),
                    "Count": list(conviction_counts.values())
                })
                fig_conv = px.pie(conviction_df, values="Count", names="Conviction", title="Conviction Levels")
                st.plotly_chart(fig_conv, use_container_width=True)
    
    else:
        st.warning("No trading ideas generated. Try refreshing data or check market conditions.")

# FOOTER
st.divider()
st.caption(f"Data sources: Yahoo Finance | FRED Economic Data | Multi-Timeframe Analysis with 15-Minute Entry Signals")