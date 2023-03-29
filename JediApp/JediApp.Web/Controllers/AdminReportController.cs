using iText.Html2pdf;
using JediApp.Services.Services;
using JediApp.Web.Areas.Identity.Data;
using JediApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using JediApp.Database.Domain;
using Microsoft.AspNetCore.SignalR;
using System.Drawing;

namespace JediApp.Web.Controllers
{
    public class AdminReportController : Controller
    {
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly JediAppDbContext _jediAppDb;

        public AdminReportController(ITransactionHistoryService transactionHistoryService, JediAppDbContext jediAppDb)
        {
            _transactionHistoryService = transactionHistoryService;
            _jediAppDb = jediAppDb;
        }
        public IActionResult Index()
        {
            ViewData["activePage"] = "AdminReport";

            var allUsersHistories = _transactionHistoryService.GetAllUsersHistories();
            AdminReport model = new AdminReport();
            //model.AnnualTurnover = allUsersHistories.Where(x => x.DateOfTransaction.Year == DateTime.Now.Year).Sum(x => x.Amount);
            //model.MonthlyTurnover = allUsersHistories.Where(x => x.DateOfTransaction.Month == DateTime.Now.Year).Sum(x => x.Amount);
            //model.DailyTurnover = allUsersHistories.Where(x => x.DateOfTransaction.Day == DateTime.Now.Year).Sum(x => x.Amount);

            model.TurnoverPLN = allUsersHistories.Where(x => x.CurrencyName == "PLN").Sum(x => x.Amount);
            model.TurnoverEUR = allUsersHistories.Where(x => x.CurrencyName == "EUR").Sum(x => x.Amount);
            model.TurnoverUSD = allUsersHistories.Where(x => x.CurrencyName == "USD").Sum(x => x.Amount);
            model.TurnoverGBP = allUsersHistories.Where(x => x.CurrencyName == "GBP").Sum(x => x.Amount);
            model.TurnoverCHF = allUsersHistories.Where(x => x.CurrencyName == "CHF").Sum(x => x.Amount);
            model.TurnoverCZK = allUsersHistories.Where(x => x.CurrencyName == "CZK").Sum(x => x.Amount);



            model.AnnualTransactions = allUsersHistories.Where(x => x.DateOfTransaction.Year == DateTime.Now.Year).Count();
            model.MonthlyTransactions = allUsersHistories.Where(x => x.DateOfTransaction.Month == DateTime.Now.Year).Count();
            model.DailyTTransactions = allUsersHistories.Where(x => x.DateOfTransaction.Day == DateTime.Now.Year).Count();

            model.UserAccounts = _jediAppDb.Users.Count();
            model.UserEmailConfirmed = _jediAppDb.Users.Where(x => x.EmailConfirmed).Count();

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

            return RedirectToAction(nameof(Index));


        }
    }
}
