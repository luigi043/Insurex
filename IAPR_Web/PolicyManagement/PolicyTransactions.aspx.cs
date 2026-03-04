using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using C = IAPR_Data.Classes;
using CCom = IAPR_Data.Classes.Common;
using P = IAPR_Data.Providers;
using U = IAPR_Data.Utils;
using System.Configuration;
namespace IAPR_Web.PolicyManagement
{
    public partial class PolicyTransactions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string script = "window.onload = function() { applyDataTable(); };";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "applyDataTable", script, true);
            if (!IsPostBack)
            {
                GetFormFields();
            }
        }

        #region private
        private void GetFormFields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsVehicleAsset();

            //Clear all DropDownLists
            ddlInsuranceCompanies.Items.Clear();
            //ddlPolicy_Type.Items.Clear();
            ddlPolicy_Transaction_Types.Items.Clear();


            //Insert Empty 1st option 
            ddlInsuranceCompanies.Items.Add(new ListItem("", ""));
            //ddlPolicy_Type.Items.Add(new ListItem("", ""));
            ddlPolicy_Transaction_Types.Items.Add(new ListItem("", ""));



            //Populate relevant dropdownlists
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlInsuranceCompanies.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            //foreach (DataRow row in ds.Tables[1].Rows)
            //{
            //    ddlPolicy_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            //}
            foreach (DataRow row in ds.Tables[12].Rows)
            {
                ddlPolicy_Transaction_Types.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

        }

        private void GetFormFieldsForPolicyUpdate_NonPayment(string ipolicy_Payment_Frequency_Type_Id)
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFields_Policy_Update_NonPayment(ipolicy_Payment_Frequency_Type_Id);

            //Clear all DropDownLists
            ddlAffectedPeriod.Items.Clear();
            ddlAffectedYear.Items.Clear();



            //Insert Empty 1st option 
            ddlAffectedPeriod.Items.Add(new ListItem("", ""));
            ddlAffectedYear.Items.Add(new ListItem("", ""));




            //Populate relevant dropdownlists
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlAffectedPeriod.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            for (int row = DateTime.Now.Year; row > DateTime.Now.Year - 2; row--)
            {
                ddlAffectedYear.Items.Add(new ListItem(row.ToString(), row.ToString()));
            }
        }
        private void GetFormFieldsForPolicyUpdate_Status(string ipolicy_Id)
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFields_Policy_Update_Status(ipolicy_Id);

            //Clear all DropDownLists
            ddlPolicyStatus.Items.Clear();




            //Insert Empty 1st option 
            ddlPolicyStatus.Items.Add(new ListItem("", ""));


            //Populate relevant dropdownlists
            lblCurrentStatus.Text = ds.Tables[0].Rows[0][1].ToString();

            foreach (DataRow row in ds.Tables[1].Rows)
            {
                ddlPolicyStatus.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
        }

        //private void GetFormFieldsForPolicyUpdate_ChangeCover(string ipolicy_Payment_Frequency_Type_Id)
        //{
        //    P.Vehicle_Asset_Provider frmF = new P.Vehicle_Asset_Provider();
        //    DataSet ds = frmF.GetFormFieldsForPolicyUpdate_Vehicle_NonPayment(ipolicy_Payment_Frequency_Type_Id);

        //    //Clear all DropDownLists

        //    ddlAffectedYear.Items.Clear();



        //    //Insert Empty 1st option 
        //    ddlAffectedPeriod.Items.Add(new ListItem("", ""));
        //    ddlAffectedYear.Items.Add(new ListItem("", ""));




        //    //Populate relevant dropdownlists
        //    foreach (DataRow row in ds.Tables[0].Rows)
        //    {
        //        ddlAffectedPeriod.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
        //    }
        //    for (int row = DateTime.Now.Year - 1; row < DateTime.Now.Year + 1; row++)
        //    {
        //        ddlAffectedYear.Items.Add(new ListItem(row.ToString(), row.ToString()));
        //    }
        //}

        private void SendNotification(int policyId)
        {



        }
        private void ClearFields(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = "";
                }

                if (c.HasControls())
                {
                    ClearFields(c);
                }
            }

        }
        private void CloseAllForms()
        {
            pnlPremiumNonPayment.Visible = false;
            pnlChangeCover.Visible = false;
            pnlPolicyStatus.Visible = false;
            pnlResumeCover.Visible = false;
            pnlChangeAddress.Visible = false;
            litPolicy_Number.Text = "";
        }
        private void ClearNonPaymentForm()
        {
            ddlAffectedPeriod.ClearSelection();
            ddlAffectedYear.ClearSelection();
            txtDateOfNonPayment.Text = "";
            pnlPremiumNonPayment.Visible = false;
            txtPolicy_Number.Text = "";
            pnlSelectPolicy.Enabled = true;
            ddlPolicy_Transaction_Types.ClearSelection();
            pnlSelectTransactionType.Visible = false;



        }
        private void ClearChangeCoverForm()
        {
            ddlNewCover.ClearSelection();
            txtInsurance_Cover_Value.Text = "";
            txtDateOfChangeCover.Text = "";
            pnlUpdateForm.Visible = false;
            ChangeCoverFormStep1();




        }
        private void ClearChangeStatusForm()
        {
            ddlPolicyStatus.ClearSelection();
            txtDateOfChageStatus.Text = "";
            pnlPolicyStatus.Visible = false;
            txtPolicy_Number.Text = "";
            pnlSelectPolicy.Enabled = true;
            ddlPolicy_Transaction_Types.ClearSelection();
            pnlSelectTransactionType.Visible = false;




        }
        private void ClearResumeCoverForm()
        {
            pnlResumeCover.Visible = false;
            txtPolicy_Number.Text = "";
            pnlSelectPolicy.Enabled = true;
            ddlPolicy_Transaction_Types.ClearSelection();
            pnlSelectTransactionType.Visible = false;




        }
        private void ClearChangeAddressForm()
        {
            pnlResumeCover.Visible = false;

            pnlChangeAddress.Visible = false;
            txtPolicy_Number.Text = "";
            pnlSelectPolicy.Enabled = true;
            ddlPolicy_Transaction_Types.ClearSelection();
            pnlSelectTransactionType.Visible = false;




        }
        private void NonPaymentForm()
        {
            int status = 0;
            P.Policy_Provider frmF = new P.Policy_Provider();
            status = frmF.Get_Policy_Status(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)));


            if (status == Convert.ToInt32(C.Common.Common.Policy_Status.Active) && status != 0)
            {
                P.Policy_Provider pro = new P.Policy_Provider();
                var rdr = pro.Get_Individual_Policy_Details(Convert.ToInt32(ddlInsuranceCompanies.SelectedValue), txtPolicy_Number.Text);
                string ipolicy_Payment_Frequency_Type_Id = "0";

                while (rdr.Read())
                {
                    ipolicy_Payment_Frequency_Type_Id = rdr["ipolicy_Payment_Frequency_Type_Id"].ToString();

                }

                if (ipolicy_Payment_Frequency_Type_Id != "0")
                {
                    //hdPolicyId.Value = ipolicy_Payment_Frequency_Type_Id;
                    GetFormFieldsForPolicyUpdate_NonPayment(ipolicy_Payment_Frequency_Type_Id);
                    pnlPremiumNonPayment.Visible = true;
                    pnlSelectTransactionType.Enabled = false;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Policy not found');", true);

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('This Policy is not currently active');", true);
            }

        }
        private void ResumeCoverForm()
        {
            try
            {


                int status = 0;
                P.Policy_Provider frmF = new P.Policy_Provider();
                status = frmF.Get_Policy_Status(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)));

                if (status != Convert.ToInt32(C.Common.Common.Policy_Status.Active) && status != 0)
                {
                    pnlResumeCover.Visible = true;
                    pnlSelectTransactionType.Enabled = false;
                }
                else
                {
                    //litPolicy_Number.Text = "<label for='" + txtPolicy_Number.ClientID + "' class='txtnamevalidation erroMessage'>This policy is currently active</label>";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('This Policy is currently active');", true);
                    pnlResumeCover.Visible = false;
                    pnlSelectTransactionType.Visible = true;
                }
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "PolicyTransactions", "ResumeCoverForm");
            }

        }
        private void ChangeCoverFormStep1()
        {
            try
            {

                int status = 0;
                P.Policy_Provider frmF = new P.Policy_Provider();
                status = frmF.Get_Policy_Status(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)));

                if (status == Convert.ToInt32(C.Common.Common.Policy_Status.Active) && status != 0)
                {
                    pnlChangeCover.Visible = true;

                    DataSet ds = frmF.GetPolicy_All_Assets(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)));
                    rptAssetList.DataSource = null;
                    rptAssetList.DataBind();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        rptAssetList.DataSource = ds.Tables[0];
                        rptAssetList.DataBind();
                        pnlAssetList.Visible = true;
                        rptAssetList.Visible = true;
                    }
                    pnlSelectTransactionType.Enabled = false;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('This Policy is not currently active');", true);
                }

            }
            catch (Exception)
            {

                throw;
            }


        }
        private void ChangeCoverFormStep2(int iPolicy_Id, int iAsset_Id, int iAsset_Type_Id)
        {
            try
            {

                P.Vehicle_Asset_Provider frmFV = new P.Vehicle_Asset_Provider();
                P.Property_Asset_Provider frmFP = new P.Property_Asset_Provider();
                P.Watercraft_Provider frmFW = new P.Watercraft_Provider();
                P.Aviation_Asset_Provider frmFA = new P.Aviation_Asset_Provider();
                P.Stock_Asset_Provider frmFS = new P.Stock_Asset_Provider();
                P.AccountReceivable_Asset_Provider frmFAc = new P.AccountReceivable_Asset_Provider();
                P.Machinery_Asset_Provider frmFM = new P.Machinery_Asset_Provider();
                P.PlantEquipment_Asset_Provider frmFPE = new P.PlantEquipment_Asset_Provider();
                P.BusinessInterruption_Asset_Provider frmFBI = new P.BusinessInterruption_Asset_Provider();
                P.KeymanInsurance_Asset_Provider frmFKI = new P.KeymanInsurance_Asset_Provider();
                P.ElectronicEquipment_Asset_Provider frmFEE = new P.ElectronicEquipment_Asset_Provider();
                DataSet ds = new DataSet();
                switch (iAsset_Type_Id)
                {
                    case 1:
                        ds = frmFV.Get_FormFields_Policy_Update_ChangeCover_Vehicle(iPolicy_Id, iAsset_Id);
                        break;
                    case 2:
                        ds = frmFP.Get_FormFields_Policy_Update_ChangeCover_Property(iPolicy_Id, iAsset_Id);
                        break;
                    case 3:
                        ds = frmFW.Get_FormFields_Policy_Update_ChangeCover_Watercraft(iPolicy_Id, iAsset_Id);
                        break;
                    case 4:
                        ds = frmFA.Get_FormFields_Policy_Update_ChangeCover_Aviation(iPolicy_Id, iAsset_Id);
                        break;
                    case 5:
                        ds = frmFS.Get_FormFields_Policy_Update_ChangeCover_Stock(iPolicy_Id, iAsset_Id);
                        break;
                    case 6:
                        ds = frmFAc.Get_FormFields_Policy_Update_ChangeCover_AccountReceivable(iPolicy_Id, iAsset_Id);
                        break;
                    case 7:
                        ds = frmFM.Get_FormFields_Policy_Update_ChangeCover_Machinery(iPolicy_Id, iAsset_Id);
                        break;
                    case 8:
                        ds = frmFPE.Get_FormFields_Policy_Update_ChangeCover_PlantEquipment(iPolicy_Id, iAsset_Id);
                        break;
                    case 9:
                        ds = frmFBI.Get_FormFields_Policy_Update_ChangeCover_BusinessInterruption(iPolicy_Id, iAsset_Id);
                        break;
                    case 10:
                        ds = frmFKI.Get_FormFields_Policy_Update_ChangeCover_KeymanInsurance(iPolicy_Id, iAsset_Id);
                        break;
                    case 11:
                        ds = frmFEE.Get_FormFields_Policy_Update_ChangeCover_ElectronicEquipment(iPolicy_Id, iAsset_Id);
                        break;
                }

                lblCurrentCover.Text = ds.Tables[0].Rows[0][1].ToString();
                ddlNewCover.Items.Clear();
                ddlNewCover.Items.Add(new ListItem("", ""));

                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    ddlNewCover.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                }
                pnlUpdateForm.Visible = true;
                pnlChangeCoverDetails.Visible = true;
                pnlAssetList.Visible = false;
            }
            catch (Exception)
            {

                throw;
            }


        }
        private void StatusForm()
        {
            GetFormFieldsForPolicyUpdate_Status(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true));
            pnlPolicyStatus.Visible = true;
            pnlSelectTransactionType.Enabled = false;
        }

        private void ChangeAddressForm()
        {
            GetAddressFormFields();
            ddlInsuranceCompanies.ClearSelection();
            ddlInsuranceCompanies.Items.FindByValue(U.CryptorEngine.GenericDecrypt(hdSelectedInsurer.Value, true)).Selected = true;

            PreloadCustomerDeatils(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)));
            pnlChangeAddress.Visible = true;
            pnlSelectTransactionType.Enabled = false;
        }
        private void RemoveAssetForm()
        {
            pnlChangeAddress.Visible = true;
        }
        private void GetAddressFormFields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsVehicleAsset();

            //Clear all DropDownLists
            ddlInsuranceCompanies.Items.Clear();

            ddlProvince.Items.Clear();


            //Insert Empty 1st option 
            ddlInsuranceCompanies.Items.Add(new ListItem("", ""));

            ddlProvince.Items.Add(new ListItem("", ""));



            //Populate relevant dropdownlists
            //Insurance companies
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlInsuranceCompanies.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }





            //Pronvinces
            foreach (DataRow row in ds.Tables[5].Rows)
            {
                ddlProvince.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }



        }
        private void PreloadCustomerDeatils(int iPolicy_Id)
        {

            try
            {

                P.Customer_Provider frmF = new P.Customer_Provider();
                DataSet ds = frmF.Get_Customer_Deatils_By_Policy(iPolicy_Id);
                //int iPolicy_Holder_Type = Convert.ToInt32(ds.Tables[0].Rows[0][7]);



                //Physical Address
                if (ds.Tables[1].Rows[0]["vcBuilding_Unit"] != DBNull.Value) txtBuilding_Unit.Text = (ds.Tables[1].Rows[0]["vcBuilding_Unit"]).ToString();
                if (ds.Tables[1].Rows[0]["vcAddress_Line_1"] != DBNull.Value) txtAddress_Line_1.Text = (ds.Tables[1].Rows[0]["vcAddress_Line_1"]).ToString();
                if (ds.Tables[1].Rows[0]["vcAddress_Line_2"] != DBNull.Value) txtAddress_Line_2.Text = (ds.Tables[1].Rows[0]["vcAddress_Line_2"]).ToString();
                if (ds.Tables[1].Rows[0]["vcSuburb"] != DBNull.Value) txtSuburb.Text = (ds.Tables[1].Rows[0]["vcSuburb"]).ToString();
                if (ds.Tables[1].Rows[0]["vcCity"] != DBNull.Value) txtCity.Text = (ds.Tables[1].Rows[0]["vcCity"]).ToString();
                if (ds.Tables[1].Rows[0]["vcPostal_Code"] != DBNull.Value) txtPostal_Code.Text = (ds.Tables[1].Rows[0]["vcPostal_Code"]).ToString();
                if (ds.Tables[1].Rows[0]["iProvince_Id"] != DBNull.Value) ddlProvince.Items.FindByValue(Convert.ToInt32(ds.Tables[1].Rows[0]["iProvince_Id"]).ToString()).Selected = true;




                if (ds.Tables[0].Rows[0][7] != DBNull.Value)
                {
                    chkPostalSameAsPhysical.Checked = false;
                    pnlPostalAddress.Visible = true;
                    if (ds.Tables[2].Rows[0]["vcPOBox_Bag"] != DBNull.Value) txtPOBox_Bag.Text = (ds.Tables[2].Rows[0]["vcPOBox_Bag"]).ToString();
                    if (ds.Tables[2].Rows[0]["vcPost_Office_Name"] != DBNull.Value) txtPost_Office_Name.Text = (ds.Tables[2].Rows[0]["vcPost_Office_Name"]).ToString();
                    if (ds.Tables[2].Rows[0]["vcPostalCode"] != DBNull.Value) txtPost_Postal_Code.Text = (ds.Tables[2].Rows[0]["vcPostalCode"]).ToString();
                }
                else
                {
                    pnlPostalAddress.Visible = false;
                    chkPostalSameAsPhysical.Checked = true;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void Notify_Policy_Holder_NonPayment(int iPolicy_Id, string affectedPeriod, string affectedYear)
        {
            P.Policy_Provider pPro = new P.Policy_Provider();
            DataSet ds = pPro.Get_Policy_Holder_All_Assets(iPolicy_Id);
            P.Notification_Provider nP = new P.Notification_Provider();

            string insurer = string.Empty;
            string policyTypeId = string.Empty;
            string receipientName = string.Empty;
            string receipientAddress = string.Empty;
            string partnerName = string.Empty;
            string policyNumber = string.Empty;


            string assetType = string.Empty;
            string assetSubType = string.Empty;
            string assetIdentifierDetails = string.Empty;

            insurer = ds.Tables[0].Rows[0]["Insurer"].ToString();
            policyTypeId = ds.Tables[0].Rows[0]["iPolicy_Type_Id"].ToString();
            receipientName = policyTypeId == "1" ? ds.Tables[1].Rows[0]["Firstname"].ToString() : "";
            receipientAddress = ds.Tables[1].Rows[0]["Email address"].ToString();// : ds.Tables[0].Rows[1]["Email address"].ToString();
            policyNumber = ds.Tables[0].Rows[0][1].ToString();

            foreach (DataRow row in ds.Tables[4].Rows)
            {
                partnerName = row["Financer"].ToString();
                assetType = row["vcAsset_Type_Description"].ToString();
                assetSubType = row["vcAsset_Sub_Type_Description"].ToString();
                if (row["Make/Model/Description"].ToString() != "")
                {
                    assetIdentifierDetails = "<p>" + assetIdentifierDetails + ds.Tables[4].Columns["Make/Model/Description"].ColumnName + ": " + row["Make/Model/Description"].ToString() + " </p>";
                }
                if (row["Asset Identifier"].ToString() != "")
                {
                    assetIdentifierDetails = "<p>" + assetIdentifierDetails + ds.Tables[4].Columns["Asset Identifier"].ColumnName + ": " + row["Asset Identifier"].ToString() + " </p>";
                }
                nP.Customer_NonPayment_Notification_Personal(receipientName, receipientAddress,
                    partnerName, policyNumber, affectedPeriod, affectedYear, assetType,
                    assetSubType, assetIdentifierDetails, "CustomerNonPaymentPersonal", insurer);
                assetIdentifierDetails = "";
            }
        }

        #endregion

        protected void btnFind_Policy_Click(object sender, EventArgs e)
        {
            P.Policy_Provider pro = new P.Policy_Provider();
            int polId = pro.Get_Policy_Id(Convert.ToInt32(ddlInsuranceCompanies.SelectedValue), txtPolicy_Number.Text);

            if (polId != 0)
            {
                hdPolicyId.Value = U.CryptorEngine.GenericEncrypt(polId.ToString(), true);
                hdSelectedInsurer.Value = U.CryptorEngine.GenericEncrypt(ddlInsuranceCompanies.SelectedValue, true);
                pnlSelectTransactionType.Visible = true;
                pnlSelectPolicy.Enabled = false;

            }
            else
            {
                pnlSelectTransactionType.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Policy not found');", true);
            }
        }

        protected void ddlPolicy_Transaction_Types_SelectedIndexChanged(object sender, EventArgs e)
        {
            CloseAllForms();


            switch (ddlPolicy_Transaction_Types.SelectedValue)
            {

                case "1":
                    NonPaymentForm();
                    break;
                case "2":
                    ChangeCoverFormStep1();
                    break;

                case "4":
                    StatusForm();
                    break;
                case "8":
                    ResumeCoverForm();
                    break;
                case "9":
                    ChangeAddressForm();
                    break;

                default:
                    break;
            }

        }
        protected void rptAssetList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string[] param = e.CommandArgument.ToString().Split(new Char[] { ';' });

                hdAssetId.Value = U.CryptorEngine.GenericEncrypt(param[1], true);
                hdAssetType.Value = U.CryptorEngine.GenericEncrypt(param[2], true);
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                if (param[3] != "") s.Append("Asset type: " + param[3] + " <br /><br />");
                if (param[4] != "") s.Append("Asset sub-type: " + param[4] + " <br /><br />");
                if (param[5] != "") s.Append("Make/Model: " + param[5] + " <br /><br />");
                if (param[6] != "") s.Append(param[6] + "<br /><br />");
                if (param[7] != "") s.Append("Identifier/Registration: " + param[7] + " <br /><br />");
                if (param[8] != "") s.Append("Current finance value: R " + param[8] + " <br /><br />");
                if (param[9] != "") s.Append("Current insurance value: R " + param[9] + " <br /><br />");
                divAssetDetails.InnerHtml = s.ToString();

                if (e.CommandName == "VehicleCoverUpdate")
                {
                    ChangeCoverFormStep2(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]), Convert.ToInt32(param[2]));
                }
                if (e.CommandName == "ChangeInsuranceValue")
                {
                    pnlUpdateForm.Visible = true;
                    pnlChangeInsuranceValue.Visible = true;
                    pnlAssetList.Visible = false;
                }
                if (e.CommandName == "RemoveAssetFromPolicy")
                {
                    pnlUpdateForm.Visible = true;
                    pnlRemoveAssetFromPolicy.Visible = true;
                    pnlAssetList.Visible = false;
                }
                btnCancelUpdate.Visible = false;
            }
        }
        protected void rptAssetList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Label lblIdentifier = (Label)e.Item.FindControl("lblIdentifier");
            //if (lblIdentifier != null)
            //{
            //    lblIdentifier.Text = U.CryptorEngine.GenericDecrypt(lblIdentifier.Text, true);
            //}
        }

        protected void btnSaveNonPaymnet_Click(object sender, EventArgs e)
        {
            try
            {

            
            P.Policy_Provider pro = new P.Policy_Provider();
            pro.Save_Single_Policy_NonPayment(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), Convert.ToInt32(ddlAffectedPeriod.SelectedValue), Convert.ToInt32(ddlAffectedYear.SelectedValue), txtDateOfNonPayment.Text);

            Notify_Policy_Holder_NonPayment(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), ddlAffectedPeriod.SelectedItem.Text, ddlAffectedYear.SelectedItem.Text);

            btnSaveNonPaymnet.Enabled = false;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Policy has been updated');", true);
            }
            catch (Exception ex)
            {

                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "PolicyTransactions_PolicyNonPayment", "btnSaveNonPaymnet_Click");
            }
        }

        protected void btnConfirmCoverResume_Click(object sender, EventArgs e)
        {
            P.Policy_Provider pro = new P.Policy_Provider();
            pro.Update_Policy_Resume_Cover(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)));

            ClearResumeCoverForm();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Policy has been updated');", true);
        }
        protected void btnSaveChangeCover_Click(object sender, EventArgs e)
        {
            bool iPolicy_Transaction_Id = false;
            P.Vehicle_Asset_Provider pV = new P.Vehicle_Asset_Provider();
            P.Property_Asset_Provider pP = new P.Property_Asset_Provider();
            P.Watercraft_Provider pW = new P.Watercraft_Provider();
            P.Aviation_Asset_Provider pA = new P.Aviation_Asset_Provider();
            P.Stock_Asset_Provider pS = new P.Stock_Asset_Provider();
            P.AccountReceivable_Asset_Provider pAR = new P.AccountReceivable_Asset_Provider();
            P.Machinery_Asset_Provider pM = new P.Machinery_Asset_Provider();
            P.PlantEquipment_Asset_Provider pPE = new P.PlantEquipment_Asset_Provider();
            P.BusinessInterruption_Asset_Provider pBI = new P.BusinessInterruption_Asset_Provider();
            P.KeymanInsurance_Asset_Provider pKI = new P.KeymanInsurance_Asset_Provider();
            P.ElectronicEquipment_Asset_Provider pEE = new P.ElectronicEquipment_Asset_Provider();

            switch (U.CryptorEngine.GenericDecrypt(hdAssetType.Value, true))
            {


                case "1":
                    iPolicy_Transaction_Id = pV.Save_ChangeCover_Vehicle_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToInt32(ddlNewCover.SelectedValue), txtDateOfChangeCover.Text);
                    break;
                case "2":
                    iPolicy_Transaction_Id = pP.Save_ChangeCover_Property_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToInt32(ddlNewCover.SelectedValue), txtDateOfChangeCover.Text);
                    break;
                case "3":
                    iPolicy_Transaction_Id = pW.Save_ChangeCover_Watercraft_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToInt32(ddlNewCover.SelectedValue), txtDateOfChangeCover.Text);
                    break;
                case "4":
                    iPolicy_Transaction_Id = pA.Save_ChangeCover_Aviation_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToInt32(ddlNewCover.SelectedValue), txtDateOfChangeCover.Text);
                    break;
                case "5":
                    iPolicy_Transaction_Id = pS.Save_ChangeCover_Stock_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToInt32(ddlNewCover.SelectedValue), txtDateOfChangeCover.Text);
                    break;
                case "6":
                    iPolicy_Transaction_Id = pAR.Save_ChangeCover_AccountsReceivable_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToInt32(ddlNewCover.SelectedValue), txtDateOfChangeCover.Text);
                    break;
                case "7":
                    iPolicy_Transaction_Id = pM.Save_ChangeCover_Machinery_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToInt32(ddlNewCover.SelectedValue), txtDateOfChangeCover.Text);
                    break;
                case "8":
                    iPolicy_Transaction_Id = pPE.Save_ChangeCover_PlantEquipment_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToInt32(ddlNewCover.SelectedValue), txtDateOfChangeCover.Text);
                    break;
                case "9":
                    iPolicy_Transaction_Id = pBI.Save_ChangeCover_BusinessInterruption_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToInt32(ddlNewCover.SelectedValue), txtDateOfChangeCover.Text);
                    break;
                case "10":
                    iPolicy_Transaction_Id = pKI.Save_ChangeCover_KeymanInsurance_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToInt32(ddlNewCover.SelectedValue), txtDateOfChangeCover.Text);
                    break;
                case "11":
                    iPolicy_Transaction_Id = pEE.Save_ChangeCover_ElectronicEquipment_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToInt32(ddlNewCover.SelectedValue), txtDateOfChangeCover.Text);
                    break;
            }


            btnSaveChangeCover.Enabled = false;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Policy has been updated');", true);
        }
        protected void btnCancelChangeCover_Click(object sender, EventArgs e)
        {
            ddlNewCover.ClearSelection();
            ddlNewCover.Items.Clear();
            pnlAssetList.Visible = true;
            pnlUpdateForm.Visible = false;
            pnlChangeCoverDetails.Visible = false;
            btnCancelUpdate.Visible = true;
        }
        protected void btnCancelUpdate_Click(object sender, EventArgs e)
        {
            pnlChangeCover.Visible = false;
            pnlSelectTransactionType.Enabled = true;
            ddlPolicy_Transaction_Types.ClearSelection();
            ddlPolicy_Transaction_Types.SelectedIndex = 0;
        }
        protected void btnSavePolicyStatus_Click(object sender, EventArgs e)
        {
            P.Policy_Provider pro = new P.Policy_Provider();
            pro.Save_PolicyStatus(Convert.ToInt32(IAPR_Data.Utils.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), Convert.ToInt32(ddlPolicyStatus.SelectedValue), txtDateOfChageStatus.Text);

            ClearChangeStatusForm();

            btnSaveNonPaymnet.Enabled = false;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Policy has been updated');", true);

        }
        protected void btnSaveChangeInsurance_Value_Click(object sender, EventArgs e)
        {
            bool iPolicy_Transaction_Id = false;
            P.Vehicle_Asset_Provider pV = new P.Vehicle_Asset_Provider();
            P.Property_Asset_Provider pP = new P.Property_Asset_Provider();
            P.Watercraft_Provider pW = new P.Watercraft_Provider();
            P.Aviation_Asset_Provider pA = new P.Aviation_Asset_Provider();
            P.Stock_Asset_Provider pS = new P.Stock_Asset_Provider();
            P.AccountReceivable_Asset_Provider pAR = new P.AccountReceivable_Asset_Provider();
            P.Machinery_Asset_Provider pM = new P.Machinery_Asset_Provider();
            P.PlantEquipment_Asset_Provider pPE = new P.PlantEquipment_Asset_Provider();
            P.BusinessInterruption_Asset_Provider pBI = new P.BusinessInterruption_Asset_Provider();
            P.KeymanInsurance_Asset_Provider pKI = new P.KeymanInsurance_Asset_Provider();
            P.ElectronicEquipment_Asset_Provider pEE = new P.ElectronicEquipment_Asset_Provider();

            switch (U.CryptorEngine.GenericDecrypt(hdAssetType.Value, true))
            {
                case "1":
                    iPolicy_Transaction_Id = pV.Save_ChangeInsuranceValue_Vehicle_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewInsuranceValue.Text.Replace(",", "").Replace(".", ",")), txtChangeInsurance_Value_Date.Text);
                    break;
                case "2":
                    iPolicy_Transaction_Id = pP.Save_ChangeInsuranceValue_Property_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewInsuranceValue.Text.Replace(",", "").Replace(".", ",")), txtChangeInsurance_Value_Date.Text);
                    break;
                case "3":
                    iPolicy_Transaction_Id = pW.Save_ChangeInsuranceValue_Watercraft_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewInsuranceValue.Text.Replace(",", "").Replace(".", ",")), txtChangeInsurance_Value_Date.Text);
                    break;
                case "4":
                    iPolicy_Transaction_Id = pA.Save_ChangeInsuranceValue_Aviation_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewInsuranceValue.Text.Replace(",", "").Replace(".", ",")), txtChangeInsurance_Value_Date.Text);
                    break;
                case "5":
                    iPolicy_Transaction_Id = pS.Save_ChangeInsuranceValue_Stock_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewInsuranceValue.Text.Replace(",", "").Replace(".", ",")), txtChangeInsurance_Value_Date.Text);
                    break;
                //case "6":
                //    iPolicy_Transaction_Id = pAR.Save_ChangeInsuranceValue_AccountsReceivable_Asset( Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewInsuranceValue.Text.Replace(",", "").Replace(".", ",")), txtChangeInsurance_Value_Date.Text);
                //    break;
                case "7":
                    iPolicy_Transaction_Id = pM.Save_ChangeInsuranceValue_Machinery_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewInsuranceValue.Text.Replace(",", "").Replace(".", ",")), txtChangeInsurance_Value_Date.Text);
                    break;
                case "8":
                    iPolicy_Transaction_Id = pPE.Save_ChangeInsuranceValue_PlantEquipment_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewInsuranceValue.Text.Replace(",", "").Replace(".", ",")), txtChangeInsurance_Value_Date.Text);
                    break;
                case "9":
                    iPolicy_Transaction_Id = pBI.Save_ChangeInsuranceValue_BusinessInterruption_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewInsuranceValue.Text.Replace(",", "").Replace(".", ",")), txtChangeInsurance_Value_Date.Text);
                    break;
                case "10":
                    iPolicy_Transaction_Id = pKI.Save_ChangeInsuranceValue_KeymanInsurance_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewInsuranceValue.Text.Replace(",", "").Replace(".", ",")), txtChangeInsurance_Value_Date.Text);
                    break;
                case "11":
                    iPolicy_Transaction_Id = pEE.Save_ChangeInsuranceValue_ElectronicEquipment_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewInsuranceValue.Text.Replace(",", "").Replace(".", ",")), txtChangeInsurance_Value_Date.Text);
                    break;
            }
            txtNewInsuranceValue.Text = "";
            txtChangeInsurance_Value_Date.Text = "";
            pnlUpdateForm.Visible = false;
            pnlChangeInsuranceValue.Visible = false;
            pnlAssetList.Visible = true;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Policy has been updated');", true);
        }
        protected void btnCancelChangeInsurance_Value_Click(object sender, EventArgs e)
        {
            pnlUpdateForm.Visible = false;
            pnlChangeInsuranceValue.Visible = false;
            pnlAssetList.Visible = true;
            btnCancelUpdate.Visible = true;
        }
        protected void BtnUpdateAddress_Click(object sender, EventArgs e)
        {
            P.Policy_Provider pPro = new P.Policy_Provider();
            C.Policy.Policy pol = new C.Policy.Policy();

            C.Policy.Policy_Holder_Consumer polHI = new C.Policy.Policy_Holder_Consumer();



            CCom.Addresses.Phycisal_address addPhy = new CCom.Addresses.Phycisal_address();

            addPhy.vcBuilding_Unit = txtBuilding_Unit.Text;
            addPhy.vcAddress_Line_1 = txtAddress_Line_1.Text;
            addPhy.vcAddress_Line_2 = txtAddress_Line_2.Text;
            addPhy.vcSuburb = txtSuburb.Text;
            addPhy.vcCity = txtCity.Text;
            addPhy.iProvince_Id = Convert.ToInt32(ddlProvince.SelectedValue);
            addPhy.vcPostal_Code = txtPostal_Code.Text;

            polHI.bPostalAddresSameAsPhysical = chkPostalSameAsPhysical.Checked ? true : false;

            CCom.Addresses.Postal_Address addPo = new CCom.Addresses.Postal_Address();
            addPo.vcPOBox_Bag = txtPOBox_Bag.Text;
            addPo.vcPost_Office_Name = txtPost_Office_Name.Text;
            addPo.vcPost_Postal_Code = txtPost_Postal_Code.Text;

            polHI.physical_Address = addPhy;
            polHI.postal_Address = addPo;

            CCom.Addresses.Postal_Address addPos = new CCom.Addresses.Postal_Address();

            pol.policy_Holder_Individual = polHI;
            P.Policy_Provider pro = new P.Policy_Provider();
            pro.Update_Policy_Address(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), pol);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Policy has been updated');", true);

        }
        protected void chkPostalSameAsPhysical_CheckedChanged(object sender, EventArgs e)
        {
            pnlPostalAddress.Visible = chkPostalSameAsPhysical.Checked ? false : true;

        }
        protected void btnRemoveAsset_Click(object sender, EventArgs e)
        {
            string linkGuid = Guid.NewGuid().ToString();
            bool iPolicy_Transaction_Id = false;
            P.Policy_Provider pPro = new P.Policy_Provider();
            int alignmentId = pPro.Remove_Asset_From_Policy(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetType.Value, true)), Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true)), ddlRemovalReasons.SelectedValue, txtRemoveAsset_Date.Text, linkGuid);
            NotifyCustomer(alignmentId);
            pnlUpdateForm.Visible = false;
            pnlRemoveAssetFromPolicy.Visible = false;
            pnlAssetList.Visible = true;
            ChangeCoverFormStep1();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Policy has been updated');", true);
        }
        protected void btnCancelRemoveAsset_Click(object sender, EventArgs e)
        {
            pnlUpdateForm.Visible = false;
            pnlRemoveAssetFromPolicy.Visible = false;
            pnlAssetList.Visible = true;
            btnCancelUpdate.Visible = true;
        }
        private void NotifyCustomer(int alignmentId)
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
                P.Customer_Provider p = new P.Customer_Provider();
                DataSet ds = p.Get_Customer_Deatils_For_Alignment(alignmentId);
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
                nP.Customer_Confirm_Policy_Details(customerName, customerEmail, objUser.vcPartner_Name, link, "CustomerConfirmPolicyDetails");
            }
        }

        protected void btnCancelNonPayment_Click(object sender, EventArgs e)
        {
            ddlAffectedPeriod.ClearSelection();
            ddlAffectedPeriod.SelectedIndex = 0;

            ddlAffectedYear.ClearSelection();
            ddlAffectedYear.SelectedIndex = 0;
            txtDateOfNonPayment.Text = "";
            pnlSelectTransactionType.Enabled = false;
            pnlPremiumNonPayment.Visible = false;
            pnlSelectTransactionType.Enabled = true;
            ddlPolicy_Transaction_Types.ClearSelection();
            ddlPolicy_Transaction_Types.SelectedIndex = 0;
        }

        protected void btnCancelPolicyStatus_Click(object sender, EventArgs e)
        {
            ddlPolicyStatus.ClearSelection();
            ddlPolicyStatus.SelectedIndex = 0;
            txtDateOfChageStatus.Text = "";
            pnlPolicyStatus.Visible = false;
            pnlSelectTransactionType.Enabled = true;
            ddlPolicy_Transaction_Types.ClearSelection();
            ddlPolicy_Transaction_Types.SelectedIndex = 0;

        }

        protected void btnCancelResumeCover_Click(object sender, EventArgs e)
        {
            pnlResumeCover.Visible = false;
            pnlSelectTransactionType.Enabled = true;
            ddlPolicy_Transaction_Types.ClearSelection();
            ddlPolicy_Transaction_Types.SelectedIndex = 0;
        }

        protected void BtnCancelUpdateAddress_Click(object sender, EventArgs e)
        {
            txtBuilding_Unit.Text = "";
            txtAddress_Line_1.Text = "";
            txtAddress_Line_2.Text = "";
            txtSuburb.Text = "";
            txtCity.Text = "";
            ddlProvince.ClearSelection();
            ddlProvince.SelectedIndex = 0;
            txtPostal_Code.Text = "";
            chkPostalSameAsPhysical.Checked = false;
            pnlPostalAddress.Visible = true;
            txtPOBox_Bag.Text = "";
            txtPost_Office_Name.Text = "";
            txtPost_Postal_Code.Text = "";

            pnlChangeAddress.Visible = false;
            pnlSelectTransactionType.Enabled = true;
            ddlPolicy_Transaction_Types.ClearSelection();
            ddlPolicy_Transaction_Types.SelectedIndex = 0;
        }
    }
}
