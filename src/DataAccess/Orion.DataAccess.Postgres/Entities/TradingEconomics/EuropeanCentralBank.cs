using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities.TradingEconomics;

[Table("EuropeanCentralBank", Schema = "TradingEconomics")]
public class EuropeanCentralBank
{

    public double GDPGrowth;
    public double InflationRate { get; set; }
    public double GdpGrowth { get; set; }

    public void ApplyMonetaryPolicy()
    {
        if (InflationRate > 2.0)
        {
            Console.WriteLine("ECB increases interest rates to control inflation.");
        }
        else if (GdpGrowth < 1.0)
        {
            Console.WriteLine("ECB lowers interest rates to stimulate growth.");
        }
        else
        {
            Console.WriteLine("ECB maintains interest rates to preserve price stability.");
        }
    }
}