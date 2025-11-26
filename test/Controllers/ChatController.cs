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

            // Mark unread messages from receiver as read
            var unreadMessages = messages.Where(m => m.ReceiverId == currentUser.Id && m.read == 0).ToList();
            if (unreadMessages.Any())
            {
                foreach (var msg in unreadMessages)
                {
                    msg.read = 1;
                }
                await _context.SaveChangesAsync();

                // Update Notification Count in Session
                var notificationCount = await _context.ChatMessages
                    .Where(m => m.ReceiverId == currentUser.Id && m.read == 0)
                    .Select(m => m.SenderId)
                    .Distinct()
                    .CountAsync();
                HttpContext.Session.SetInt32("NotificationCount", notificationCount);
            }

            var receiver = await _userManager.FindByIdAsync(receiverId);
            ViewBag.ReceiverName = receiver?.UserName ?? "Unknown User";

            ViewBag.ReceiverId = receiverId;
            ViewBag.CurrentUserId = currentUser.Id;
            return View(messages);
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationCount()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Content("");

            var notificationCount = await _context.ChatMessages
                .Where(m => m.ReceiverId == currentUser.Id && m.read == 0)
                .Select(m => m.SenderId)
                .Distinct()
                .CountAsync();

            HttpContext.Session.SetInt32("NotificationCount", notificationCount);

            return ViewComponent("Notifications");
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUserNotifications()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Json(new List<object>());

            var unreadSenders = await _context.ChatMessages
                .Where(m => m.ReceiverId == currentUser.Id && m.read == 0)
                .GroupBy(m => m.SenderId)
                .Select(g => new { SenderId = g.Key, Count = g.Count() })
                .ToListAsync();

            var notifications = new List<object>();

            foreach (var item in unreadSenders)
            {
                var sender = await _userManager.FindByIdAsync(item.SenderId);
                if (sender != null)
                {
                    notifications.Add(new
                    {
                        userId = item.SenderId,
                        userName = sender.UserName,
                        unreadCount = item.Count
                    });
                }
            }

            return Json(notifications);
        }
    }
}
