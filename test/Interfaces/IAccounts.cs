using test.Models;
using test.ModelViews;

namespace test.Interfaces
{
    public interface IAccounts
    {
        public  Task<User> GetUserbyid(int id);
        public List<User> GetUsers();
        public  Task<bool> adduser(registerviewmodel user);
        public bool removeuser(int id);
        public bool savechanges();
         public Task<User> SignIn(LoginViewModel user);
    }
}
