using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace VistulaExchange.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public UserController(IUserService userService, IUserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["activePage"] = "User";

            var users = await _userRepository.GetAllUsersAsync();

            return View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteUserAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditConfirmed(User user)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", user);
            }

            var updatedUser = await _userService.UpdateUserAsync(user);

            if (updatedUser == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
