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
using test.Services;
using test.ModelViews;

namespace test.Controllers
{
    [Authorize]
    public class AnimalController : Controller
    {
        private readonly DepiContext _context;
        private readonly PhotoServices _photoServices;
        public AnimalController(DepiContext context,PhotoServices photoServices)
        {
            _context = context;
            _photoServices = photoServices;
        }
        public async Task<IActionResult> Index(String? filter)
        {
            IQueryable<Animal> animals;

            if (filter == null || filter == "any")
            {
                var useridclaim = User.FindFirst("ID");
                int userid = int.Parse(useridclaim.Value);
                 animals = _context.Animals.FromSql($"select name,type,age,animalid,photo,userid from Animals as a where userid != {userid} and NOT EXISTS(select animalid from requests where userid = {userid} and animalID= a.animalID)");

            }
            else
            {
                var useridclaim = User.FindFirst("ID");
                int userid = int.Parse(useridclaim.Value);
                 animals =  from anm in _context.Animals where anm.Userid! != userid && anm.Type == filter && (_context.Requests.FirstOrDefault(r => r.Userid == userid && r.AnimalId == anm.AnimalId) == null) select anm;
            }
            var animviewmodel = new ModelViews.Animalviewmodel
            {
                animals = animals,
                filter = filter ?? "any"
            };
            return View(animviewmodel);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create ([Bind("Name,Type,Age,Photo")] CreateAnimalViewModel animalVM)
        {

            if (ModelState.IsValid)
            {
                var photoresult = await _photoServices.AddPhotoAsync(animalVM.Photo);
                var useridclaim = User.FindFirst("ID");
                int userid = int.Parse(useridclaim.Value);
                var animal = new Animal
                {
                    Name = animalVM.Name,
                    Type = animalVM.Type,
                    Age = animalVM.Age,
                    Photo = photoresult.Url.ToString(),
                    Userid = userid
                };

                await _context.Animals.AddAsync(animal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(animalVM);
        }

    }
}
