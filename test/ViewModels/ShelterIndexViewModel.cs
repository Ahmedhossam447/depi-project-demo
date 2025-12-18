using test.Models;

namespace test.ViewModels
{
    public class ShelterIndexViewModel
    {
        public List<Product> Products { get; set; }
        public List<Animal> Animals { get; set; }
        
        // Pagination properties for products
        public int ProductCurrentPage { get; set; } = 1;
        public int ProductTotalCount { get; set; }
        public int ProductTotalPages { get; set; }
        public int ProductPageSize { get; set; } = 6;
    }
}
