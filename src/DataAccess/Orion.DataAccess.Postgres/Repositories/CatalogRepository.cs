// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using Orion.DataAccess.Data;
// using Orion.Domain.IRepositories;
// using Product = Orion.Domain.IRepositories.Product;
//
// namespace Orion.DataAccess.Repositories;
// public class CatalogRepository(OrionDbContext context) : ICatalogServices
// {
//     public async Task<IEnumerable<Product>> GetAllAsync()
//     {
//         var products =  context.Products.ToListAsync();
//         if (products == null )
//             throw new ArgumentException("products be null or empty.");
//
//         return await products;
//     }
//
//     async Task IRepository<Product>.GetByIdAsync(Guid id)
//     {
//         await GetByIdAsync(id);
//     }
//
//     async Task IRepository<Product>.AddAsync(Product entity)
//     {
//         await AddAsync(entity);
//     }
//
//     async Task IRepository<Product>.UpdateAsync(Product entity)
//     {
//         await UpdateAsync(entity);
//     }
//
//     public async Task<List<Core.Catalog.Domain.Product>> CreateProducts(List<Core.Catalog.Domain.Product> products)
//     {
//         if (products == null)
//             throw new ArgumentException("product be null or empty.");
//
//         await context.Products.AddRangeAsync(products);
//         await context.SaveChangesAsync();
//         
//         return products;
//     }
//
//
//     public async Task<Product> Create(List<Product> products)
//     {
//         if (products == null || !products.Any())
//             throw new ArgumentException("product be null or empty.");
//
//         await context.Products.AddRangeAsync(products);
//         await context.SaveChangesAsync();
//         
//         return products.First();
//     }
//
//
//     async Task<IEnumerable<Product>> IRepository<Product>.GetAllAsync()
//     {
//         return await GetAllAsync();
//     }
//
//     public async Task<Core.Catalog.Domain.Product?> GetByIdAsync(Guid id)
//     {
//         var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
//         if (product == null )
//             throw new ArgumentException("product id cannot be null or empty.");
//
//         return product;
//     }
//
//     public async Task AddAsync(Product product)
//     {
//         if (product == null)
//             throw new ArgumentException("Product cannot be null or empty.");
//
//         await context.Products.AddAsync(product);
//         await context.SaveChangesAsync();
//     }
//
//     public async Task UpdateAsync(Product entity)
//     {
//         var product =  context.Products.FirstOrDefault(x => x.Id == entity.Id);
//         if (product == null)
//             throw new ArgumentException("Product cannot be null or empty.");
//
//         context.Products.Update(product);
//         await context.SaveChangesAsync();
//     }
//
//     public async Task DeleteAsync(Guid id)
//     {
//         var product =  context.Products.FirstOrDefault(x => x.Id == id);
//         if (product == null)
//             throw new ArgumentException("Product cannot be null or empty.");
//
//         context.Products.Remove(product);
//         await context.SaveChangesAsync();
//     }
//
//     public async Task BulkCreate(List<Product> entity)
//     {
//         throw new NotImplementedException();
//     }
//
//     public async Task BulkCreate(List<Product> products)
//     {
//         if (products == null)
//             throw new ArgumentException("Products cannot be null or empty.");
//
//         await context.Products.AddRangeAsync(products);
//         await context.SaveChangesAsync();
//     }
// }