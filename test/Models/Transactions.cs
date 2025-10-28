using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models
{
    [Table("Transactions")]
    public partial class Transactions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Orders? Order { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public int Amount { get; set; }
        public int PaymentMethodId { get; set; }
        [ForeignKey("PaymentMethodId")]
        public PaymentMethods? PaymentMethod { get; set; }
        public string Status { get; set; }


    }
}
