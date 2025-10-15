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

namespace test.Repository
{
    public class AccountRepository : IAccounts
    {
        private readonly DepiContext _context;
        public AccountRepository(DepiContext context) {
        
        _context = context;
        }
        public async Task<bool> adduser(registerviewmodel user)
        {
            var checking = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.email);
                if (checking != null)
            {
                return false;
            }
            await _context.AddAsync(user);
            return savechanges();
        }

        public async Task<User> GetUserbyid(int id)
        {
          return await _context.Users.FirstOrDefaultAsync(u => u.Id==id);
        }

        public List<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public bool removeuser(int id)
        {
            throw new NotImplementedException();
        }

        public bool savechanges()
        {
            var saved=_context.SaveChanges();
            


            return saved > 0 ? true : false;
        }

        public async Task<User> SignIn(LoginViewModel user)
        {
            return await _context.Users.FirstOrDefaultAsync(u=>u.Email==user.email&&u.Password==user.password);
            
        }
    }
}
