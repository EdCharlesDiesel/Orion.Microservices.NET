import os
import sys
import logging
import traceback
from datetime import datetime, timedelta
from typing import Dict, Optional, Tuple, Any, List
from dataclasses import dataclass, field
import warnings
import streamlit as st
import plotly.graph_objects as go
import plotly.express as px
from plotly.subplots import make_subplots
import pandas as pd
import numpy as np
import yfinance as yf
import ta

warnings.filterwarnings('ignore')

# ============================================================================
# PAGE CONFIGURATION
# ============================================================================

st.set_page_config(
    page_title="Macro Dashboard Pro",
    page_icon="📊",
    layout="wide",
    initial_sidebar_state="expanded"
)

# ============================================================================
# CONFIGURATION
# ============================================================================

@dataclass
class AppConfig:
    """Application configuration"""
    # Asset configuration - using correct Yahoo Finance symbols
    assets: Dict[str, str] = field(default_factory=lambda: {
        "EUR/USD": "EURUSD=X",
        "GBP/USD": "GBPUSD=X",
        "USD/JPY": "JPY=X",
        "USD/ZAR": "ZAR=X",
        "AUD/USD": "AUDUSD=X",
        "NZD/USD": "NZDUSD=X",
        "USD/CAD": "CAD=X",
        "USD/CHF": "CHF=X",
        "XAU/USD": "GC=F",  # Gold futures
    })
    
    # Timeframe configuration - simplified for reliability
    timeframes: Dict[str, Dict] = field(default_factory=lambda: {
        "Weekly": {"interval": "1wk", "period": "3mo"},
        "Daily": {"interval": "1d", "period": "3mo"},
        "4 Hour": {"interval": "1h", "period": "1mo"},
        "Hourly": {"interval": "1h", "period": "1mo"},
        "15 Minute": {"interval": "15m", "period": "5d"}
    })
    
    # Strategy parameters
    risk_per_trade: float = 0.02
    atr_sl_mult: float = 1.5
    min_rr: float = 2.0
    adx_trend_min: float = 20  # Lowered threshold
    rsi_os: float = 40
    rsi_ob: float = 60
    stoch_os: float = 25
    stoch_ob: float = 75

    # Per-pair ATR stop multipliers
    pair_atr_multipliers: Dict[str, float] = field(default_factory=lambda: {
        "EUR/USD": 1.5,
        "GBP/USD": 1.8,
        "USD/JPY": 1.5,
        "USD/ZAR": 2.5,
        "AUD/USD": 1.5,
        "NZD/USD": 1.6,
        "USD/CAD": 1.5,
        "USD/CHF": 1.5,
        "XAU/USD": 2.0,
    })

    # Minimum stop distance in price units per pair
    pair_min_stop: Dict[str, float] = field(default_factory=lambda: {
        "EUR/USD": 0.0010,
        "GBP/USD": 0.0015,
        "USD/JPY": 0.10,
        "USD/ZAR": 0.05,
        "AUD/USD": 0.0010,
        "NZD/USD": 0.0010,
        "USD/CAD": 0.0010,
        "USD/CHF": 0.0010,
        "XAU/USD": 2.00,
    })
    
    # Trading sessions (UTC)
    london_start: int = 8
    london_end: int = 16
    ny_start: int = 13
    ny_end: int = 21
    
    # DXY symbol
    dxy_symbol: str = "DX-Y.NYB"
    
    # Cache TTL (seconds)
    cache_ttl: int = 300


# ============================================================================
# LOGGING SETUP
# ============================================================================

def setup_logging() -> logging.Logger:
    """Configure application logging"""
    logger = logging.getLogger("ForexDashboard")
    logger.setLevel(logging.INFO)
    
    if not logger.handlers:
        handler = logging.StreamHandler()
        formatter = logging.Formatter(
            '%(asctime)s - %(name)s - %(levelname)s - %(message)s'
        )
        handler.setFormatter(formatter)
        logger.addHandler(handler)
    
    return logger

logger = setup_logging()


# ============================================================================
# CONFIGURATION MANAGEMENT
# ============================================================================

@st.cache_resource
def get_config() -> AppConfig:
    """Get application configuration"""
    return AppConfig()

config = get_config()


# ============================================================================
# DATA FETCHING WITH CACHING
# ============================================================================

