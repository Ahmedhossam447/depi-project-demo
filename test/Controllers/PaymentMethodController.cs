using Microsoft.AspNetCore.Mvc;

namespace test.Controllers
{
    public class PaymentMethodController : Controller
    {
        public IActionResult create(string Userid)
        {
            return View();
        }
    }
}
