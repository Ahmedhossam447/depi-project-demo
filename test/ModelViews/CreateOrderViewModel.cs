using test.Models;

namespace test.ModelViews
{
    public class CreateOrderViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? UserId { get; set; }
        public string? productName { get; set; }
        public int productPrice { get; set; }
        public List<PaymentMethods>? paymentMethods { get; set; }
        public int selectedPaymentMethodid { get; set; }
    }
}
