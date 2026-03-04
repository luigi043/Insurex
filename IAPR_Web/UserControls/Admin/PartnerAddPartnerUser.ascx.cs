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

namespace IAPR_Web.UserControls.Admin
{
    public partial class PartnerAddPartnerUser : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetFormFields();
            }
        }
        private void GetFormFields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsGeneric();

            //Clear all DropDownLists


            //Insert Empty 1st option 



            //Populate relevant dropdownlists
            //Insurance companies




        }


        protected void btnAddPartnerUser_Click(object sender, EventArgs e)
        {
            P.User_Provider uP = new P.User_Provider();
            CCom.CurrentUser objUser = new CCom.CurrentUser();

            objUser = uP.GetUserFromSession();

            if (objUser == null)
            {

                //Response.Redirect("");
            }
            P.Admin_Provider pro = new P.Admin_Provider();
            if (objUser != null)
            {
                if (!pro.Check_Username(txtEmail_Address.Text))
                {
                    SaveUser(objUser);
                    ClearForm();
                    litUserExists.Text = "<label for='" + txtEmail_Address.ClientID + "' class=''></label>";
                    //pnlSaveButtons.Visible = false;
                    //pnlSuccess.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('User saved');", true);
                }
                else
                {
                    litUserExists.Text = "<label for='" + txtEmail_Address.ClientID + "' class='txtnamevalidation erroMessage'>Username already exists</label>";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Username is already in use');", true);
                }
            }
        }

        public bool SaveUser(CCom.CurrentUser u)
        {
            string pw = string.Empty;
            bool saved = false;

            P.User_Provider uP = new P.User_Provider();
            CCom.CurrentUser objUser = new CCom.CurrentUser();

            objUser = uP.GetUserFromSession();

            if (objUser == null)
            {

                //Response.Redirect("");
            }



            try
            {

                CCom.CurrentUser user = new CCom.CurrentUser();

                if (u.iPartner_Type_Id == 1)
                {
                    user.iUser_Type_Id = 6;
                }
                if (u.iPartner_Type_Id == 2)
                {
                    user.iUser_Type_Id = 4;
                }
                user.iPartner_Type_Id = Convert.ToInt32(u.iPartner_Type_Id);
                user.iPartner_Id = Convert.ToInt32(u.iPartner_Id);
                user.vcName = txtFirst_Names.Text;
                user.vcSurname = txtSurname.Text;
                user.vcPosition_Title = txtPosition_Title.Text;
                user.vcUsername = txtEmail_Address.Text;
                user.vcContactNumber = txtContact_Number.Text;
                //user.vcPassword = "password12";
                user.bUserReceiveNotifications = rblNotifications.SelectedValue == "Yes" ? true : false;

                P.Admin_Provider pro = new P.Admin_Provider();
                pw = pro.Save_User(user);
                P.Notification_Provider nP = new P.Notification_Provider();
                nP.NotifyNewUser(user.vcName, user.vcUsername, objUser.vcPartner_Name, ConfigurationManager.AppSettings["Application_URL"], pw, "NewUser");

                saved = true;

            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddPartner-UserControl", "SaveInsurer");
            }
            return saved;
        }
        private void ClearForm()
        {

            txtFirst_Names.Text = "";
            txtSurname.Text = "";
            txtEmail_Address.Text = "";
            txtPosition_Title.Text = "";

            txtContact_Number.Text = "";
            rblNotifications.ClearSelection();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}