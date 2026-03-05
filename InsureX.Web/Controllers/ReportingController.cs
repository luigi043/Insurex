using InsureX.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P = IAPR_Data.Providers;

namespace InsureX.Web.Controllers
{
    [Authorize]
    public class ReportingController : Controller
    {
        [HttpGet]
        public IActionResult UninsuredAssets()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var rP = new P.Report_Provider();
                ViewBag.Assets = rP.getUninsuredAssets(partnerId);
            }
            catch { }
            return View();
        }

        [HttpGet]
        public IActionResult ReinstatedCover()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var rP = new P.Report_Provider();
                ViewBag.Assets = rP.getReinstatedCover(partnerId);
            }
            catch { }
            return View();
        }

        [HttpGet]
        public IActionResult AllAssets()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var rP = new P.Report_Provider();
                ViewBag.Assets = rP.getAllAssets(partnerId);
            }
            catch { }
            return View();
        }

        [HttpGet]
        public IActionResult MonthlyReport()
        {
            return View(new MonthlyReportViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MonthlyReport(MonthlyReportViewModel model)
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var rP = new P.Report_Provider();
                ViewBag.ReportData = rP.getMonthlyReport(partnerId,
                    model.FromDate ?? DateTime.Now.AddMonths(-1),
                    model.ToDate ?? DateTime.Now);
            }
            catch { }
            return View(model);
        }

        [HttpGet]
        public IActionResult Export(string reportType, string format = "csv")
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var rP = new P.Report_Provider();

                // TODO: Generate export file based on reportType and format
                var content = new byte[0];
                var contentType = format == "pdf" ? "application/pdf" : "text/csv";
                var fileName = $"{reportType}_{DateTime.Now:yyyyMMdd}.{format}";

                return File(content, contentType, fileName);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(reportType);
            }
        }
    }
}
