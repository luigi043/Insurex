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
namespace IAPR_Web
{
    public partial class AssetToPolicy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CheckQueryStrings())
                {
                    if (Check_Alignment_Exists())
                    {
                        GetFormFields();
                        int ai = Convert.ToInt32(Request.QueryString["Ai"]);
                        int at = Convert.ToInt32(Request.QueryString["aType"]);
                        int phi = Convert.ToInt32(Request.QueryString["PhI"]);
                        string kl = Request.QueryString["kL"].ToString();
                        P.Customer_Provider p = new P.Customer_Provider();
                        int alnmlId = p.Get_Policy_To_Asset_Alignment_Id(ai, at, kl, phi);
                        PreloadCustomerDeatils(alnmlId);
                        GetAssetDetails(alnmlId);
                        GetAssetCoversTypes(alnmlId);
                    }
                    else
                    {
                        pnlExists.Visible = false;
                        pnlNotFound.Visible = true;
                    }
                }




            }
        }

        private bool Check_Alignment_Exists()
        {
            int ai = Convert.ToInt32(Request.QueryString["Ai"]);
            int at = Convert.ToInt32(Request.QueryString["aType"]);
            int phi = Convert.ToInt32(Request.QueryString["PhI"]);
            string kl = Request.QueryString["kL"].ToString();

            P.Customer_Provider p = new P.Customer_Provider();
            return (p.Check_Policy_To_Asset_Alignment_Exists(ai, at, kl, phi));

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

                int iPolicy_Holder_Type = Convert.ToInt32(ds.Tables[0].Rows[0]["iPolicy_Holder_Type_Id"]);
                ddlPolicy_Type.ClearSelection();
                ddlPolicy_Type.Items.FindByValue(iPolicy_Holder_Type.ToString()).Selected = true;
                ddlPolicy_Type.Enabled = false;
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
            divAssetDeatils.InnerHtml = s.ToString();
            lblFinancer.Text = ds.Tables[0].Rows[0]["Financer"].ToString();

        }

        private void GetAssetCoversTypes(int alnmI)
        {

            P.Customer_Provider frmF = new P.Customer_Provider();
            DataSet ds = frmF.Get_Asset_Cover_Types_For_Alignment(alnmI);
            ddlAsset_Cover_Type.Items.Clear();
            ddlAsset_Cover_Type.Items.Add(new ListItem("", ""));
            foreach (DataRow row in ds.Tables[0].Rows)
            {

                ddlAsset_Cover_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));

            }

        }
        private void GetFormFields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsVehicleAsset();

            //Clear all DropDownLists
            ddlInsuranceCompanies.Items.Clear();
            ddlPolicy_Type.Items.Clear();
            ddlPolicy_Payment_Frequency.Items.Clear();

            ddlIdentification_Type.Items.Clear();
            ddlPerson_Title.Items.Clear();
            ddlProvince.Items.Clear();


            //Insert Empty 1st option 
            ddlInsuranceCompanies.Items.Add(new ListItem("", ""));
            ddlPolicy_Type.Items.Add(new ListItem("", ""));
            ddlPolicy_Payment_Frequency.Items.Add(new ListItem("", ""));
            ddlIdentification_Type.Items.Add(new ListItem("", ""));
            ddlPerson_Title.Items.Add(new ListItem("", ""));
            ddlProvince.Items.Add(new ListItem("", ""));



            //Populate relevant dropdownlists
            //Insurance companies
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlInsuranceCompanies.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }



            //Identification_Types
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                ddlIdentification_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Insurance Policy type
            foreach (DataRow row in ds.Tables[3].Rows)
            {
                ddlPolicy_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
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
                ddlPolicy_Payment_Frequency.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Asset_Type

            foreach (DataRow row in ds.Tables[14].Rows)
            {

            }

        }
        private void SaveBasicPolicyData()
        {
            CP.Policy pol = new CP.Policy();

            pol.iInsurance_Company_Id = Convert.ToInt32(ddlInsuranceCompanies.SelectedValue);
            pol.vcPolicy_Number =txtPolicy_Number.Text;

            pol.iPolicy_Type_Id = Convert.ToInt32(ddlPolicy_Type.SelectedValue);
            pol.iPolicy_Payment_Frequency_Type_Id = Convert.ToInt32(ddlPolicy_Payment_Frequency.SelectedValue);



            int ai = Convert.ToInt32(Request.QueryString["Ai"]);
            int at = Convert.ToInt32(Request.QueryString["aType"]);
            int phi = Convert.ToInt32(Request.QueryString["PhI"]);
            string kl = Request.QueryString["kL"].ToString();

            P.Customer_Provider CP = new P.Customer_Provider();
            int alnmlId = CP.Get_Policy_To_Asset_Alignment_Id(ai, at, kl, phi);

            CP.Save_New_Policy_For_Alignment(pol, alnmlId,  Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue));//Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","))




        }

        //private int SaveBasicPolicyData_Business()
        //{
        //    CP.Policy pol = new CP.Policy();

        //    pol.iInsurance_Company_Id = Convert.ToInt32(ddlInsuranceCompanies.SelectedValue);
        //    pol.vcPolicy_Number = txtPolicy_Number.Text;

        //    pol.iPolicy_Type_Id = Convert.ToInt32(ddlPolicy_Type.SelectedValue);
        //    pol.iPolicy_Payment_Frequency_Type_Id = Convert.ToInt32(ddlPolicy_Payment_Frequency.SelectedValue);

        //    P.Customer_Provider pro = new P.Customer_Provider();
        //   // return (pro.Save_New_Policy_Business_For_Alignment(pol));
        //}
        protected void btnCheckPolicy_Click(object sender, EventArgs e)
        {
            P.Policy_Provider p = new P.Policy_Provider();
            if (p.Check_Policy_Exists(Convert.ToInt32(ddlInsuranceCompanies.SelectedValue), txtPolicy_Number.Text))
            {
                pnlExistingPolicy.Visible = true;
                pnlPolicyPaymentFrequency.Visible = false;
            }
            else
            {
                pnlNewPolicy.Visible = true;
                pnlPolicyPaymentFrequency.Visible = true;
            }
            pnlStep2.Enabled = false;
            btnCheckPolicy.Visible = false;
            pnlInsurance_Value.Visible = true;
        }

        protected void btnSaveNewPolicy_Click(object sender, EventArgs e)
        {
            if (Check_Alignment_Exists())
            {
                SaveBasicPolicyData();
                btnSaveNewPolicy.Visible = false;
                pnlIntro.Visible = false;
                pnlStep1.Visible = false;
                pnlStep2.Visible = false;
                pnlInsurance_Value.Visible = false;
                pnlNewPolicy.Visible = false;
                pnlSuccess.Visible = true;
            }

        }

        protected void btnSaveExistingPolicy_Click(object sender, EventArgs e)
        {
            P.Policy_Provider p = new P.Policy_Provider();
            if (p.Check_Policy_Exists(Convert.ToInt32(ddlInsuranceCompanies.SelectedValue), txtPolicy_Number.Text))
            {
                int pID = p.Get_Policy_Id(Convert.ToInt32(ddlInsuranceCompanies.SelectedValue), txtPolicy_Number.Text);

                int ai = Convert.ToInt32(Request.QueryString["Ai"]);
                int at = Convert.ToInt32(Request.QueryString["aType"]);
                int phi = Convert.ToInt32(Request.QueryString["PhI"]);
                string kl = Request.QueryString["kL"].ToString();

                P.Customer_Provider CP = new P.Customer_Provider();
                int alnmlId = CP.Get_Policy_To_Asset_Alignment_Id(ai, at, kl, phi);

                CP.Save_Existing_Policy_For_Alignment(pID, alnmlId, Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue));//, Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","))
                btnSaveExistingPolicy.Visible = false;
                pnlIntro.Visible = false;
                pnlStep1.Visible = false;
                pnlStep2.Visible = false;
                pnlInsurance_Value.Visible = false;
                pnlSuccess.Visible = true;
            }
        }
    }
}