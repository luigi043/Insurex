using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using P = IAPR_Data.Providers;
using CP = IAPR_Data.Classes.Policy;
using CCom = IAPR_Data.Classes.Common;

namespace InsureX.Web.Controllers
{
    [Authorize]
    public class PolicyController : Controller
    {
        #region Form Field Helpers

        /// <summary>
        /// Load all dropdown data from GetFormFields_Provider into ViewBag
        /// </summary>
        private void LoadFormFields()
        {
            var frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsVehicleAsset();

            // Tables[0] = Insurance Companies
            ViewBag.InsuranceCompanies = DataTableToSelectList(ds.Tables[0]);
            // Tables[2] = Identification Types
            ViewBag.IdentificationTypes = DataTableToSelectList(ds.Tables[2]);
            // Tables[3] = Policy Types
            ViewBag.PolicyTypes = DataTableToSelectList(ds.Tables[3]);
            // Tables[4] = Person Titles
            ViewBag.PersonTitles = DataTableToSelectList(ds.Tables[4]);
            // Tables[5] = Provinces
            ViewBag.Provinces = DataTableToSelectList(ds.Tables[5]);
            // Tables[10] = Payment Frequency
            ViewBag.PaymentFrequencies = DataTableToSelectList(ds.Tables[10]);
            // Tables[14] = Asset Types
            ViewBag.AssetTypes = DataTableToSelectList(ds.Tables[14]);
        }

        private static List<KeyValuePair<string, string>> DataTableToSelectList(DataTable dt)
        {
            var list = new List<KeyValuePair<string, string>>();
            foreach (DataRow row in dt.Rows)
                list.Add(new KeyValuePair<string, string>(row[0].ToString()!, row[1].ToString()!));
            return list;
        }

        #endregion

        #region Add New Policy

