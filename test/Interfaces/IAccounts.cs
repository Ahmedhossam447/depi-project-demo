using Microsoft.AspNetCore.Identity;
using test.Models;
using test.ModelViews;

namespace test.Interfaces
{
    public interface IAccounts
    {
        public  Task<IdentityUser> GetUserbyid(string id);
        public List<IdentityUser> GetUsers();
        public  Task<bool> adduser(registerviewmodel user);
        public bool removeuser(string id);
        public bool savechanges();
         public Task<IdentityUser> SignIn(LoginViewModel user);
    }
}
