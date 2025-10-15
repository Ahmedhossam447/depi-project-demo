using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test.Data;
using System.Security.Claims;
using test.Models;
using Microsoft.AspNetCore.Authentication;
using test.ModelViews;
using test.Repository;
using test.Interfaces;

namespace test.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccounts _accountRepository;

        public AccountController (IAccounts accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public IActionResult login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> login([Bind("email,password")]LoginViewModel view)
        {
            if (!ModelState.IsValid) {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(view);
            }
            var user = await _accountRepository.SignIn(view);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "IInvalid username or password.");
                return View(view);
            }
            var cliams = new List<Claim>();
            cliams.Add(new Claim(ClaimTypes.Name, user.Username));
            cliams.Add(new Claim("ID", user.Id.ToString()));
            cliams.Add(new Claim(ClaimTypes.Role, user.Role));
            var identity = new ClaimsIdentity(cliams, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("MyCookieAuth", principal);

            if (user.Role=="Shelter")
            return RedirectToAction("Index","Shelter");
            return RedirectToAction("Index", "Animal");

        }
        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("login");
        }
        public IActionResult register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> register([Bind("Id,Username,Email,Password,Role,Phonenumber")] registerviewmodel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            if (await _accountRepository.adduser(user))
            {
                return RedirectToAction("login");
            }
            else ModelState.AddModelError(string.Empty, "This email address is already in use.");
            {
                return View(user);
            }
        }
    }
}
