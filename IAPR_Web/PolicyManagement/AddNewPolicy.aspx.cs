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
namespace IAPR_Web.PolicyManagement
{
    public partial class AddNewPolicy : System.Web.UI.Page
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
            ddlPolicy_Type.Items.Clear();

            ddlIdentification_Type.Items.Clear();
            ddlPerson_Title.Items.Clear();
            ddlProvince.Items.Clear();
            ddlAsset_Type.Items.Clear();
            ddlPolicy_Payment_Frequency.Items.Clear();

            //Insert Empty 1st option 
            ddlInsuranceCompanies.Items.Add(new ListItem("", ""));
            ddlPolicy_Type.Items.Add(new ListItem("", ""));
            ddlPolicy_Payment_Frequency.Items.Add(new ListItem("", ""));
            ddlIdentification_Type.Items.Add(new ListItem("", ""));
            ddlPerson_Title.Items.Add(new ListItem("", ""));
            ddlProvince.Items.Add(new ListItem("", ""));
            ddlAsset_Type.Items.Add(new ListItem("", ""));


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
                ddlAsset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

        }
        private void StartJourney()
        {
            CCom.CurrentUser objUser = new CCom.CurrentUser();
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();

            if (objUser.iUser_Type_Id != 1 || objUser.iUser_Type_Id != 2)
            {
                GetFormFields();

            }
            pnlStep1.Enabled = true;
            btnContinue1.Enabled = true;
        }
        private int SaveBasicPolicyData_Personal()
        {
            int polId = 0;
            try
            {
                CP.Policy pol = new CP.Policy();

                pol.iInsurance_Company_Id = Convert.ToInt32(ddlInsuranceCompanies.SelectedValue);
                pol.vcPolicy_Number = txtPolicy_Number.Text;

                pol.iPolicy_Type_Id = Convert.ToInt32(ddlPolicy_Type.SelectedValue);
                pol.iPolicy_Payment_Frequency_Type_Id = Convert.ToInt32(ddlPolicy_Payment_Frequency.SelectedValue);


                CP.Policy_Holder_Consumer polHI = new CP.Policy_Holder_Consumer();

                polHI.iIdentification_Type_Id = Convert.ToInt32(ddlIdentification_Type.SelectedValue);
                polHI.iPerson_Title_Id = Convert.ToInt32(ddlPerson_Title.SelectedValue);
                polHI.vcFirst_Names = txtFirst_Names.Text;
                polHI.vcSurname = txtSurname.Text;
                polHI.vcIdentification_Number = txtIdentification_Number.Text;
                polHI.vcContact_Number = txtContact_Number.Text;
                polHI.vcAlternative_Contact_Number = txtAlternative_Contact_Number.Text;
                polHI.vcEmail_Address = txtEmail_Address.Text;


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
                AT.Vehicle_Asset veh_Asset = new AT.Vehicle_Asset();


                pol.policy_Holder_Individual = polHI;


                P.Policy_Provider pro = new P.Policy_Provider();
                polId = pro.Save_New_Policy_Personal(pol);
            }
            catch (Exception ex)
            {

                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddNewPolicy", "SaveBasicPolicyData_Personal");
            }
            return polId;
        }

        private int SaveBasicPolicyData_Business()
        {
            int polId = 0;
            try
            {


                CP.Policy pol = new CP.Policy();

                pol.iInsurance_Company_Id = Convert.ToInt32(ddlInsuranceCompanies.SelectedValue);
                pol.vcPolicy_Number = txtPolicy_Number.Text;

                pol.iPolicy_Type_Id = Convert.ToInt32(ddlPolicy_Type.SelectedValue);
                pol.iPolicy_Payment_Frequency_Type_Id = Convert.ToInt32(ddlPolicy_Payment_Frequency.SelectedValue);


                CP.Policy_Holder_Business polBi = new CP.Policy_Holder_Business();

                polBi.vcBusiness_Name = txtBusiness_Name.Text;
                polBi.vcBusiness_Registration_Number = txtBusiness_Registration_Number.Text;
                polBi.vcBusiness_Contact_Fullname = txtBusiness_Contact_Fullname.Text;
                polBi.vcBusiness_Contact_Number = txtBusiness_Contact_Number.Text;
                polBi.vcBusiness_Contact_Alternative_Number = txtBusiness_Contact_Alternative_Number.Text;
                polBi.vcBusiness_Email_Address = txtBusiness_Email_Address.Text;


                CCom.Addresses.Phycisal_address addPhy = new CCom.Addresses.Phycisal_address();

                addPhy.vcBuilding_Unit = txtBuilding_Unit.Text;
                addPhy.vcAddress_Line_1 = txtAddress_Line_1.Text;
                addPhy.vcAddress_Line_2 = txtAddress_Line_2.Text;
                addPhy.vcSuburb = txtSuburb.Text;
                addPhy.vcCity = txtCity.Text;
                addPhy.iProvince_Id = Convert.ToInt32(ddlProvince.SelectedValue);
                addPhy.vcPostal_Code = txtPostal_Code.Text;

                polBi.bPostalAddresSameAsPhysical = chkPostalSameAsPhysical.Checked ? true : false;

                CCom.Addresses.Postal_Address addPo = new CCom.Addresses.Postal_Address();
                addPo.vcPOBox_Bag = txtPOBox_Bag.Text;
                addPo.vcPost_Office_Name = txtPost_Office_Name.Text;
                addPo.vcPost_Postal_Code = txtPost_Postal_Code.Text;



                polBi.physical_Address = addPhy;
                polBi.postal_Address = addPo;


                CCom.Addresses.Postal_Address addPos = new CCom.Addresses.Postal_Address();
                AT.Vehicle_Asset veh_Asset = new AT.Vehicle_Asset();


                pol.policy_Holder_Business = polBi;


                P.Policy_Provider pro = new P.Policy_Provider();
                polId = pro.Save_New_Policy_Business(pol);
            }
            catch (Exception ex)
            {

                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddNewPolicy", "SaveBasicPolicyData_Business");
            }
            return polId;
        }
        private void DeterminePolicyType()
        {
            switch (ddlPolicy_Type.SelectedValue)
            {

                case "2":
                    pnlBusinessDetails.Visible = true;
                    pnlPersonalDetails.Visible = false;
                    break;

                default:
                    ;
                    break;
            }

        }
        private void DetermineAssesTypeForm()
        {
            switch (ddlAsset_Type.SelectedValue)
            {

                case "1":
                    pnlAddVehicleAsset.Visible = true;
                    break;

                default:
                    ;
                    break;
            }

        }

        private bool SaveAssetData(int polId)
        {
            bool saved = false;
            switch (ddlAsset_Type.SelectedValue)
            {


                case "1":
                    saved = ucAddVehicleAsset.SaveVehicleData(polId);
                    ClearUsercontrolFields(ucAddVehicleAsset);
                    pnlAddVehicleAsset.Visible = false;
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
        private void ClearFormFields()
        {
            ddlInsuranceCompanies.ClearSelection();
            ddlInsuranceCompanies.SelectedIndex = 0;
            ddlPolicy_Type.ClearSelection();
            ddlPolicy_Type.SelectedIndex = 0;
            ddlPolicy_Payment_Frequency.ClearSelection();
            ddlPolicy_Payment_Frequency.SelectedIndex = 0;
            ddlAsset_Type.ClearSelection();
            ddlAsset_Type.SelectedIndex = 0;
            txtPolicy_Number.Text = "";

            pnlPersonalDetails.Visible = false;
            pnlBusinessDetails.Visible = false;
            txtBuilding_Unit.Text = "";
            txtAddress_Line_1.Text = "";
            txtAddress_Line_2.Text = "";
            txtSuburb.Text = "";
            txtCity.Text = "";
            ddlProvince.ClearSelection();
            ddlProvince.SelectedIndex = 0;
            txtPostal_Code.Text = "";
            txtPOBox_Bag.Text = "";
            txtPost_Office_Name.Text = "";
            txtPost_Postal_Code.Text = "";
            pnlStep2.Visible = false;
            pnlStep1.Enabled = false;

        }
        private void ClearUsercontrolFields(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                foreach (TextBox tb in this.Controls.OfType<TextBox>().ToArray())
                {
                    tb.Text = "";
                }

                foreach (DropDownList tb in this.Controls.OfType<DropDownList>().ToArray())
                {
                    tb.ClearSelection();
                    tb.SelectedIndex = 1;
                }
                ClearUsercontrolFields(c);
            }

        }
        #endregion

        protected void btnCreatePolicy_Click(object sender, EventArgs e)
        {
            try
            {

                if (!CheckAssetExists(1))
                {
                    int polId = 0;
                    if (ddlPolicy_Type.SelectedValue == "1")
                    {
                        polId = SaveBasicPolicyData_Personal();
                    }
                    else
                    {
                        polId = SaveBasicPolicyData_Business();
                    }


                    if (SaveAssetData(polId))
                    {
                        //pnlSaveButtons.Visible = false;
                        //pnlSuccess.Visible = true;
                        ClearFormFields();
                        StartJourney();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Asset saved');", true);

                    }
                }
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddNewPolicy", "btnCreatePolicy_Click");
            }
        }
        protected void btnCancelCreatePolicy_Click(object sender, EventArgs e)
        {
            pnlStep1.Enabled = true;
            btnContinue1.Enabled = true;
            pnlStep2.Visible = false;

        }
        protected void btnContinue1_Click(object sender, EventArgs e)
        {
            P.Policy_Provider p = new P.Policy_Provider();
            if (p.Check_Policy_Exists(Convert.ToInt32(ddlInsuranceCompanies.SelectedValue),  txtPolicy_Number.Text))
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('A policy with these details already exists. Please use the \"Add asset  to policy\" option in the menu ');", true);
                //litPolicyNumber.Text = "<label for='" + txtPolicy_Number.ClientID + "' class='txtnamevalidation erroMessage'>A policy with these details already exists</label>";
            }
            else
            {
                litPolicyNumber.Text = "";
                pnlStep2.Enabled = true;
                pnlStep2.Visible = true;
                pnlStep1.Enabled = false;
                btnContinue1.Enabled = false;
                ShowAssetForm();
                DeterminePolicyType();




            }

        }

        protected void chkPostalSameAsPhysical_CheckedChanged(object sender, EventArgs e)
        {
            pnlPostalAddress.Visible = chkPostalSameAsPhysical.Checked ? false : true;

        }

        private bool CheckAssetExists(int polId)
        {
            bool exists = false;
            switch (ddlAsset_Type.SelectedValue)
            {


                case "1":
                    exists = ucAddVehicleAsset.CheckVehicleDetailsExists();
                    break;
                case "2":
                    exists = ucAddBuildingAsset.CheckPropertyDetailsExists();
                    break;

                case "3":
                    exists = ucAddWatercraftAsset.CheckWatercraftDetailsExists();
                    break;

                case "4":
                    exists = ucAddAviationAsset.CheckAviationDetailsExists();

                    break;
                //case "5":
                //    exists = ucAddStockAsset.SaveStockAsset(polId);

                //    break;
                //case "6":
                //    exists = ucAddAccountReceivableAsset.SaveAccountReceivableAsset(polId);

                //    break;
                case "7":
                    exists = ucAddMachineryAsset.CheckMachineryDetailsExists();
                    break;
                case "8":
                    exists = ucAddPlantEquipmentAsset.CheckPlantEquipmentDetailsExists();
                    break;
                //case "9":
                //    exists = ucAddBusinessInterruptionAsset.SaveBusinessInterruptionAsset(polId);
                //    break;
                //case "10":
                //    exists = ucAddKeymanInsuranceAsset.SaveKeymanInsuranceAsset(polId);
                //    break;
                case "11":
                    exists = ucAddElectronicEquipmentAsset.CheckElectronicEquipmentDetailsExists();
                    break;


            }

            return exists;


        }


    }
}