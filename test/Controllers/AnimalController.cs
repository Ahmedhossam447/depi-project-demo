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
using Microsoft.AspNetCore.Identity;

namespace test.Controllers
{
    [Authorize(Roles = "User")]
    public class AnimalController : Controller
    {
        private readonly IAnimal _animalRepository;
        private readonly PhotoServices _photoServices;
        private readonly UserManager<IdentityUser> _userManager;
        public AnimalController(IAnimal animalRepository, PhotoServices photoServices, UserManager<IdentityUser> userManager)
        {
            _animalRepository = animalRepository;
            _photoServices = photoServices;
            _userManager = userManager;
        }
        public IActionResult Index(String? filter, bool mine)
        {
            var userid = _userManager.GetUserId(User);
            var animalviewmodel = _animalRepository.AnimalDisplay(filter, userid, mine);
            ViewBag.Userid = userid;

            return View(animalviewmodel);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Type,Age,Photo")] CreateAnimalViewModel animalVM)
        {

            if (ModelState.IsValid)
            {
                var photoresult = await _photoServices.AddPhotoAsync(animalVM.Photo);
                var userid = _userManager.GetUserId(User);
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
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var animal = await _animalRepository.GetByIdAsync(id);
            if (animal == null)
            {
                return View("index");
            }
            var animalVM = new AnimalEditViewModel
            {
                Id = animal.AnimalId,
                Name = animal.Name,
                Type = animal.Type,
                Age = animal.Age
            };
            return View(animalVM);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(AnimalEditViewModel animalVM)
        {
            if (ModelState.IsValid)
            {
                var animal = await _animalRepository.GetByIdAsync(animalVM.Id);
                if (animal == null)
                {
                    return View("Index");
                }
                animal.Name = animalVM.Name;
                animal.Type = animalVM.Type;
                animal.Age = animalVM.Age;
                if (animalVM.Photo != null)
                {
                    await _photoServices.DeletePhotoAsync(animal.Photo);
                    var photoresult = await _photoServices.AddPhotoAsync(animalVM.Photo);
                    animal.Photo = photoresult.Url.ToString();
                }
                bool mine = true;
                _animalRepository.savechanges();
                return RedirectToAction("Index", new { mine = mine });
            }
            return View(animalVM);

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var animal = await _animalRepository.GetByIdAsync(id);
            if (animal == null)
            {
                return View("Index");
            }
            await _photoServices.DeletePhotoAsync(animal.Photo);
            await _animalRepository.DeleteAnimal(animal);
            return RedirectToAction("Index");
        }
    }
}
