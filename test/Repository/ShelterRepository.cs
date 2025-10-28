﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using test.Data;
using test.Interfaces;
using test.Models;

namespace test.Repository
{
    public class ShelterRepository : IShelter
       
    {
        private readonly DepiContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ShelterRepository (DepiContext context,UserManager<IdentityUser> userManager)
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

        public async Task<List<IdentityUser>> GetAllShelters()
        {
            List<IdentityUser> identityUsers = new List<IdentityUser>();

            var shelters1 =await _userManager.Users.ToListAsync();
            foreach (var shelter in shelters1)
                if(await _userManager.IsInRoleAsync(shelter, "Shelter"))
                {
                    identityUsers.Add(shelter);
                }
            return identityUsers;
        }

        public Task<bool> RemoveProduct(Product product)
        {
            throw new NotImplementedException();
        }
        public bool SaveChanges(){
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;

        }
    }
}
