using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models
{
    [Table("PaymentMethod")]
    public partial class PaymentMethods
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentMethodId { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual IdentityUser? User { get; set; }
        public string MethodType { get; set; }
        public string last4Digits { get; set; }
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }
        public string GatewatyToken { get; set; }
    }
}
