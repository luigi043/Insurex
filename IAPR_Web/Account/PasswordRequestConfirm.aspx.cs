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
    public partial class PasswordRequestConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CheckQueryStrings())

                {
                    SendPassword();
                }
            }
        }

        private bool CheckQueryStrings()
        {
            bool exists = true;
            string id = Request.QueryString["id"] as string;
            if (id == null)
            {
                exists = false;
            }

            string gd = Request.QueryString["gd"] as string;
            if (gd == null)
            {
                //If it exists
                exists = false;
            }


            return exists;

        }
        private void SendPassword()
        {
            bool sent = false;
            int id = Convert.ToInt32(Request.QueryString["id"]);
            string gd = Request.QueryString["gd"];
            try
            {


                string linkDetails = string.Empty;
                P.User_Provider pro = new P.User_Provider();
                SqlDataReader dr = pro.Get_User_Password_Reminder(id, gd);
                while (dr.Read())
                {
                    P.Notification_Provider nP = new P.Notification_Provider();
                    nP.Password_Reminder_Confirm(dr["vcUsername"].ToString(), dr["vcPassword"].ToString(), "PasswordReminderConfirm");
                    pnlSuccess.Visible = true;
                    sent = true;
                }

                if (sent)
                {
                    pnlSuccess.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Your password has been sent to your email addrress');", true);
                }
                else
                {
                    pnlFail.Visible = true;
                }
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddPartner-UserControl", "SaveInsurer");
            }

        }

        protected void btnPasswordReminder_Click(object sender, EventArgs e)
        {
            Response.Redirect("/account/login.aspx");

        }
    }
}
