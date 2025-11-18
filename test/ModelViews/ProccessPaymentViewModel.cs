using test.Models;

namespace test.ModelViews
{
    public class ProccessPaymentViewModel
    {
        public int orderid { get; set; }
        public int totalprice { get; set; }
        public List<OrderDetails>? orderDetails { get; set; }
        public List<PaymentMethods>? paymentMethods { get; set; }
        public int selectedPaymentMethodid { get; set; }
    }
}