@st.cache_data(ttl=config.cache_ttl, show_spinner=False)
def fetch_data(symbol: str, interval: str, period: str) -> pd.DataFrame:
    """Fetch data from Yahoo Finance with caching"""
    try:
        ticker = yf.Ticker(symbol)
        df = ticker.history(period=period, interval=interval)
        
        if df.empty:
            return df
        
        return df
        
    except Exception as e:
        logger.error(f"Error fetching {symbol}: {e}")
        return pd.DataFrame()


# ============================================================================
# MACRO DATA
# ============================================================================

@st.cache_data(ttl=3600)
def get_macro_data() -> Dict:
    """Get macro data (using defaults for reliability)"""
    return {
        "USD": {"GDP": 2.5, "Inflation": 3.2, "Rates": 5.5, "Unemployment": 3.8},
        "ZAR": {"GDP": 1.2, "Inflation": 5.0, "Rates": 8.25, "Unemployment": 32.1},
        "JPY": {"GDP": 1.1, "Inflation": 2.8, "Rates": -0.1, "Unemployment": 2.6},
        "NZD": {"GDP": 2.2, "Inflation": 3.8, "Rates": 5.5, "Unemployment": 3.9},
        "AUD": {"GDP": 2.0, "Inflation": 4.1, "Rates": 4.35, "Unemployment": 3.9},
        "CAD": {"GDP": 1.5, "Inflation": 3.4, "Rates": 5.0, "Unemployment": 5.1},
        "EUR": {"GDP": 0.8, "Inflation": 2.9, "Rates": 4.5, "Unemployment": 6.5},
        "GBP": {"GDP": 0.6, "Inflation": 3.4, "Rates": 5.25, "Unemployment": 4.2},
        "CHF": {"GDP": 0.9, "Inflation": 2.1, "Rates": 1.75, "Unemployment": 2.0}
    }


# ============================================================================
# TECHNICAL INDICATORS
# ============================================================================

class TechnicalAnalyzer:
    """Technical indicator calculator"""
    
    @staticmethod
    def add_indicators(df: pd.DataFrame) -> pd.DataFrame:
        """Add comprehensive technical indicators"""
        if df.empty or len(df) < 20:
            return df
        
        df = df.copy()
        
        try:
            # RSI
            df['RSI'] = ta.momentum.RSIIndicator(df['Close'], window=14).rsi()
            
            # MACD
            macd = ta.trend.MACD(df['Close'])
            df['MACD'] = macd.macd()
            df['MACD_Signal'] = macd.macd_signal()
            df['MACD_Histogram'] = macd.macd_diff()
            
            # Moving Averages
            df['SMA_20'] = ta.trend.sma_indicator(df['Close'], window=20)
            df['SMA_50'] = ta.trend.sma_indicator(df['Close'], window=50)
            df['EMA_20'] = ta.trend.ema_indicator(df['Close'], window=20)
            df['EMA_50'] = ta.trend.ema_indicator(df['Close'], window=50)
            
            # Bollinger Bands
            bb = ta.volatility.BollingerBands(df['Close'], window=20, window_dev=2)
            df['BB_Upper'] = bb.bollinger_hband()
            df['BB_Middle'] = bb.bollinger_mavg()
            df['BB_Lower'] = bb.bollinger_lband()
            
            # ATR
            atr = ta.volatility.AverageTrueRange(df['High'], df['Low'], df['Close'], window=14)
            df['ATR'] = atr.average_true_range()
            
            # Stochastic
            stoch = ta.momentum.StochasticOscillator(df['High'], df['Low'], df['Close'], 
                                                      window=14, smooth_window=3)
            df['Stoch_K'] = stoch.stoch()
            df['Stoch_D'] = stoch.stoch_signal()
            
            # ADX
            adx = ta.trend.ADXIndicator(df['High'], df['Low'], df['Close'], window=14)
            df['ADX'] = adx.adx()
            df['ADX_Pos'] = adx.adx_pos()
            df['ADX_Neg'] = adx.adx_neg()
            
            # Support/Resistance
            df['Resistance_20'] = df['High'].rolling(window=20).max()
            df['Support_20'] = df['Low'].rolling(window=20).min()
            
        except Exception as e:
            logger.error(f"Error calculating indicators: {e}")
        
        return df


analyzer = TechnicalAnalyzer()


# ============================================================================
# ENTRY SIGNAL GENERATOR
# ============================================================================

