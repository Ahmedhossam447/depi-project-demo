using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Interfaces;
using test.Models;
using test.ModelViews;
using test.Repository;

namespace test.Controllers
{
    public class TransactionController : Controller
    { private readonly ITransaction _transactionRepository;
        private readonly IOrder _orderRepository;
        private readonly DepiContext _context;
        private readonly UserManager<IdentityUser> _usermanager;

        public TransactionController(ITransaction transaction, IOrder orderRepository,DepiContext depiContext,UserManager<IdentityUser> userManager)
        {
            _transactionRepository = transaction;
            _orderRepository = orderRepository;
            _context = depiContext;
            _usermanager = userManager;
        }

        [HttpGet]
public IActionResult ProccessPayment()
        {
            var userid =_usermanager.GetUserId(User);
            var paymentmethods = _context.PaymentMethods.Where(p => p.UserId == userid).ToList();
            var order = _context.Orders.FirstOrDefault(o=>o.UserId==userid && o.OrderStatus==0);
            var orderdetails = new List<OrderDetails>();
            orderdetails = _context.OrderDetails
                .Where(od => od.OrderId == order.OrderId)
                .Include(od => od.Product)
                .ToList();
            var model = new ProccessPaymentViewModel
            {
                paymentMethods = paymentmethods,
                orderid= order.OrderId,
                totalprice= order.TotalPrice,
                orderDetails = orderdetails
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ProccessPayment(ProccessPaymentViewModel model)
        {
           var order=await _orderRepository.GetOrderFortransaction(model.orderid);
            var payment= _context.PaymentMethods.FirstOrDefault(p => p.PaymentMethodId == model.selectedPaymentMethodid);
            var orderdetails = new List<OrderDetails>();
            orderdetails = _context.OrderDetails
                .Where(od => od.OrderId == order.OrderId)
                .Include(od => od.Product)
                .ToList();
            _transactionRepository.beginTransaction();
            try
            {
                if (order == null)
                {
                    var Message = "Order not found.";
                    return Json(new { status = "failed", message = Message });
                }
                if (order.OrderStatus == 1)
                {
                    var Message = "Order is already paid.";
                    return Json(new { status = "failed", message = Message });
                }
                foreach (var item in orderdetails)
                {
                    
                    if (item.Product == null)
                    {
                        _transactionRepository.rollbackTransaction();
                        var Message = $"Product with ID {item.productId} not found.";
                        return Json(new { status = "failed", message = Message });
                    }
                    if (item.Product.Quantity < item.Quantity)
                    {
                        _transactionRepository.rollbackTransaction();
                        var Message = $"Insufficient stock for product {item.Product.Type}. Available stock: {item.Product.Quantity}, Requested quantity: {item.Quantity}.";
                        return Json(new { status = "failed", message = Message });
                    }
                }
                // Simulate payment processing logic here
                var paymentresult = _transactionRepository.AddTransaction(new Transactions
                {
                    OrderId = order.OrderId,
                    TransactionDate = DateTime.Now,
                    Amount = order.TotalPrice,
                    PaymentMethod = payment,
                    Status = "Paid"

                });

                if (paymentresult)
                {
                    order.OrderStatus = 1;
                    foreach(var item in orderdetails)
                    {
                        item.Product.Quantity -= item.Quantity;
                    }

                    await _transactionRepository.savechangesAsync();
                    _transactionRepository.commitTransaction();
                    var Message = "Payment processed successfully.";
                    HttpContext.Session.SetInt32("CartCount", 0);
                    return Json(new { status = "succeeded", message = Message });
                }

                else
                {
                    _transactionRepository.rollbackTransaction();
                    var Message = "Payment processing failed.";
                    return Json(new {status="failed",message=Message});
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _transactionRepository.rollbackTransaction();
                var Message = "The item went out of stock while processing the payment";
                return Json(new { status = "failed", message = Message });
            }
            catch (Exception ex)
            {
                _transactionRepository.rollbackTransaction();
                return Json(new { status = "failed", message = ex.Message });
            }
            

        }
    }
}
