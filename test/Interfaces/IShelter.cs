using Microsoft.AspNetCore.Identity;
using test.Migrations.Depi;
using test.Models;

namespace test.Interfaces
{
    public interface IShelter
    {
        public  Task<List<Product>> GetAllProducts(string id);
        public Task<(List<Product> Products, int TotalCount, int TotalPages)> GetProductsPaginated(string id, int page, int pageSize = 6);
        public Task<Product> GetProductbyId(int id);
        public Task<bool> AddProduct(Product product);
        public Task<bool> UpdateProduct(Product product);
        public Task<bool> RemoveProduct(Product product);
        public Task<List<ApplicationUser>> GetAllShelters();
        public bool SaveChanges();

    }
}
