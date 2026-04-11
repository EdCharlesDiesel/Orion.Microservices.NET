using System.IO.Compression;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orion.Admin.Commands;
using Orion.Admin.Models.Products;
using Orion.Admin.Queries;
using Orion.Admin.Tools;
using Orion.Domain.IRepositories;

namespace Orion.Admin.Controllers
{
    [Authorize(Roles= "Admins")]
    public class ProductController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromServices] IProductsListQuery query)
        {
            // var results = await query.GetAllProducts();
            // var vm = new ProductsListViewModel { Items = results };
            // return View(vm);
            throw new NotImplementedException();
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            ProductFullEditViewModel vm,
            [FromServices] ICommandHandler<CreateProductCommand> command)
        {
            if (ModelState.IsValid) { 

                //Getting FileName
                var fileName = Path.GetFileName(vm.ProductName);
                //Getting file Extension
                var fileExtension = Path.GetExtension(fileName);
                // concatenating  FileName + FileExtension
                var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);


                // using (var memoryStream = new MemoryStream())
                // {
                //     await vm.CoverImage.CopyTo(memoryStream);
                //     vm.Image = memoryStream.ToArray();
                // }

                // DecompressWithBrotli(vm.)

                


                
                await command.HandleAsync(new CreateProductCommand(vm));
                return RedirectToAction(
                    nameof(Index));
            }

            return View("Edit", vm);
        }

        public static Stream DecompressWithBrotli(Stream toDecompress)
        {
            var decompressedStream = new MemoryStream();
            using (BrotliStream decompressionStream = new BrotliStream(toDecompress, CompressionMode.Decompress))
            {
                decompressionStream.CopyTo(decompressedStream);
            }
            decompressedStream.Position = 0;
            return decompressedStream;
        }
        [HttpGet]
        public async Task<IActionResult> Edit(
            int id,
            [FromServices] IProductRepository repo)
        {
            // if (id == 0) return RedirectToAction(
            //     nameof(Index));
            // var aggregate = await repo.Get(id);
            // if (aggregate == null) return RedirectToAction(
            //     nameof(Index));
            // var vm = new ProductFullEditViewModel(aggregate);
            // return View(vm);
            throw new NotImplementedException();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(
            ProductFullEditViewModel vm,
            [FromServices] ICommandHandler<UpdateProductCommand> command)
        {
            if (ModelState.IsValid)
            {
                await command.HandleAsync(new UpdateProductCommand(vm));
                return RedirectToAction(
                    nameof(Index));
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(
            int id,
            [FromServices] ICommandHandler<DeleteProductCommand> command)
        {
            if (id>0)
            {
                await command.HandleAsync(new DeleteProductCommand(id));
                
            }
            return RedirectToAction(
                    nameof(Index));
        }

         [HttpPost]
        public IActionResult Index(IFormFile files)
        {
            if (files != null)
            {
                if (files.Length > 0)
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(files.FileName);
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // concatenating  FileName + FileExtension
                    var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

                    // var objfiles = new Files()
                    // {
                    //     DocumentId = 0,
                    //     Name= newFileName,
                    //     FileType = fileExtension,
                    //     CreatedOn = DateTime.Now
                    // };
                    
                    // using (var target = new MemoryStream())
                    // {
                    //     files.CopyTo(target);
                    //     objfiles.DataFiles = target.ToArray();
                    // }

                    // _context.Files.Add(objfiles);
                    // _context.SaveChanges();

                }
            }
            return View();
        }

        
    }
}
