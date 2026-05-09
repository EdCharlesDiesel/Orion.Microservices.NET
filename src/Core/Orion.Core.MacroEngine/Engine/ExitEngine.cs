using Orion.Core.MacroEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Engine
{
    public class ExitEngine
    {
        public bool ShouldExit(OpenPosition pos, Candle candle, out double exitPrice)
        {
            exitPrice = 0;

            if (pos.Direction == "LONG")
            {
                // Stop Loss
                if (candle.Low <= pos.StopLoss)
                {
                    exitPrice = pos.StopLoss;
                    return true;
                }

                // Take Profit
                if (candle.High >= pos.TakeProfit)
                {
                    exitPrice = pos.TakeProfit;
                    return true;
                }
            }
            else
            {
                if (candle.High >= pos.StopLoss)
                {
                    exitPrice = pos.StopLoss;
                    return true;
                }

                if (candle.Low <= pos.TakeProfit)
                {
                    exitPrice = pos.TakeProfit;
                    return true;
                }
            }

            return false;
        }
    }
}
