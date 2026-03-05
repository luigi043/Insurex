using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P = IAPR_Data.Providers;

namespace InsureX.Web.Controllers
{
    [Authorize]
    public class AssetController : Controller
    {
        [HttpGet]
        public IActionResult AddNewAsset()
        {
            try
            {
                var gfP = new P.GetFormFields_Provider();
                ViewBag.AssetTypes = gfP.getAssetTypes();
                ViewBag.Customers = gfP.getFinancerCustomers(
                    int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0"));
            }
            catch { }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewAsset(IFormCollection form)
        {
            try
            {
                // TODO: Build asset from form and save via appropriate provider
                TempData["Success"] = "Asset created successfully.";
                return RedirectToAction("FindAsset");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult FindAsset(string? searchTerm = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                    var sP = new P.Search_Provider();
                    ViewBag.Results = sP.SearchAssets(searchTerm, partnerId);
                }
            }
            catch { }
            ViewBag.SearchTerm = searchTerm;
            return View();
        }

        [HttpGet]
        public IActionResult UnconfirmedInsurance()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var gaP = new P.Generic_Asset_Provider();
                ViewBag.UnconfirmedAssets = gaP.getUnconfirmedAssetsByPartner(partnerId);
            }
            catch { }
            return View();
        }

        [HttpGet]
        public IActionResult UpdateFinanceValue()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateFinanceValue(IFormCollection form)
        {
            try
            {
                // TODO: Update finance value via provider
                TempData["Success"] = "Finance value updated successfully.";
                return RedirectToAction("UpdateFinanceValue");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult RequestInsuranceDetails(int id)
        {
            try
            {
                // TODO: Send insurance details request via provider
                TempData["Success"] = "Insurance details request sent.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("UnconfirmedInsurance");
        }

        /// <summary>
        /// Returns the partial view for a specific asset type form.
        /// Called via jQuery AJAX when user selects an asset type.
        /// </summary>
        [HttpGet]
        public IActionResult AssetTypePartial(string assetType)
        {
            return assetType?.ToLower() switch
            {
                "vehicle" => PartialView("AssetTypes/_Vehicle"),
                "aviation" => PartialView("AssetTypes/_Aviation"),
                "property" => PartialView("AssetTypes/_Property"),
                "electronic_equipment" => PartialView("AssetTypes/_ElectronicEquipment"),
                "machinery" => PartialView("AssetTypes/_Machinery"),
                "plant_equipment" => PartialView("AssetTypes/_PlantEquipment"),
                "stock" => PartialView("AssetTypes/_Stock"),
                "watercraft" => PartialView("AssetTypes/_Watercraft"),
                "keyman_insurance" => PartialView("AssetTypes/_KeymanInsurance"),
                "account_receivable" => PartialView("AssetTypes/_AccountReceivable"),
                "business_interruption" => PartialView("AssetTypes/_BusinessInterruption"),
                _ => PartialView("AssetTypes/_Vehicle")
            };
        }
    }
}
