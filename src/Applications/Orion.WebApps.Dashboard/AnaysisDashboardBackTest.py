import streamlit as st
import pandas as pd
import numpy as np
import plotly.graph_objects as go
from plotly.subplots import make_subplots
from datetime import datetime, timedelta
from pathlib import Path
import warnings
warnings.filterwarnings("ignore")

# ─── PAGE CONFIG ──────────────────────────────────────────────────────────────
st.set_page_config(
    page_title="EUR/USD Backtest Lab",
    page_icon="📈",
    layout="wide",
    initial_sidebar_state="expanded",
)

# ─── CUSTOM CSS ───────────────────────────────────────────────────────────────
st.markdown("""
<style>
    .main { background-color: #0e1117; }
    .stApp { background-color: #0e1117; }
    .metric-card {
        background: #1a1d27;
        border: 1px solid #2d3148;
        border-radius: 10px;
        padding: 14px 18px;
        text-align: center;
    }
    .metric-label { font-size: 11px; color: #8b8fa8; text-transform: uppercase; letter-spacing: 1px; }
    .metric-value { font-size: 22px; font-weight: 700; margin-top: 4px; }
    .bull { color: #26a69a; }
    .bear { color: #ef5350; }
    .neutral { color: #ffa726; }
    .badge-bull { background:#1b3a38; color:#26a69a; padding:3px 10px; border-radius:12px; font-size:13px; font-weight:700; }
    .badge-bear { background:#3a1b1b; color:#ef5350; padding:3px 10px; border-radius:12px; font-size:13px; font-weight:700; }
    .badge-neutral { background:#3a2f1a; color:#ffa726; padding:3px 10px; border-radius:12px; font-size:13px; font-weight:700; }
    .idea-card {
        background: #1a1d27;
        border-left: 3px solid #26a69a;
        border-radius: 6px;
        padding: 10px 14px;
        margin-bottom: 8px;
        font-size: 13px;
    }
    .idea-card.bear { border-left-color: #ef5350; }
    .idea-card.neutral { border-left-color: #ffa726; }
    h1, h2, h3 { color: #e0e0e0 !important; }
    .stSelectbox label, .stDateInput label, .stSlider label { color: #8b8fa8; }
</style>
""", unsafe_allow_html=True)

# ─── DATA LOADING ─────────────────────────────────────────────────────────────
DATA_PATH = "C:\\Users\\kmokhethi\\Downloads\\HISTDATA_COM_NT_EURUSD_M12025\\DAT_NT_EURUSD_M1_2025.csv"

DATA_PATH = Path("C:/Users/kmokhethi/Downloads/HISTDATA_COM_NT_EURUSD_M12025/DAT_NT_EURUSD_M1_2025.csv")

@st.cache_data(show_spinner="Loading EUR/USD M1 data…")
def load_data():
    df = pd.read_csv(
        DATA_PATH,
        sep=";",
        header=None,
        names=["datetime", "open", "high", "low", "close", "volume"],
        parse_dates=["datetime"],
        date_format="%Y%m%d %H%M%S",
    )
    df = df.dropna(subset=["datetime"])
    df = df.sort_values("datetime").reset_index(drop=True)
    df["date"] = df["datetime"].dt.date
    return df

@st.cache_data(show_spinner=False)
def resample(df_day, tf):
    rules = {"M1": "1min", "M5": "5min", "M15": "15min", "M30": "30min", "H1": "1h"}
    rule = rules[tf]
    r = df_day.set_index("datetime").resample(rule).agg(
        open=("open", "first"),
        high=("high", "max"),
        low=("low", "min"),
        close=("close", "last"),
        volume=("volume", "sum"),
    ).dropna()
    r = r.reset_index()
    return r

