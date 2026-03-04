using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using C = IAPR_Data.Classes;
using AT = IAPR_Data.Classes.AssetTypes;
using P = IAPR_Data.Providers;

namespace IAPR_Web.PolicyManagement
{
    public partial class AddAssetToPolicy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
            ddlAsset_Type.Items.Clear();

            //Insert Empty 1st option 
            ddlInsuranceCompanies.Items.Add(new ListItem("", ""));
            ddlAsset_Type.Items.Add(new ListItem("", ""));

            //Populate relevant dropdownlists
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlInsuranceCompanies.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            foreach (DataRow row in ds.Tables[14].Rows)
            {
                ddlAsset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

        }



        private void SaveVehicleData(int policyId)
        {

        }
        private bool SaveAssetData(int polId)
        {
            bool saved = false;
            switch (ddlAsset_Type.SelectedValue)
            {


                case "1":
                    saved = ucAddVehicleAsset.SaveVehicleData(polId);
                    break;
                case "2":
                    saved = ucAddBuildingAsset.SavePropertyData(polId);
                    break;

                case "3":
                    saved = ucAddWatercraftAsset.SaveWatercraftAsset(polId);
                    break;

                case "4":
                    saved = ucAddAviationAsset.SaveAviationAsset(polId);

                    break;
                case "5":
                    saved = ucAddStockAsset.SaveStockAsset(polId);

                    break;
                case "6":
                    saved = ucAddAccountReceivableAsset.SaveAccountReceivableAsset(polId);

                    break;
                case "7":
                    saved = ucAddMachineryAsset.SaveMachineryAsset(polId);
                    break;
                case "8":
                    saved = ucAddPlantEquipmentAsset.SavePlantEquipmentAsset(polId);
                    break;
                case "9":
                    saved = ucAddBusinessInterruptionAsset.SaveBusinessInterruptionAsset(polId);
                    break;
                case "10":
                    saved = ucAddKeymanInsuranceAsset.SaveKeymanInsuranceAsset(polId);
                    break;
                case "11":
                    saved = ucAddElectronicEquipmentAsset.SaveElectronicEquipmentAsset(polId);
                    break;




            }

            return saved;


        }
        private void ShowAssetForm()
        {

            switch (ddlAsset_Type.SelectedValue)
            {


                case "1":
                    ucAddVehicleAsset.GetFormFields();
                    pnlAddVehicleAsset.Visible = true;
                    break;
                case "2":
                    ucAddBuildingAsset.GetFormFields();
                    pnlAddBuildingProperty.Visible = true;
                    break;
                case "3":
                    ucAddWatercraftAsset.GetFormFields();
                    pnlAddWatercraftAsset.Visible = true;
                    break;
                case "4":
                    ucAddAviationAsset.GetFormFields();
                    pnlAddAviationAsset.Visible = true;
                    break;
                case "5":
                    ucAddStockAsset.GetFormFields();
                    pnlAddStockAsset.Visible = true;
                    break;
                case "6":
                    ucAddAccountReceivableAsset.GetFormFields();
                    pnlAddAccountReceivableAsset.Visible = true;
                    break;
                case "7":
                    ucAddMachineryAsset.GetFormFields();
                    pnlAddMachineryAsset.Visible = true;
                    break;
                case "8":
                    ucAddPlantEquipmentAsset.GetFormFields();
                    pnlAddPlantEquipmentAsset.Visible = true;
                    break;
                case "9":
                    ucAddBusinessInterruptionAsset.GetFormFields();
                    pnlAddBusinessInterruptionAsset.Visible = true;
                    break;

                case "10":
                    ucAddKeymanInsuranceAsset.GetFormFields();
                    pnlAddKeymanInsuranceAsset.Visible = true;
                    break;
                case "11":
                    ucAddElectronicEquipmentAsset.GetFormFields();
                    pnlAddElectronicEquipmentAsset.Visible = true;
                    break;

            }




        }
        #endregion

        protected void btnAddVehicleToPolicy_Click(object sender, EventArgs e)
        {

        }

        protected void btnFind_Policy_Click(object sender, EventArgs e)
        {
            P.Policy_Provider pro = new P.Policy_Provider();
            string polId = pro.Get_Policy_Id(Convert.ToInt32(ddlInsuranceCompanies.SelectedValue), txtPolicy_Number.Text).ToString();
            if (polId != "0")
            {
                litPolicyNumber.Text = "";
                Session["selectedPolicy"] = polId;
                hdPolicyId.Value = polId;
                pnlAddAsset.Visible = true;
                pnlAddAsset.Enabled = true;

                pnlPolicyDetails.Enabled = false;

                ShowAssetForm();

            }
            else
            {
                litPolicyNumber.Text = "<label for='" + txtPolicy_Number.ClientID + "' class='txtnamevalidation erroMessage'>Policy not found</label>";
                pnlPolicyDetails.Enabled = true;

            }
        }
        protected void ddlVehicle_Make_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        protected void btnAddAssetToPolicy_Click(object sender, EventArgs e)
        {
            if (SaveAssetData(Convert.ToInt32(Session["selectedPolicy"].ToString())))
            {
                pnlSaveButtons.Visible = false;
                pnlSuccess.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Asset saved');", true);
                // Response.Redirect(Request.RawUrl);
            }

        }
        protected void btnCancelCreatePolicy_Click(object sender, EventArgs e)
        {
            pnlPolicyDetails.Enabled = true;
            btnFind_Policy.Enabled = true;
            pnlAddAsset.Visible = false;

        }
    }
}