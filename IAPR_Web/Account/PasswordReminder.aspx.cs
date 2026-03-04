using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using AT = IAPR_Data.Classes;
using CP = IAPR_Data.Classes.Policy;
using CCom = IAPR_Data.Classes.Common;
using P = IAPR_Data.Providers;
using U = IAPR_Data.Utils;
using System.Configuration;

namespace IAPR_Web.Account
{
    public partial class PasswordReminder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { }
        }

        protected void btnPasswordReminder_Click(object sender, EventArgs e)
        {
            P.Admin_Provider pro = new P.Admin_Provider();

            if (pro.Check_Username(txtEmail_Address.Text))
            {
                SendPassword();
                litUserExists.Text = "<label for='" + txtEmail_Address.ClientID + "' class=''></label>";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Password request confirmation has been sent to your email addrress');", true);
            }
            else
            {
                litUserExists.Text = "<label for='" + txtEmail_Address.ClientID + "' class='txtnamevalidation erroMessage'>Username not found</label>";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Username not found');", true);
            }
        }

        private void SendPassword()
        {
                      

            try
            {


                string linkDetails = string.Empty;
                P.User_Provider pro = new P.User_Provider();
                SqlDataReader dr = pro.Get_User_Password_Reminder_Details(txtEmail_Address.Text);
                while (dr.Read())

                {
                    linkDetails = "/Account/PasswordRequestConfirm.aspx?id=" + dr["iPassword_reminder_Id"].ToString() + "&gd=" + dr["GUID"].ToString();

                }
                P.Notification_Provider nP = new P.Notification_Provider();
                nP.Password_Reminder_Request(txtEmail_Address.Text, ConfigurationManager.AppSettings["Application_URL"] + linkDetails, "PasswordReminderRequest");
                Response.Redirect("/account/login.aspx");

            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddPartner-UserControl", "SaveInsurer");
            }

        }

        protected void btnCancelChangeCover_Click(object sender, EventArgs e)
        {
            Response.Redirect("/account/login.aspx");
        }
    }
}