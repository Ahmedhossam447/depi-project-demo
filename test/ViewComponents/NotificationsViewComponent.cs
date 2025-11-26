using Microsoft.AspNetCore.Mvc;

namespace test.ViewComponents
{
    public class NotificationsViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationsViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            int notificationCount = _httpContextAccessor.HttpContext.Session.GetInt32("NotificationCount") ?? 0;
            return View(notificationCount);
        }
    }
}
