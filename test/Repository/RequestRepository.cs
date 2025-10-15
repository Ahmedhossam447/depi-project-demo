using Azure.Core;
using Microsoft.EntityFrameworkCore;
using ServiceStack.Web;
using test.Data;
using test.Interfaces;
using test.Models;

namespace test.Repository
{
    public class RequestRepository : IRequests
    {
        private readonly DepiContext _context;

        public RequestRepository(DepiContext context)
        {
            _context = context;
        }

        public List<Animal> AnimalsNeeded(int userid, List<Models.Request> requests)
        {
            var animalsidsrequested = requests.Select(r => r.AnimalId).Distinct().ToList();
           var animalsrequested =  _context.Animals
                .Where(a => animalsidsrequested.Contains(a.AnimalId))
                .ToList();

            return animalsrequested;
        }

        public List<User> RequestGot(int userid, List<Models.Request> requests)
        {
            var userrequestedids = requests.Select(r => r.Userid).Distinct().ToList();
            var usersrequested =_context.Users
                .Where(u => userrequestedids.Contains(u.Id))
                .ToList();
            return usersrequested;
        }

        public List<User> RequestSent(int userid, List<Models.Request> requests)
        {
            var useridsrequestedto = requests.Select(r => r.Useridreq).Distinct().ToList();
            var usersrequestedto =  _context.Users
                .Where(u => useridsrequestedto.Contains(u.Id))
                .ToList();
            return usersrequestedto;
        }
        public async Task<List<Models.Request>> LoadRequests()
        {
            return  await _context.Requests.ToListAsync();

        }
        public async Task<bool> addRequest(Models.Request request)
        {
            await _context.Requests.AddAsync(request);
            return savechanges();
           
        }
        public async Task<bool> approverequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return false;
            }
            request.Status = "approved";
            return savechanges();

        }
        public async Task<bool> rejectRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return false;
            }
            _context.Requests.Where(m => m.Reqid == id).ExecuteDelete();
            return savechanges();
  

        }
        public bool savechanges()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
