using VistulaExchange.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VistulaExchange.Web.Controllers
{
    public class UserAccountBalance : Controller
    {
        private readonly IUserDashboardService _userDashboardService;

        public UserAccountBalance(IUserDashboardService userDashboardService)
        {
            _userDashboardService = userDashboardService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["activePage"] = "AccountBalance";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var model = await _userDashboardService.GetDashboardAsync(userId);
            return View(model);
        }
    }
}
