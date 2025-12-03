using test.Models;

namespace test.ModelViews
{
    public class MyOrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderStatus { get; set; } // 0 = Pending, 1 = Paid/Done
        public int TotalPrice { get; set; }
        public string? PaymentMethodType { get; set; }
        public string? PaymentLast4Digits { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; } = new List<OrderDetailViewModel>();
    }

    public class OrderDetailViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductType { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductPhoto { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int TotalPrice { get; set; }
        public string? ShelterName { get; set; }
        public string? ShelterUserId { get; set; }
        public string? ShelterEmail { get; set; }
        public string? ShelterPhone { get; set; }
    }

    public class MyOrdersPageViewModel
    {
        public List<MyOrderViewModel> Orders { get; set; } = new List<MyOrderViewModel>();
        public int TotalOrders => Orders.Count;
        public int CompletedOrders => Orders.Count(o => o.OrderStatus == 1);
        public int PendingOrders => Orders.Count(o => o.OrderStatus == 0);
    }
}

