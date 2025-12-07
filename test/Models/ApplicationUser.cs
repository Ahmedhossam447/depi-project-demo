using Microsoft.AspNetCore.Identity;

namespace test.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? PhotoUrl { get; set; }
        public string? FullName { get; set; }

        public string? Bio { get; set; }

        public string? location { get; set; }
    }
}

