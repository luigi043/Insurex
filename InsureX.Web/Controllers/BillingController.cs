using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P = IAPR_Data.Providers;

namespace InsureX.Web.Controllers
{
    [Authorize]
    public class BillingController : Controller
    {
        [HttpGet]
        public IActionResult ViewPartnerInvoice()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                // TODO: Load invoices via billing provider
            }
            catch { }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult NewCharge()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult NewCharge(IFormCollection form)
        {
            try
            {
                // TODO: Create new billing charge via provider
                TempData["Success"] = "Charge created successfully.";
                return RedirectToAction("ViewPartnerInvoice");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCharge()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCharge(IFormCollection form)
        {
            try
            {
                // TODO: Update billing charge fee via provider
                TempData["Success"] = "Charge updated successfully.";
                return RedirectToAction("ViewPartnerInvoice");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }
    }
}
