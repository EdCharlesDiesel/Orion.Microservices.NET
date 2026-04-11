let priceChart = null;
let equityChart = null;

function renderCandlestickChart(elementId, data) {
    const ctx = document.getElementById(elementId);
    if (!ctx) return;

    if (priceChart) {
        priceChart.destroy();
    }

    const candlestickData = [];
    for (let i = 0; i < data.dates.length; i++) {
        candlestickData.push({
            x: new Date(data.dates[i]),
            o: data.opens[i],
            h: data.highs[i],
            l: data.lows[i],
            c: data.closes[i]
        });
    }

    const traces = [{
        type: 'candlestick',
        x: data.dates.map(d => new Date(d)),
        open: data.opens,
        high: data.highs,
        low: data.lows,
        close: data.closes,
        increasing: { line: { color: '#26a69a' }, fillcolor: '#26a69a' },
        decreasing: { line: { color: '#ef5350' }, fillcolor: '#ef5350' },
        name: 'Price'
    }];

    if (data.ema9) {
        traces.push({
            type: 'scatter',
            mode: 'lines',
            x: data.dates.map(d => new Date(d)),
            y: data.ema9,
            line: { color: '#f9d71c', width: 1.2 },
            name: 'EMA9'
        });
    }

    if (data.ema21) {
        traces.push({
            type: 'scatter',
            mode: 'lines',
            x: data.dates.map(d => new Date(d)),
            y: data.ema21,
            line: { color: '#ff9800', width: 1.2 },
            name: 'EMA21'
        });
    }

    if (data.ema50) {
        traces.push({
            type: 'scatter',
            mode: 'lines',
            x: data.dates.map(d => new Date(d)),
            y: data.ema50,
            line: { color: '#ab47bc', width: 1.2 },
            name: 'EMA50'
        });
    }

    if (data.bbUpper && data.bbLower) {
        traces.push({
            type: 'scatter',
            mode: 'lines',
            x: data.dates.map(d => new Date(d)),
            y: data.bbUpper,
            line: { color: 'rgba(100,149,237,0.5)', width: 1, dash: 'dot' },
            name: 'BB Upper'
        });

        traces.push({
            type: 'scatter',
            mode: 'lines',
            x: data.dates.map(d => new Date(d)),
            y: data.bbLower,
            line: { color: 'rgba(100,149,237,0.5)', width: 1, dash: 'dot' },
            fill: 'tonexty',
            fillcolor: 'rgba(100,149,237,0.04)',
            name: 'BB Lower'
        });
    }

    const layout = {
        template: 'plotly_dark',
        height: 500,
        margin: { l: 40, r: 40, t: 20, b: 40 },
        xaxis: { title: 'Time', gridcolor: '#1e2130' },
        yaxis: { title: 'Price', gridcolor: '#1e2130' },
        hovermode: 'x unified'
    };

    priceChart = Plotly.newPlot(elementId, traces, layout);
}

function renderLineChart(elementId, data) {
    const ctx = document.getElementById(elementId);
    if (!ctx) return;

    if (equityChart) {
        equityChart.destroy();
    }

    const trace = {
        type: 'scatter',
        mode: 'lines',
        x: Array.from({ length: data.length }, (_, i) => i),
        y: data,
        fill: 'tozeroy',
        fillcolor: data[data.length - 1] >= 0 ? 'rgba(38,166,154,0.15)' : 'rgba(239,83,80,0.15)',
        line: { color: data[data.length - 1] >= 0 ? '#26a69a' : '#ef5350', width: 2 }
    };

    const layout = {
        template: 'plotly_dark',
        height: 300,
        title: 'Equity Curve (pips)',
        margin: { l: 40, r: 40, t: 40, b: 40 },
        xaxis: { title: 'Trade Number', gridcolor: '#1e2130' },
        yaxis: { title: 'Cumulative P&L (pips)', gridcolor: '#1e2130' }
    };

    equityChart = Plotly.newPlot(elementId, [trace], layout);
}