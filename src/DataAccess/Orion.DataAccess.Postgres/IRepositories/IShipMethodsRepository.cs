using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IShipMethodsRepository
{
    Task<IEnumerable<ShipMethod>> GetAllAsync();
    Task<ShipMethod?> GetByIdAsync(int id);
    Task AddAsync(ShipMethod entity);
    void Update(ShipMethod entity);
    void Delete(ShipMethod entity);
}