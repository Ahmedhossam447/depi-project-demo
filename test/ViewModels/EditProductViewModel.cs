using System.ComponentModel.DataAnnotations;

namespace test.ViewModels
{
    public class EditProductViewModel
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int? Quantity { get; set; }
        [Required]
        public int? Price { get; set; }
        [Required]
        public string Disc { get; set; }
    }
}
