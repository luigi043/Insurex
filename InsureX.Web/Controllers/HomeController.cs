using InsureX.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using C = IAPR_Data.Classes;
using CCom = IAPR_Data.Classes.Common;
using P = IAPR_Data.Providers;

namespace InsureX.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private static readonly CultureInfo ZaCulture = new("en-ZA");

        public IActionResult Index()
        {
            var model = new DashboardViewModel();

            try
            {
                int userTypeId = int.Parse(User.FindFirst("iUser_Type_Id")?.Value ?? "0");
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");

                var dP = new P.Daschboard_Provider();
                DataSet? ds = null;

                switch (userTypeId)
                {
                    case 1:
                    case 2: // Admin
                        ds = dP.Get_Admin_Landing_DashboardTable();
                        break;
                    case 3:
                    case 4: // Financer
                        ds = dP.Get_Financer_Landing_DashboardTable(partnerId);
                        break;
                    case 5:
                    case 6: // Insurer
                        ds = dP.Get_Insurer_Landing_DashboardTable(partnerId);
                        break;
                }

                if (ds != null && ds.Tables.Count >= 6)
                {
                    model.PremiumUnpaidAssetCount = SafeInt(ds.Tables[0], "iNumber_Of_Assets");
                    model.PremiumUnpaidAssetTotal = SafeDecimal(ds.Tables[0], "dcUninsured_Finance_Value");

                    model.NoInsuranceAssetCount = SafeInt(ds.Tables[1], "iNumber_Of_Assets");
                    model.NoInsuranceAssetTotal = SafeDecimal(ds.Tables[1], "dcUninsured_Finance_Value");

                    model.AllAssetCount = SafeInt(ds.Tables[2], "iNumber_Of_Assets");
                    model.AllAssetTotal = SafeDecimal(ds.Tables[2], "dcFinance_Value");

                    model.InsuredAssetCount = SafeInt(ds.Tables[3], "iNumber_Of_Assets");
                    model.InsuredAssetTotal = SafeDecimal(ds.Tables[3], "dcFinance_Value");

                    model.AdequatelyInsuredTotal = SafeDecimal(ds.Tables[4], "dcFinance_Value");
                    model.UnderInsuredTotal = SafeDecimal(ds.Tables[5], "dcFinance_Value");

                    model.UninsuredAssetCount = model.PremiumUnpaidAssetCount + model.NoInsuranceAssetCount;
                    model.UninsuredAssetTotal = model.PremiumUnpaidAssetTotal + model.NoInsuranceAssetTotal;
                    model.InsuredShortFall = model.AllAssetTotal - model.InsuredAssetTotal;

                    // Compute percentages
                    if (model.UninsuredAssetTotal > 0)
                    {
                        model.PremiumUnpaidAssetTotalPercent = Math.Round(model.PremiumUnpaidAssetTotal / model.UninsuredAssetTotal * 100, 1);
                        model.NoInsuranceAssetTotalPercent = Math.Round(model.NoInsuranceAssetTotal / model.UninsuredAssetTotal * 100, 1);
                    }
                    if (model.InsuredAssetTotal > 0)
                    {
                        model.AdequatelyInsuredTotalPercent = Math.Round(model.AdequatelyInsuredTotal / model.InsuredAssetTotal * 100, 1);
                        model.UnderInsuredTotalPercent = Math.Round(model.UnderInsuredTotal / model.InsuredAssetTotal * 100, 1);
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
        /// Insurance Status doughnut chart (Arrears vs Unconfirmed)
        /// </summary>
        [HttpGet]
        public IActionResult ChartInsuranceStatus()
        {
            try
            {
                int userTypeId = int.Parse(User.FindFirst("iUser_Type_Id")?.Value ?? "0");
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var dP = new P.Daschboard_Provider();
                DataSet? ds = null;

                if (userTypeId <= 2)
                    ds = dP.Get_Admin_Landing_Dashboard_ArrearVsUnconfirmed();
                else
                    ds = dP.Get_Financer_Landing_Dashboard_ArrearVsUnconfirmed(partnerId);

                if (ds?.Tables.Count > 0)
                {
                    var data = new List<object>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        data.Add(new { label = row[0]?.ToString(), value = row[1]?.ToString() });
                    }
                    return Json(new { chart = new { caption = "Insurance Status", subcaption = "All Assets" }, data });
                }
            }
            catch { }
            return Json(new { data = Array.Empty<object>() });
        }

        /// <summary>
        /// Uninsured assets count (doughnut - Premiums Unpaid vs No Insurance)
        /// </summary>
        [HttpGet]
        public IActionResult ChartUninsuredAssetsCount()
        {
            try
            {
                int userTypeId = int.Parse(User.FindFirst("iUser_Type_Id")?.Value ?? "0");
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var dP = new P.Daschboard_Provider();
                DataSet? ds = null;

                if (userTypeId <= 2)
                    ds = dP.Get_Admin_Landing_DashboardTable();
                else
                    ds = dP.Get_Financer_Landing_DashboardTable(partnerId);

                if (ds?.Tables.Count >= 2)
                {
                    int premiumUnpaid = SafeInt(ds.Tables[0], "iNumber_Of_Assets");
                    int noInsurance = SafeInt(ds.Tables[1], "iNumber_Of_Assets");
                    return Json(new
                    {
                        chart = new { caption = "Uninsured Assets", subcaption = "By Reason" },
                        data = new[]
                        {
                            new { label = "Premiums Unpaid", value = premiumUnpaid.ToString() },
                            new { label = "No Insurance Details", value = noInsurance.ToString() }
                        }
                    });
                }
            }
            catch { }
            return Json(new { data = Array.Empty<object>() });
        }

        /// <summary>
        /// Insured assets chart (Adequately vs Under-insured)
        /// </summary>
        [HttpGet]
        public IActionResult ChartInsuredAssets()
        {
            try
            {
                int userTypeId = int.Parse(User.FindFirst("iUser_Type_Id")?.Value ?? "0");
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var dP = new P.Daschboard_Provider();
                DataSet? ds = null;

                if (userTypeId <= 2)
                    ds = dP.Get_Admin_Landing_DashboardTable();
                else
                    ds = dP.Get_Financer_Landing_DashboardTable(partnerId);

                if (ds?.Tables.Count >= 6)
                {
                    decimal adequately = SafeDecimal(ds.Tables[4], "dcFinance_Value");
                    decimal underInsured = SafeDecimal(ds.Tables[5], "dcFinance_Value");
                    return Json(new
                    {
                        chart = new { caption = "Insured Assets", subcaption = "By Cover Status" },
                        data = new[]
                        {
                            new { label = "Adequately Insured", value = adequately.ToString("F2") },
                            new { label = "Under Insured", value = underInsured.ToString("F2") }
                        }
                    });
                }
            }
            catch { }
            return Json(new { data = Array.Empty<object>() });
        }

        /// <summary>
        /// Uninsured by financer bar chart
        /// </summary>
        [HttpGet]
        public IActionResult ChartUninsuredByFinancer()
        {
            try
            {
                var dP = new P.Daschboard_Provider();
                var ds = dP.Get_Admin_Landing_Dashboard_UninsuredByFinancer();
                if (ds?.Tables.Count > 0)
                {
                    var categories = new List<object>();
                    var dataset = new List<object>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        categories.Add(new { label = row["Financer"]?.ToString() });
                        dataset.Add(new { value = row["Finance_Value"]?.ToString() });
                    }
                    return Json(new
                    {
                        chart = new { caption = "Uninsured Assets by Lender", theme = "fusion" },
                        categories = new[] { new { category = categories } },
                        dataset = new[] { new { seriesname = "Finance Value", data = dataset } }
                    });
                }
            }
            catch { }
            return Json(new { data = Array.Empty<object>() });
        }

        /// <summary>
        /// Uninsured age analysis stacked bar chart
        /// </summary>
        [HttpGet]
        public IActionResult ChartUninsuredStatistics()
        {
            try
            {
                var dP = new P.Daschboard_Provider();
                var ds = dP.Get_Admin_Landing_Dashboard_UninsuredAgeAnalysis();
                if (ds?.Tables.Count > 0)
                {
                    var data = new List<object>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        data.Add(new { label = row[0]?.ToString(), value = row[1]?.ToString() });
                    }
                    return Json(new
                    {
                        chart = new { caption = "Uninsured Age Analysis", theme = "fusion" },
                        data
                    });
                }
            }
            catch { }
            return Json(new { data = Array.Empty<object>() });
        }

        /// <summary>
        /// Non-payment reinstated by month chart
        /// </summary>
        [HttpGet]
        public IActionResult ChartNonPaymentReinstated()
        {
            try
            {
                var dP = new P.Daschboard_Provider();
                var ds = dP.Get_Admin_Landing_Dashboard_NonPaymentReinstated();
                if (ds?.Tables.Count > 0)
                {
                    var data = new List<object>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        data.Add(new { label = row["Month"]?.ToString(), value = row["Finance_Value"]?.ToString() });
                    }
                    return Json(new
                    {
                        chart = new { caption = "Non-Payment Reinstated Assets", subcaption = "By Month", theme = "fusion" },
                        data
                    });
                }
            }
            catch { }
            return Json(new { data = Array.Empty<object>() });
        }

        /// <summary>
        /// Uninsured by insurer chart
        /// </summary>
        [HttpGet]
        public IActionResult ChartUninsuredByInsurer()
        {
            try
            {
                var dP = new P.Daschboard_Provider();
                var ds = dP.Get_Admin_Landing_Dashboard_UninsuredByInsurer();
                if (ds?.Tables.Count > 0)
                {
                    var data = new List<object>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        data.Add(new { label = row["Insurer"]?.ToString(), value = row["Finance_Value"]?.ToString() });
                    }
                    return Json(new
                    {
                        chart = new { caption = "Uninsured Assets by Insurer", theme = "fusion" },
                        data
                    });
                }
            }
            catch { }
            return Json(new { data = Array.Empty<object>() });
        }

        /// <summary>
        /// Communications by month chart
        /// </summary>
        [HttpGet]
        public IActionResult ChartCommunications()
        {
            try
            {
                var dP = new P.Daschboard_Provider();
                var ds = dP.Get_Admin_Landing_Dashboard_Communications();
                if (ds?.Tables.Count > 0)
                {
                    var data = new List<object>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        data.Add(new { label = row[0]?.ToString(), value = row[1]?.ToString() });
                    }
                    return Json(new
                    {
                        chart = new { caption = "Communications", subcaption = "Current Month", theme = "fusion" },
                        data
                    });
                }
            }
            catch { }
            return Json(new { data = Array.Empty<object>() });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        #region Helpers

        private static int SafeInt(DataTable dt, string column)
        {
            if (dt.Rows.Count > 0 && dt.Columns.Contains(column))
                return Convert.ToInt32(dt.Rows[0][column]);
            return 0;
        }

        private static decimal SafeDecimal(DataTable dt, string column)
        {
            if (dt.Rows.Count > 0 && dt.Columns.Contains(column))
                return Convert.ToDecimal(dt.Rows[0][column]);
            return 0m;
        }

        #endregion
    }
}
