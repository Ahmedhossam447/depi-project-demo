using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using test.Data;
using test.Interfaces;
using test.Models;

namespace test.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IContact _contactRepository;
        private readonly IAnimal _animalRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DepiContext _context;

        public AdminController(IContact contactRepository, IAnimal animalRepository, UserManager<ApplicationUser> userManager,DepiContext depi)
        {
            _contactRepository = contactRepository;
            _animalRepository = animalRepository;
            _userManager = userManager;
            _context = depi;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _contactRepository.GetAllMessagesAsync();
            return View(messages);
        }

        public async Task<IActionResult> Animals()
        {
            var animals = await _animalRepository.GetAllAnimalsAsync();
            return View(animals);
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditAnimal(int id)
        {
            var animal = await _animalRepository.GetByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            return View(animal);
        }

        [HttpPost]
        public async Task<IActionResult> EditAnimal(Animal animal)
        {
            if (ModelState.IsValid)
            {
                var existingAnimal = await _animalRepository.GetByIdAsync(animal.AnimalId);
                if (existingAnimal == null)
                {
                    return NotFound();
                }

                existingAnimal.Name = animal.Name;
                existingAnimal.Type = animal.Type;
                existingAnimal.Age = animal.Age;
                existingAnimal.Photo = animal.Photo;

                await _animalRepository.UpdateAnimal(existingAnimal);
                return RedirectToAction("Animals");
            }
            return View(animal);
        }

        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _animalRepository.GetByIdAsync(id);
            if (animal != null)
            {
                await _animalRepository.DeleteAnimal(animal);
            }
            return RedirectToAction("Animals");
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var orders = await _context.Orders.Where(o => o.UserId == id).ToListAsync();
                if (orders.Any())
                {
                    _context.Orders.RemoveRange(orders);
                    await _context.SaveChangesAsync();
                }
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Users");
        }
    }
}
