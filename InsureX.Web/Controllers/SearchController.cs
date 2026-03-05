using InsureX.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using P = IAPR_Data.Providers;

namespace InsureX.Web.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        [HttpGet]
        public IActionResult Index(string q)
        {
            var model = new SearchResultViewModel { Query = q ?? "" };

            if (!string.IsNullOrWhiteSpace(q))
            {
                try
                {
                    int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                    var searchProv = new P.Search_Provider();
                    DataSet ds = searchProv.Get_Search_Insurer_By_PolicyNumber(partnerId, q);

                    if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            model.Results.Add(new SearchResultItem
                            {
                                Id = Convert.ToInt32(row["iPolicy_Id"] ?? row[0]),
                                Type = "Policy",
                                Description = row["vcPolicy_Number"]?.ToString() ?? row[1]?.ToString() ?? "",
                                Status = row.Table.Columns.Contains("vcStatus") ? row["vcStatus"]?.ToString() ?? "" : ""
                            });
                        }

                        // Load summary data from Tables[1] if available
                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                        {
                            ViewBag.PolicySummaries = ds.Tables[1];
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }

            return View(model);
        }

        /// <summary>
        /// AJAX endpoint for getting policy assets
        /// </summary>
        [HttpGet]
        public IActionResult PolicyAssets(int policyId)
        {
            try
            {
                var policyProv = new P.Policy_Provider();
                DataSet ds = policyProv.GetPolicy_All_Assets(policyId);

                if (ds?.Tables.Count > 0)
                {
                    var assets = new List<object>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        assets.Add(new
                        {
                            assetId = row["iAsset_Id"]?.ToString(),
                            type = row["vcAsset_Type"]?.ToString(),
                            description = row["vcDescription"]?.ToString(),
                            financeValue = row["dcFinance_Value"]?.ToString(),
                            insuredValue = row["dcInsured_Value"]?.ToString(),
                            status = row["vcStatus"]?.ToString()
                        });
                    }
                    return Json(new { success = true, data = assets });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }

            return Json(new { success = true, data = Array.Empty<object>() });
        }
    }
}
