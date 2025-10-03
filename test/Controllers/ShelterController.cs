using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace test.Controllers
{
    [Authorize]
    public class ShelterController : Controller
    {
        private readonly DepiContext _context;
        public ShelterController(DepiContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            int userid = int.Parse(User.FindFirst("ID")!.Value);
            var products = await _context.Products.Where(s => s.Userid == userid).ToListAsync();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Type,Price,Disc")] Product product)
        {
            if (ModelState.IsValid)
            {
                var useridclaim = User.FindFirst("ID");
                if (useridclaim != null)
                {
                    int userid = int.Parse(useridclaim.Value);
                    product.Userid = userid;
                }
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }
        public async Task<IActionResult> userview()
        {
            var shlters = await _context.Users.Where(s => s.Role == "shelter").ToListAsync();
            return View(shlters);
        }
        [HttpPost]
   public async Task<IActionResult> Shelterpage([Bind("Id,Username,Email,Phonenumber")] User shelter)
        {

            var products=await _context.Products.Where(s => s.Userid == shelter.Id).ToListAsync();
            ViewBag.email = shelter.Email;
            ViewBag.phonenumber = shelter.Phonenumber;
            ViewBag.username = shelter.Username;
            return View(products);
        }

    }
}
