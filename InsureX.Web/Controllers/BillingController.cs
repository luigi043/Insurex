using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using P = IAPR_Data.Providers;

namespace InsureX.Web.Controllers
{
    [Authorize]
    public class BillingController : Controller
    {
        #region View Partner Invoice

        public IActionResult ViewPartnerInvoice()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var billingProv = new P.Billing_Provider();
                DataSet ds = billingProv.Get_Partner_Invoices(partnerId);

                if (ds?.Tables.Count > 0)
                    ViewBag.Invoices = ds.Tables[0];
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View();
        }

        #endregion

        #region New Charge

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult NewCharge()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewCharge(IFormCollection form)
        {
            try
            {
                var billingProv = new P.Billing_Provider();
                billingProv.Add_New_Charge(
                    int.Parse(form["PartnerId"].ToString()),
                    form["ChargeType"].ToString(),
                    decimal.Parse(form["Amount"].ToString()),
                    form["Description"].ToString()
                );

                TempData["Success"] = "Charge added successfully.";
                return RedirectToAction("NewCharge");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return View();
        }

        #endregion

        #region Update Charge

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UpdateCharge()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCharge(IFormCollection form)
        {
            try
            {
                var billingProv = new P.Billing_Provider();
                billingProv.Update_Charge(
                    int.Parse(form["ChargeId"].ToString()),
                    decimal.Parse(form["Amount"].ToString())
                );

                TempData["Success"] = "Charge updated successfully.";
                return RedirectToAction("UpdateCharge");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return View();
        }

        #endregion
    }
}
