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
using test.ModelViews;

namespace test.Controllers
{
    [Authorize]
    public class ShelterController : Controller
    {
        private readonly DepiContext _context;
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly IShelter _ShelterRepository;
        private readonly IAnimal _animalRepository;

        public ShelterController(DepiContext context, UserManager<IdentityUser> userManager, IShelter shelter, IAnimal animalRepository)
        {
            _usermanager = userManager;
            _context = context;
            _ShelterRepository = shelter;
            _animalRepository = animalRepository;
        }

        [Authorize(Roles = "Shelter")]
        public async Task<IActionResult> Index(string view)
        {
            var userid = _usermanager.GetUserId(User);
            List<Animal> animals=null;
            List<Product> products=null;
            if (view == "animals")
            {
                 animals = await _animalRepository.GetAllUserAnimalsAsync(userid);
            }
            else
            {
                 products = await _ShelterRepository.GetAllProducts(userid);
            }
            

            var viewModel = new ShelterIndexViewModel
            {
                Products = products,
                Animals = animals
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userid = _usermanager.GetUserId(User);
                var product = new Product
                {
                    Type= model.Type,
                    Disc = model.Disc,
                    Price = model.Price,
                    Userid = userid,
                    Quantity= model.Quantity
                };
                await _ShelterRepository.AddProduct(product);
                return RedirectToAction("Index");
            }
            return View(model);
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
            ViewBag.ShelterId = shelter.Id;
            ViewBag.email = shelter.Email;
            ViewBag.phonenumber = shelter.PhoneNumber;
            ViewBag.username = shelter.UserName;
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _ShelterRepository.GetProductbyId(id);

            if (product == null)
            {
                return NotFound();
            }
            var editModel = new EditProductViewModel
            {
                ProductId = product.Productid,
                Type = product.Type,
                Price = product.Price,
                Quantity = product.Quantity,
                Disc = product.Disc
            };
            return View(editModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                var existingProduct = await _ShelterRepository.GetProductbyId(model.ProductId);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                existingProduct.Type = model.Type;
                existingProduct.Price = model.Price;
                existingProduct.Quantity = model.Quantity;
                existingProduct.Disc = model.Disc;

                await _ShelterRepository.UpdateProduct(existingProduct);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _ShelterRepository.GetProductbyId(id);
            if (product != null)
            {
                await _ShelterRepository.RemoveProduct(product);
            }
            return RedirectToAction("Index");
        }
    }
}
