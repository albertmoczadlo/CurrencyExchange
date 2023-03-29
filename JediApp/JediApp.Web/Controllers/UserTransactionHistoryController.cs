using iText.Html2pdf;
using JediApp.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JediApp.Web.Controllers
{
    public class UserTransactionHistoryController : Controller
    {
        private readonly ITransactionHistoryService _transactionHistoryService;

        public UserTransactionHistoryController(ITransactionHistoryService transactionHistoryService)
        {
            _transactionHistoryService = transactionHistoryService;
        }
        public IActionResult Index()
        {
            ViewData["activePage"] = "UserTransactionHistory";

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
