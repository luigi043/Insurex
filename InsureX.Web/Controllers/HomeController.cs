using InsureX.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using C = IAPR_Data.Classes;
using P = IAPR_Data.Providers;

namespace InsureX.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new DashboardViewModel();

            try
            {
                var uP = new P.User_Provider();
                int userTypeId = int.Parse(User.FindFirst("iUser_Type_Id")?.Value ?? "0");
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");

                var dP = new P.Daschboard_Provider();

                // Get dashboard totals based on role
                DataTable? dt = null;
                switch (userTypeId)
                {
                    case 1:
                    case 2: // Admin
                        dt = dP.getAdminDashboard();
                        break;
                    case 3:
                    case 4: // Financer
                        dt = dP.getFinancerDashboard(partnerId);
                        break;
                    case 5:
                    case 6: // Insurer
                        dt = dP.getInsurerDashboard(partnerId);
                        break;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string status = row["Policy_Status"]?.ToString() ?? "";
                        int count = Convert.ToInt32(row["AssetCount"]);
                        decimal total = Convert.ToDecimal(row["Finance_Value"]);

                        model.AllAssetCount += count;
                        model.AllAssetTotal += total;

                        if (status == "Confirmed" || status == "Pending")
                        {
                            model.InsuredAssetCount += count;
                            model.InsuredAssetTotal += total;
                        }
                        else
                        {
                            model.UninsuredAssetCount += count;
                            model.UninsuredAssetTotal += total;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(model);
        }

        /// <summary>
        /// Returns dashboard chart data as JSON for jQuery AJAX calls.
        /// </summary>
        [HttpGet]
        public IActionResult ChartData(string chartType)
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var dP = new P.Daschboard_Provider();

                // Return appropriate chart data based on type
                var data = new { labels = new[] { "Insured", "Uninsured" }, values = new[] { 70, 30 } };
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
