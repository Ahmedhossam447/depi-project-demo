using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using test.Interfaces;
using test.ModelViews;

namespace test.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAnimal _animalRepository;
        private readonly IRequests _requestRepository;

        public ProfileController(UserManager<IdentityUser> userManager, IAnimal animalRepository, IRequests requestRepository)
        {
            _userManager = userManager;
            _animalRepository = animalRepository;
            _requestRepository = requestRepository;
        }

        public async Task<IActionResult> Index(string? userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            IdentityUser targetUser;
            bool isOwner = false;
            bool canViewContactInfo = false;

            if (string.IsNullOrEmpty(userId) || userId == currentUser.Id)
            {
                targetUser = currentUser;
                isOwner = true;
                canViewContactInfo = true;
            }
            else
            {
                targetUser = await _userManager.FindByIdAsync(userId);
                if (targetUser == null) return NotFound();

                isOwner = false;
                // Check if there is an accepted request between current user and target user
                canViewContactInfo = await _requestRepository.HasAcceptedRequest(currentUser.Id, targetUser.Id);
            }

            var roles = await _userManager.GetRolesAsync(targetUser);
            var role = roles.FirstOrDefault() ?? "User";

            var animals = await _animalRepository.GetAllUserAnimalsAsync(targetUser.Id);

            var viewModel = new UserProfileViewModel
            {
                User = targetUser,
                Animals = animals,
                Role = role,
                IsOwner = isOwner,
                CanViewContactInfo = canViewContactInfo
            };

            // Pass IsOwner to ViewData for the partial view
            ViewData["IsOwner"] = isOwner;
            ViewData["TargetUserId"] = targetUser.Id; // Needed for AJAX filter

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> FilterAnimals(string query, string? userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            string targetUserId = userId ?? currentUser.Id;
            
            
            var animals = await _animalRepository.GetAllUserAnimalsAsync(targetUserId);

            if (!string.IsNullOrEmpty(query))
            {
                animals = animals.Where(a => 
                    (a.Name != null && a.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) || 
                    (a.Type != null && a.Type.Contains(query, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            // Determine ownership for the partial view
            bool isOwner = targetUserId == currentUser.Id;
            ViewData["IsOwner"] = isOwner;

            return PartialView("_AnimalListPartial", animals);
        }
    }
}
