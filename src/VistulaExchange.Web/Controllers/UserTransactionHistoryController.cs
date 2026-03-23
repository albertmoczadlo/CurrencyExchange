using iText.Html2pdf;
using VistulaExchange.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VistulaExchange.Web.Controllers
{
    public class UserTransactionHistoryController : Controller
    {
        private readonly ITransactionHistoryService _transactionHistoryService;

        public UserTransactionHistoryController(ITransactionHistoryService transactionHistoryService)
        {
            _transactionHistoryService = transactionHistoryService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["activePage"] = "UserTransactionHistory";

            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userID))
            {
                return Challenge();
            }

            var model = await _transactionHistoryService.GetUserHistoryByUserIdAsync(userID);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPDF(string GridHtml)
        {
            var userName = User.Identity?.Name ?? "user";

            using (MemoryStream stream = new MemoryStream())
            {
                HtmlConverter.ConvertToPdf(GridHtml, stream);
                return File(stream.ToArray(), "application/pdf", $"{userName}_Transactions_History.pdf");
            }
        }
    }
}
