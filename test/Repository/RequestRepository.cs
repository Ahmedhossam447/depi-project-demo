using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceStack.Web;
using System.Globalization;
using test.Data;
using test.Interfaces;
using test.Models;
using Request = test.Models.Request;

namespace test.Repository
{
    public class RequestRepository : IRequests
    {
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly DepiContext _context;

        public RequestRepository(DepiContext context,UserManager<IdentityUser> _usermanager)
        {
            this._usermanager = _usermanager;
            _context = context;
        }

        public List<Animal> AnimalsNeeded(string userid, List<Models.Request> requests)
        {
            var animalsidsrequested = requests.Select(r => r.AnimalId).Distinct().ToList();
           var animalsrequested =  _context.Animals
                .Where(a => animalsidsrequested.Contains(a.AnimalId))
                .ToList();

            return animalsrequested;
        }

        public List<IdentityUser> RequestGot(string userid, List<Models.Request> requests)
        {
            var userrequestedids = requests.Select(r => r.Userid).Distinct().ToList();
            var usersrequested =_usermanager.Users
                .Where(u => userrequestedids.Contains(u.Id))
                .ToList();
            return usersrequested;
        }

        public List<IdentityUser> RequestSent(string userid, List<Models.Request> requests)
        {
            var useridsrequestedto = requests.Select(r => r.Useridreq).Distinct().ToList();
            var usersrequestedto = _usermanager.Users
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
        public async Task<Request> GetRequestById(int id)
        {
            return await _context.Requests.FindAsync(id);
        }

        public async Task<bool> DeleteRequest(Request request)
        {
            _context.Requests.Remove(request);
            return savechanges();
        }

        public async Task<bool> HasAcceptedRequest(string userId1, string userId2)
        {
            return await _context.Requests.AnyAsync(r => 
                ((r.Userid == userId1 && r.Useridreq == userId2) || (r.Userid == userId2 && r.Useridreq == userId1)) 
                && r.Status == "approved");
        }
        public bool savechanges()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