        [HttpGet]
        public IActionResult AddNewPolicy()
        {
            LoadFormFields();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewPolicy(IFormCollection form)
        {
            try
            {
                int policyTypeId = int.Parse(form["PolicyTypeId"].ToString());
                int polId = 0;

                if (policyTypeId == 1)
                {
                    polId = SavePolicyPersonal(form);
                }
                else
                {
                    polId = SavePolicyBusiness(form);
                }

                if (polId > 0)
                {
                    // Save asset data
                    string assetType = form["AssetTypeId"].ToString();
                    SaveAssetByType(assetType, polId, form);
                    TempData["Success"] = "Policy and asset saved successfully.";
                    return RedirectToAction("AddNewPolicy");
                }

                TempData["Error"] = "Failed to save policy.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            LoadFormFields();
            return View();
        }

        private int SavePolicyPersonal(IFormCollection form)
        {
            var pol = new CP.Policy
            {
                iInsurance_Company_Id = int.Parse(form["InsurerId"].ToString()),
                vcPolicy_Number = form["PolicyNumber"].ToString(),
                iPolicy_Type_Id = int.Parse(form["PolicyTypeId"].ToString()),
                iPolicy_Payment_Frequency_Type_Id = int.Parse(form["PaymentFrequencyId"].ToString())
            };

            var polHI = new CP.Policy_Holder_Consumer
            {
                iIdentification_Type_Id = int.Parse(form["IdentificationTypeId"].ToString()),
                iPerson_Title_Id = int.Parse(form["PersonTitleId"].ToString()),
                vcFirst_Names = form["FirstNames"].ToString(),
                vcSurname = form["Surname"].ToString(),
                vcIdentification_Number = form["IdentificationNumber"].ToString(),
                vcContact_Number = form["ContactNumber"].ToString(),
                vcAlternative_Contact_Number = form["AltContactNumber"].ToString(),
                vcEmail_Address = form["Email"].ToString()
            };

            var addPhy = new CCom.Addresses.Phycisal_address
            {
                vcBuilding_Unit = form["BuildingUnit"].ToString(),
                vcAddress_Line_1 = form["AddressLine1"].ToString(),
                vcAddress_Line_2 = form["AddressLine2"].ToString(),
                vcSuburb = form["Suburb"].ToString(),
                vcCity = form["City"].ToString(),
                iProvince_Id = int.Parse(form["ProvinceId"].ToString()),
                vcPostal_Code = form["PostalCode"].ToString()
            };

            polHI.physical_Address = addPhy;
            polHI.bPostalAddresSameAsPhysical = form["PostalSameAsPhysical"] == "on";

            var addPo = new CCom.Addresses.Postal_Address
            {
                vcPOBox_Bag = form["POBox"].ToString(),
                vcPost_Office_Name = form["PostOfficeName"].ToString(),
                vcPost_Postal_Code = form["PostPostalCode"].ToString()
            };
            polHI.postal_Address = addPo;

            pol.policy_Holder_Individual = polHI;

            var pro = new P.Policy_Provider();
            return pro.Save_New_Policy_Personal(pol);
        }

        private int SavePolicyBusiness(IFormCollection form)
        {
            var pol = new CP.Policy
            {
                iInsurance_Company_Id = int.Parse(form["InsurerId"].ToString()),
                vcPolicy_Number = form["PolicyNumber"].ToString(),
                iPolicy_Type_Id = int.Parse(form["PolicyTypeId"].ToString()),
                iPolicy_Payment_Frequency_Type_Id = int.Parse(form["PaymentFrequencyId"].ToString())
            };

            // Business policy holder
            var polHB = new CP.Policy_Holder_Business
            {
                vcCompany_Name = form["CompanyName"].ToString(),
                vcCompany_Registration_Number = form["RegistrationNumber"].ToString(),
                vcVAT_Number = form["VATNumber"].ToString(),
                vcContact_Number = form["ContactNumber"].ToString(),
                vcEmail_Address = form["Email"].ToString()
            };

            var addPhy = new CCom.Addresses.Phycisal_address
            {
                vcBuilding_Unit = form["BuildingUnit"].ToString(),
                vcAddress_Line_1 = form["AddressLine1"].ToString(),
                vcAddress_Line_2 = form["AddressLine2"].ToString(),
                vcSuburb = form["Suburb"].ToString(),
                vcCity = form["City"].ToString(),
                iProvince_Id = int.Parse(form["ProvinceId"].ToString()),
                vcPostal_Code = form["PostalCode"].ToString()
            };
            polHB.physical_Address = addPhy;

            pol.policy_Holder_Business = polHB;

            var pro = new P.Policy_Provider();
            return pro.Save_New_Policy_Business(pol);
        }

        private void SaveAssetByType(string assetType, int polId, IFormCollection form)
        {
            var assetProv = new P.Asset_Provider();
            // Delegate to asset provider based on asset type
            // Asset type-specific save is handled by the provider layer
            assetProv.Save_Asset_To_Policy(polId, int.Parse(assetType), form);
        }

        #endregion

        #region Add Asset to Existing Policy

        [HttpGet]
        public IActionResult AddAssetToPolicy()
        {
            LoadFormFields();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAssetToPolicy(IFormCollection form)
        {
            try
            {
                string policyNumber = form["PolicyNumber"].ToString();
                string assetType = form["AssetTypeId"].ToString();

                var policyProv = new P.Policy_Provider();
                int polId = policyProv.GetPolicyIdByNumber(policyNumber);

                if (polId > 0)
                {
                    SaveAssetByType(assetType, polId, form);
                    TempData["Success"] = "Asset added to policy successfully.";
                    return RedirectToAction("AddAssetToPolicy");
                }

                TempData["Error"] = "Policy not found.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            LoadFormFields();
            return View();
        }

        #endregion

        #region Policy Transactions

        public IActionResult PolicyTransactions()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var policyProv = new P.Policy_Provider();
                DataSet ds = policyProv.Get_Policy_Transactions(partnerId);

                if (ds?.Tables.Count > 0)
                    ViewBag.Transactions = ds.Tables[0];
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View();
        }

        #endregion

        #region Confirm Policy Cover

        [HttpGet]
        public IActionResult ConfirmPolicyCover()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var policyProv = new P.Policy_Provider();
                DataSet ds = policyProv.Get_Pending_Policy_Confirmations(partnerId);

                if (ds?.Tables.Count > 0)
                    ViewBag.PendingPolicies = ds.Tables[0];
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmPolicyCover(int policyId, string action)
        {
            try
            {
                var policyProv = new P.Policy_Provider();
                if (action == "confirm")
                    policyProv.Confirm_Policy_Cover(policyId);
                else
                    policyProv.Reject_Policy_Cover(policyId);

                TempData["Success"] = action == "confirm" ? "Policy cover confirmed." : "Policy cover rejected.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("ConfirmPolicyCover");
        }

        #endregion

        #region Insurer Policy Transactions

        [Authorize(Roles = "InsurerUser")]
        public IActionResult InsurerPolicyTransactions()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var policyProv = new P.Policy_Provider();
                DataSet ds = policyProv.Get_Insurer_Policy_Transactions(partnerId);

                if (ds?.Tables.Count > 0)
                    ViewBag.Transactions = ds.Tables[0];
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View();
        }

        #endregion
    }
}
