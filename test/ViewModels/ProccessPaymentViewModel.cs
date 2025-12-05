using test.Models;

namespace test.ViewModels
{
    public class ProccessPaymentViewModel
    {
        public int orderid { get; set; }
        public int totalprice { get; set; }
        public List<OrderDetails>? orderDetails { get; set; }
        public List<PaymentMethods>? paymentMethods { get; set; }
        public int selectedPaymentMethodid { get; set; }
        
        // Grouped items for order summary
        public List<GroupedItemViewModel> GroupedItems { get; set; } = new();
        public int TotalQuantity { get; set; }
    }
}
