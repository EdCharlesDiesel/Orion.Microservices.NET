using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orion.DataAccess.Postgres.Aggregates;

namespace Orion.Domain.IRepositories
{
    public interface IFeatureRepository : IRepository<IFeature>
    {
        IFeature GetFeatureByUsername(string username);
        List<T> GetFeaturesByUsername<T>(string username);
        Task<IFeature> Get(int id);
        IFeature New();
    }
}
