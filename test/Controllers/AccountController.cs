using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
using test.Repository;

namespace test.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccounts _accountRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IEmailSender emailSender;
        

        public AccountController (IAccounts accountRepository, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager ,IEmailSender emailSender)
        {
            _accountRepository = accountRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
        }
        public IActionResult login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            var user1 = await userManager.FindByNameAsync(user.email);
            if (user1 != null && !user1.EmailConfirmed && await userManager.CheckPasswordAsync(user1, user.password))
            {
                ModelState.AddModelError(string.Empty, "email not confirmed yet");
                return View(user);
            }
            var result = await signInManager.PasswordSignInAsync(user.email, user.password, true, false);
            if (result.Succeeded &&User.IsInRole("User"))
            {
                return RedirectToAction("index","Animal");
            }
            else if (result.Succeeded && User.IsInRole("Shelter"))
            {
                return RedirectToAction("index", "Shelter");
            }
            else
            {
                    ModelState.AddModelError(string.Empty, "invalid attempt");
                
                return View(user);
            }
        }
    


        public async Task<IActionResult> logout()
        {
          await signInManager.SignOutAsync();
            return RedirectToAction("login");

        }
        public IActionResult register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> register(registerviewmodel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            var userr = new IdentityUser { UserName = user.username, Email = user.email };
            var result = await userManager.CreateAsync(userr, user.password);

            if (result.Succeeded)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(userr);
                userr.PhoneNumber = user.phonenumber;
                var confirmationlink = Url.Action("ConfirmEmail", "Account", new { Userid = userr.Id, token = token }, Request.Scheme);
                await emailSender.SendEmailAsync(userr.Email, "email confirmation", confirmationlink);
                await userManager.AddToRoleAsync(userr, user.role);
                ModelState.AddModelError(string.Empty, "Please confirm your email");
                return RedirectToAction("login");
            }
            else
            {
                foreach(var errors in result.Errors) {
                    ModelState.AddModelError(string.Empty, errors.Description);
                }
                return View(user);
            }
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "home");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("NotFound");
            }
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }

            return View("NotFound");

        }
        
        
    }
}
