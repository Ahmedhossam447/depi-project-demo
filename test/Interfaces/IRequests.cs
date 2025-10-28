using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using test.Models;

namespace test.Interfaces
{
    public interface IRequests
    {
        public  List<IdentityUser> RequestSent(string userid, List<Request> requests);
        public List<IdentityUser> RequestGot(string userid, List<Request> requests);
        public List<Animal> AnimalsNeeded(string  userid,List<Request> requests);
        public Task<List<Models.Request>> LoadRequests();
        public Task<bool> addRequest(Request request);
        public Task<bool> approverequest(int id);
        public Task<bool> rejectRequest(int id);


        public bool savechanges();



    }
}
