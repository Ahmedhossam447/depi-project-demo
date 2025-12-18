using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using test.Data;
using test.Interfaces;
using test.Models;

namespace test.Repository
{
    public class ShelterRepository : IShelter
       
    {
        private readonly DepiContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShelterRepository (DepiContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<bool> AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            return SaveChanges();
        }

        public async Task<List<Product>> GetAllProducts(string id)
        {
            var products =await _context.Products.Where(s => s.Userid == id).ToListAsync();
            return products;
        }

        public async Task<(List<Product> Products, int TotalCount, int TotalPages)> GetProductsPaginated(string id, int page, int pageSize = 6)
        {
            var baseQuery = _context.Products.Where(s => s.Userid == id);
            
            // Get total count
            var totalCount = await baseQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            
            // Apply pagination
            var products = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return (products, totalCount, totalPages);
        }

        public async Task<List<ApplicationUser>> GetAllShelters()
        {
            List<ApplicationUser> applicationUsers = new List<ApplicationUser>();

            var shelters1 =await _userManager.Users.ToListAsync();
            foreach (var shelter in shelters1)
                if(await _userManager.IsInRoleAsync(shelter, "Shelter"))
                {
                    applicationUsers.Add(shelter);
                }
            return applicationUsers;
        }

        public async Task<bool> RemoveProduct(Product product)
        {
            _context.Products.Remove(product);
            return SaveChanges();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            return SaveChanges();
        }
        public bool SaveChanges(){
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;

        }
        public async Task<Product> GetProductbyId(int id)
        {
            var products = await _context.Products.FirstOrDefaultAsync(p=>p.Productid==id);
            return products;
        }
    }
}
