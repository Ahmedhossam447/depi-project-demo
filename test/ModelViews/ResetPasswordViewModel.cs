using System.ComponentModel.DataAnnotations;

namespace test.ModelViews
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string confirmPassword { get; set; }
        public string token { get; set; }

    }
}
