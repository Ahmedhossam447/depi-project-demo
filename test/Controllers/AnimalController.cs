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
using test.Repository;
using test.Interfaces;

namespace test.Controllers
{
    [Authorize]
    public class AnimalController : Controller
    {
        private readonly IAnimal _animalRepository;
        private readonly PhotoServices _photoServices;
        public AnimalController(IAnimal animalRepository,PhotoServices photoServices)
        {
            _animalRepository = animalRepository;
            _photoServices = photoServices;
        }
        public  IActionResult Index(String? filter)
        {
            var useridclaim = User.FindFirst("ID");
            int userid = int.Parse(useridclaim.Value);
            var animalviewmodel = _animalRepository.AnimalDisplay(filter,userid);

            return View(animalviewmodel);
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

                _animalRepository.AddAnimal(animal);
                return RedirectToAction("Index");
            }
            return View(animalVM);
        }

    }
}