class EntrySignalGenerator:
    """Generates entry signals from 15-minute timeframe"""
    
    def __init__(self):
        self.config = config
    
    def get_entry_signal(self, df_15m: pd.DataFrame, bias: str) -> Dict:
        """Generate entry signal based on 15-minute data"""
        if df_15m.empty or len(df_15m) < 5:
            return {'signal': 0, 'confidence': 0, 'reasons': ['Insufficient data']}
        
        df = analyzer.add_indicators(df_15m)
        last = df.iloc[-1]
        prev = df.iloc[-2] if len(df) > 1 else last
        
        # Get values with safe fallbacks
        k = last.get('Stoch_K', 50) if not pd.isna(last.get('Stoch_K', 50)) else 50
        d = last.get('Stoch_D', 50) if not pd.isna(last.get('Stoch_D', 50)) else 50
        prev_k = prev.get('Stoch_K', 50) if not pd.isna(prev.get('Stoch_K', 50)) else 50
        prev_d = prev.get('Stoch_D', 50) if not pd.isna(prev.get('Stoch_D', 50)) else 50
        rsi = last.get('RSI', 50) if not pd.isna(last.get('RSI', 50)) else 50
        price = last['Close']
        bb_lower = last.get('BB_Lower', price * 0.99)
        bb_upper = last.get('BB_Upper', price * 1.01)
        
        signal = 0
        confidence = 0
        reasons = []
        
        if bias == 'Long':
            # Stochastic oversold crossover
            if prev_k <= prev_d and k > d and k < self.config.stoch_os:
                signal = 1
                confidence += 2
                reasons.append(f"Stochastic bullish crossover (K={k:.1f})")
            
            # RSI oversold
            if rsi < self.config.rsi_os:
                confidence += 1
                reasons.append(f"RSI oversold ({rsi:.1f})")
            
            # Lower BB touch
            if price <= bb_lower * 1.002:
                confidence += 1
                reasons.append("Price at lower Bollinger Band")
        
        elif bias == 'Short':
            # Stochastic overbought crossunder
            if prev_k >= prev_d and k < d and k > self.config.stoch_ob:
                signal = -1
                confidence += 2
                reasons.append(f"Stochastic bearish crossover (K={k:.1f})")
            
            # RSI overbought
            if rsi > self.config.rsi_ob:
                confidence += 1
                reasons.append(f"RSI overbought ({rsi:.1f})")
            
            # Upper BB touch
            if price >= bb_upper * 0.998:
                confidence += 1
                reasons.append("Price at upper Bollinger Band")
        
        return {
            'signal': signal,
            'confidence': min(confidence, 5),
            'reasons': reasons,
            'stoch_k': k,
            'stoch_d': d,
            'rsi': rsi,
            'price': price
        }


entry_generator = EntrySignalGenerator()


# ============================================================================
# STOP LOSS CALCULATOR
# ============================================================================

class StopLossCalculator:
    """Structure + ATR + Per-Pair Volatility stop loss calculator"""

    def __init__(self, cfg: AppConfig):
        self.config = cfg

    def pip_size(self, pair: str) -> float:
        """Return the value of 1 pip for the given pair."""
        if "JPY" in pair:
            return 0.01
        if pair == "XAU/USD":
            return 0.10
        if "ZAR" in pair:
            return 0.001
        return 0.0001

    def price_to_pips(self, pair: str, distance: float) -> float:
        """Convert a price distance to pips for the given pair."""
        ps = self.pip_size(pair)
        return round(distance / ps, 1) if ps > 0 else 0.0

    def get_swing_stop(self, df: pd.DataFrame, bias: str, lookback: int = 20) -> Optional[float]:
        """Find the most recent significant swing point to anchor the stop."""
        if df.empty or len(df) < lookback:
            return None
        recent = df.tail(lookback)
        if bias == 'Long':
            return float(recent['Low'].min())
        else:
            return float(recent['High'].max())

    def calculate(
        self,
        df: pd.DataFrame,
        pair: str,
        bias: str,
        current_price: float,
        atr: float,
        lookback: int = 20
    ) -> Dict:
        """Calculate the optimal stop loss for a trade setup."""
        atr_mult = self.config.pair_atr_multipliers.get(pair, self.config.atr_sl_mult)
        min_dist = self.config.pair_min_stop.get(pair, 0.0010)

        # ATR stop (baseline)
        if bias == 'Long':
            atr_stop = current_price - (atr * atr_mult)
        else:
            atr_stop = current_price + (atr * atr_mult)

        # Structure stop
        swing_level = self.get_swing_stop(df, bias, lookback)
        method_parts = []

        if swing_level is not None:
            buffer = atr * 0.25
            if bias == 'Long':
                struct_stop = swing_level - buffer
                if struct_stop < atr_stop:
                    stop = struct_stop
                    method_parts.append(f"Swing Low")
                else:
                    stop = atr_stop
                    method_parts.append(f"ATR")
            else:
                struct_stop = swing_level + buffer
                if struct_stop > atr_stop:
                    stop = struct_stop
                    method_parts.append(f"Swing High")
                else:
                    stop = atr_stop
                    method_parts.append(f"ATR")
        else:
            stop = atr_stop
            method_parts.append(f"ATR")

        # Minimum distance enforcement
        actual_dist = abs(current_price - stop)
        if actual_dist < min_dist:
            if bias == 'Long':
                stop = current_price - min_dist
            else:
                stop = current_price + min_dist

        stop_pips = self.price_to_pips(pair, abs(current_price - stop))

        return {
            "stop": stop,
            "method": " | ".join(method_parts),
            "distance_pips": stop_pips,
        }


