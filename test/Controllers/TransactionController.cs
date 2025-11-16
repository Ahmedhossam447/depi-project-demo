using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Interfaces;
using test.Models;

namespace test.Controllers
{
    public class TransactionController : Controller
    { private readonly ITransaction _transactionRepository;
        private readonly IOrder _orderRepository;
        private readonly DepiContext _context;
        public TransactionController(ITransaction transaction, IOrder orderRepository,DepiContext depiContext)
        {
            _transactionRepository = transaction;
            _orderRepository = orderRepository;
            _context = depiContext;
        }
        public async Task<IActionResult> ProccessPayment(int orderid,int paymentid)
        {
           var order=await _orderRepository.GetOrderFortransaction(orderid);
            var payment= _context.PaymentMethods.FirstOrDefault(p => p.PaymentMethodId == paymentid);
            _transactionRepository.beginTransaction();
            try
            {
                if (order == null)
                {
                    ViewBag.message = "Order not found.";
                    return View();
                }
                if (order.OrderStatus == 1)
                {
                    ViewBag.message = "Order is already paid.";
                    return View();
                }
                if (order.Product == null)
                {
                    ViewBag.Message = "Product not found for the order.";
                    return View();
                }
                if (order.Product.Quantity < order.Quantity)
                {
                    ViewBag.Message = "Insufficient stock for the product.";
                    return View();
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
                    order.Product.Quantity -= order.Quantity;
                    await _transactionRepository.savechangesAsync();
                    _transactionRepository.commitTransaction();
                    ViewBag.Message = "Payment processed successfully.";
                    return View(); 
                }

                else
                {
                    _transactionRepository.rollbackTransaction();
                    ViewBag.Message = "Payment processing failed.";
                    return View();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _transactionRepository.rollbackTransaction();
                ViewBag.Message = "The item went out of stock while processing the payment";
                return View();
            }
            catch (Exception ex)
            {
                _transactionRepository.rollbackTransaction();
                return StatusCode(500, "An error occurred while processing the payment: " + ex.Message);
            }
            

        }
    }
}
