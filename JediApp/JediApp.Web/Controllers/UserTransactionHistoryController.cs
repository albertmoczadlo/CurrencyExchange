using iText.Html2pdf;
using JediApp.Database.Domain;
using JediApp.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JediApp.Web.Controllers
{
    public class UserTransactionHistoryController : Controller
    {
        private readonly ITransactionHistoryService _transactionHistoryService;
        //private readonly UserManager<User> _userManager;


        public UserTransactionHistoryController(ITransactionHistoryService transactionHistoryService)
        //public UserTransactionHistory(ITransactionHistoryService transactionHistoryService, UserManager<User> userManager)
        {
            _transactionHistoryService = transactionHistoryService;
            //_userManager = userManager;
        }
        public IActionResult Index()
        {
            ViewData["activePage"] = "UserTransactionHistory";

            //753af7b5-9592-4153-a076-5086845c3491

            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = _transactionHistoryService.GetUserHistoryByUserId(userID);

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPDF(string GridHtml)
        {

            var userName = User.Identity.Name;

            using (MemoryStream stream = new MemoryStream())
            {
                HtmlConverter.ConvertToPdf(GridHtml, stream);
                return File(stream.ToArray(), "application/pdf", $"{ userName }_Transactions_History.pdf");
            }

            return RedirectToAction(nameof(Index));


        }


        

    }
}
