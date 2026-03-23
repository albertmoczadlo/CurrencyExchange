using iText.Html2pdf;
using VistulaExchange.Database.Domain;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Web.Areas.Identity.Data;
using VistulaExchange.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace VistulaExchange.Web.Controllers
{
    public class AdminReportController : Controller
    {
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly VistulaExchangeDbContext _jediAppDb;

        public AdminReportController(ITransactionHistoryService transactionHistoryService, VistulaExchangeDbContext jediAppDb)
        {
            _transactionHistoryService = transactionHistoryService;
            _jediAppDb = jediAppDb;
        }
        public IActionResult Index()
        {
            ViewData["activePage"] = "AdminReport";

            var allUsersHistories = _transactionHistoryService.GetAllUsersHistories();
            var now = DateTime.Now;
            var model = new AdminReport();

            model.TurnoverPLN = allUsersHistories.Where(x => x.CurrencyName == "PLN").Sum(x => x.Amount);
            model.TurnoverEUR = allUsersHistories.Where(x => x.CurrencyName == "EUR").Sum(x => x.Amount);
            model.TurnoverUSD = allUsersHistories.Where(x => x.CurrencyName == "USD").Sum(x => x.Amount);
            model.TurnoverGBP = allUsersHistories.Where(x => x.CurrencyName == "GBP").Sum(x => x.Amount);
            model.TurnoverCHF = allUsersHistories.Where(x => x.CurrencyName == "CHF").Sum(x => x.Amount);
            model.TurnoverCZK = allUsersHistories.Where(x => x.CurrencyName == "CZK").Sum(x => x.Amount);

            model.AnnualTransactions = allUsersHistories.Count(x => x.DateOfTransaction.Year == now.Year);
            model.MonthlyTransactions = allUsersHistories.Count(x => x.DateOfTransaction.Year == now.Year && x.DateOfTransaction.Month == now.Month);
            model.DailyTTransactions = allUsersHistories.Count(x => x.DateOfTransaction.Date == now.Date);

            model.UserAccounts = _jediAppDb.Users.Count();
            model.UserEmailConfirmed = _jediAppDb.Users.Count(x => x.EmailConfirmed);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPDF(string GridHtml)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                HtmlConverter.ConvertToPdf(GridHtml, stream);
                return File(stream.ToArray(), "application/pdf", "Exchange_Office_Report.pdf");
            }
        }
    }
}
