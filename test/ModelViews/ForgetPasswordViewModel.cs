using System.ComponentModel.DataAnnotations;

namespace test.ModelViews
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
    }
}
