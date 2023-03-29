using iText.Html2pdf;
using JediApp.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JediApp.Web.Controllers
{
    public class AdminTransactionHistoryController : Controller
    {
        private readonly ITransactionHistoryService _transactionHistoryService;

        public AdminTransactionHistoryController(ITransactionHistoryService transactionHistoryService)
        {
            _transactionHistoryService = transactionHistoryService;
        }
        public IActionResult Index()
        {
            ViewData["activePage"] = "AdminTransactionHistory";

            var model = _transactionHistoryService.GetAllUsersHistories();

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

            return RedirectToAction(nameof(Index));
        }
    }
}
