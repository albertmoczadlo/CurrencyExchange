using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Database.Repositories;
using JediApp.Services.Services;
using JediApp.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


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

        // GET: /Movies/Edit/5
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

        //public async Task<IActionResult> Edit(string id)
        //{
        //    var user =await _userRepository.GetUserById(id);

        //    return user== null ? NotFound() : View(user);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public async Task<IActionResult> Edit(User user)
        //{
        //    //var userOld =await _userRepository.GetUserById(id); 


        //    if (user == null) 
        //    {
        //        return NotFound();
        //    }

        //    _userRepository.AddUser(user);

        //    //_userService.UpdateUser(user);

        //    return RedirectToAction("Index");
        //}

        //public async Task<IActionResult> Edit(Guid? id)
        //{
        //    if (id == null || _dbContext.Users == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _dbContext.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["UserId"] = new SelectList(_dbContext.Set<User>(), "Id", "Id", user.Id);
        //    return View(user);
        //}

        //public async Task<IActionResult> Edit(string id, [Bind("FirstName,LastName")] User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _dbContext.Update(user);
        //            await _dbContext.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UsersExists(user.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UserId"] = new SelectList(_dbContext.Set<User>(), "Id", "Id", user.Id);
        //    return View(user);
        //}

        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null || _dbContext.Users == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _dbContext.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    if (_dbContext.Users == null)
        //    {
        //        return Problem("Entity set 'JediAppDbContextConnection.User'  is null.");
        //    }
        //    var user = await _dbContext.Users.FindAsync(id);
        //    if (user != null)
        //    {
        //        _dbContext.Users.Remove(user);
        //    }

        //    await _dbContext.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UsersExists(string id)
        //{
        //    return _dbContext.Users.Any(e => e.Id == id);
        //}

    }
}
