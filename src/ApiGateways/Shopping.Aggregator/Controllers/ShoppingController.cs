using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.Aggregator.Models;
using Orion.Aggregator.Services;

namespace Orion.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController(
        ICatalogAggregatorService catalogService,
        IBasketAggregatorService basketService,
        IOrderAggregatorService orderService)
        : ControllerBase
    {
        private readonly ICatalogAggregatorService _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        private readonly IBasketAggregatorService _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        private readonly IOrderAggregatorService _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));

        [HttpGet("{userName}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            var basket = await _basketService.GetBasket(userName);

            foreach (var item in basket.Items)
            {
                var product = await _catalogService.GetCatalog(item.ProductId);

                // set additional product fields
            }            

            var orders = await _orderService.GetOrdersByUserName(userName);

            var shoppingModel = new ShoppingModel
            {
                UserName = userName,
                BasketWithProducts = basket,
                Orders = orders
            };
            
            return Ok(shoppingModel);
        }

    }
}