def add_indicators(df):
    c = df["close"].values
    n = len(c)
    if n < 3:
        df["ema9"] = c
        df["ema21"] = c
        df["ema50"] = c
        df["rsi"] = 50.0
        df["macd"] = 0.0
        df["signal"] = 0.0
        df["bb_upper"] = c
        df["bb_lower"] = c
        df["bb_mid"] = c
        return df

    def ema(arr, p):
        out = np.full(len(arr), np.nan)
        k = 2 / (p + 1)
        # seed with SMA
        start = min(p - 1, len(arr) - 1)
        out[start] = np.mean(arr[:start + 1])
        for i in range(start + 1, len(arr)):
            out[i] = arr[i] * k + out[i - 1] * (1 - k)
        return out

    df["ema9"] = ema(c, 9)
    df["ema21"] = ema(c, 21)
    df["ema50"] = ema(c, 50)

    # RSI
    delta = np.diff(c, prepend=c[0])
    gain = np.where(delta > 0, delta, 0.0)
    loss = np.where(delta < 0, -delta, 0.0)
    period = 14
    avg_gain = np.full(n, np.nan)
    avg_loss = np.full(n, np.nan)
    if n >= period:
        avg_gain[period - 1] = np.mean(gain[:period])
        avg_loss[period - 1] = np.mean(loss[:period])
        for i in range(period, n):
            avg_gain[i] = (avg_gain[i - 1] * (period - 1) + gain[i]) / period
            avg_loss[i] = (avg_loss[i - 1] * (period - 1) + loss[i]) / period
        with np.errstate(divide="ignore", invalid="ignore"):
            rs = np.where(avg_loss == 0, 100.0, avg_gain / avg_loss)
        df["rsi"] = np.where(np.isnan(avg_gain), np.nan, 100 - 100 / (1 + rs))
    else:
        df["rsi"] = np.nan

    # MACD
    macd_line = ema(c, 12) - ema(c, 26)
    signal_line = ema(macd_line, 9)
    df["macd"] = macd_line
    df["signal"] = signal_line

    # Bollinger Bands (20)
    bb_p = 20
    bb_mid = pd.Series(c).rolling(bb_p).mean().values
    bb_std = pd.Series(c).rolling(bb_p).std().values
    df["bb_mid"] = bb_mid
    df["bb_upper"] = bb_mid + 2 * bb_std
    df["bb_lower"] = bb_mid - 2 * bb_std

    return df

def get_sessions(date):
    base = pd.Timestamp(date)
    sessions = {
        "Asian":  (base.replace(hour=0,  minute=0),  base.replace(hour=8,  minute=59)),
        "London": (base.replace(hour=7,  minute=0),  base.replace(hour=15, minute=59)),
        "NY":     (base.replace(hour=13, minute=0),  base.replace(hour=21, minute=59)),
    }
    return sessions

def session_analysis(df_day):
    results = {}
    for name, (s, e) in get_sessions(df_day["datetime"].dt.date.iloc[0]).items():
        seg = df_day[(df_day["datetime"] >= s) & (df_day["datetime"] <= e)]
        if len(seg) == 0:
            results[name] = None
            continue
        o = seg["open"].iloc[0]
        c = seg["close"].iloc[-1]
        h = seg["high"].max()
        l = seg["low"].min()
        direction = "🟢 Bull" if c > o else ("🔴 Bear" if c < o else "⚪ Flat")
        results[name] = {
            "open": o, "close": c, "high": h, "low": l,
            "range_pips": round((h - l) * 10000, 1),
            "direction": direction,
        }
    return results

def compute_daily_bias(df_day):
    if len(df_day) < 5:
        return "Neutral", 50.0
    # Use EMA trend + session direction
    opens = df_day["open"].iloc[0]
    closes = df_day["close"].iloc[-1]
    highs = df_day["high"].max()
    lows = df_day["low"].min()
    mid = (highs + lows) / 2
    pct_change = (closes - opens) / opens * 100

    # Check higher highs / lower lows structure
    h4 = df_day.set_index("datetime").resample("4h").agg(
        open=("open","first"), high=("high","max"),
        low=("low","min"), close=("close","last")
    ).dropna()

    bull_score = 0
    if closes > opens: bull_score += 2
    if closes > mid: bull_score += 1
    if len(h4) >= 2:
        if h4["close"].iloc[-1] > h4["close"].iloc[0]: bull_score += 2
        if h4["high"].iloc[-1] > h4["high"].iloc[-2]: bull_score += 1
        if h4["low"].iloc[-1] > h4["low"].iloc[-2]: bull_score += 1
    if pct_change > 0.1: bull_score += 1

    total = 8
    bull_pct = (bull_score / total) * 100
    if bull_pct >= 62:
        return "Bullish", bull_pct
    elif bull_pct <= 38:
        return "Bearish", 100 - bull_pct
    else:
        return "Neutral", 50.0

