using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace test.ViewModels
{
    public class CreateProductViewModel
    {
        [Required]
        public string Type { get; set; }
        
        [Required]
        public int? Quantity { get; set; }
        
        [Required]
        public int? Price { get; set; }
        
        [Required]
        public string Disc { get; set; }
        
        public IFormFile? Photo { get; set; }
    }
}
