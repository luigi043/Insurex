using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P = IAPR_Data.Providers;

namespace InsureX.Web.Controllers
{
    [Authorize]
    public class PolicyController : Controller
    {
        [HttpGet]
        public IActionResult AddNewPolicy()
        {
            try
            {
                var gfP = new P.GetFormFields_Provider();
                ViewBag.AssetTypes = gfP.getAssetTypes();
                ViewBag.Insurers = gfP.getInsurers();
            }
            catch { }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewPolicy(IFormCollection form)
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var pP = new P.Policy_Provider();

                // TODO: Build policy object from form and save via provider
                TempData["Success"] = "Policy created successfully.";
                return RedirectToAction("PolicyTransactions");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult AddAssetToPolicy()
        {
            try
            {
                var gfP = new P.GetFormFields_Provider();
                ViewBag.AssetTypes = gfP.getAssetTypes();
            }
            catch { }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAssetToPolicy(IFormCollection form)
        {
            try
            {
                // TODO: Add asset to existing policy via provider
                TempData["Success"] = "Asset added to policy successfully.";
                return RedirectToAction("PolicyTransactions");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult PolicyTransactions()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var pP = new P.Policy_Provider();
                ViewBag.Transactions = pP.getPolicyTransactions(partnerId);
            }
            catch { }
            return View();
        }

        [HttpGet]
        public IActionResult ConfirmPolicyCover()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var pP = new P.Policy_Provider();
                ViewBag.PendingPolicies = pP.getPolicyTransactions(partnerId);
            }
            catch { }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmPolicyCover(IFormCollection form)
        {
            try
            {
                // TODO: Confirm policy cover via provider
                TempData["Success"] = "Policy cover confirmed.";
                return RedirectToAction("ConfirmPolicyCover");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult InsurerPolicyTransactions()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var pP = new P.Policy_Provider();
                ViewBag.Transactions = pP.getPolicyTransactions(partnerId);
            }
            catch { }
            return View();
        }
    }
}
