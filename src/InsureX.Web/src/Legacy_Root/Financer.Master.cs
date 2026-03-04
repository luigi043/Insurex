using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using C = IAPR_Data.Classes;
using Pro = IAPR_Data.Providers;
namespace IAPR_Web
{
    public partial class FinancerMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }




            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            C.Common.CurrentUser objUser = new C.Common.CurrentUser();
            Pro.User_Provider uP = new Pro.User_Provider();

            objUser = uP.GetUserFromSession();


            if (objUser == null)
            {
                Response.Redirect("/account/login.aspx");
            }
            else
            {
                lblFirstName.Text = objUser.vcName;
                                
                //Image img = (Image)this.FindControl("clientlogo");
                //img.Attributes.Add("src", "http://insurex.lendority.co.za/ClientLogos/" + objUser.vcPartnerLogo);

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "formatCurrencyTextBox()", true);


                string _dataTableScript = @"$('#dataTable').DataTable();";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "mydataTable", _dataTableScript, true);


                string _selectPickerScript = @"$('.selectpicker').selectpicker();";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "myselectPicker", _selectPickerScript, true);
            }
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }
        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            Session["CurrentUser"] = null;
            Response.Redirect("login.aspx");
        }
    }

}