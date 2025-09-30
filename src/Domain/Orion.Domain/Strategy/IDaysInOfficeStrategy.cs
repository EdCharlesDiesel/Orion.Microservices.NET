using System.Collections.Generic;

namespace Orion.Domain.Strategy
{
    public interface IDaysInOfficeStrategy
    {
         int GetDaysInOffice(IEnumerable<Term> terms);
    }
}