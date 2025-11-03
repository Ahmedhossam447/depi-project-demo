using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Interfaces;
using test.Models;

namespace test.Controllers
{
    public class TransactionController : Controller
    { private readonly ITransaction _transactionRepository;
        private readonly IOrder _orderRepository;
        public TransactionController(ITransaction transaction, IOrder orderRepository)
        {
            _transactionRepository = transaction;
            _orderRepository = orderRepository;
        }
        public async Task<IActionResult> ProccessPayment(int orderid,PaymentMethods payment)
        {
           var order=await _orderRepository.GetOrderFortransaction(orderid);
            _transactionRepository.beginTransaction();
            try
            {
                if (order == null)
                {
                    return NotFound("Order not found.");
                }
                if (order.OrderStatus == 1)
                {
                    return BadRequest("Order is already paid.");
                }
                if (order.Product == null)
                {
                    return NotFound("Product associated with the order not found.");
                }
                if (order.Product.Quantity < order.Quantity)
                {
                    return BadRequest("Insufficient stock for the product.");
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
                    return Ok("Payment processed successfully.");
                }
                else
                {
                    _transactionRepository.rollbackTransaction();
                    return StatusCode(500, "Failed to record the transaction.");
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _transactionRepository.rollbackTransaction();
                return BadRequest("sorry the item just went out of stock");
            }
            catch (Exception ex)
            {
                _transactionRepository.rollbackTransaction();
                return StatusCode(500, "An error occurred while processing the payment: " + ex.Message);
            }
            

        }
    }
}