def generate_trading_ideas(df_day, bias, indicators):
    ideas = []
    c = df_day["close"].iloc[-1]
    h = df_day["high"].max()
    l = df_day["low"].min()
    rng = h - l
    mid = (h + l) / 2
    pp = (h + l + c) / 3   # pivot point
    r1 = 2 * pp - l
    r2 = pp + rng
    s1 = 2 * pp - h
    s2 = pp - rng

    last = indicators.iloc[-1] if len(indicators) > 0 else None
    rsi_val = last["rsi"] if last is not None and not np.isnan(last.get("rsi", np.nan)) else None
    ema9 = last["ema9"] if last is not None and not np.isnan(last.get("ema9", np.nan)) else None
    ema21 = last["ema21"] if last is not None and not np.isnan(last.get("ema21", np.nan)) else None

    # Key levels
    ideas.append({"type": "level", "icon": "📍", "text": f"Pivot Point: {pp:.5f} | R1: {r1:.5f} | R2: {r2:.5f}"})
    ideas.append({"type": "level", "icon": "📍", "text": f"S1: {s1:.5f} | S2: {s2:.5f} | Mid Range: {mid:.5f}"})

    # Bias-based ideas
    if bias == "Bullish":
        ideas.append({"type": "bull", "icon": "🟢", "text": f"BUY on pullback to S1 ({s1:.5f}) or mid-range ({mid:.5f}). Target R1 ({r1:.5f})."})
        if ema9 and ema21 and ema9 > ema21:
            ideas.append({"type": "bull", "icon": "🟢", "text": f"EMA9 > EMA21 — trend aligned. Look for long entries on dips."})
        if rsi_val and rsi_val < 40:
            ideas.append({"type": "bull", "icon": "🟢", "text": f"RSI at {rsi_val:.1f} — oversold on bullish day. Reversal long opportunity."})
    elif bias == "Bearish":
        ideas.append({"type": "bear", "icon": "🔴", "text": f"SELL on bounce to R1 ({r1:.5f}) or mid-range ({mid:.5f}). Target S1 ({s1:.5f})."})
        if ema9 and ema21 and ema9 < ema21:
            ideas.append({"type": "bear", "icon": "🔴", "text": f"EMA9 < EMA21 — bearish trend confirmed. Look for short entries on bounces."})
        if rsi_val and rsi_val > 60:
            ideas.append({"type": "bear", "icon": "🔴", "text": f"RSI at {rsi_val:.1f} — overbought on bearish day. Reversal short opportunity."})
    else:
        ideas.append({"type": "neutral", "icon": "⚪", "text": f"Range day. BUY near lows ({l:.5f}) / SELL near highs ({h:.5f}). Fade the extremes."})
        ideas.append({"type": "neutral", "icon": "⚪", "text": f"Wait for breakout above {h:.5f} or breakdown below {l:.5f} for direction."})

    if rsi_val:
        if rsi_val > 70:
            ideas.append({"type": "bear", "icon": "⚠️", "text": f"RSI overbought ({rsi_val:.1f}). Caution on longs — watch for reversal."})
        elif rsi_val < 30:
            ideas.append({"type": "bull", "icon": "⚠️", "text": f"RSI oversold ({rsi_val:.1f}). Caution on shorts — watch for bounce."})

    return ideas, {"pp": pp, "r1": r1, "r2": r2, "s1": s1, "s2": s2}

