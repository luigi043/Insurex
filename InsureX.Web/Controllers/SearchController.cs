using InsureX.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P = IAPR_Data.Providers;

namespace InsureX.Web.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        [HttpGet]
        public IActionResult Index(string? q = null)
        {
            var model = new SearchResultViewModel { Query = q ?? "" };

            if (!string.IsNullOrEmpty(q))
            {
                try
                {
                    int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                    var sP = new P.Search_Provider();
                    var dt = sP.SearchAssets(q, partnerId);

                    if (dt != null)
                    {
                        foreach (System.Data.DataRow row in dt.Rows)
                        {
                            model.Results.Add(new SearchResultItem
                            {
                                Id = Convert.ToInt32(row["Asset_Id"]),
                                Type = row["Asset_Type"]?.ToString() ?? "",
                                Description = row["Description"]?.ToString() ?? "",
                                Status = row["Status"]?.ToString() ?? ""
                            });
                        }
                    }
                }
                catch { }
            }

            return View(model);
        }
    }
}
