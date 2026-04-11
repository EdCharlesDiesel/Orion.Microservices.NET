using Microsoft.AspNetCore.Mvc;
using Orion.Admin.Commands;
using Orion.Admin.Models.Customers;
using Orion.Admin.Queries;
using Orion.Admin.Tools;
using Orion.Domain.IRepositories;

namespace Orion.Admin.Controllers
{
    //TODO Add comments
    
    public class CustomersController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromServices] ICustomersListQuery query)
        {
            var results = await query.GetAllCustomers();
            var vm = new CustomersListViewModel { Items = results };
            return View(vm);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerFullEditViewModel vm,
        [FromServices] ICommandHandler<CreateCustomerCommand> command)
        {
            if (ModelState.IsValid) { 
                await command.HandleAsync(new CreateCustomerCommand(vm));
                return RedirectToAction(
                    nameof(Index));
            }

            return View("Edit", vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id,[FromServices] ICustomerRepository repo)
        {
            if (id == 0) return RedirectToAction(
                nameof(Index));
            var aggregate = await repo.Get(id);
            if (aggregate == null) return RedirectToAction(
                nameof(Index));
            var vm = new CustomerFullEditViewModel(aggregate);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            CustomerFullEditViewModel vm,
            [FromServices] ICommandHandler<UpdateCustomerCommand> command)
        {
            if (ModelState.IsValid)
            {
                await command.HandleAsync(new UpdateCustomerCommand(vm));
                return RedirectToAction(
                    nameof(Index));
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id,[FromServices] ICommandHandler<DeleteCustomerCommand> command)
        {
            if (id>0)
            {
                await command.HandleAsync(new DeleteCustomerCommand(id));
                
            }
            return RedirectToAction(
                    nameof(Index));
        }
    }
}