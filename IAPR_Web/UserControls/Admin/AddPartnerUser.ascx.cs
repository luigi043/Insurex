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
    public partial class AddPartnerUser : System.Web.UI.UserControl
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
            ddlPartnerType.Items.Clear();



            //Insert Empty 1st option 
            ddlPartnerType.Items.Add(new ListItem("", ""));


            //Populate relevant dropdownlists
            //Insurance companies
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                ddlPartnerType.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }



        }

        protected void ddlPartnerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsGeneric();

            //Clear all DropDownLists
            ddlPartnerList.Items.Clear();



            //Insert Empty 1st option 
            ddlPartnerList.Items.Add(new ListItem("", ""));


            //Populate relevant dropdownlists
            //Insurance companies
            if (Convert.ToInt32(ddlPartnerType.SelectedValue) == Convert.ToInt32(CCom.Common.Partner_types.Insurance_provider))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ddlPartnerList.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                }
            }

            //Finance Companies
            if (Convert.ToInt32(ddlPartnerType.SelectedValue) == Convert.ToInt32(CCom.Common.Partner_types.Lender))
            {
                foreach (DataRow row in ds.Tables[13].Rows)
                {
                    ddlPartnerList.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                }
            }
        }

        protected void btnAddPartnerUser_Click(object sender, EventArgs e)
        {
            P.Admin_Provider pro = new P.Admin_Provider();

            if (!pro.Check_Username(txtEmail_Address.Text))
            {
                SaveUser();
                litUserExists.Text = "<label for='" + txtEmail_Address.ClientID + "' class=''></label>";
                pnlSaveButtons.Visible = false;
                pnlSuccess.Visible = true;
                ClearForm();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('User saved');", true);
            }
            else
            {
                litUserExists.Text = "<label for='" + txtEmail_Address.ClientID + "' class='txtnamevalidation erroMessage'>Username already exists</label>";
            }
        }

        public bool SaveUser()
        {
            string pw = string.Empty;
            bool saved = false;

            try
            {

                CCom.CurrentUser user = new CCom.CurrentUser();

                if (ddlPartnerType.SelectedValue == "1")
                {
                    user.iUser_Type_Id = 6;
                }
                if (ddlPartnerType.SelectedValue == "2")
                {
                    user.iUser_Type_Id = 4;
                }
                user.iPartner_Type_Id = Convert.ToInt32(ddlPartnerType.SelectedValue);
                user.iPartner_Id = Convert.ToInt32(ddlPartnerList.SelectedValue);
                user.vcName = txtFirst_Names.Text;
                user.vcSurname = txtSurname.Text;
                user.vcPosition_Title = txtPosition_Title.Text;
                user.vcUsername = txtEmail_Address.Text;
                user.vcContactNumber = txtContact_Number.Text;
                user.bUserReceiveNotifications = rblNotifications.SelectedValue == "Yes" ? true : false;

                P.Admin_Provider pro = new P.Admin_Provider();
                pw = pro.Save_User(user);

                P.Notification_Provider nP = new P.Notification_Provider();
                nP.NotifyNewUser(user.vcName, user.vcUsername, ddlPartnerList.SelectedItem.Text, ConfigurationManager.AppSettings["Application_URL"], pw, "NewUser");

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