def build_chart(df_tf, bias, levels, tf_label, show_ema, show_bb, show_sessions, selected_date):
    df_tf = add_indicators(df_tf.copy())

    colors = {"Bullish": "#26a69a", "Bearish": "#ef5350", "Neutral": "#ffa726"}
    bias_color = colors.get(bias, "#ffa726")

    rows = 3
    row_heights = [0.55, 0.23, 0.22]
    subplot_titles = [f"EUR/USD {tf_label} — {selected_date}", "RSI (14)", "MACD"]
    fig = make_subplots(
        rows=rows, cols=1,
        shared_xaxes=True,
        row_heights=row_heights,
        vertical_spacing=0.04,
        subplot_titles=subplot_titles,
    )

    # ── Candlestick ──
    fig.add_trace(go.Candlestick(
        x=df_tf["datetime"],
        open=df_tf["open"], high=df_tf["high"],
        low=df_tf["low"], close=df_tf["close"],
        increasing_line_color="#26a69a", decreasing_line_color="#ef5350",
        increasing_fillcolor="#26a69a", decreasing_fillcolor="#ef5350",
        name="Price", showlegend=False,
        whiskerwidth=0.2,
    ), row=1, col=1)

    if show_ema:
        for col_name, color, label in [("ema9", "#f9d71c", "EMA9"), ("ema21", "#ff9800", "EMA21"), ("ema50", "#ab47bc", "EMA50")]:
            fig.add_trace(go.Scatter(
                x=df_tf["datetime"], y=df_tf[col_name],
                line=dict(color=color, width=1.2),
                name=label, showlegend=True,
            ), row=1, col=1)

    if show_bb:
        fig.add_trace(go.Scatter(
            x=df_tf["datetime"], y=df_tf["bb_upper"],
            line=dict(color="rgba(100,149,237,0.5)", width=1, dash="dot"),
            name="BB Upper", showlegend=False,
        ), row=1, col=1)
        fig.add_trace(go.Scatter(
            x=df_tf["datetime"], y=df_tf["bb_lower"],
            line=dict(color="rgba(100,149,237,0.5)", width=1, dash="dot"),
            fill="tonexty", fillcolor="rgba(100,149,237,0.04)",
            name="BB Lower", showlegend=False,
        ), row=1, col=1)
        fig.add_trace(go.Scatter(
            x=df_tf["datetime"], y=df_tf["bb_mid"],
            line=dict(color="rgba(100,149,237,0.3)", width=0.8, dash="dash"),
            name="BB Mid", showlegend=False,
        ), row=1, col=1)

    # Key Levels
    x_range = [df_tf["datetime"].min(), df_tf["datetime"].max()]
    level_colors = {"pp": "#9e9e9e", "r1": "#ef5350", "r2": "#e53935", "s1": "#26a69a", "s2": "#00897b"}
    for key, price in levels.items():
        fig.add_shape(type="line", x0=x_range[0], x1=x_range[1], y0=price, y1=price,
                      line=dict(color=level_colors[key], width=0.8, dash="dot"), row=1, col=1)
        fig.add_annotation(x=x_range[1], y=price, text=key.upper(),
                           font=dict(size=9, color=level_colors[key]),
                           showarrow=False, xanchor="left", row=1, col=1)

    # Sessions
    if show_sessions:
        sess_colors = {"Asian": "rgba(255,193,7,0.06)", "London": "rgba(30,136,229,0.07)", "NY": "rgba(239,83,80,0.06)"}
        for name, (s, e) in get_sessions(selected_date).items():
            fig.add_vrect(x0=s, x1=e, fillcolor=sess_colors[name], layer="below",
                          line_width=0, annotation_text=name,
                          annotation_font=dict(size=9, color="#555"),
                          annotation_position="top left", row=1, col=1)

    # ── RSI ──
    fig.add_trace(go.Scatter(
        x=df_tf["datetime"], y=df_tf["rsi"],
        line=dict(color="#7986cb", width=1.5),
        name="RSI", showlegend=False,
    ), row=2, col=1)
    for lvl, color in [(70, "rgba(239,83,80,0.4)"), (30, "rgba(38,166,154,0.4)"), (50, "rgba(150,150,150,0.3)")]:
        fig.add_hline(y=lvl, line=dict(color=color, width=0.8, dash="dash"), row=2, col=1)
    fig.add_hrect(y0=70, y1=100, fillcolor="rgba(239,83,80,0.06)", layer="below", row=2, col=1)
    fig.add_hrect(y0=0, y1=30, fillcolor="rgba(38,166,154,0.06)", layer="below", row=2, col=1)

    # ── MACD ──
    macd_hist = df_tf["macd"] - df_tf["signal"]
    fig.add_trace(go.Bar(
        x=df_tf["datetime"], y=macd_hist,
        marker_color=np.where(macd_hist >= 0, "#26a69a", "#ef5350"),
        name="MACD Hist", showlegend=False, opacity=0.7,
    ), row=3, col=1)
    fig.add_trace(go.Scatter(
        x=df_tf["datetime"], y=df_tf["macd"],
        line=dict(color="#42a5f5", width=1.2), name="MACD", showlegend=False,
    ), row=3, col=1)
    fig.add_trace(go.Scatter(
        x=df_tf["datetime"], y=df_tf["signal"],
        line=dict(color="#ff7043", width=1.2), name="Signal", showlegend=False,
    ), row=3, col=1)

    fig.update_layout(
        height=720,
        plot_bgcolor="#0e1117",
        paper_bgcolor="#0e1117",
        font=dict(color="#c0c0c0", size=11),
        xaxis_rangeslider_visible=False,
        legend=dict(orientation="h", x=0, y=1.02, bgcolor="rgba(0,0,0,0)"),
        margin=dict(l=10, r=10, t=40, b=10),
        hovermode="x unified",
    )
    for i in range(1, rows + 1):
        fig.update_xaxes(
            gridcolor="#1e2130", zeroline=False,
            tickfont=dict(size=9), row=i, col=1,
        )
        fig.update_yaxes(
            gridcolor="#1e2130", zeroline=False,
            tickfont=dict(size=9), row=i, col=1,
        )

    return fig


