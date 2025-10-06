namespace Orion.Aggregator.Models
{
    public abstract class BasketItemExtendedModel
    {
        private string _productId;
        public int Quantity { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }

        public string ProductId
        {
            get => _productId;
            set => _productId = value;
        }

        //Product Related Additional Fields
    }
}