sl_calculator = StopLossCalculator(config)


# ============================================================================
# TRADING IDEAS ENGINE
# ============================================================================

def generate_trading_ideas(data_by_timeframe, macro, dxy_by_timeframe):
    """Generate trading ideas using multi-timeframe analysis"""
    ideas = []
    
    for pair_name in config.assets.keys():
        # Get data for all timeframes
        df_daily = data_by_timeframe.get('Daily', {}).get(pair_name, pd.DataFrame())
        df_4h = data_by_timeframe.get('4 Hour', {}).get(pair_name, pd.DataFrame())
        df_1h = data_by_timeframe.get('Hourly', {}).get(pair_name, pd.DataFrame())
        df_15m = data_by_timeframe.get('15 Minute', {}).get(pair_name, pd.DataFrame())
        
        # Check data availability
        daily_ok = not df_daily.empty and len(df_daily) >= 20
        h4_ok = not df_4h.empty and len(df_4h) >= 20
        h1_ok = not df_1h.empty and len(df_1h) >= 20
        m15_ok = not df_15m.empty and len(df_15m) >= 20
        
        if not (daily_ok and h4_ok and h1_ok and m15_ok):
            continue
        
        idea = analyze_multi_timeframe_simple(df_daily, df_4h, df_1h, df_15m, pair_name)
        
        if idea and idea['bias'] != 'Neutral':
            ideas.append(idea)
    
    # Sort by conviction and strength
    ideas.sort(key=lambda x: (x['conviction'] == 'High', x['strength_score']), reverse=True)
    
    return ideas


