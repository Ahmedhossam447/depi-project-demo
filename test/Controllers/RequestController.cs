using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IRequests _RequestRepository;
        public RequestController(IRequests RequestsRepository)
        {
            _RequestRepository = RequestsRepository;
        }
        public async Task<IActionResult> Index()
        {
            var requests =await _RequestRepository.LoadRequests();
            var useridclaim = User.FindFirst("ID");
            var userid = int.Parse(useridclaim.Value);
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
        public async Task<IActionResult> Create([Bind("AnimalId,Useridreq")]Request request)
        {
            if (ModelState.IsValid)
            {
                var useridclaim = User.FindFirst("ID");
                request.Userid = int.Parse(useridclaim.Value);
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
    }
}
