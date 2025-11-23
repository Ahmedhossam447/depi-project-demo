using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using test.Data;
using test.Models;
using test.ModelViews;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace test.Controllers
{
    public class PaymentMethodController : Controller
    {
        private readonly DepiContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PaymentMethodController(DepiContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePaymentMethodViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Basic Card Type Detection
                string cardType = "Unknown";
                string cleanNumber = model.CardNumber.Replace(" ", "");
                
                if (cleanNumber.StartsWith("4"))
                {
                    cardType = "Visa";
                }
                else if (cleanNumber.StartsWith("5"))
                {
                    cardType = "MasterCard";
                }

                var paymentMethod = new PaymentMethods
                {
                    UserId = user.Id,
                    MethodType = cardType,
                    last4Digits = cleanNumber.Length >= 4 ? cleanNumber.Substring(cleanNumber.Length - 4) : cleanNumber,
                    expiryMonth = model.ExpiryMonth.ToString("00"),
                    expiryYear = model.ExpiryYear.ToString(),
                    GatewatyToken = "tok_test_placeholder_" + Guid.NewGuid().ToString().Substring(0, 8)
                };

                _context.PaymentMethods.Add(paymentMethod);
                await _context.SaveChangesAsync();

                // Redirect back to the payment process page
                return RedirectToAction("ProccessPayment", "Transaction");
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);
            if (paymentMethod == null)
            {
                return RedirectToAction("ProccessPayment", "Transaction");
            }
            var model = new EditPaymentMethodViewModel
            {
                PaymentMethodId = paymentMethod.PaymentMethodId,
                ExpiryMonth = int.Parse(paymentMethod.expiryMonth),
                ExpiryYear = int.Parse(paymentMethod.expiryYear)
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (EditPaymentMethodViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var paymentMethod = await _context.PaymentMethods.FindAsync(model.PaymentMethodId);

            if (paymentMethod == null)
            {
                ModelState.AddModelError("", "Payment method not found.");
                return (RedirectToAction("ProccessPayment", "Transaction"));
            }
            paymentMethod.expiryMonth = model.ExpiryMonth.ToString("00");
            paymentMethod.expiryYear = model.ExpiryYear.ToString();
            _context.PaymentMethods.Update(paymentMethod);
            await _context.SaveChangesAsync();
            return RedirectToAction("ProccessPayment", "Transaction");


        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var paymentmethod =await _context.PaymentMethods.FirstOrDefaultAsync(p => p.PaymentMethodId == id);
            if (paymentmethod == null)
            {
                ModelState.AddModelError(string.Empty, "The payment method already deleted");
                return RedirectToAction("ProccessPayment", "Transaction");
            }
             _context.PaymentMethods.Remove(paymentmethod);
          await  _context.SaveChangesAsync();
            return RedirectToAction("ProccessPayment", "Transaction");
        }

    }
}
