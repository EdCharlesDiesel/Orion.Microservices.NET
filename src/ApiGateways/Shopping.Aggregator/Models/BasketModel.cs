using System.Collections.Generic;

namespace Orion.Shopping.Aggregator.Models
{
    public abstract class BasketModel
    {
        public string UserName { get; set; }

        public List<BasketItemExtendedModel> Items { get; set; } = new List<BasketItemExtendedModel>();

        public decimal TotalPrice { get; set; }
    }
}
