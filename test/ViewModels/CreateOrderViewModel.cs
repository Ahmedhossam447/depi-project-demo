using test.Models;

namespace test.ViewModels
{
    public class CreateOrderViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? UserId { get; set; }
        public string? productName { get; set; }
        public int productPrice { get; set; }

    }
}
