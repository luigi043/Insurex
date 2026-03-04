using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using AT = IAPR_Data.Classes.AssetTypes;
using CP = IAPR_Data.Classes.Policy;
using CCom = IAPR_Data.Classes.Common;
using P = IAPR_Data.Providers;
using U = IAPR_Data.Utils;
using System.Configuration;
namespace IAPR_Web.PolicyManagement
{
    public partial class ConfirmPolicyCover : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            P.User_Provider uP = new P.User_Provider();
            CCom.CurrentUser objUser = new CCom.CurrentUser();

            objUser = uP.GetUserFromSession();

            if (objUser == null)
            {

                Response.Redirect("/account/login.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    if (CheckQueryStrings())
                    {
                        if (Check_Alignment_Exists_Confirmation())
                        {
                            GetFormFields();
                            int ai = Convert.ToInt32(Request.QueryString["Ai"]);
                            int at = Convert.ToInt32(Request.QueryString["aType"]);
                            int phi = Convert.ToInt32(Request.QueryString["PhI"]);
                            string kl = Request.QueryString["kL"].ToString();
                            P.Customer_Provider p = new P.Customer_Provider();
                            int alnmlId = p.Get_Policy_To_Asset_Alignment_Id_For_Confirmation(ai, at, kl, phi);
                            hdAlnmlId.Value = U.CryptorEngine.GenericEncrypt(alnmlId.ToString(), true);
                            PreloadCustomerDeatils(alnmlId);
                            GetAssetDetails(alnmlId);
                            GetAssetDetailsConfirmation(alnmlId);
                            pnlStep2.Visible = true;
                        }
                    }
                    else
                    {
                        GetFormFields();
                        GetPoliciesAwaitingConfirmation(objUser.iPartner_Id);
                        pnlPoliciesAwaitingConfirmation.Visible = true;
                    }



                }
            }
        }


        private bool Check_Alignment_Exists_Confirmation()
        {
            int ai = Convert.ToInt32(Request.QueryString["Ai"]);
            int at = Convert.ToInt32(Request.QueryString["aType"]);
            int phi = Convert.ToInt32(Request.QueryString["PhI"]);
            string kl = Request.QueryString["kL"].ToString();

            P.Customer_Provider p = new P.Customer_Provider();
            return (p.Check_Policy_To_Asset_Alignment_Confirmation_Exists(ai, at, kl, phi));

        }
        private bool CheckQueryStrings()
        {
            bool exists = true;
            string Ai = Request.QueryString["Ai"] as string;
            if (Ai == null)
            {
                exists = false;
            }

            string aType = Request.QueryString["aType"] as string;
            if (aType == null)
            {
                //If it exists
                exists = false;
            }

            string PhI = Request.QueryString["PhI"] as string;
            if (PhI == null)
            {
                exists = false;
            }

            string kl = Request.QueryString["kL"] as string;
            if (kl == null)
            {
                exists = false;
            }

            //      @iAsset_Id int
            //, @iAsset_Type_Id int

            // , @vcLinkKey varchar(200)
            //,@iPolicy_Holder_Id
            return exists;

        }
        private void PreloadCustomerDeatils(int alnmI)
        {
            pnlStep1.Visible = true;
            pnlStep1.Enabled = false;
            try
            {

                P.Customer_Provider frmF = new P.Customer_Provider();
                DataSet ds = frmF.Get_Customer_Deatils_For_Alignment(alnmI);

                int iPolicy_Holder_Type = Convert.ToInt32(ds.Tables[0].Rows[0][7]);

                if (iPolicy_Holder_Type == Convert.ToInt32(CCom.Common.Policy_Holder_Types.Personal))
                {
                    pnlPersonalDetails.Visible = true;
                    //ddlIdentification_Type.Items.FindByValue(Convert.ToInt32(ds.Tables[1].Rows[0][1]).ToString()).Selected = true;
                    //ddlPerson_Title.Items.FindByValue(Convert.ToInt32(ds.Tables[1].Rows[0][2]).ToString()).Selected = true;

                    ddlIdentification_Type.ClearSelection();
                    ddlPerson_Title.ClearSelection();
                    if (ds.Tables[1].Rows[0]["iIdentification_Type_Id"] != DBNull.Value) ddlIdentification_Type.Items.FindByValue(Convert.ToInt32(ds.Tables[1].Rows[0]["iIdentification_Type_Id"]).ToString()).Selected = true;
                    if (ds.Tables[1].Rows[0]["iPerson_Title_Id"] != DBNull.Value) ddlPerson_Title.Items.FindByValue(Convert.ToInt32(ds.Tables[1].Rows[0]["iPerson_Title_Id"]).ToString()).Selected = true;

                    if (ds.Tables[1].Rows[0]["vcFirst_Names"] != DBNull.Value) txtFirst_Names.Text = ds.Tables[1].Rows[0]["vcFirst_Names"].ToString();
                    if (ds.Tables[1].Rows[0]["vcSurname"] != DBNull.Value) txtSurname.Text = ds.Tables[1].Rows[0]["vcSurname"].ToString();
                    if (ds.Tables[1].Rows[0]["vcIdentification_Number"] != DBNull.Value) txtIdentification_Number.Text = ds.Tables[1].Rows[0]["vcIdentification_Number"].ToString();
                    if (ds.Tables[1].Rows[0]["vcContact_Number"] != DBNull.Value) txtContact_Number.Text = ds.Tables[1].Rows[0]["vcContact_Number"].ToString();
                    if (ds.Tables[1].Rows[0]["vcAlternative_Contact_Number"] != DBNull.Value) txtAlternative_Contact_Number.Text = ds.Tables[1].Rows[0]["vcAlternative_Contact_Number"].ToString();
                    if (ds.Tables[1].Rows[0]["vcEmail_Address"] != DBNull.Value) txtEmail_Address.Text = ds.Tables[1].Rows[0]["vcEmail_Address"].ToString();



                }

                if (iPolicy_Holder_Type == Convert.ToInt32(CCom.Common.Policy_Holder_Types.Business))
                {
                    pnlBusinessDetails.Visible = true;

                    if (ds.Tables[1].Rows[0]["vcBusiness_Name"] != DBNull.Value) txtBusiness_Name.Text = ds.Tables[1].Rows[0]["vcBusiness_Name"].ToString();
                    if (ds.Tables[1].Rows[0]["vcBusiness_Registration_Number"] != DBNull.Value) txtBusiness_Registration_Number.Text = ds.Tables[1].Rows[0]["vcBusiness_Registration_Number"].ToString();
                    if (ds.Tables[1].Rows[0]["vcBusiness_Contact_Fullname"] != DBNull.Value) txtBusiness_Contact_Fullname.Text = ds.Tables[1].Rows[0]["vcBusiness_Contact_Fullname"].ToString();
                    if (ds.Tables[1].Rows[0]["vcBusiness_Contact_Number"] != DBNull.Value) txtBusiness_Contact_Number.Text = ds.Tables[1].Rows[0]["vcBusiness_Contact_Number"].ToString();
                    if (ds.Tables[1].Rows[0]["vcBusiness_Contact_Alternative_Number"] != DBNull.Value) txtBusiness_Contact_Alternative_Number.Text = ds.Tables[1].Rows[0]["vcBusiness_Contact_Alternative_Number"].ToString();
                    if (ds.Tables[1].Rows[0]["vcBusiness_Email_Address"] != DBNull.Value) txtBusiness_Email_Address.Text = ds.Tables[1].Rows[0]["vcBusiness_Email_Address"].ToString();

                }


                //Physical Address
                if (ds.Tables[2].Rows[0]["vcBuilding_Unit"] != DBNull.Value) txtBuilding_Unit.Text = ds.Tables[2].Rows[0]["vcBuilding_Unit"].ToString();
                if (ds.Tables[2].Rows[0]["vcAddress_Line_1"] != DBNull.Value) txtAddress_Line_1.Text = ds.Tables[2].Rows[0]["vcAddress_Line_1"].ToString();
                if (ds.Tables[2].Rows[0]["vcAddress_Line_2"] != DBNull.Value) txtAddress_Line_2.Text = ds.Tables[2].Rows[0]["vcAddress_Line_2"].ToString();
                if (ds.Tables[2].Rows[0]["vcSuburb"] != DBNull.Value) txtSuburb.Text = ds.Tables[2].Rows[0]["vcSuburb"].ToString();
                if (ds.Tables[2].Rows[0]["vcCity"] != DBNull.Value) txtCity.Text = ds.Tables[2].Rows[0]["vcCity"].ToString();
                if (ds.Tables[2].Rows[0]["vcPostal_Code"] != DBNull.Value) txtPostal_Code.Text = ds.Tables[2].Rows[0]["vcPostal_Code"].ToString();
                ddlProvince.ClearSelection();
                if (ds.Tables[2].Rows[0]["iProvince_Id"] != DBNull.Value) ddlProvince.Items.FindByValue(Convert.ToInt32(ds.Tables[2].Rows[0]["iProvince_Id"]).ToString()).Selected = true;




                if (ds.Tables[0].Rows[0]["iPolicy_Postal_Address_Id"] != DBNull.Value)
                {
                    chkPostalSameAsPhysical.Checked = false;
                    pnlPostalAddress.Visible = true;
                    if (ds.Tables[3].Rows[0]["vcPOBox_Bag"] != DBNull.Value) txtPOBox_Bag.Text = ds.Tables[3].Rows[0]["vcPOBox_Bag"].ToString();
                    if (ds.Tables[3].Rows[0]["vcPost_Office_Name"] != DBNull.Value) txtPost_Office_Name.Text = ds.Tables[3].Rows[0]["vcPost_Office_Name"].ToString();
                    if (ds.Tables[3].Rows[0]["vcPostalCode"] != DBNull.Value) txtPost_Postal_Code.Text = ds.Tables[3].Rows[0]["vcPostalCode"].ToString();
                }
                else
                {
                    pnlPostalAddress.Visible = false;
                    chkPostalSameAsPhysical.Checked = true;
                }
                pnlStep2.Visible = true;
            }
            catch (Exception)
            {


            }
        }
        //private void PreloadCustomerDeatils(int alnmI)
        //{
        //    pnlStep1.Visible = true;
        //    pnlStep1.Enabled = false;
        //    try
        //    {

        //        P.Customer_Provider frmF = new P.Customer_Provider();
        //        DataSet ds = frmF.Get_Customer_Deatils_For_Alignment(alnmI);

        //        int iPolicy_Holder_Type = Convert.ToInt32(ds.Tables[0].Rows[0][7]);

        //        if (iPolicy_Holder_Type == Convert.ToInt32(CCom.Common.Policy_Holder_Types.Personal))
        //        {
        //            pnlPersonalDetails.Visible = true;
        //            //ddlIdentification_Type.Items.FindByValue(Convert.ToInt32(ds.Tables[1].Rows[0][1]).ToString()).Selected = true;
        //            //ddlPerson_Title.Items.FindByValue(Convert.ToInt32(ds.Tables[1].Rows[0][2]).ToString()).Selected = true;

        //            ddlIdentification_Type.ClearSelection();
        //            ddlPerson_Title.ClearSelection();
        //            if (ds.Tables[1].Rows[0]["iIdentification_Type_Id"] != DBNull.Value) ddlIdentification_Type.Items.FindByValue(Convert.ToInt32(ds.Tables[1].Rows[0]["iIdentification_Type_Id"]).ToString()).Selected = true;
        //            if (ds.Tables[1].Rows[0]["iPerson_Title_Id"] != DBNull.Value) ddlPerson_Title.Items.FindByValue(Convert.ToInt32(ds.Tables[1].Rows[0]["iPerson_Title_Id"]).ToString()).Selected = true;

        //            if (ds.Tables[1].Rows[0]["vcFirst_Names"] != DBNull.Value) txtFirst_Names.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcFirst_Names"].ToString(), true);
        //            if (ds.Tables[1].Rows[0]["vcSurname"] != DBNull.Value) txtSurname.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcSurname"].ToString(), true);
        //            if (ds.Tables[1].Rows[0]["vcIdentification_Number"] != DBNull.Value) txtIdentification_Number.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcIdentification_Number"].ToString(), true);
        //            if (ds.Tables[1].Rows[0]["vcContact_Number"] != DBNull.Value) txtContact_Number.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcContact_Number"].ToString(), true);
        //            if (ds.Tables[1].Rows[0]["vcAlternative_Contact_Number"] != DBNull.Value) txtAlternative_Contact_Number.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcAlternative_Contact_Number"].ToString(), true);
        //            if (ds.Tables[1].Rows[0]["vcEmail_Address"] != DBNull.Value) txtEmail_Address.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcEmail_Address"].ToString(), true);



        //        }

        //        if (iPolicy_Holder_Type == Convert.ToInt32(CCom.Common.Policy_Holder_Types.Business))
        //        {
        //            pnlBusinessDetails.Visible = true;

        //            if (ds.Tables[1].Rows[0]["vcBusiness_Name"] != DBNull.Value) txtBusiness_Name.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcBusiness_Name"].ToString(), true);
        //            if (ds.Tables[1].Rows[0]["vcBusiness_Registration_Number"] != DBNull.Value) txtBusiness_Registration_Number.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcBusiness_Registration_Number"].ToString(), true);
        //            if (ds.Tables[1].Rows[0]["vcBusiness_Contact_Fullname"] != DBNull.Value) txtBusiness_Contact_Fullname.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcBusiness_Contact_Fullname"].ToString(), true);
        //            if (ds.Tables[1].Rows[0]["vcBusiness_Contact_Number"] != DBNull.Value) txtBusiness_Contact_Number.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcBusiness_Contact_Number"].ToString(), true);
        //            if (ds.Tables[1].Rows[0]["vcBusiness_Contact_Alternative_Number"] != DBNull.Value) txtBusiness_Contact_Alternative_Number.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcBusiness_Contact_Alternative_Number"].ToString(), true);
        //            if (ds.Tables[1].Rows[0]["vcBusiness_Email_Address"] != DBNull.Value) txtBusiness_Email_Address.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[1].Rows[0]["vcBusiness_Email_Address"].ToString(), true);

        //        }


        //        //Physical Address
        //        if (ds.Tables[2].Rows[0]["vcBuilding_Unit"] != DBNull.Value) txtBuilding_Unit.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[2].Rows[0]["vcBuilding_Unit"].ToString(), true);
        //        if (ds.Tables[2].Rows[0]["vcAddress_Line_1"] != DBNull.Value) txtAddress_Line_1.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[2].Rows[0]["vcAddress_Line_1"].ToString(), true);
        //        if (ds.Tables[2].Rows[0]["vcAddress_Line_2"] != DBNull.Value) txtAddress_Line_2.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[2].Rows[0]["vcAddress_Line_2"].ToString(), true);
        //        if (ds.Tables[2].Rows[0]["vcSuburb"] != DBNull.Value) txtSuburb.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[2].Rows[0]["vcSuburb"].ToString(), true);
        //        if (ds.Tables[2].Rows[0]["vcCity"] != DBNull.Value) txtCity.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[2].Rows[0]["vcCity"].ToString(), true);
        //        if (ds.Tables[2].Rows[0]["vcPostal_Code"] != DBNull.Value) txtPostal_Code.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[2].Rows[0]["vcPostal_Code"].ToString(), true);
        //        ddlProvince.ClearSelection();
        //        if (ds.Tables[2].Rows[0]["iProvince_Id"] != DBNull.Value) ddlProvince.Items.FindByValue(Convert.ToInt32(ds.Tables[2].Rows[0]["iProvince_Id"]).ToString()).Selected = true;




        //        if (ds.Tables[0].Rows[0]["iPolicy_Postal_Address_Id"] != DBNull.Value)
        //        {
        //            chkPostalSameAsPhysical.Checked = false;
        //            pnlPostalAddress.Visible = true;
        //            if (ds.Tables[3].Rows[0]["vcPOBox_Bag"] != DBNull.Value) txtPOBox_Bag.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[3].Rows[0]["vcPOBox_Bag"].ToString(), true);
        //            if (ds.Tables[3].Rows[0]["vcPost_Office_Name"] != DBNull.Value) txtPost_Office_Name.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[3].Rows[0]["vcPost_Office_Name"].ToString(), true);
        //            if (ds.Tables[3].Rows[0]["vcPostalCode"] != DBNull.Value) txtPost_Postal_Code.Text = U.CryptorEngine.GenericDecrypt(ds.Tables[3].Rows[0]["vcPostalCode"].ToString(), true);
        //        }
        //        else
        //        {
        //            pnlPostalAddress.Visible = false;
        //            chkPostalSameAsPhysical.Checked = true;
        //        }
        //        pnlStep2.Visible = true;
        //    }
        //    catch (Exception)
        //    {


        //    }
        //}
        private void GetAssetDetails(int alnmI)
        {
            P.Customer_Provider frmF = new P.Customer_Provider();
            DataSet ds = frmF.Get_Asset_Details_For_Alignment(alnmI);
            System.Text.StringBuilder s = new System.Text.StringBuilder();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                foreach (DataColumn c in ds.Tables[0].Columns)
                {
                    s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                    //if (c.ColumnName.ToString() == "Asset description" || c.ColumnName.ToString() == "Asset sub-type description" || c.ColumnName.ToString() == "Make" || c.ColumnName.ToString() == "Model" || c.ColumnName.ToString() == "Model Variant")
                    //{
                    //    s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                    //}
                    //else
                    //{
                    //    s.Append(c.ColumnName + ": " + U.CryptorEngine.GenericDecrypt(row[c].ToString(), true) + "<br /><br />");
                    //}
                }
            }
            divAssetDetails.InnerHtml = s.ToString();

        }
        private void GetAssetDetailsConfirmation(int alnmI)
        {
            P.Customer_Provider frmF = new P.Customer_Provider();
            DataSet ds = frmF.Get_Asset_Details_For_Alignment_Confirmation(alnmI);
            System.Text.StringBuilder s = new System.Text.StringBuilder();

            foreach (DataRow row in ds.Tables[1].Rows)
            {
                foreach (DataColumn c in ds.Tables[1].Columns)
                {
                    if (c.ColumnName != "iAsset_Type_Id")
                    {
                        s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                    }
                }
            }
            divPolicyDetails.InnerHtml = s.ToString();

        }
        private void GetFormFields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsVehicleAsset();

            //Clear all DropDownLists

            ddlIdentification_Type.Items.Clear();
            ddlPerson_Title.Items.Clear();
            ddlProvince.Items.Clear();


            //Insert Empty 1st option 
            //ddlInsuranceCompanies.Items.Add(new ListItem("", ""));
            //ddlPolicy_Type.Items.Add(new ListItem("", ""));
            //ddlPolicy_Payment_Frequency.Items.Add(new ListItem("", ""));
            ddlIdentification_Type.Items.Add(new ListItem("", ""));
            ddlPerson_Title.Items.Add(new ListItem("", ""));
            ddlProvince.Items.Add(new ListItem("", ""));



            //Populate relevant dropdownlists
            //Insurance companies
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                // ddlInsuranceCompanies.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }



            //Identification_Types
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                ddlIdentification_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            foreach (DataRow row in ds.Tables[3].Rows)
            {
                // ddlPolicy_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Person_Titles
            foreach (DataRow row in ds.Tables[4].Rows)
            {
                ddlPerson_Title.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Pronvinces
            foreach (DataRow row in ds.Tables[5].Rows)
            {
                ddlProvince.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Policy_Payment_Frequency
            foreach (DataRow row in ds.Tables[10].Rows)
            {
                // ddlPolicy_Payment_Frequency.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Asset_Type

            foreach (DataRow row in ds.Tables[14].Rows)
            {

            }

        }
        private void ConfirmPolicy(int alnmlId)
        {

            P.Insurer_Provider iP = new P.Insurer_Provider();
            iP.Save_Confirm_Policy_For_Alignment(alnmlId, Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ",")));
        }
        private void RejectPolicy(int alnmlId)
        {
            CCom.CurrentUser objUser = new CCom.CurrentUser();
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();

            if (objUser == null)
            {
                Response.Redirect("/account/login.aspx", false);
            }
            else
            {
                P.Insurer_Provider iP = new P.Insurer_Provider();
                iP.Reject_Policy_For_Alignment(alnmlId, txtRejectionReason.Text);


                P.Customer_Provider p = new P.Customer_Provider();
                DataSet ds = p.Get_Customer_Deatils_For_Alignment(alnmlId);
                string Kl = ds.Tables[0].Rows[0][4].ToString();
                string Ai = ds.Tables[0].Rows[0][2].ToString();
                string atype = ds.Tables[0].Rows[0][3].ToString();
                string PhI = ds.Tables[0].Rows[0][8].ToString();

                string link = ConfigurationManager.AppSettings["Application_URL"] + "/AssetToPolicy.aspx?Kl=" + Kl + "&Ai=" + Ai + "&atype=" + atype + "&PhI=" + PhI;

                string customerName = string.Empty;
                customerName = ds.Tables[0].Rows[0][7].ToString() == "1" ? ds.Tables[1].Rows[0][3].ToString() : ds.Tables[1].Rows[0][1].ToString();

                string customerEmail = string.Empty;
                customerEmail = ds.Tables[0].Rows[0][7].ToString() == "1" ? ds.Tables[1].Rows[0][11].ToString() : ds.Tables[1].Rows[0][9].ToString();
                P.Notification_Provider nP = new P.Notification_Provider();
                nP.Customer_Confirm_Policy_Details_After_Rejection(customerName, customerEmail, objUser.vcPartner_Name, link, "CustomerConfirmPolicyDetailsAfterRejection");

            }
        }
        private void GetPoliciesAwaitingConfirmation(int iInsurance_Company_Id)
        {
            try
            {

                pnlPoliciesAwaitingConfirmation.Visible = true;

                P.Insurer_Provider frmF = new P.Insurer_Provider();
                DataSet ds = frmF.Get_Policies_Awaiting_Confirmation(iInsurance_Company_Id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    rptPoliciesAwaitingConfirmation.DataSource = ds.Tables[0];
                    rptPoliciesAwaitingConfirmation.DataBind();
                    rptPoliciesAwaitingConfirmation.Visible = true;
                    rptPoliciesAwaitingConfirmation.Visible = true;
                    lblNoPolicies.Visible = false;
                }

                else
                {
                    lblNoPolicies.Visible = true;
                }

            }
            catch (Exception)
            {

                throw;
            }


        }

        protected void btnConfirmPolicy_Click(object sender, EventArgs e)
        {


            P.Customer_Provider cP = new P.Customer_Provider();
            int alnmlId = Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAlnmlId.Value, true));// cP.Get_Policy_To_Asset_Alignment_Id_For_Confirmation(ai, at, kl, phi);

            if (ddlAction.SelectedValue == "1")
            {
                ConfirmPolicy(alnmlId);
            }
            else
            {
                RejectPolicy(alnmlId);
            }

            if (CheckQueryStrings())
            {
                pnlStep1.Visible = pnlStep2.Visible = false;
                pnlComplete.Visible = true;
            }
            else
            {
                Response.Redirect(Request.RawUrl);
            }

        }

        protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
        {

            pnlRejection.Visible = ddlAction.SelectedValue == "2" ? true : false;
        }

        protected void rptPoliciesAwaitingConfirmation_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName == "ViewDeatils")
                {

                    int alnmlId = Convert.ToInt32(e.CommandArgument.ToString());
                    hdAlnmlId.Value = U.CryptorEngine.GenericEncrypt(alnmlId.ToString(), true);
                    PreloadCustomerDeatils(alnmlId);
                    GetAssetDetails(alnmlId);
                    GetAssetDetailsConfirmation(alnmlId);
                    pnlStep2.Visible = true;
                    pnlPoliciesAwaitingConfirmation.Visible = false;

                }
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlPoliciesAwaitingConfirmation.Enabled = true;
            pnlPoliciesAwaitingConfirmation.Visible = true;

            pnlStep1.Visible = false;
            pnlStep2.Visible = false;

        }
    }
}