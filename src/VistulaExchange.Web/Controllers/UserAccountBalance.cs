using VistulaExchange.Database.Interface;
using VistulaExchange.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VistulaExchange.Web.Controllers
{
    public class UserAccountBalance : Controller
    {
        private readonly IUserWalletRepository _userWalletkRepository;

        public UserAccountBalance(IUserWalletRepository userWalletkRepository)
        {
            _userWalletkRepository = userWalletkRepository;
        }

        public IActionResult Index()
        {
            ViewData["activePage"] = "AccountBalance";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userWallet = _userWalletkRepository.GetWallet(userId);
            var userAccountBalance = userWallet.WalletPositions.ToList()
                .Select(a => new UserDeposit()
                {
                    Currency = a.Currency.ShortName,
                    Amount = a.CurrencyAmount
                });

            return View(userAccountBalance);
        }
    }
}
