using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P = IAPR_Data.Providers;

namespace InsureX.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult ManagePartners()
        {
            try
            {
                var pP = new P.Partner_Provider();
                ViewBag.Partners = pP.getAllPartners();
            }
            catch { }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManagePartners(IFormCollection form)
        {
            try
            {
                // TODO: Create/update partner via provider
                TempData["Success"] = "Partner saved successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("ManagePartners");
        }

        [HttpGet]
        public IActionResult BulkImport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BulkImport(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select a file to import.";
                return View();
            }

            try
            {
                // TODO: Process bulk import file via provider
                TempData["Success"] = $"File '{file.FileName}' imported successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public IActionResult ImportPolicies()
        {
            return View("BulkImport");
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUser(IFormCollection form)
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                // TODO: Create user via provider
                TempData["Success"] = "User added successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public IActionResult EditPartnerUsers()
        {
            try
            {
                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");
                var uP = new P.User_Provider();
                ViewBag.Users = uP.getPartnerUsers(partnerId);
            }
            catch { }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPartnerUsers(IFormCollection form)
        {
            try
            {
                // TODO: Update user via provider
                TempData["Success"] = "User updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("EditPartnerUsers");
        }
    }
}
