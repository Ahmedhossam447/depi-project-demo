using Microsoft.AspNetCore.Authentication;
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
    public class RequestController : Controller
    {
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IRequests _RequestRepository;
        private readonly IAnimal _animalRepository;
        public RequestController(IRequests RequestsRepository,UserManager<ApplicationUser> userManager, IAnimal animalRepository)
        {
            _usermanager = userManager;
            _RequestRepository = RequestsRepository;
            _animalRepository = animalRepository;
        }
        public async Task<IActionResult> Index()
        {
            var requests =await _RequestRepository.LoadRequests();
            var useridclaim = User.FindFirst("ID");
            var userid = _usermanager.GetUserId(User);
            var usersrequested = _RequestRepository.RequestGot(userid, requests);
            var animals = _RequestRepository.AnimalsNeeded(userid, requests);
            var users= _RequestRepository.RequestSent(userid, requests);
            ViewBag.usersrequested = usersrequested;
            ViewBag.animals = animals;
            ViewBag.users = users;
            ViewBag.userid = userid;

            return View(requests);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Request request)
        {
            if (ModelState.IsValid)
            {
                await _RequestRepository.addRequest(request);
                return RedirectToAction("Index","Animal");
            }
            return RedirectToAction("Index", "Animal");
        }
        [HttpPost]
        public async Task<IActionResult> approve(int id)
        {
            if(await _RequestRepository.approverequest(id))
            return RedirectToAction("Index");
            else
                return NotFound();

        }
        [HttpPost]
        public async Task<IActionResult> reject(int id)
        {
            if (await _RequestRepository.rejectRequest(id))
                return RedirectToAction("Index");
            else
                return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var request = await _RequestRepository.GetRequestById(id);
            if (request != null)
            {
                await _RequestRepository.DeleteRequest(request);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CompleteAdoption(int id)
        {
            var request = await _RequestRepository.GetRequestById(id);
            if (request != null)
            {
                var animal = await _animalRepository.GetByIdAsync(request.AnimalId);
                if (animal != null)
                {
                    await _animalRepository.DeleteAnimal(animal);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
