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
namespace test.Controllers
{
    public class RequestController : Controller
    {
        private readonly DepiContext _context;
        public RequestController(DepiContext context)
        {
            _context = context;
        }   
        public async Task<IActionResult> Index()
        {
            var requests = await _context.Requests.ToListAsync();
            var useridclaim = User.FindFirst("ID");
            var userid= int.Parse(useridclaim.Value);
            var useridsrequestedto = requests.Select(r => r.Useridreq).Distinct().ToList();
            var usersrequestedto = await _context.Users
                .Where(u => useridsrequestedto.Contains(u.Id))
                .ToListAsync();
            var userrequestedids = requests.Select(r => r.Userid).Distinct().ToList();
            var usersrequested = await _context.Users
                .Where(u => userrequestedids.Contains(u.Id))
                .ToListAsync();
            var animalsidsrequested = requests.Select(r => r.AnimalId).Distinct().ToList();
            var animalsrequested = await _context.Animals
                .Where(a => animalsidsrequested.Contains(a.AnimalId))
                .ToListAsync();
            ViewBag.usersrequested = usersrequested;
            ViewBag.animals = animalsrequested;
            ViewBag.users = usersrequestedto;
            ViewBag.userid = userid;

            return View(requests);
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("AnimalId,Useridreq")]Request request)
        {
            if (ModelState.IsValid)
            {
                var useridclaim = User.FindFirst("ID");
                request.Userid = int.Parse(useridclaim.Value);
                await   _context.Requests.AddAsync(request);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Animal");
            }
            return RedirectToAction("Index", "Animal");
        }
        [HttpPost]
        public async Task<IActionResult> approve(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
            return RedirectToAction("Index");
            }
            request.Status = "approved";
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> reject(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
           _context.Requests.Where(m => m.Reqid == id).ExecuteDelete();
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");



        }
    }
}
