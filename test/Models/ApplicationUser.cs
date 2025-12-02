using Microsoft.AspNetCore.Identity;

namespace test.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? PhotoUrl { get; set; }
    }
}

