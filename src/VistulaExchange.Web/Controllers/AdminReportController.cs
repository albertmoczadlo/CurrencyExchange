using iText.Html2pdf;
using VistulaExchange.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VistulaExchange.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminReportController : Controller
    {
        private readonly IAdminDashboardService _adminDashboardService;

        public AdminReportController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["activePage"] = "AdminReport";
            return View(await _adminDashboardService.GetDashboardAsync());
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
