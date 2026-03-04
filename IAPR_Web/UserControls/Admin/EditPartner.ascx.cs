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
    public partial class EditPartner : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
        public void GetFormFields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsGeneric();

            //Clear all DropDownLists
            ddlPartnerType.Items.Clear();
            ddlProvince.Items.Clear();
            //chkAssetsFinanced.Items.Clear();

            //Insert Empty 1st option 
            ddlPartnerType.Items.Add(new ListItem("", ""));
            ddlProvince.Items.Add(new ListItem("", ""));

            //Populate relevant dropdownlists
            //Insurance companies
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                ddlPartnerType.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }


            //Pronvinces
            foreach (DataRow row in ds.Tables[5].Rows)
            {
                ddlProvince.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Asset Types
            foreach (DataRow row in ds.Tables[12].Rows)
            {
                // chkAssetsFinanced.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            ViewState["PackageAssets"] = ds.Tables[15];
        }
        protected void chkPostalSameAsPhysical_CheckedChanged(object sender, EventArgs e)
        {
            pnlPostalAddress.Visible = chkPostalSameAsPhysical.Checked ? false : true;

        }


        protected void btnAddPartner_Click(object sender, EventArgs e)
        {
            P.Admin_Provider pro = new P.Admin_Provider();


            if (ddlPartnerType.SelectedValue == "1")
            {
                UpdateInsurer(Convert.ToInt32(ddlPartners.SelectedValue));

            }
            else
            {
                UpdateFinancer(Convert.ToInt32(ddlPartners.SelectedValue));
            }

            litUserExists.Text = "<label for='" + txtEmail_Address.ClientID + "' class=''></label>";
            //pnlSaveButtons.Visible = false;
            //pnlSuccess.Visible = true;
            ClearForm();
            pnlStep2.Visible = false;            
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Partner updated');", true);

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
            GetPartnerDetails();
            // txtPartnerName.Text = "test";
            //  pnlStep1.Enabled = false;
            pnlStep2.Visible = true;
            hdPartnerID.Value = IAPR_Data.Utils.CryptorEngine.GenericEncrypt(ddlPartners.SelectedValue, true);
            hdPartnerTypeId.Value = IAPR_Data.Utils.CryptorEngine.GenericEncrypt(ddlPartnerType.SelectedValue, true);

        }

        public bool UpdateFinancer(int iPartner_Id)
        {
            bool saved = false;
            if (!Page.IsValid)
            {
                return false;
            }
            try
            {
                AT.Partners.Financer fi = new AT.Partners.Financer();
                List<AT.Partners.Financed_Assets> lsfinanced_Assets = new List<AT.Partners.Financed_Assets>();

                CCom.CurrentUser user = new CCom.CurrentUser();
                fi.iPackage_Id = Convert.ToInt32(rblPackages.SelectedValue);
                fi.vcFinancer_Name = txtPartnerName.Text;
                fi.vcBusiness_registration_Number = txtBusinessNumber.Text;
                fi.vcVat_Registration_Number = txtVatRegistrationNumber.Text;
                fi.vcBuilding_Unit = txtBuilding_Unit.Text;
                fi.vcAddress_Line_1 = txtAddress_Line_1.Text;
                fi.vcAddress_Line_2 = txtAddress_Line_2.Text;
                fi.vcSuburb = txtSuburb.Text;
                fi.vcCity = txtCity.Text;
                fi.vcPostal_Code = txtPostal_Code.Text;
                fi.iProvince_Id = Convert.ToInt32(ddlProvince.SelectedValue);
                fi.vcPOBox_Bag = txtPOBox_Bag.Text;
                fi.vcPost_Office_Name = txtPost_Office_Name.Text;
                fi.vcPost_Office_Postal_Code = txtPost_Postal_Code.Text;
                fi.vcContact_Number = txtCompanyContactNumber.Text;


                //int j = 1;
                //foreach (ListItem i in chkAssetsFinanced.Items)
                //{
                //    AT.Partners.Financed_Assets ifinanced_Assets = new AT.Partners.Financed_Assets();
                //    if (i.Selected == true)
                //    {
                //        ifinanced_Assets.iCounter = j;
                //        ifinanced_Assets.iAsset_Type_Id = Convert.ToInt32(i.Value);

                //        lsfinanced_Assets.Add(ifinanced_Assets);
                //    }
                //    j++;
                //}

                P.Admin_Provider pro = new P.Admin_Provider();
                pro.Update_Financer(iPartner_Id, fi, lsfinanced_Assets);



                P.Notification_Provider nP = new P.Notification_Provider();
                //nP.NotifyNewUser(txtFirst_Names.Text, txtEmail_Address.Text, txtPartnerName.Text, ConfigurationManager.AppSettings["Application_URL"], pw, "NewUser");

                saved = true;
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddPartner-UserControl", "SaveFinancer");
            }
            return saved;
        }

        public bool UpdateInsurer(int iPartner_Id)
        {
            bool saved = false;
            if (!Page.IsValid)
            {
                return false;
            }
            try
            {
                AT.Partners.Insurer inS = new AT.Partners.Insurer();
                CCom.CurrentUser user = new CCom.CurrentUser();

                inS.vcInsurance_Company_Name = txtPartnerName.Text;
                inS.vcBusiness_registration_Number = txtBusinessNumber.Text;
                inS.vcVat_Registration_Number = txtVatRegistrationNumber.Text;
                inS.vcBuilding_Unit = txtBuilding_Unit.Text;
                inS.vcAddress_Line_1 = txtAddress_Line_1.Text;
                inS.vcAddress_Line_2 = txtAddress_Line_2.Text;
                inS.vcSuburb = txtSuburb.Text;
                inS.vcCity = txtCity.Text;
                inS.vcPostal_Code = txtPostal_Code.Text;
                inS.iProvince_Id = Convert.ToInt32(ddlProvince.SelectedValue);
                inS.vcPOBox_Bag = txtPOBox_Bag.Text;
                inS.vcPost_Office_Name = txtPost_Office_Name.Text;
                inS.vcPost_Office_Postal_Code = txtPost_Postal_Code.Text;
                inS.vcContact_Number = txtCompanyContactNumber.Text;


                P.Admin_Provider pro = new P.Admin_Provider();
                pro.Update_Insurer(iPartner_Id, inS);

                P.Notification_Provider nP = new P.Notification_Provider();
                //nP.NotifyNewUser(user.vcName, user.vcUsername, txtPartnerName.Text, ConfigurationManager.AppSettings["Application_URL"], pw, "NewUser");


                saved = true;


            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddPartner-UserControl", "SaveInsurer");
            }
            return saved;
        }


        private void GetPartnerDetails()
        {

            try
            {

                P.Partner_Provider frmF = new P.Partner_Provider();
                DataSet ds = frmF.Get_Partner_Deatils(Convert.ToInt32(ddlPartnerType.SelectedValue), Convert.ToInt32(ddlPartners.SelectedValue));


                if (ddlPartnerType.SelectedValue == ((int)CCom.Common.Partner_types.Insurance_provider).ToString())
                {
                    if (ds.Tables[0].Rows[0][1] != DBNull.Value) txtPartnerName.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[0].Rows[0][1].ToString(), true);

                    if (ds.Tables[0].Rows[0][2] != DBNull.Value) txtBusinessNumber.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[0].Rows[0][2].ToString(), true);

                    if (ds.Tables[1].Rows[0]["vcName"] != DBNull.Value) txtFirst_Names.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcName"].ToString(), true);
                    if (ds.Tables[1].Rows[0]["vcSurname"] != DBNull.Value) txtSurname.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcSurname"].ToString(), true);
                    if (ds.Tables[1].Rows[0]["vcPosition_Title"] != DBNull.Value) txtPosition_Title.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcPosition_Title"].ToString(), true);
                    if (ds.Tables[1].Rows[0]["vcUsername"] != DBNull.Value) txtEmail_Address.Text = U.CryptorEngine.ValidationDecrypt(ds.Tables[1].Rows[0]["vcUsername"].ToString(), true);
                    if (ds.Tables[1].Rows[0]["vcContactNumber"] != DBNull.Value) txtContact_Number.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcContactNumber"].ToString(), true);


                    bool bUserReceiveNotifications = Convert.ToBoolean(ds.Tables[1].Rows[0][9]);
                    if (!bUserReceiveNotifications)
                    {
                        rblNotifications.ClearSelection();
                        rblNotifications.SelectedIndex = bUserReceiveNotifications ? 0 : 1;
                    }
                }
                if (ddlPartnerType.SelectedValue == ((int)CCom.Common.Partner_types.Lender).ToString())
                {
                    if (ds.Tables[0].Rows[0]["vcFinancer_Name"] != DBNull.Value) txtPartnerName.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[0].Rows[0]["vcFinancer_Name"].ToString(), true);

                    if (ds.Tables[0].Rows[0]["vcBusiness_registration_Number"] != DBNull.Value) txtBusinessNumber.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[0].Rows[0]["vcBusiness_registration_Number"].ToString(), true);

                    if (ds.Tables[2].Rows[0]["vcName"] != DBNull.Value) txtFirst_Names.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[2].Rows[0]["vcName"].ToString(), true);
                    if (ds.Tables[2].Rows[0]["vcSurname"] != DBNull.Value) txtSurname.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[2].Rows[0]["vcSurname"].ToString(), true);
                    if (ds.Tables[2].Rows[0]["vcPosition_Title"] != DBNull.Value) txtPosition_Title.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[2].Rows[0]["vcPosition_Title"].ToString(), true);
                    if (ds.Tables[2].Rows[0]["vcUsername"] != DBNull.Value) txtEmail_Address.Text = U.CryptorEngine.ValidationDecrypt(ds.Tables[2].Rows[0]["vcUsername"].ToString(), true);
                    if (ds.Tables[2].Rows[0]["vcContactNumber"] != DBNull.Value) txtContact_Number.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[2].Rows[0]["vcContactNumber"].ToString(), true);


                    //foreach (ListItem i in chkAssetsFinanced.Items)
                    //{
                    //    foreach (DataRow row in ds.Tables[1].Rows)
                    //    {
                    //        if (row[0].ToString() == i.Value)
                    //        {
                    //            i.Selected = true;
                    //            i.Enabled = false;
                    //        }
                    //    }
                    //}

                    //string province = Convert.ToInt32(ds.Tables[0].Rows[0]["iProvince_Id"]).ToString();
                    // if (ds.Tables[0].Rows[0]["iProvince_Id"] != DBNull.Value)
                    ddlProvince.ClearSelection();
                    rblPackages.Items.FindByValue(ds.Tables[0].Rows[0]["iPackage_Id"].ToString()).Selected = true;

                    pnlAssetsFinanced.Visible = true;

                    bool bUserReceiveNotifications = Convert.ToBoolean(ds.Tables[2].Rows[0][9]);
                    if (!bUserReceiveNotifications)
                    {
                        rblNotifications.ClearSelection();
                        rblNotifications.SelectedIndex = bUserReceiveNotifications ? 0 : 1;
                    }
                    ApplyPackagessets();
                }
                //Physical Address
                if (ds.Tables[0].Rows[0]["vcBuilding_Unit"] != DBNull.Value) txtBuilding_Unit.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[0].Rows[0]["vcBuilding_Unit"].ToString(), true); ;
                if (ds.Tables[0].Rows[0]["vcAddress_Line_1"] != DBNull.Value) txtAddress_Line_1.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[0].Rows[0]["vcAddress_Line_1"].ToString(), true);
                if (ds.Tables[0].Rows[0]["vcAddress_Line_2"] != DBNull.Value) txtAddress_Line_2.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[0].Rows[0]["vcAddress_Line_2"].ToString(), true);
                if (ds.Tables[0].Rows[0]["vcSuburb"] != DBNull.Value) txtSuburb.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[0].Rows[0]["vcSuburb"].ToString(), true);
                if (ds.Tables[0].Rows[0]["vcCity"] != DBNull.Value) txtCity.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[0].Rows[0]["vcCity"].ToString(), true);
                if (ds.Tables[0].Rows[0]["vcPostal_Code"] != DBNull.Value) txtPostal_Code.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[0].Rows[0]["vcPostal_Code"].ToString(), true);
                if (ds.Tables[0].Rows[0]["vcContact_Number"] != DBNull.Value) txtCompanyContactNumber.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[0].Rows[0]["vcContact_Number"].ToString(), true);
                string province = Convert.ToInt32(ds.Tables[0].Rows[0]["iProvince_Id"]).ToString();
                // if (ds.Tables[0].Rows[0]["iProvince_Id"] != DBNull.Value)
                ddlProvince.ClearSelection();
                ddlProvince.Items.FindByValue(province).Selected = true;

                bool bPostalSameAsPhysical = false;
                //if (ddlPartnerType.SelectedValue == "1")
                //{
                bPostalSameAsPhysical = ddlPartnerType.SelectedValue == "1" ? Convert.ToBoolean(ds.Tables[0].Rows[0][9]) : Convert.ToBoolean(ds.Tables[0].Rows[0][11]);
                // }
                if (!bPostalSameAsPhysical)
                {
                    chkPostalSameAsPhysical.Checked = false;
                    pnlPostalAddress.Visible = true;
                    if (ds.Tables[0].Rows[0]["vcPOBox_Bag"] != DBNull.Value) txtPOBox_Bag.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[0].Rows[0]["vcPOBox_Bag"].ToString(), true);
                    if (ds.Tables[0].Rows[0]["vcPost_Office_Name"] != DBNull.Value) txtPost_Office_Name.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[0].Rows[0]["vcPost_Office_Name"].ToString(), true);
                    if (ds.Tables[0].Rows[0]["vcPost_Office_Postal_Code"] != DBNull.Value) txtPost_Postal_Code.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[0].Rows[0]["vcPost_Office_Postal_Code"].ToString(), true);
                }
                else
                {
                    chkPostalSameAsPhysical.Checked = true;
                    pnlPostalAddress.Visible = false;

                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlStep2.Visible = false;
        }
        protected void rblPackages_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyPackagessets();
        }
        private void ClearForm()
        {
            ddlPartnerType.ClearSelection();
            ddlPartnerType.SelectedIndex = 0;
            ddlPartners.ClearSelection();
            ddlPartners.SelectedIndex = 0;
            txtPartnerName.Text = "";
            txtBusinessNumber.Text = "";
            txtVatRegistrationNumber.Text = "";
            rblPackages.ClearSelection();
            divPackageAssets.InnerHtml = "";
            txtBuilding_Unit.Text = "";
            txtAddress_Line_1.Text = "";
            txtAddress_Line_2.Text = "";
            txtSuburb.Text = "";
            txtCity.Text = "";
            txtPostal_Code.Text = "";
            ddlProvince.ClearSelection();
            ddlProvince.SelectedIndex = 0;
            chkPostalSameAsPhysical.Checked = false;
            txtPOBox_Bag.Text = "";
            txtPost_Office_Name.Text = "";
            txtPost_Postal_Code.Text = "";
            txtCompanyContactNumber.Text = "";

            txtFirst_Names.Text = "";
            txtSurname.Text = "";
            txtEmail_Address.Text = "";
            txtPosition_Title.Text = "";

            txtContact_Number.Text = "";
            rblNotifications.ClearSelection();
        }
        private void ApplyPackagessets()
        {
            DataTable dtPackageAssets = null;
            System.Text.StringBuilder s = new System.Text.StringBuilder();


            if (ViewState["PackageAssets"] != null)
            {
                dtPackageAssets = (DataTable)ViewState["PackageAssets"];
                s.Append(rblPackages.SelectedItem.Text + " package includes the following assets types:<br /><br />");
                foreach (DataRow row in dtPackageAssets.Rows)
                {
                    if (row[0].ToString() == rblPackages.SelectedValue)
                    {
                         s.Append("-" + row[3] + "<br />");

                    }
                }
                divPackageAssets.InnerHtml = s.ToString();
            }
        }
    }
}
