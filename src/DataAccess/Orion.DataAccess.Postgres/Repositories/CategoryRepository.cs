using Orion.DataAccess.Postgres.Aggregates;
using Orion.DataAccess.Postgres.IRepositories;
using Orion.Domain.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        //private OrionDbContext context;
        //public CategoryRepository(OrionDbContext context)
        //{
        //    this.context = context;
        //}
        //public IUnitOfWork UnitOfWork => context;

        //public async Task<ICategory> Get(int id)
        //{
        //    return await context.Categories.Where(m => m.Id == id)
        //        .FirstOrDefaultAsync();
        //}

        //public object GetAll()
        //{
        //    throw new System.NotImplementedException();
        //}

        //public object GetById(int id)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public ICategory New()
        //{
        //    var model = new Category();
        //    context.Categories.Add(model);
        //    return model;
        //}

        public async Task<IEnumerable<ICategory>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<ICategory>> IRepository<ICategory>.GetAllAsync()
        {
            return await GetAllAsync();
        }

        public async Task GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        async Task IRepository<ICategory>.AddAsync(ICategory entity)
        {
            await AddAsync(entity);
        }

        async Task IRepository<ICategory>.UpdateAsync(ICategory entity)
        {
            await UpdateAsync(entity);
        }

        public async Task AddAsync(ICategory entity)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(ICategory entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICategory> Get(int id)
        {
            throw new NotImplementedException();
        }

        ICategory ICategoryRepository.New()
        {
            return New();
        }

        async Task<ICategory> ICategoryRepository.Get(int id)
        {
            return await Get(id);
        }

        public ICategory New()
        {
            throw new NotImplementedException();
        }
    }
}
