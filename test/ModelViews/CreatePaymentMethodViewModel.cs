using System.ComponentModel.DataAnnotations;

namespace test.ModelViews
{
    public class CreatePaymentMethodViewModel
    {
        [Required(ErrorMessage = "Card number is required")]
        [Display(Name = "Card Number")]
        [RegularExpression(@"^[\d\s]+$", ErrorMessage = "Invalid card number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Expiry Month is required")]
        [Range(1, 12, ErrorMessage = "Invalid month")]
        [Display(Name = "Expiry Month")]
        public int ExpiryMonth { get; set; }

        [Required(ErrorMessage = "Expiry Year is required")]
        [Display(Name = "Expiry Year")]
        public int ExpiryYear { get; set; }
    }
}