def analyze_multi_timeframe_simple(df_daily, df_4h, df_1h, df_15m, pair_name):
    """Simplified multi-timeframe analysis with proper TP levels"""
    
    # Add indicators
    df_daily = analyzer.add_indicators(df_daily)
    df_4h = analyzer.add_indicators(df_4h)
    df_1h = analyzer.add_indicators(df_1h)
    df_15m = analyzer.add_indicators(df_15m)
    
    # Get latest values
    daily = df_daily.iloc[-1]
    four_hour = df_4h.iloc[-1]
    hourly = df_1h.iloc[-1]
    fifteen_min = df_15m.iloc[-1]
    
    # Simple trend determination
    daily_trend = 'Long' if daily['Close'] > daily.get('EMA_20', daily['Close']) else 'Short'
    
    # RSI signals
    daily_rsi = daily.get('RSI', 50)
    
    # Count signals
    long_signals = 0
    short_signals = 0
    reasons = []
    
    # Daily analysis
    if daily_trend == 'Long':
        long_signals += 2
        reasons.append(f"Daily: Bullish EMA alignment")
    else:
        short_signals += 2
        reasons.append(f"Daily: Bearish EMA alignment")
    
    if not pd.isna(daily_rsi):
        if daily_rsi < 40:
            long_signals += 1
            reasons.append(f"Daily RSI oversold ({daily_rsi:.1f})")
        elif daily_rsi > 60:
            short_signals += 1
            reasons.append(f"Daily RSI overbought ({daily_rsi:.1f})")
    
    # ADX check
    daily_adx = daily.get('ADX', 0)
    if not pd.isna(daily_adx) and daily_adx > config.adx_trend_min:
        if daily_trend == 'Long':
            long_signals += 1
        else:
            short_signals += 1
        reasons.append(f"Strong trend (ADX={daily_adx:.1f})")
    
    # Determine final bias
    if long_signals > short_signals:
        final_bias = 'Long'
        strength = long_signals
    elif short_signals > long_signals:
        final_bias = 'Short'
        strength = short_signals
    else:
        return None
    
    # Get entry signal
    entry_signal = entry_generator.get_entry_signal(df_15m, final_bias)
    
    # Conviction
    if strength >= 4:
        conviction = "High"
    elif strength >= 2:
        conviction = "Medium"
    else:
        conviction = "Low"
    
    # Calculate price levels
    atr = fifteen_min.get('ATR', fifteen_min['Close'] * 0.005)
    if pd.isna(atr) or atr <= 0:
        atr = fifteen_min['Close'] * 0.005
    
    current_price = fifteen_min['Close']
    
    # Calculate resistance and support levels for better TP placement
    resistance_20 = daily.get('Resistance_20', current_price * 1.02)
    support_20 = daily.get('Support_20', current_price * 0.98)
    
    # Stop loss
    sl_result = sl_calculator.calculate(
        df=df_1h,
        pair=pair_name,
        bias=final_bias,
        current_price=current_price,
        atr=atr,
        lookback=20
    )
    stop_loss = sl_result["stop"]
    stop_loss_method = sl_result["method"]
    stop_loss_pips = sl_result["distance_pips"]
    
    if final_bias == 'Long':
        entry = current_price
        
        # TP1: 1.5x ATR or first resistance, whichever is closer
        tp1_atr = current_price + (atr * 1.5)
        tp1 = min(tp1_atr, resistance_20) if resistance_20 > current_price else tp1_atr
        
        # TP2: 3x ATR
        tp2 = current_price + (atr * 3.0)
        
        risk_reward_1 = (tp1 - entry) / (entry - stop_loss) if (entry - stop_loss) > 0 else 0
        risk_reward_2 = (tp2 - entry) / (entry - stop_loss) if (entry - stop_loss) > 0 else 0
        
    else:  # Short
        entry = current_price
        
        # TP1: 1.5x ATR or first support, whichever is closer
        tp1_atr = current_price - (atr * 1.5)
        tp1 = max(tp1_atr, support_20) if support_20 < current_price else tp1_atr
        
        # TP2: 3x ATR
        tp2 = current_price - (atr * 3.0)
        
        risk_reward_1 = (entry - tp1) / (stop_loss - entry) if (stop_loss - entry) > 0 else 0
        risk_reward_2 = (entry - tp2) / (stop_loss - entry) if (stop_loss - entry) > 0 else 0
    
    # Build thesis
    thesis = " | ".join(reasons)
    if entry_signal and entry_signal['signal'] != 0:
        thesis += f" | Entry: {', '.join(entry_signal['reasons'][:2])}"
    
    return {
        "pair": pair_name,
        "bias": final_bias,
        "conviction": conviction,
        "strength_score": strength,
        "thesis": thesis,
        "entry": entry,
        "take_profit_1": tp1,
        "take_profit_2": tp2,
        "stop_loss": stop_loss,
        "stop_loss_method": stop_loss_method,
        "stop_loss_pips": stop_loss_pips,
        "risk_reward_1": risk_reward_1,
        "risk_reward_2": risk_reward_2,
        "atr": atr,
        "entry_signal": entry_signal,
    }


# ============================================================================
# DATA LOADING
# ============================================================================

@st.cache_data(ttl=config.cache_ttl)
def load_all_timeframes() -> Tuple[Dict, Dict]:
    """Load data for all timeframes"""
    data_by_timeframe = {tf: {} for tf in config.timeframes.keys()}
    dxy_by_timeframe = {}
    
    # Load DXY
    for tf_name, tf_config in config.timeframes.items():
        try:
            dxy_df = fetch_data(config.dxy_symbol, tf_config["interval"], tf_config["period"])
            if not dxy_df.empty:
                dxy_by_timeframe[tf_name] = dxy_df
        except Exception as e:
            logger.warning(f"Failed to load DXY ({tf_name}): {e}")
    
    # Load forex pairs
    progress_bar = st.progress(0)
    total_pairs = len(config.assets) * len(config.timeframes)
    current = 0
    
    for tf_name, tf_config in config.timeframes.items():
        for pair_name, symbol in config.assets.items():
            try:
                df = fetch_data(symbol, tf_config["interval"], tf_config["period"])
                if not df.empty:
                    data_by_timeframe[tf_name][pair_name] = df
                current += 1
                progress_bar.progress(current / total_pairs)
            except Exception as e:
                logger.warning(f"Failed to load {pair_name} ({tf_name}): {e}")
                current += 1
                continue
    
    progress_bar.empty()
    return data_by_timeframe, dxy_by_timeframe


