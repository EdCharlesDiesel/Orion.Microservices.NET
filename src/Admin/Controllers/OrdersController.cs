using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orion.Admin.Commands;
using Orion.Admin.Models.Orders;
using Orion.Admin.Queries;
using Orion.Admin.Tools;
using Orion.Domain.IRepositories;

namespace Orion.Admin.Controllers
{
   [Authorize(Roles= "Admins")]
    public class OrdersController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromServices] IOrdersListQuery query)
        {
            var results = await query.GetAllOrders();
            var vm = new OrdersListViewModel { Items = results };
            return View(vm);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View("Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Create(
            OrderFullEditViewModel vm,
            [FromServices] ICommandHandler<CreateOrderCommand> command)
        {
            if (ModelState.IsValid) { 
                await command.HandleAsync(new CreateOrderCommand(vm));
                return RedirectToAction(
                    nameof(Index));
            }

            return View("Edit", vm);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(
            int id,
            [FromServices] IOrderRepository repo)
        {
            if (id == 0) return RedirectToAction(
                nameof(Index));
            var aggregate = await repo.Get(id);
            if (aggregate == null) return RedirectToAction(
                nameof(Index));
            var vm = new OrderFullEditViewModel(aggregate);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(
            OrderFullEditViewModel vm,
            [FromServices] ICommandHandler<UpdateOrderCommand> command)
        {
            if (ModelState.IsValid)
            {
                await command.HandleAsync(new UpdateOrderCommand(vm));
                return RedirectToAction(
                    nameof(Index));
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(
            int id,
            [FromServices] ICommandHandler<DeleteOrderCommand> command)
        {
            if (id>0)
            {
                await command.HandleAsync(new DeleteOrderCommand(id));
                
            }
            return RedirectToAction(
                    nameof(Index));
        }
    }
}
