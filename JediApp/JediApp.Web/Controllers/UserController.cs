using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Services.Services.Interfaces;
using JediApp.Services.Services.Service;
using JediApp.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace JediApp.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly JediAppDbContext _jediAppDb;

        public UserController(UserService userService, IServiceProvider userRepository, JediAppDbContext jediAppDb)
        {
            _userService = userService;
            _userRepository = ActivatorUtilities.GetServiceOrCreateInstance<IUserRepository>(userRepository);
            _jediAppDb = jediAppDb;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["activePage"] = "User";

            var users = await _userRepository.GetAllUsers();

            return View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userRepository.GetUserById(id);

            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userRepository.GetUserById(id);

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
            var user =await _userRepository.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            _userRepository.DeleteUser(user);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user =await _userRepository.GetUserById(id);

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
            if (ModelState.IsValid)
            {
                var updateUser =await _userService.UpdateUser(user);

                _jediAppDb.Update(updateUser);

                await _jediAppDb.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