# ============================================================================
# UI COMPONENTS
# ============================================================================

def render_sidebar() -> str:
    """Render sidebar with settings"""
    with st.sidebar:
        st.header("⚙️ Dashboard Settings")
        selected_timeframe = st.selectbox(
            "Default Chart Timeframe",
            ["Daily", "4 Hour", "Hourly", "15 Minute"]
        )
        st.divider()
        
        st.header("🎯 Strategy Parameters")
        st.info(f"""
        **Entry Conditions:**
        - Stochastic crossover
        - RSI confirmation
        - Bollinger Band touch
        
        **Stop Loss Method:**
        - Structure + ATR validation
        
        **Risk:**
        - Risk per trade: {config.risk_per_trade*100}%
        - Min R/R: {config.min_rr}:1
        """)
        st.divider()
        
        st.caption(f"Updated: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
    
    return selected_timeframe


def render_kpis(daily_data: Dict):
    """Render KPI metrics"""
    cols = st.columns(4)
    pairs = ["EUR/USD", "GBP/USD", "USD/JPY", "AUD/USD"]
    
    for i, pair in enumerate(pairs):
        if i < len(cols):
            with cols[i]:
                df = daily_data.get(pair)
                if df is not None and not df.empty and len(df) > 0:
                    price = df['Close'].iloc[-1]
                    if len(df) > 1:
                        change = df['Close'].pct_change().iloc[-1] * 100
                    else:
                        change = 0
                    st.metric(pair, f"{price:.4f}", f"{change:+.2f}%")


# ============================================================================
# MAIN APPLICATION
# ============================================================================

def main():
    """Main application entry point"""
    
    st.title("💹 Forex Dashboard Pro")
    st.caption("Multi-Timeframe Analysis with 15-Minute Entry Signals")
    
    # Sidebar
    selected_timeframe = render_sidebar()
    
    # Load data
    with st.spinner("Loading market data..."):
        data_by_timeframe, dxy_by_timeframe = load_all_timeframes()
    
    macro = get_macro_data()
    daily_data = data_by_timeframe.get('Daily', {})
    
    # KPIs
    if daily_data:
        render_kpis(daily_data)
    
    # Tabs
    tab1, tab2, tab3, tab4 = st.tabs([
        "📊 Overview",
        "📈 Technical Chart",
        "⏱️ 15-Minute Entry",
        "🎯 Trading Ideas"
    ])
    
    # Overview Tab
    with tab1:
        st.subheader("Market Overview")
        
        # Available pairs summary
        if daily_data:
            st.write("### Available Pairs")
            available = []
            for pair, df in daily_data.items():
                if not df.empty and len(df) > 0:
                    price = df['Close'].iloc[-1]
                    available.append({"Pair": pair, "Price": price, "Data Points": len(df)})
            
            if available:
                st.dataframe(pd.DataFrame(available), use_container_width=True)
        else:
            st.warning("No data available. Please check your internet connection.")
    
    # Technical Chart Tab
    with tab2:
        st.subheader("Technical Analysis Chart")
        available_pairs = [p for p in daily_data.keys() if not daily_data[p].empty]
        
        if available_pairs:
            col1, col2 = st.columns(2)
            with col1:
                pair = st.selectbox("Select Pair", available_pairs, key="chart_pair")
            with col2:
                tf = st.selectbox("Timeframe", list(config.timeframes.keys()), key="chart_tf")
            
            df = data_by_timeframe[tf].get(pair, pd.DataFrame())
            if not df.empty and len(df) > 0:
                df = analyzer.add_indicators(df)
                
                # Create chart
                fig = make_subplots(
                    rows=2, cols=1,
                    shared_xaxes=True,
                    vertical_spacing=0.05,
                    row_heights=[0.7, 0.3],
                    subplot_titles=(f'{pair} - {tf}', 'RSI')
                )
                
                # Candlestick
                fig.add_trace(go.Candlestick(
                    x=df.index, open=df['Open'], high=df['High'],
                    low=df['Low'], close=df['Close'], name="Price"
                ), row=1, col=1)
                
                # EMAs
                if 'EMA_20' in df.columns:
                    fig.add_trace(go.Scatter(x=df.index, y=df['EMA_20'], 
                                             name="EMA 20", line=dict(color='orange', width=1)), row=1, col=1)
                if 'EMA_50' in df.columns:
                    fig.add_trace(go.Scatter(x=df.index, y=df['EMA_50'], 
                                             name="EMA 50", line=dict(color='blue', width=1)), row=1, col=1)
                
                # Bollinger Bands
                if 'BB_Upper' in df.columns:
                    fig.add_trace(go.Scatter(x=df.index, y=df['BB_Upper'], 
                                             name="BB Upper", line=dict(color='gray', dash='dash')), row=1, col=1)
                    fig.add_trace(go.Scatter(x=df.index, y=df['BB_Lower'], 
                                             name="BB Lower", line=dict(color='gray', dash='dash')), row=1, col=1)
                
                # RSI
                if 'RSI' in df.columns:
                    fig.add_trace(go.Scatter(x=df.index, y=df['RSI'], 
                                             name="RSI", line=dict(color='purple')), row=2, col=1)
                    fig.add_hline(y=70, line_dash="dash", line_color="red", row=2, col=1)
                    fig.add_hline(y=30, line_dash="dash", line_color="green", row=2, col=1)
                
                fig.update_layout(height=600, showlegend=True)
                st.plotly_chart(fig, use_container_width=True)
                
                # Current indicators
                last = df.iloc[-1]
                cols = st.columns(4)
                with cols[0]:
                    st.metric("RSI", f"{last.get('RSI', 0):.1f}")
                with cols[1]:
                    st.metric("ADX", f"{last.get('ADX', 0):.1f}")
                with cols[2]:
                    st.metric("ATR", f"{last.get('ATR', 0):.5f}")
                with cols[3]:
                    st.metric("Stoch K", f"{last.get('Stoch_K', 0):.1f}")
            else:
                st.warning(f"No data available for {pair}")
        else:
            st.warning("No data available. Check your internet connection.")
    
    # 15-Minute Entry Tab
    with tab3:
        st.subheader("⏱️ 15-Minute Entry Signals")
        
        available_pairs = [p for p in daily_data.keys() if not daily_data[p].empty]
        if available_pairs:
            pair_entry = st.selectbox("Select Pair", available_pairs, key="entry_pair")
            
            df_15m = data_by_timeframe.get('15 Minute', {}).get(pair_entry, pd.DataFrame())
            df_daily = data_by_timeframe.get('Daily', {}).get(pair_entry, pd.DataFrame())
            
            if not df_15m.empty and not df_daily.empty:
                # Determine trend bias
                daily = analyzer.add_indicators(df_daily).iloc[-1]
                if daily.get('ADX', 0) > config.adx_trend_min:
                    trend_bias = 'Long' if daily['Close'] > daily.get('EMA_20', daily['Close']) else 'Short'
                else:
                    trend_bias = 'Neutral'
                
                st.write(f"**Trend Bias:** {trend_bias}")
                
                # Get entry signal
                entry_signal = entry_generator.get_entry_signal(df_15m, trend_bias)
                
                # Display
                col1, col2, col3 = st.columns(3)
                with col1:
                    if entry_signal['signal'] == 1:
                        st.success("### 🟢 LONG SIGNAL")
                    elif entry_signal['signal'] == -1:
                        st.error("### 🔴 SHORT SIGNAL")
                    else:
                        st.info("### ⚪ NO SIGNAL")
                with col2:
                    st.metric("Confidence", f"{entry_signal['confidence']}/5")
                with col3:
                    st.metric("Price", f"{entry_signal['price']:.5f}")
                
                if entry_signal['reasons']:
                    st.write("**Conditions Met:**")
                    for reason in entry_signal['reasons']:
                        st.success(f"✅ {reason}")
            else:
                st.warning("Insufficient data for 15-minute analysis")
    
    # Trading Ideas Tab
    with tab4:
        st.subheader("🎯 Trading Ideas")
        st.caption("Multi-timeframe analysis with entry signals and take-profit levels")
        
        if st.button("🔄 Generate Trading Ideas", type="primary", key="gen_ideas"):
            with st.spinner("Analyzing all pairs across multiple timeframes..."):
                ideas = generate_trading_ideas(data_by_timeframe, macro, dxy_by_timeframe)
            
            if ideas:
                st.success(f"✅ Generated {len(ideas)} trading ideas")
                
                # Summary metrics
                col1, col2, col3, col4 = st.columns(4)
                with col1:
                    st.metric("Total Ideas", len(ideas))
                with col2:
                    st.metric("Long", sum(1 for i in ideas if i["bias"] == "Long"))
                with col3:
                    st.metric("Short", sum(1 for i in ideas if i["bias"] == "Short"))
                with col4:
                    high_conv = sum(1 for i in ideas if i["conviction"] == "High")
                    st.metric("High Conviction", high_conv)
                
                st.divider()
                
                # Display each idea
                for idx, idea in enumerate(ideas):
                    with st.container():
                        # Header with bias indicator
                        if idea["bias"] == "Long":
                            st.success(f"### {idx+1}. {idea['pair']} - LONG 📈")
                        else:
                            st.error(f"### {idx+1}. {idea['pair']} - SHORT 📉")
                        
                        # Conviction and strength
                        col_meta1, col_meta2, col_meta3 = st.columns(3)
                        with col_meta1:
                            st.caption(f"**Conviction:** {idea['conviction']}")
                        with col_meta2:
                            st.caption(f"**Strength Score:** {idea['strength_score']}/4")
                        with col_meta3:
                            st.caption(f"**ATR:** {idea['atr']:.5f}")
                        
                        # Thesis
                        st.markdown(f"**📝 Thesis:** {idea['thesis']}")
                        
                        # Price Levels
                        st.markdown("**💰 Price Levels:**")
                        
                        cols = st.columns(5)
                        with cols[0]:
                            st.metric("Entry", f"{idea['entry']:.5f}")
                        with cols[1]:
                            st.metric("TP1", f"{idea['take_profit_1']:.5f}", 
                                     delta=f"R:R 1:{idea['risk_reward_1']:.2f}")
                        with cols[2]:
                            st.metric("TP2", f"{idea['take_profit_2']:.5f}", 
                                     delta=f"R:R 1:{idea['risk_reward_2']:.2f}")
                        with cols[3]:
                            st.metric("Stop Loss", f"{idea['stop_loss']:.5f}")
                        with cols[4]:
                            if idea["bias"] == "Long":
                                risk_pct = ((idea['entry'] - idea['stop_loss']) / idea['entry']) * 100
                            else:
                                risk_pct = ((idea['stop_loss'] - idea['entry']) / idea['entry']) * 100
                            st.metric("Risk %", f"{risk_pct:.2f}%")
                        
                        # Stop loss details
                        st.caption(f"🛡️ **Stop Method:** {idea['stop_loss_method']} | **Distance:** {idea['stop_loss_pips']} pips")
                        
                        # Entry signal details if available
                        if idea['entry_signal'] and idea['entry_signal']['signal'] != 0:
                            with st.expander("📊 Entry Signal Details"):
                                st.write(f"**Confidence:** {idea['entry_signal']['confidence']}/5")
                                st.write(f"**Stochastic K:** {idea['entry_signal']['stoch_k']:.1f}")
                                st.write(f"**Stochastic D:** {idea['entry_signal']['stoch_d']:.1f}")
                                st.write(f"**RSI:** {idea['entry_signal']['rsi']:.1f}")
                                st.write("**Reasons:**")
                                for reason in idea['entry_signal']['reasons']:
                                    st.write(f"  • {reason}")
                        
                        st.divider()
                
                # Export option
                if len(ideas) > 0:
                    export_df = pd.DataFrame([{
                        "Pair": i["pair"],
                        "Bias": i["bias"],
                        "Conviction": i["conviction"],
                        "Entry": i["entry"],
                        "TP1": i["take_profit_1"],
                        "TP2": i["take_profit_2"],
                        "Stop Loss": i["stop_loss"],
                        "R:R (TP1)": i["risk_reward_1"],
                        "R:R (TP2)": i["risk_reward_2"],
                        "Stop Pips": i["stop_loss_pips"],
                        "Thesis": i["thesis"]
                    } for i in ideas])
                    
                    st.download_button(
                        label="📥 Download Trading Ideas (CSV)",
                        data=export_df.to_csv(index=False),
                        file_name=f"trading_ideas_{datetime.now().strftime('%Y%m%d_%H%M')}.csv",
                        mime="text/csv"
                    )
                
            else:
                st.warning("⚠️ No trading ideas generated.")
                st.markdown("""
                This could be due to:
                - Insufficient data for some pairs
                - No clear trends detected
                - Market conditions not meeting criteria
                
                Try refreshing the page or checking individual charts.
                """)


if __name__ == "__main__":
    try:
        main()
    except Exception as e:
        logger.error(f"Application error: {e}\n{traceback.format_exc()}")
        st.error(f"An error occurred: {str(e)}")
        st.stop()