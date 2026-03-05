using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using P = IAPR_Data.Providers;

namespace InsureX.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        #region Manage Partners

        [HttpGet]
        public IActionResult ManagePartners()
        {
            try
            {
                var partnerProv = new P.Partner_Provider();
                DataSet ds = partnerProv.Get_All_Partners();

                if (ds?.Tables.Count > 0)
                    ViewBag.Partners = ds.Tables[0];
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManagePartners(IFormCollection form)
        {
            try
            {
                string action = form["action"].ToString();
                var partnerProv = new P.Partner_Provider();

                switch (action)
                {
                    case "add":
                        partnerProv.Add_Partner(
                            form["PartnerName"].ToString(),
                            form["PartnerType"].ToString(),
                            form["ContactEmail"].ToString(),
                            form["ContactNumber"].ToString()
                        );
                        TempData["Success"] = "Partner added successfully.";
                        break;

                    case "edit":
                        partnerProv.Update_Partner(
                            int.Parse(form["PartnerId"].ToString()),
                            form["PartnerName"].ToString(),
                            form["ContactEmail"].ToString(),
                            form["ContactNumber"].ToString()
                        );
                        TempData["Success"] = "Partner updated successfully.";
                        break;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return RedirectToAction("ManagePartners");
        }

        #endregion

        #region Bulk Import

        [HttpGet]
        public IActionResult BulkImport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BulkImport(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    TempData["Error"] = "Please select a file to upload.";
                    return View();
                }

                int partnerId = int.Parse(User.FindFirst("iPartner_Id")?.Value ?? "0");

                // Save uploaded file to temp location
                string tempPath = Path.Combine(Path.GetTempPath(), file.FileName);
                using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var assetProv = new P.Asset_Provider();
                int imported = assetProv.Bulk_Import_Assets(partnerId, tempPath);

                // Cleanup temp file
                if (System.IO.File.Exists(tempPath))
                    System.IO.File.Delete(tempPath);

                TempData["Success"] = $"{imported} records imported successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Import error: " + ex.Message;
            }

            return View();
        }

        #endregion

        #region Add User

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
                var userProv = new P.User_Provider();
                userProv.Add_New_User(
                    form["FirstName"].ToString(),
                    form["LastName"].ToString(),
                    form["Email"].ToString(),
                    form["Role"].ToString(),
                    int.Parse(form["PartnerId"].ToString())
                );

                TempData["Success"] = "User added successfully.";
                return RedirectToAction("AddUser");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return View();
        }

        #endregion

        #region Edit Partner Users

        [HttpGet]
        public IActionResult EditPartnerUsers(int? partnerId)
        {
            try
            {
                var partnerProv = new P.Partner_Provider();

                // Load partners for dropdown
                DataSet dsPartners = partnerProv.Get_All_Partners();
                if (dsPartners?.Tables.Count > 0)
                    ViewBag.Partners = dsPartners.Tables[0];

                // If partner selected, load users
                if (partnerId.HasValue && partnerId.Value > 0)
                {
                    var userProv = new P.User_Provider();
                    DataSet dsUsers = userProv.Get_Users_By_Partner(partnerId.Value);
                    if (dsUsers?.Tables.Count > 0)
                        ViewBag.Users = dsUsers.Tables[0];

                    ViewBag.SelectedPartnerId = partnerId.Value;
                }
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
