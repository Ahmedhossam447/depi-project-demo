using System.ComponentModel.DataAnnotations;

namespace test.ModelViews
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
    }
}
