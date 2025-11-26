using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Data;

namespace test.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DepiContext _context;

        public ChatController(UserManager<IdentityUser> userManager, DepiContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index(string? receiverId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(receiverId))
            {
                return View();
            }

            var hasApprovedRequest = await _context.Requests.AnyAsync(r => 
                r.Status == "approved" &&
                ((r.Userid == currentUser.Id && r.Useridreq == receiverId) || 
                 (r.Userid == receiverId && r.Useridreq == currentUser.Id)));

            if (!hasApprovedRequest)
            {
                return RedirectToAction("Index", "Home");
            }

            var messages = await _context.ChatMessages
                .Where(m => (m.SenderId == currentUser.Id && m.ReceiverId == receiverId) ||
                            (m.SenderId == receiverId && m.ReceiverId == currentUser.Id))
                .OrderBy(m => m.Time)
                .ToListAsync();

            var receiver = await _userManager.FindByIdAsync(receiverId);
            ViewBag.ReceiverName = receiver?.UserName ?? "Unknown User";

            ViewBag.ReceiverId = receiverId;
            ViewBag.CurrentUserId = currentUser.Id;
            return View(messages);
        }
    }
}
