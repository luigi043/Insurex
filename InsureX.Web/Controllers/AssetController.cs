using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using P = IAPR_Data.Providers;

namespace InsureX.Web.Controllers
{
    [Authorize]
    public class AssetController : Controller
    {
        #region Form Field Helpers

        private void LoadAssetFormFields()
        {
            var frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsVehicleAsset();

            ViewBag.IdentificationTypes = DataTableToList(ds.Tables[2]);
            ViewBag.PersonTitles = DataTableToList(ds.Tables[4]);
            ViewBag.Provinces = DataTableToList(ds.Tables[5]);
            ViewBag.AssetTypes = DataTableToList(ds.Tables[14]);
            ViewBag.PolicyTypes = DataTableToList(ds.Tables[3]);
            ViewBag.InsuranceCompanies = DataTableToList(ds.Tables[0]);
            ViewBag.PaymentFrequencies = DataTableToList(ds.Tables[10]);
        }

        private void LoadFinancerAssetTypes(int financerId)
        {
            var frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsAssetsFinancedByFinancer(financerId);
            ViewBag.AssetTypes = DataTableToList(ds.Tables[0]);
        }

        private static List<KeyValuePair<string, string>> DataTableToList(DataTable dt)
        {
            var list = new List<KeyValuePair<string, string>>();
            foreach (DataRow row in dt.Rows)
                list.Add(new KeyValuePair<string, string>(row[0].ToString()!, row[1].ToString()!));
            return list;
        }

        #endregion

        #region Add New Asset

        [HttpGet]
        public IActionResult AddNewAsset()
        {
            int userTypeId = int.Parse(User.FindFirst("iUser_Type_Id")?.Value ?? "0");
            int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");

            if (userTypeId >= 3) // Financer
                LoadFinancerAssetTypes(partnerId);
            else
                LoadAssetFormFields();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewAsset(IFormCollection form)
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                string assetType = form["AssetTypeId"].ToString();

                var assetProv = new P.Asset_Provider();
                bool saved = assetProv.Save_New_Asset_Without_Policy(partnerId, int.Parse(assetType), form);

                if (saved)
                {
                    TempData["Success"] = "Asset saved successfully.";
                    return RedirectToAction("AddNewAsset");
                }

                TempData["Error"] = "Failed to save asset.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            LoadAssetFormFields();
            return View();
        }

        #endregion

        #region Find Asset

        [HttpGet]
        public IActionResult FindAsset(string searchTerm)
        {
            ViewBag.SearchTerm = searchTerm;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                try
                {
                    int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                    var assetProv = new P.Asset_Provider();
                    DataSet ds = assetProv.Find_Assets(partnerId, searchTerm);

                    if (ds?.Tables.Count > 0)
                        ViewBag.Assets = ds.Tables[0];
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }

            return View();
        }

        #endregion

        #region Asset Type Partial (jQuery AJAX)

        [HttpGet]
        public IActionResult AssetTypePartial(string assetType)
        {
            string partialName = assetType switch
            {
                "1" => "AssetTypes/_Vehicle",
                "2" => "AssetTypes/_Property",
                "3" => "AssetTypes/_Watercraft",
                "4" => "AssetTypes/_Aviation",
                "5" => "AssetTypes/_Stock",
                "6" => "AssetTypes/_AccountReceivable",
                "7" => "AssetTypes/_Machinery",
                "8" => "AssetTypes/_PlantEquipment",
                "9" => "AssetTypes/_BusinessInterruption",
                "10" => "AssetTypes/_KeymanInsurance",
                "11" => "AssetTypes/_ElectronicEquipment",
                _ => ""
            };

            if (string.IsNullOrEmpty(partialName))
                return Content("");

            // Load cover types for asset-specific dropdowns
            try
            {
                var frmF = new P.GetFormFields_Provider();
                DataSet ds = frmF.GetFormFieldsVehicleAsset();
                if (ds.Tables.Count > 6)
                    ViewBag.CoverTypes = DataTableToList(ds.Tables[6]);
            }
            catch { }

            return PartialView(partialName);
        }

        #endregion

        #region Unconfirmed Insurance

        public IActionResult UnconfirmedInsurance()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var assetProv = new P.Asset_Provider();
                DataSet ds = assetProv.Get_Unconfirmed_Insurance(partnerId);

                if (ds?.Tables.Count > 0)
                    ViewBag.UnconfirmedAssets = ds.Tables[0];
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View();
        }

        #endregion

        #region Update Finance Value

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
                int assetId = int.Parse(form["AssetId"].ToString());
                decimal newValue = decimal.Parse(form["NewFinanceValue"].ToString());

                var assetProv = new P.Asset_Provider();
                assetProv.Update_Finance_Value(assetId, newValue);

                TempData["Success"] = "Finance value updated.";
                return RedirectToAction("UpdateFinanceValue");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return View();
        }

        #endregion

        #region Request Insurance Details

        public IActionResult RequestInsuranceDetails()
        {
            return View();
        }

        #endregion
    }
}
