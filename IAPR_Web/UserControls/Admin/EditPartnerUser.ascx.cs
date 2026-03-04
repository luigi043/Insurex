using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using C = IAPR_Data.Classes;
using P = IAPR_Data.Providers;
using System.Globalization;
using CCom = IAPR_Data.Classes.Common;
using U = IAPR_Data.Utils;
namespace IAPR_Web.UserControls.Admin
{
    public partial class EditPartnerUser : System.Web.UI.UserControl
    {
        CCom.CurrentUser objUser = new CCom.CurrentUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ViewState["SortOrder"] = "ASC";
                ViewState["SortField"] = "vcName";
                P.User_Provider uP = new P.User_Provider();

                objUser = uP.GetUserFromSession();

                if (objUser.iUser_Type_Id == 1 || objUser.iUser_Type_Id == 2)
                {
                    pnlPartner.Visible = true;
                }
                else
                {
                    GetFormFields();
                    hdPartnerID.Value = IAPR_Data.Utils.CryptorEngine.GenericEncrypt(objUser.iPartner_Id.ToString(), true);
                    hdPartnerTypeId.Value = IAPR_Data.Utils.CryptorEngine.GenericEncrypt(objUser.iPartner_Type_Id.ToString(), true);
                    Get_Partner_Users(objUser.iPartner_Id, objUser.iPartner_Type_Id, ViewState["SortField"].ToString());
                    pnlSelectUser.Visible = true;
                }
            }
        }
        public void GetFormFields()
        {
            //if (!IsPostBack)
            //{
            //GetFormFields();
            ViewState["SortOrder"] = "ASC";
            ViewState["SortField"] = "vcName";
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();
                                  
            if (objUser.iUser_Type_Id == 1 || objUser.iUser_Type_Id == 2)
            {
                pnlPartner.Visible = true;

            }
            else
            {
                hdPartnerID.Value = IAPR_Data.Utils.CryptorEngine.GenericEncrypt(objUser.iPartner_Id.ToString(), true);
                hdPartnerTypeId.Value = IAPR_Data.Utils.CryptorEngine.GenericEncrypt(objUser.iPartner_Type_Id.ToString(), true);
                Get_Partner_Users(objUser.iPartner_Id, objUser.iPartner_Type_Id, ViewState["SortField"].ToString());
                pnlSelectUser.Visible = true;
            }
            //}
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsGeneric();

            //Clear all DropDownLists
            ddlPartnerType.Items.Clear();
            ddlUserStatus.Items.Clear();


            //Insert Empty 1st option 
            ddlPartnerType.Items.Add(new ListItem("", ""));
            ddlUserStatus.Items.Add(new ListItem("", ""));

            //Populate relevant dropdownlists
            //Insurance companies
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                ddlPartnerType.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //User status
            foreach (DataRow row in ds.Tables[14].Rows)
            {
                ddlUserStatus.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }


        }
        private void Get_Partner_Users(int iPartner_Id, int iPartner_Type_Id, string sortField)
        {

            try
            {
                var param = sortField;
                var pi = typeof(C.Common.CurrentUser).GetProperty(param);

                rptUserList.DataSource = null;
                rptUserList.DataBind();

                List<C.Common.CurrentUser> uninsured_AssetsL = new List<C.Common.CurrentUser>();
                P.User_Provider frmF = new P.User_Provider();
                uninsured_AssetsL = frmF.Get_Partner_Users(iPartner_Id, iPartner_Type_Id);

                if (uninsured_AssetsL.Count > 0)
                {
                    if (ViewState["SortOrder"].ToString() == "ASC")
                    {
                        rptUserList.DataSource = uninsured_AssetsL.OrderBy(x => pi.GetValue(x, null)).ToList();

                    }
                    else
                    {
                        rptUserList.DataSource = uninsured_AssetsL.OrderByDescending(x => pi.GetValue(x, null)).ToList();

                    }
                    rptUserList.DataBind();
                    rptUserList.Visible = true;

                }
                ViewState["SortField"] = sortField;

            }
            catch (Exception)
            {

                throw;
            }


        }
        protected void rptUserList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string[] param = e.CommandArgument.ToString().Split(new Char[] { ';' });
            if (e.CommandName == "Sort")
            {
                if (ViewState["SortField"].ToString() == param[0] && ViewState["SortOrder"].ToString() == "ASC")
                {
                    ViewState["SortOrder"] = "DES";
                }
                else
                {
                    ViewState["SortOrder"] = "ASC";
                }
                ViewState["SortField"] = param[0];
                Get_Partner_Users(Convert.ToInt32(IAPR_Data.Utils.CryptorEngine.GenericDecrypt(hdPartnerID.Value, true)), Convert.ToInt32(IAPR_Data.Utils.CryptorEngine.GenericDecrypt(hdPartnerTypeId.Value, true)), param[0]);
            }

            if (e.CommandName == "EditUser")
            {
                // iUser_Id") + "; " 
                //+ DataBinder.Eval(Container.DataItem, "iUser_Type_Id")
                //+ ";" + DataBinder.Eval(Container.DataItem, "vcName")
                //+ ";" + DataBinder.Eval(Container.DataItem, "vcSurname")
                //+ ";" + DataBinder.Eval(Container.DataItem, "vcUsername")
                //+ ";" + DataBinder.Eval(Container.DataItem, "vcPosition_Title")
                //+ ";" + DataBinder.Eval(Container.DataItem, "vcContactNumber")
                //+ ";" + DataBinder.Eval(Container.DataItem, "bUserReceiveNotifications
                hdUID.Value = IAPR_Data.Utils.CryptorEngine.GenericEncrypt(param[0], true);
                txtFirst_Names.Text = param[2];
                txtSurname.Text = param[3];
                txtEmail_Address.Text = param[4];
                txtPosition_Title.Text = param[5];
                txtContact_Number.Text = param[6];
                if (Convert.ToBoolean(param[7]))
                {
                    rblNotifications.ClearSelection();
                    rblNotifications.SelectedIndex = 0;
                }
                else
                {
                    rblNotifications.ClearSelection();
                    rblNotifications.SelectedIndex = 1;
                }
                ddlUserStatus.ClearSelection();
                ddlUserStatus.Items.FindByValue((param[8])).Selected = true;
                pnlEditUser.Visible = true;
                pnlSelectUser.Visible = false;
            }


        }


        protected void ddlPartnerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsGeneric();

            //Clear all DropDownLists
            ddlPartners.Items.Clear();

            //Insert Empty 1st option 
            ddlPartners.Items.Add(new ListItem("", ""));


            //Populate relevant dropdownlists
            //Insurance companies
            if (Convert.ToInt32(ddlPartnerType.SelectedValue) == Convert.ToInt32(CCom.Common.Partner_types.Insurance_provider))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ddlPartners.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                }
            }

            //Finance Companies
            if (Convert.ToInt32(ddlPartnerType.SelectedValue) == Convert.ToInt32(CCom.Common.Partner_types.Lender))
            {
                foreach (DataRow row in ds.Tables[13].Rows)
                {
                    ddlPartners.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                }
            }
        }
        protected void ddlPartners_SelectedIndexChanged(object sender, EventArgs e)
        {
            Get_Partner_Users(Convert.ToInt32(ddlPartners.SelectedValue), Convert.ToInt32(ddlPartnerType.SelectedValue), ViewState["SortField"].ToString());
            hdPartnerID.Value = IAPR_Data.Utils.CryptorEngine.GenericEncrypt(ddlPartners.SelectedValue, true);
            hdPartnerTypeId.Value = IAPR_Data.Utils.CryptorEngine.GenericEncrypt(ddlPartnerType.SelectedValue, true);
            pnlSelectUser.Visible = true;
        }
        protected void btnUpdateUser_Click(object sender, EventArgs e)
        {
            try
            {

                CCom.CurrentUser user = new CCom.CurrentUser();
                user.iUser_Id = Convert.ToInt32(IAPR_Data.Utils.CryptorEngine.GenericDecrypt(hdUID.Value, true));
                user.vcName = txtFirst_Names.Text;
                user.vcSurname = txtSurname.Text;
                user.vcPosition_Title = txtPosition_Title.Text;
                user.vcContactNumber = txtContact_Number.Text;
                user.bUserReceiveNotifications = rblNotifications.SelectedValue == "Yes" ? true : false;
                user.iUser_Status_Id = Convert.ToInt32(ddlUserStatus.SelectedValue);
                P.User_Provider pro = new P.User_Provider();
                pro.Update_User(user);
                Get_Partner_Users(Convert.ToInt32(ddlPartners.SelectedValue), Convert.ToInt32(ddlPartnerType.SelectedValue), ViewState["SortField"].ToString());
                pnlSaveButtons.Visible = false;
                pnlSuccess.Visible = true;
                pnlEditUser.Visible = false;
                pnlSelectUser.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('User updated');", true);
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "EditPartnerUser-UserControl", "btnUpdateUser_Click");
            }

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            pnlEditUser.Visible = false;
            pnlSelectUser.Visible = true;
        }
    }
}