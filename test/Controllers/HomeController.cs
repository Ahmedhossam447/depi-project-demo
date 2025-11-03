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

        public HomeController()
        {
            _context = new DepiContext();
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
    }
}
