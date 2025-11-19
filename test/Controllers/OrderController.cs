using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ServiceStack;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> create([FromForm] CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _usermanager.FindByIdAsync(model.UserId);
                var product = await _shelterRepository.GetProductbyId(model.ProductId);
                if (user == null || product == null)
                {
                    return Json(new { Message = "error" });
                }
                var orderExists = await _context.Orders
                    .FirstOrDefaultAsync(o => o.UserId == user.Id && o.OrderStatus == 0);
                if (orderExists == null)
                {
                     orderExists = new Orders
                    {
                        UserId = user.Id,
                        OrderDate = DateTime.Now,
                        OrderStatus = 0
                    };
                    await _context.Orders.AddAsync(orderExists);
                    await _context.SaveChangesAsync();
                }
                var orderdetails = new OrderDetails
                {
                    OrderId = orderExists.OrderId,
                    productId = product.Productid,
                    Quantity = model.Quantity,
                    TotalPrice = model.Quantity * (int)product.Price
                };
                _context.OrderDetails.Add(orderdetails);
                await _context.SaveChangesAsync();
                var cartcount = _context.OrderDetails.Where(o => o.OrderId == orderExists.OrderId).Sum(o=>o.Quantity);
                HttpContext.Session.SetInt32("CartCount", cartcount);
                return Json(new { Message = "success", CartCount=cartcount });
            }
            return Json(new { Message = "error" });

            
        }
        public async Task<IActionResult> orderdetails()
        {
            var userid = _usermanager.GetUserId(User);
            if (string.IsNullOrEmpty(userid))
            {
                return Unauthorized();
            }
            var orders = _context.Orders
                .Where(o => o.UserId == userid && o.OrderStatus == 0)
                .FirstOrDefault();
            if (orders == null)
            {
                return PartialView("_CartDetailsPartial", new List<OrderDetails>());
            }
            var orderdetails = _context.OrderDetails
                .Where(od => od.OrderId == orders.OrderId)
                .Include(od => od.Product)
                .ToList();
            orders.TotalPrice = orderdetails.Sum(od => od.TotalPrice);
            await _context.SaveChangesAsync();
            return PartialView("_CartDetailsPartial", orderdetails);
        }
        [HttpPost]
        public async Task<IActionResult> remove(int orderDetailsId)
        {
            var orderdetails = await _context.OrderDetails.FindAsync(orderDetailsId);
            if (orderdetails == null)
            {
                return Json(new { Message = "error" });
            }
            _context.OrderDetails.Remove(orderdetails);
            await _context.SaveChangesAsync();
            var userid = _usermanager.GetUserId(User);
            var order = _context.Orders
                .FirstOrDefault(o => o.UserId == userid && o.OrderStatus == 0);
            var cartcount = _context.OrderDetails.Where(o => o.OrderId == order.OrderId).Sum(o => o.Quantity);
            HttpContext.Session.SetInt32("CartCount", cartcount);
            return Json(new { Message = "success", CartCount = cartcount });
        }
    }
}
