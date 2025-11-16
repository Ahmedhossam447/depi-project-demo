using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using test.Data;
using test.Interfaces;
using test.Models;
using test.ModelViews;

namespace test.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IShelter _shelterRepository;
        private readonly IOrder _orderRepository;
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly DepiContext _context;
        public OrderController(IShelter shelterRepository, IOrder orderRepository, UserManager<IdentityUser> userManager,DepiContext depiContext)
        {
            _shelterRepository = shelterRepository;
            _orderRepository = orderRepository;
            _usermanager = userManager;
            _context = depiContext;
        }
        [HttpGet]
        public IActionResult create(string userid ,int productid ,int quantity ,string productname ,int price)
        {
            var paymentmethods = _context.PaymentMethods.Where(p=>p.UserId == userid).ToList();
            CreateOrderViewModel model = new CreateOrderViewModel
            {
                ProductId = productid,
                Quantity = quantity,
                UserId = userid,
                productName = productname,
                productPrice = price,
                paymentMethods = paymentmethods
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> create(CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _usermanager.FindByIdAsync(model.UserId);
                var product = await _shelterRepository.GetProductbyId(model.ProductId);
                if (user == null || product == null)
                {
                    return NotFound();
                }
                var order = new Orders
                {
                    UserId = user.Id,
                    ProductId = product.Productid,
                    Quantity = model.Quantity,
                    TotalPrice = (model.Quantity * (int)product.Price),
                    OrderDate = DateTime.Now,
                    OrderStatus = 0
                };
                var result = _orderRepository.AddOrder(order);
                if (result)
                {
                    return RedirectToAction("ProccessPayment", "Transaction", new {orderid=order.OrderId,paymentid=model.selectedPaymentMethodid});
                }
            }
            else
            {
                return View(model);
            }
            return NotFound();
        }
    }
}
