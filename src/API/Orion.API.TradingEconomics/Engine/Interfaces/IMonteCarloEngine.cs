using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces

{
    /// <summary>
    /// Defines a Monte Carlo simulation engine for trade equity projection.
    /// </summary>
    public interface IMonteCarloEngine
    {
        /// <summary>
        /// Runs Monte Carlo simulations over the provided trade results.
        /// </summary>
        /// <param name="trades">Historical trade results used as the simulation sample pool.</param>
        /// <param name="simulations">Number of simulation iterations to run. Defaults to 1000.</param>
        /// <returns>
        /// A list of final equity values — one per simulation — starting from an initial
        /// equity of 100,000.
        /// </returns>
        List<decimal> Run(List<TradeResult> trades, int simulations = 1000);
    }
}