using InsureX.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;
using P = IAPR_Data.Providers;

namespace InsureX.Web.Controllers
{
    [Authorize]
    public class ReportingController : Controller
    {
        #region Uninsured Assets

        public IActionResult UninsuredAssets()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                int userTypeId = int.Parse(User.FindFirst("iUser_Type_Id")?.Value ?? "0");
                var reportProv = new P.Report_Provider();
                DataSet? ds = null;

                if (userTypeId <= 2) // Admin
                    ds = reportProv.Get_Admin_Uninsured_Assets();
                else
                    ds = reportProv.Get_Financer_Uninsured_Assets(partnerId);

                if (ds?.Tables.Count > 0)
                    ViewBag.Assets = ds.Tables[0];
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View();
        }

        #endregion

        #region Reinstated Cover

        public IActionResult ReinstatedCover()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                int userTypeId = int.Parse(User.FindFirst("iUser_Type_Id")?.Value ?? "0");
                var reportProv = new P.Report_Provider();
                DataSet? ds = null;

                if (userTypeId <= 2)
                    ds = reportProv.Get_Admin_Reinstated_Cover();
                else
                    ds = reportProv.Get_Financer_Reinstated_Cover(partnerId);

                if (ds?.Tables.Count > 0)
                    ViewBag.Assets = ds.Tables[0];
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View();
        }

        #endregion

        #region All Assets

        public IActionResult AllAssets()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                int userTypeId = int.Parse(User.FindFirst("iUser_Type_Id")?.Value ?? "0");
                var reportProv = new P.Report_Provider();
                DataSet? ds = null;

                if (userTypeId <= 2)
                    ds = reportProv.Get_Admin_All_Assets();
                else
                    ds = reportProv.Get_Financer_All_Assets(partnerId);

                if (ds?.Tables.Count > 0)
                    ViewBag.Assets = ds.Tables[0];
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View();
        }

        #endregion

        #region Monthly Report

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
                if (model.FromDate.HasValue && model.ToDate.HasValue)
                {
                    int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                    int userTypeId = int.Parse(User.FindFirst("iUser_Type_Id")?.Value ?? "0");
                    var reportProv = new P.Report_Provider();
                    DataSet? ds = null;

                    if (userTypeId <= 2)
                        ds = reportProv.Get_Admin_Monthly_Report(model.FromDate.Value, model.ToDate.Value);
                    else
                        ds = reportProv.Get_Financer_Monthly_Report(partnerId, model.FromDate.Value, model.ToDate.Value);

                    if (ds?.Tables.Count > 0)
                        ViewBag.ReportData = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View(model);
        }

        #endregion

        #region Export

        [HttpGet]
        public IActionResult Export(string reportType, string format = "csv")
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                int userTypeId = int.Parse(User.FindFirst("iUser_Type_Id")?.Value ?? "0");
                var reportProv = new P.Report_Provider();
                DataSet? ds = null;

                switch (reportType)
                {
                    case "UninsuredAssets":
                        ds = userTypeId <= 2 ? reportProv.Get_Admin_Uninsured_Assets()
                                              : reportProv.Get_Financer_Uninsured_Assets(partnerId);
                        break;
                    case "ReinstatedCover":
                        ds = userTypeId <= 2 ? reportProv.Get_Admin_Reinstated_Cover()
                                              : reportProv.Get_Financer_Reinstated_Cover(partnerId);
                        break;
                    case "AllAssets":
                        ds = userTypeId <= 2 ? reportProv.Get_Admin_All_Assets()
                                              : reportProv.Get_Financer_All_Assets(partnerId);
                        break;
                }

                if (ds?.Tables.Count > 0)
                {
                    string csv = DataTableToCsv(ds.Tables[0]);
                    string fileName = $"{reportType}_{DateTime.Now:yyyy_MM_dd}.csv";
                    return File(Encoding.UTF8.GetBytes(csv), "text/csv", fileName);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(reportType ?? "AllAssets");
        }

        private static string DataTableToCsv(DataTable dt)
        {
            var sb = new StringBuilder();

            // Headers
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb.Append(dt.Columns[i].ColumnName);
                if (i < dt.Columns.Count - 1) sb.Append(",");
            }
            sb.AppendLine();

            // Data
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string val = row[i]?.ToString()?.Replace("\"", "\"\"") ?? "";
                    if (val.Contains(',') || val.Contains('"') || val.Contains('\n'))
                        val = $"\"{val}\"";
                    sb.Append(val);
                    if (i < dt.Columns.Count - 1) sb.Append(",");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        #endregion
    }
}
