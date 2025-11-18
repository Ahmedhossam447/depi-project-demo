using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Orders ?Order { get; set; }
        public int productId { get; set; }
        [ForeignKey("productId")]
        public Product ?Product { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
    }
}
