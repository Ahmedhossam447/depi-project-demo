using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(255)]
        public string? PhotoUrl { get; set; }
        [MaxLength(100)]
        public string? FullName { get; set; }
        [MaxLength(600)]
        [RegularExpression("^(Cairo|Giza|Alexandria|Mansoura|Port Said|Luxor|Aswan|Tanta|Suez|Hurghada)$",
         ErrorMessage = "Please select a valid city from the list.")]
        public string? location { get; set; }
    }
}

