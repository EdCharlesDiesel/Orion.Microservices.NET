using Orion.DataAccess.Postgres.Aggregates;
using Orion.Domain.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories
{
    public class ProductRepository : IProductRepository
    {
        // private readonly OrionDbContext _context;
        // public ProductRepository(OrionDbContext context)
        // {
        //     _context = context;
        // }
        // public IUnitOfWork UnitOfWork => _context;
        //
        // public async Task<IProduct> Get(int id)
        // {
        //     return await _context.Products.Where(m => m.Id == id)
        //         .FirstOrDefaultAsync();
        //     throw new NotImplementedException();
        // }
        //
        // public async Task<IProduct> Delete(int id)
        // {
        //     var model = await Get(id);
        //     if (model == null) return null;
        //     _context.Products.Remove(model as Product);
        //     model.AddDomainEvent(
        //         new ProductDeleteEvent(
        //             model.Id, (model as Product).EntityVersion));
        //     return model;
        //
        // }
        //
        // public IProduct New()
        // {
        //     var model = new Product {EntityVersion=1 };
        //     _context.Products.Add(model);
        //     return model;
        // }
        //
        //  public async Task<bool> UploadFile(MultipartReader reader,MultipartSection? section)
        //  {
        //      while (section != null)
        //      {
        //          var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(
        //           section.ContentDisposition, out var contentDisposition
        //          );
        //
        //          if (hasContentDispositionHeader)
        //          {
        //              if (contentDisposition.DispositionType.Equals("form-data") &&
        //              (!string.IsNullOrEmpty(contentDisposition.FileName.Value) ||
        //              !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value)))
        //              {
        //                  string filePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
        //                  byte[] fileArray;
        //                  using (var memoryStream = new MemoryStream())
        //                  {
        //                      await section.Body.CopyToAsync(memoryStream);
        //                      fileArray = memoryStream.ToArray();
        //                  }
        //                  using (var fileStream = File.Create(Path.Combine(filePath, contentDisposition.FileName.Value)))
        //                  {
        //                      await fileStream.WriteAsync(fileArray);
        //                  }
        //              }
        //          }
        //          section = await reader.ReadNextSectionAsync();
        //      }
        //      return true;
        //  }
        // public IUnitOfWork UnitOfWork => throw new NotImplementedException();
        //
        // public Task<IProduct> Delete(int id)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public Task<IProduct> Get(int id)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public IProduct New()
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task<IEnumerable<IProduct>> GetAllAsync()
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task<IProduct> GetByIdAsync(Guid id)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task AddAsync(IProduct entity)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task UpdateAsync(IProduct entity)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task DeleteAsync(Guid id)
        // {
        //     throw new NotImplementedException();
        // }
        public async Task<IEnumerable<IProduct>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(IProduct entity)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(IProduct entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IProduct> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IProduct New()
        {
            throw new NotImplementedException();
        }

        public async Task<IProduct> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
