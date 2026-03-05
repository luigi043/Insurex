using InsureX.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using C = IAPR_Data.Classes;
using Pro = IAPR_Data.Providers;

namespace InsureX.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var uP = new Pro.User_Provider();
                var objUser = uP.ValidateUser(model.UserName, model.Password);

                if (objUser != null)
                {
                    string role = objUser.iUser_Type_Id switch
                    {
                        1 or 2 => "Admin",
                        3 or 4 => "BankUser",
                        5 or 6 => "InsurerUser",
                        _ => "User"
                    };

                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, objUser.iUser_Id.ToString()),
                        new(ClaimTypes.Name, objUser.vcUsername ?? ""),
                        new(ClaimTypes.Role, role),
                        new("iUser_Type_Id", objUser.iUser_Type_Id.ToString()),
                        new("vcName", objUser.vcName ?? ""),
                        new("vcSurname", objUser.vcSurname ?? ""),
                        new("iPartner_Id", objUser.iPartner_Id.ToString()),
                        new("iPartner_Type_Id", objUser.iPartner_Type_Id.ToString()),
                        new("iUser_Status_Id", objUser.iUser_Status_Id.ToString()),
                    };

                    var identity = new ClaimsIdentity(claims, "InsureXCookie");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("Identity.Application", principal,
                        new AuthenticationProperties { IsPersistent = model.RememberMe });

                    if (objUser.iUser_Status_Id == 2)
                        return RedirectToAction("ChangePassword");

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Log in failed. Please check your credentials.");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Identity.Application");
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PasswordReminder() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult PasswordReminder(PasswordReminderViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var uP = new Pro.User_Provider();
                uP.SendPasswordResetEmail(model.Email);
            }
            catch { /* Swallow to avoid leaking user existence */ }

            TempData["Message"] = "If an account with that email exists, a password reset link has been sent.";
            return RedirectToAction("PasswordRequestConfirm");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PasswordRequestConfirm() => View();

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
                var uP = new Pro.User_Provider();
                bool changed = uP.ChangePassword(userId, model.CurrentPassword, model.NewPassword);

                if (changed)
                {
                    TempData["Success"] = "Password changed successfully.";
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Current password is incorrect.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var uP = new Pro.User_Provider();
                int userId = uP.RegisterNewUser(model.FirstName, model.LastName, model.Email, model.Password);

                if (userId > 0)
                {
                    TempData["Success"] = "Registration successful. Please log in.";
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }
    }
}
