using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Entities.Common;

public class Emu :Entity<Guid>
{
    public List<Country> MemberStates { get; set; } = new List<Country>();
    public EuropeanCentralBank Ecb { get; set; } = new EuropeanCentralBank();

    public void RunYearlyCycle()
    {
        Console.WriteLine("=== Running EMU Annual Economic Policy Cycle ===");
        Ecb.ApplyMonetaryPolicy();

        foreach (var country in MemberStates)
        {
            country.SubmitEconomicPlan();
            country.TakeCorrectiveAction();

            if (country.NeedsMonetaryFlexibility)
            {
                Console.WriteLine($"{country.Name} faces challenges due to lack of monetary policy independence.");
            }
        }

        Console.WriteLine("EMU economic cycle completed.\n");
    }

    public void EvaluateSouthAfricaImpact()
    {
        Console.WriteLine("=== Evaluating South Africa’s Connection to the EMU ===");
        Console.WriteLine("- Trade affected by Euro exchange rate volatility.");
        Console.WriteLine("- EU economic performance impacts demand for South African exports.");
        Console.WriteLine("- EU trade policy affects tariffs and quotas.");
    }
}
