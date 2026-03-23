using iText.Html2pdf;
using VistulaExchange.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VistulaExchange.Web.Controllers
{
    public class AdminTransactionHistoryController : Controller
    {
        private readonly ITransactionHistoryService _transactionHistoryService;

        public AdminTransactionHistoryController(ITransactionHistoryService transactionHistoryService)
        {
            _transactionHistoryService = transactionHistoryService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["activePage"] = "AdminTransactionHistory";

            var model = await _transactionHistoryService.GetAllUsersHistoriesAsync();

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPDF(string GridHtml)
        {

            using (MemoryStream stream = new MemoryStream())
            {
                HtmlConverter.ConvertToPdf(GridHtml, stream);
                return File(stream.ToArray(), "application/pdf", "All_Transactions_History.pdf");
            }
        }
    }
}
