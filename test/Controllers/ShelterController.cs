using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using test.Data;
using test.Interfaces;
using test.Models;

namespace test.Controllers
{
    [Authorize]
    public class ShelterController : Controller
    {
        private readonly DepiContext _context;
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly IShelter _ShelterRepository;
        public ShelterController(DepiContext context,UserManager<IdentityUser> userManager,IShelter shelter )
        {
            _usermanager = userManager;
            _context = context;
            _ShelterRepository = shelter;
        }
        [Authorize (Roles ="Shelter")]
        public async Task<IActionResult> Index()
        {
            var userid = _usermanager.GetUserId(User);
            var products =await _ShelterRepository.GetAllProducts(userid);
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
             
                    var userid = _usermanager.GetUserId(User);
                    product.Userid = userid;
               await _ShelterRepository.AddProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }
        public async Task<IActionResult> userview()
        {
            var shlters = await _ShelterRepository.GetAllShelters();
            return View(shlters);
        }
        [HttpGet]
  public async Task<IActionResult> Shelterpage(IdentityUser shelter)
        {

            var products = await _ShelterRepository.GetAllProducts(shelter.Id);
            var userid = _usermanager.GetUserId(User);
            ViewBag.userid = userid;
            ViewBag.email = shelter.Email;
            ViewBag.phonenumber = shelter.PhoneNumber;
            ViewBag.username = shelter.UserName;
            return View(products);
        }

    }
}
