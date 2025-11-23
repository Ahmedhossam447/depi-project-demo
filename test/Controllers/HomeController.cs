using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace test.Controllers
{
    public class HomeController : Controller
    {
        private readonly DepiContext _context;
        private readonly test.Interfaces.IContact _contactRepository;

        public HomeController(test.Interfaces.IContact contactRepository)
        {
            _context = new DepiContext();
            _contactRepository = contactRepository;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult FAQs()
        {
            return View();
        }
        public IActionResult contactus()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactMessage message1)
        {
            if (ModelState.IsValid)
            {
                await _contactRepository.AddMessageAsync(message1);
                TempData["SuccessMessage"] = "Your message has been sent successfully!";
                return RedirectToAction("contactus");
            }
            return View("contactus", message1);
        }
    }
}
