using System;
using System.Collections.Generic;
using System.Linq;

namespace Orion.Domain.Strategy
{
    public class DefaultDaysInOfficeStrategy : IDaysInOfficeStrategy
    {
        public int GetDaysInOffice(IEnumerable<Term> terms)
        {
            var enumerable = terms as Term[] ?? terms.ToArray();
            if (!enumerable.Any())
            {
                return 0;
            }

            int totalDays = 0;

            foreach (var term in enumerable)
            {
                if (term.EndOfTerm != null)
                {
                    var diff = term.EndOfTerm - term.StartOfTerm;

                    totalDays += Convert.ToInt32((int)diff);
                }
            }

            return totalDays;
        }
    }

    public class Term
    {
        public int EndOfTerm { get; set; }
        public int StartOfTerm { get; set; }
    }
}
