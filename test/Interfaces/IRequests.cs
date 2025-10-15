using Microsoft.EntityFrameworkCore;
using test.Models;

namespace test.Interfaces
{
    public interface IRequests
    {
        public  List<User> RequestSent(int userid, List<Request> requests);
        public List<User> RequestGot(int userid, List<Request> requests);
        public List<Animal> AnimalsNeeded(int userid,List<Request> requests);
        public Task<List<Models.Request>> LoadRequests();
        public Task<bool> addRequest(Request request);
        public Task<bool> approverequest(int id);
        public Task<bool> rejectRequest(int id);


        public bool savechanges();



    }
}
