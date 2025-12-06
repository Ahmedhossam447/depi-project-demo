using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace test.ViewModels
{
    public class EditProductViewModel
    {
        [Required]
        public int ProductId { get; set; }
        
        public string? Type { get; set; } // Read-only, not editable
        
        [Required]
        public int? Quantity { get; set; }
        
        [Required]
        public int? Price { get; set; }
        
        [Required]
        public string Disc { get; set; }
        
        public IFormFile? Photo { get; set; } // New photo upload
        
        public string? CurrentPhotoUrl { get; set; } // Current photo to display
    }
}