# ─── MAIN ─────────────────────────────────────────────────────────────────────
def main():
    st.markdown("## 📊 EUR/USD Backtest Lab — 2025 M1 Data")

    df = load_data()
    all_dates = sorted(df["date"].unique())
    date_options = [str(d) for d in all_dates]

    # ── SIDEBAR ───────────────────────────────────────────────────────────────
    with st.sidebar:
        st.markdown("### ⚙️ Controls")
        selected_str = st.selectbox("📅 Select Trading Day", date_options, index=0)
        selected_date = pd.Timestamp(selected_str).date()

        tf = st.selectbox("⏱ Timeframe", ["M5", "M15", "M30", "H1", "M1"], index=1)

        st.markdown("---")
        st.markdown("**Overlays**")
        show_ema = st.checkbox("EMA (9 / 21 / 50)", value=True)
        show_bb = st.checkbox("Bollinger Bands (20)", value=False)
        show_sessions = st.checkbox("Session Shading", value=True)

        st.markdown("---")
        st.markdown("**Quick Navigate**")
        idx = date_options.index(selected_str)
        col1, col2 = st.columns(2)
        if col1.button("◀ Prev"):
            idx = max(0, idx - 1)
            selected_str = date_options[idx]
            selected_date = pd.Timestamp(selected_str).date()
        if col2.button("Next ▶"):
            idx = min(len(date_options) - 1, idx + 1)
            selected_str = date_options[idx]
            selected_date = pd.Timestamp(selected_str).date()

        st.markdown("---")
        st.caption(f"📦 {len(df):,} M1 candles loaded\n\n🗓 {len(all_dates)} trading days")

    # ── LOAD DAY DATA ─────────────────────────────────────────────────────────
    df_day = df[df["date"] == selected_date].copy()
    if df_day.empty:
        st.warning("No data for selected date.")
        return

    df_tf = resample(df_day, tf) if tf != "M1" else df_day.copy()
    df_tf_ind = add_indicators(df_tf.copy())

    bias, confidence = compute_daily_bias(df_day)
    sess = session_analysis(df_day)
    ideas, levels = generate_trading_ideas(df_day, bias, df_tf_ind)

    day_open  = df_day["open"].iloc[0]
    day_close = df_day["close"].iloc[-1]
    day_high  = df_day["high"].max()
    day_low   = df_day["low"].min()
    day_range = round((day_high - day_low) * 10000, 1)
    day_chg   = round((day_close - day_open) * 10000, 1)
    day_chg_pct = round((day_close - day_open) / day_open * 100, 3)
    chg_sign  = "+" if day_chg >= 0 else ""

    # ── HEADER ROW ────────────────────────────────────────────────────────────
    bias_badge = {
        "Bullish": '<span class="badge-bull">🟢 BULLISH</span>',
        "Bearish": '<span class="badge-bear">🔴 BEARISH</span>',
        "Neutral": '<span class="badge-neutral">⚪ NEUTRAL</span>',
    }[bias]
    st.markdown(
        f"<h3 style='margin-bottom:4px'>{selected_str} &nbsp; {bias_badge} &nbsp;"
        f"<span style='font-size:14px;color:#666'>Confidence {confidence:.0f}%</span></h3>",
        unsafe_allow_html=True,
    )

    # ── METRIC STRIP ──────────────────────────────────────────────────────────
    c1, c2, c3, c4, c5, c6 = st.columns(6)
    chg_cls = "bull" if day_chg >= 0 else "bear"

    def metric(col, label, value, cls=""):
        col.markdown(
            f'<div class="metric-card">'
            f'<div class="metric-label">{label}</div>'
            f'<div class="metric-value {cls}">{value}</div>'
            f'</div>', unsafe_allow_html=True
        )

    metric(c1, "Open",  f"{day_open:.5f}")
    metric(c2, "Close", f"{day_close:.5f}", chg_cls)
    metric(c3, "High",  f"{day_high:.5f}", "bull")
    metric(c4, "Low",   f"{day_low:.5f}", "bear")
    metric(c5, "Range", f"{day_range} pips")
    metric(c6, "Change", f"{chg_sign}{day_chg} ({chg_sign}{day_chg_pct}%)", chg_cls)

    st.markdown("<br>", unsafe_allow_html=True)

    # ── CHART + TRADING IDEAS ─────────────────────────────────────────────────
    chart_col, ideas_col = st.columns([3, 1])

    with chart_col:
        fig = build_chart(df_tf, bias, levels, tf, show_ema, show_bb, show_sessions, selected_date)
        st.plotly_chart(fig, use_container_width=True)

    with ideas_col:
        st.markdown("#### 💡 Trading Ideas")
        for idea in ideas:
            cls = {"bull": "", "bear": "bear", "neutral": "neutral", "level": "neutral"}.get(idea["type"], "")
            st.markdown(
                f'<div class="idea-card {cls}">{idea["icon"]} {idea["text"]}</div>',
                unsafe_allow_html=True,
            )

        st.markdown("#### 📐 Key Levels")
        lev_df = pd.DataFrame([
            {"Level": "R2", "Price": f"{levels['r2']:.5f}"},
            {"Level": "R1", "Price": f"{levels['r1']:.5f}"},
            {"Level": "PP", "Price": f"{levels['pp']:.5f}"},
            {"Level": "S1", "Price": f"{levels['s1']:.5f}"},
            {"Level": "S2", "Price": f"{levels['s2']:.5f}"},
        ])
        st.dataframe(lev_df, hide_index=True, use_container_width=True)

    # ── SESSION BREAKDOWN ─────────────────────────────────────────────────────
    st.markdown("#### 🌍 Session Breakdown")
    s_cols = st.columns(3)
    for i, (name, data) in enumerate(sess.items()):
        with s_cols[i]:
            if data is None:
                st.markdown(f'<div class="metric-card"><div class="metric-label">{name}</div><div style="color:#555">No data</div></div>', unsafe_allow_html=True)
            else:
                dir_color = "bull" if "Bull" in data["direction"] else ("bear" if "Bear" in data["direction"] else "neutral")
                st.markdown(
                    f'<div class="metric-card">'
                    f'<div class="metric-label">🌐 {name} Session</div>'
                    f'<div class="metric-value {dir_color}">{data["direction"]}</div>'
                    f'<div style="font-size:12px;color:#8b8fa8;margin-top:6px">'
                    f'Range: <b>{data["range_pips"]} pips</b><br>'
                    f'H: {data["high"]:.5f} · L: {data["low"]:.5f}'
                    f'</div></div>',
                    unsafe_allow_html=True,
                )

    # ── DAILY OVERVIEW TABLE ───────────────────────────────────────────────────
    st.markdown("---")
    with st.expander("📅 Full Month Overview — Daily Bias Table", expanded=False):
        month_options = sorted(set(str(d)[:7] for d in all_dates))
        sel_month = st.selectbox("Month", month_options, index=0, key="month_sel")
        month_dates = [d for d in all_dates if str(d).startswith(sel_month)]

        rows_data = []
        with st.spinner("Computing daily biases…"):
            for d in month_dates:
                ddf = df[df["date"] == d]
                if ddf.empty:
                    continue
                b, conf = compute_daily_bias(ddf)
                o = ddf["open"].iloc[0]
                c = ddf["close"].iloc[-1]
                h = ddf["high"].max()
                l = ddf["low"].min()
                chg = round((c - o) * 10000, 1)
                rng = round((h - l) * 10000, 1)
                rows_data.append({
                    "Date": str(d),
                    "Bias": b,
                    "Conf%": f"{conf:.0f}",
                    "Open": round(o, 5),
                    "Close": round(c, 5),
                    "High": round(h, 5),
                    "Low": round(l, 5),
                    "Chg (pips)": f"+{chg}" if chg >= 0 else str(chg),
                    "Range (pips)": rng,
                })

        if rows_data:
            overview_df = pd.DataFrame(rows_data)

            def style_bias(val):
                if val == "Bullish":  return "background-color:#1b3a38; color:#26a69a; font-weight:700"
                if val == "Bearish":  return "background-color:#3a1b1b; color:#ef5350; font-weight:700"
                return "background-color:#3a2f1a; color:#ffa726; font-weight:700"

            def style_chg(val):
                try:
                    v = float(str(val).replace("+", ""))
                    return "color:#26a69a" if v >= 0 else "color:#ef5350"
                except:
                    return ""

            styled = overview_df.style.map(style_bias, subset=["Bias"]).map(style_chg, subset=["Chg (pips)"])
            st.dataframe(styled, hide_index=True, use_container_width=True, height=420)

    # ── SIMPLE BACKTEST ────────────────────────────────────────────────────────
    st.markdown("---")
    with st.expander("🧪 Simple Strategy Backtest — EMA Cross (M15)", expanded=False):
        st.caption("**Rules:** BUY when EMA9 crosses above EMA21 on M15. SELL when EMA9 crosses below EMA21. Fixed 10-pip SL / 20-pip TP.")

        bt_month = st.selectbox("Backtest Month", month_options, index=0, key="bt_month")
        bt_dates = [d for d in all_dates if str(d).startswith(bt_month)]

        sl_pips = st.slider("Stop Loss (pips)", 5, 50, 10)
        tp_pips = st.slider("Take Profit (pips)", 5, 100, 20)

        trades = []
        with st.spinner("Running backtest…"):
            for d in bt_dates:
                ddf = df[df["date"] == d]
                if len(ddf) < 50:
                    continue
                m15 = resample(ddf, "M15")
                if len(m15) < 30:
                    continue
                m15 = add_indicators(m15)
                sl = sl_pips * 0.0001
                tp = tp_pips * 0.0001

                for i in range(1, len(m15)):
                    prev = m15.iloc[i - 1]
                    curr = m15.iloc[i]
                    if (np.isnan(prev["ema9"]) or np.isnan(prev["ema21"]) or
                            np.isnan(curr["ema9"]) or np.isnan(curr["ema21"])):
                        continue
                    # Bull cross
                    if prev["ema9"] <= prev["ema21"] and curr["ema9"] > curr["ema21"]:
                        entry = curr["close"]
                        hit_tp = any(m15.iloc[j]["high"] >= entry + tp for j in range(i + 1, min(i + 20, len(m15))))
                        hit_sl = any(m15.iloc[j]["low"] <= entry - sl for j in range(i + 1, min(i + 20, len(m15))))
                        result = "TP" if hit_tp and (not hit_sl or
                                 next((j for j in range(i+1, min(i+20, len(m15))) if m15.iloc[j]["high"] >= entry+tp), 999) <
                                 next((j for j in range(i+1, min(i+20, len(m15))) if m15.iloc[j]["low"] <= entry-sl), 999)) else "SL" if hit_sl else "OPEN"
                        pnl = tp_pips if result == "TP" else (-sl_pips if result == "SL" else 0)
                        trades.append({"Date": str(d), "Time": str(curr["datetime"]), "Dir": "LONG", "Entry": round(entry, 5), "Result": result, "PnL (pips)": pnl})

                    # Bear cross
                    elif prev["ema9"] >= prev["ema21"] and curr["ema9"] < curr["ema21"]:
                        entry = curr["close"]
                        hit_tp = any(m15.iloc[j]["low"] <= entry - tp for j in range(i + 1, min(i + 20, len(m15))))
                        hit_sl = any(m15.iloc[j]["high"] >= entry + sl for j in range(i + 1, min(i + 20, len(m15))))
                        result = "TP" if hit_tp and (not hit_sl or
                                 next((j for j in range(i+1, min(i+20, len(m15))) if m15.iloc[j]["low"] <= entry-tp), 999) <
                                 next((j for j in range(i+1, min(i+20, len(m15))) if m15.iloc[j]["high"] >= entry+sl), 999)) else "SL" if hit_sl else "OPEN"
                        pnl = tp_pips if result == "TP" else (-sl_pips if result == "SL" else 0)
                        trades.append({"Date": str(d), "Time": str(curr["datetime"]), "Dir": "SHORT", "Entry": round(entry, 5), "Result": result, "PnL (pips)": pnl})

        if trades:
            trade_df = pd.DataFrame(trades)
            completed = trade_df[trade_df["Result"] != "OPEN"]
            total_pnl = completed["PnL (pips)"].sum()
            wins = len(completed[completed["PnL (pips)"] > 0])
            losses = len(completed[completed["PnL (pips)"] < 0])
            wr = wins / len(completed) * 100 if len(completed) > 0 else 0

            mc1, mc2, mc3, mc4 = st.columns(4)
            metric(mc1, "Total Trades", len(completed))
            metric(mc2, "Win Rate", f"{wr:.1f}%", "bull" if wr > 50 else "bear")
            metric(mc3, "Total P&L", f"{'+' if total_pnl >= 0 else ''}{total_pnl:.0f} pips", "bull" if total_pnl >= 0 else "bear")
            metric(mc4, "W / L", f"{wins} / {losses}")

            st.markdown("<br>", unsafe_allow_html=True)

            # Equity curve
            completed2 = completed.copy().reset_index(drop=True)
            completed2["Cumulative PnL"] = completed2["PnL (pips)"].cumsum()
            eq_fig = go.Figure(go.Scatter(
                x=list(range(len(completed2))), y=completed2["Cumulative PnL"],
                fill="tozeroy",
                fillcolor="rgba(38,166,154,0.15)" if total_pnl >= 0 else "rgba(239,83,80,0.15)",
                line=dict(color="#26a69a" if total_pnl >= 0 else "#ef5350", width=2),
            ))
            eq_fig.update_layout(
                title="Equity Curve (pips)",
                height=260, plot_bgcolor="#0e1117", paper_bgcolor="#0e1117",
                font=dict(color="#c0c0c0"), margin=dict(l=10, r=10, t=30, b=10),
                xaxis=dict(gridcolor="#1e2130"), yaxis=dict(gridcolor="#1e2130"),
            )
            st.plotly_chart(eq_fig, use_container_width=True)

            def style_result(val):
                if val == "TP": return "color:#26a69a; font-weight:700"
                if val == "SL": return "color:#ef5350; font-weight:700"
                return "color:#ffa726"

            styled_trades = trade_df.style.map(style_result, subset=["Result"]).map(style_chg, subset=["PnL (pips)"])
            st.dataframe(styled_trades, hide_index=True, use_container_width=True, height=300)
        else:
            st.info("No signals generated for this period.")


if __name__ == "__main__":
    main()