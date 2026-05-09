using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Interfaces
{
    public interface IVolatilityService
    {
        Task<double> GetVolatilityAsync(string pair);
    }
}
