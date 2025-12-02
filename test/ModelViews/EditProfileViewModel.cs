using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace test.ModelViews
{
    public class EditProfileViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Profile Photo")]
        public IFormFile? Photo { get; set; }

        public string? CurrentPhotoUrl { get; set; }
    }
}

