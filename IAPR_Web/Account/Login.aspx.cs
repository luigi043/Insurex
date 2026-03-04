using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using IAPR_Web.Models;
using C = IAPR_Data.Classes;
using Pro = IAPR_Data.Providers;
namespace IAPR_Web.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register";
            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            //var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            //if (!String.IsNullOrEmpty(returnUrl))
            //{
            //    RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            //}
        }

        protected void LogIn(object sender, EventArgs e)
        {            
            try
            {


                C.Common.CurrentUser objUser = new C.Common.CurrentUser();
                Pro.User_Provider uP = new Pro.User_Provider();
                objUser = uP.ValidateUser(txtUserName.Text, txtPassword.Text);
                if (objUser != null)
                {
                    string role = "User";
                    switch (objUser.iUser_Type_Id)
                    {
                        case 1: case 2: role = "Admin"; break;
                        case 3: case 4: role = "BankUser"; break;
                        case 5: case 6: role = "InsurerUser"; break;
                    }

                    // Look up TenantId and OrganizationId from the Identity database
                    int? tenantId = null;
                    int? orgId = null;
                    using (var dbCtx = new C.ApplicationDbContext())
                    {
                        var appUser = dbCtx.Users.FirstOrDefault(u => u.UserName == objUser.vcUsername);
                        if (appUser != null)
                        {
                            tenantId = appUser.TenantId;
                            orgId = appUser.OrganizationId;
                        }
                    }

                    var identity = new System.Security.Claims.ClaimsIdentity(new[] {
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, objUser.iUser_Id.ToString()),
                        new System.Security.Claims.Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, objUser.vcUsername),
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role),
                        new System.Security.Claims.Claim("iUser_Type_Id", objUser.iUser_Type_Id.ToString()),
                        new System.Security.Claims.Claim("vcName", objUser.vcName ?? ""),
                        new System.Security.Claims.Claim("vcSurname", objUser.vcSurname ?? ""),
                        new System.Security.Claims.Claim("iPartner_Id", objUser.iPartner_Id.ToString()),
                        new System.Security.Claims.Claim("iPartner_Type_Id", objUser.iPartner_Type_Id.ToString()),
                        new System.Security.Claims.Claim("iUser_Status_Id", objUser.iUser_Status_Id.ToString()),
                        new System.Security.Claims.Claim("TenantId", tenantId?.ToString() ?? ""),
                        new System.Security.Claims.Claim("OrganizationId", orgId?.ToString() ?? "")
                    }, DefaultAuthenticationTypes.ApplicationCookie);

                    HttpContext.Current.GetOwinContext().Authentication.SignIn(
                        new Microsoft.Owin.Security.AuthenticationProperties { IsPersistent = true }, identity);

                    if (objUser.iUser_Status_Id == 2)
                    {
                        Response.Redirect("/account/changepassword.aspx", false);

                    }
                    else
                    {
                        switch (objUser.iUser_Type_Id)
                        {
                            case 1:
                            case 2:
                                Response.Redirect("/AdminHome.aspx", false);
                                break;
                            case 3:
                            case 4:
                                Response.Redirect("/FinancerHome.aspx", false);
                                break;
                            case 5:
                            case 6:
                                Response.Redirect("/InsurerHome.aspx", false);
                                break;
                        }
                        ErrorMessage.Visible = false;
                    }
                }

                else
                {
                    ErrorMessage.Visible = true;
                    litFailureText.Text = "<label for='" + txtUserName.ClientID + "' class='txtnamevalidation erroMessage'>Log in failed</label>";
                }

            }
            catch (Exception ex)
            {

                Response.Redirect("http://localhost:81/account/login?" + ex.Message);
                Response.Write(ex.StackTrace + " ******:******  " + ex.Message);
                throw;
            }
        }
    }
}