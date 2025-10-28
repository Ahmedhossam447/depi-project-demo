using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test.Data;
using System.Security.Claims;
using test.Models;
using Microsoft.AspNetCore.Authentication;
using test.ModelViews;
using test.Repository;
using test.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace test.Repository
{
    public class AccountRepository : IAccounts
    {
        private readonly DepiContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public AccountRepository(DepiContext context,UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager) {
        
        _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public async Task<bool> adduser(registerviewmodel user)
        {
  
                return savechanges();
        }

        public async Task<IdentityUser> GetUserbyid(string id)
        {
            return await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public List<IdentityUser> GetUsers()
        {
            throw new NotImplementedException();
        }

        public bool removeuser(string id)
        {
            throw new NotImplementedException();
        }

        public bool savechanges()
        {
            var saved=_context.SaveChanges();
            


            return saved > 0 ? true : false;
        }

        public async Task<IdentityUser> SignIn(LoginViewModel user)
        {
            return await userManager.Users.FirstOrDefaultAsync(u=>u.Email==user.email&&u.Email==user.password);
            
        }
    }
}
