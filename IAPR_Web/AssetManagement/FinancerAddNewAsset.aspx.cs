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
using System.Configuration;
using U = IAPR_Data.Utils;
namespace IAPR_Web.AssetManagement
{
    public partial class FinancerAddNewAsset : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                StartJourney();
            }
        }

        #region private
        private void StartJourney()
        {
            CCom.CurrentUser objUser = new CCom.CurrentUser();
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();

            if (objUser.iUser_Type_Id != 1 || objUser.iUser_Type_Id != 2)
            {
                GetFormFields();
                DeterminePackage(objUser.iPartner_Package_Id);
                GetFinancerAssetTypes(objUser.iPartner_Id);
            }
            pnlStep1.Enabled = true;
            btnContinue1.Enabled = true;
        }
        private void GetFormFields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsVehicleAsset();

            //Clear all DropDownLists


            ddlIdentification_Type.Items.Clear();
            ddlPerson_Title.Items.Clear();
            ddlProvince.Items.Clear();
            ddlAsset_Type.Items.Clear();
            ddlPolicy_Type.Items.Clear();
            ddlInsuranceCompanies.Items.Clear();
            ddlPolicy_TypeInsurance.Items.Clear();
            ddlPolicy_Payment_Frequency.Items.Clear();
            //Insert Empty 1st option 

            ddlIdentification_Type.Items.Add(new ListItem("", ""));
            ddlPerson_Title.Items.Add(new ListItem("", ""));
            ddlProvince.Items.Add(new ListItem("", ""));
            ddlAsset_Type.Items.Add(new ListItem("", ""));
            ddlPolicy_Type.Items.Add(new ListItem("", ""));

            ddlInsuranceCompanies.Items.Add(new ListItem("", ""));
            ddlPolicy_TypeInsurance.Items.Add(new ListItem("", ""));
            ddlPolicy_Payment_Frequency.Items.Add(new ListItem("", ""));

            //Populate relevant dropdownlists
            //Insurance companies




            //Identification_Types
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                ddlIdentification_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
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


            //Asset_Type

            foreach (DataRow row in ds.Tables[14].Rows)
            {
                ddlAsset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            foreach (DataRow row in ds.Tables[3].Rows)
            {
                ddlPolicy_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }


            //Insurance companies
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlInsuranceCompanies.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }


            //Policy_Payment_Frequency
            foreach (DataRow row in ds.Tables[10].Rows)
            {
                ddlPolicy_Payment_Frequency.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Insurance Policy type
            foreach (DataRow row in ds.Tables[3].Rows)
            {
                ddlPolicy_TypeInsurance.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }



        }
        private void DeterminePackage(int iPackage_Id)
        {
            switch (iPackage_Id)
            {
                case 1:
                    ShowForConsumer();
                    break;
                case 2:
                    ShowForCommercial();
                    break;
                case 3:
                    ShowForConsumerAndCommercial();
                    break;
            }
        }
        private void ShowForConsumer()
        {
            ddlPolicy_Type.ClearSelection();
            ddlPolicy_Type.Items.FindByText(CCom.Common.Policy_Holder_Types.Personal.ToString()).Selected = true;
            ddlPolicy_Type.Enabled = false;
            divCustomerType.Attributes.Add("style", "display:none;");
        }
        private void ShowForCommercial()
        {
            ddlPolicy_Type.ClearSelection();
            ddlPolicy_Type.Items.FindByText(CCom.Common.Policy_Holder_Types.Business.ToString()).Selected = true;
            ddlPolicy_Type.Enabled = false;
            divCustomerType.Attributes.Add("style", "display:none;");
        }
        private void ShowForConsumerAndCommercial()
        {
            //ddlPolicy_Type.ClearSelection();
            //ddlPolicy_Type.Items.FindByText(CCom.Common.Policy_Holder_Types.Business.ToString()).Selected = true;
            // ddlPolicy_Type.Enabled = false;
            //divCustomerType.Attributes.Add("style", "display:none;");
        }
        private void GetFinancerAssetTypes(int iFinancer_Id)
        {
            P.GetFormFields_Provider p = new P.GetFormFields_Provider();
            DataSet ds = p.GetFormFieldsAssetsFinancedByFinancer(iFinancer_Id);

            ddlAsset_Type.Items.Clear();
            ddlAsset_Type.Items.Add(new ListItem("", ""));
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlAsset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
        }
        private int Save_Personal_NoPolicy()
        {
            CP.Policy pol = new CP.Policy();

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
            string linkGuid = Guid.NewGuid().ToString();

            P.Financer_Provider pro = new P.Financer_Provider();
            return (pro.Save_Financer_Asset_Personal(pol, linkGuid));
        }

        private int Save_Business_NoPolicy()
        {
            CP.Policy pol = new CP.Policy();




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
            string linkGuid = Guid.NewGuid().ToString();

            P.Financer_Provider pro = new P.Financer_Provider();
            return (pro.Save_Financer_Asset_Business(pol, linkGuid));
        }

        //private int SaveBasicPolicyData_Personal_WithPolicy()
        //{
        //    int polId = 0;
        //    try
        //    {
        //        CP.Policy pol = new CP.Policy();

        //        pol.iInsurance_Company_Id = Convert.ToInt32(ddlInsuranceCompanies.SelectedValue);
        //        pol.vcPolicy_Number = txtPolicy_Number.Text;

        //        pol.iPolicy_Type_Id = Convert.ToInt32(ddlPolicy_Type.SelectedValue);
        //        pol.iPolicy_Payment_Frequency_Type_Id = Convert.ToInt32(ddlPolicy_Payment_Frequency.SelectedValue);


        //        CP.Policy_Holder_Consumer polHI = new CP.Policy_Holder_Consumer();

        //        polHI.iIdentification_Type_Id = Convert.ToInt32(ddlIdentification_Type.SelectedValue);
        //        polHI.iPerson_Title_Id = Convert.ToInt32(ddlPerson_Title.SelectedValue);
        //        polHI.vcFirst_Names = txtFirst_Names.Text;
        //        polHI.vcSurname = txtSurname.Text;
        //        polHI.vcIdentification_Number = txtIdentification_Number.Text;
        //        polHI.vcContact_Number = txtContact_Number.Text;
        //        polHI.vcAlternative_Contact_Number = txtAlternative_Contact_Number.Text;
        //        polHI.vcEmail_Address = txtEmail_Address.Text;


        //        CCom.Addresses.Phycisal_address addPhy = new CCom.Addresses.Phycisal_address();

        //        addPhy.vcBuilding_Unit = txtBuilding_Unit.Text;
        //        addPhy.vcAddress_Line_1 = txtAddress_Line_1.Text;
        //        addPhy.vcAddress_Line_2 = txtAddress_Line_2.Text;
        //        addPhy.vcSuburb = txtSuburb.Text;
        //        addPhy.vcCity = txtCity.Text;
        //        addPhy.iProvince_Id = Convert.ToInt32(ddlProvince.SelectedValue);
        //        addPhy.vcPostal_Code = txtPostal_Code.Text;

        //        polHI.bPostalAddresSameAsPhysical = chkPostalSameAsPhysical.Checked ? true : false;

        //        CCom.Addresses.Postal_Address addPo = new CCom.Addresses.Postal_Address();
        //        addPo.vcPOBox_Bag = txtPOBox_Bag.Text;
        //        addPo.vcPost_Office_Name = txtPost_Office_Name.Text;
        //        addPo.vcPost_Postal_Code = txtPost_Postal_Code.Text;

        //        polHI.physical_Address = addPhy;
        //        polHI.postal_Address = addPo;


        //        CCom.Addresses.Postal_Address addPos = new CCom.Addresses.Postal_Address();
        //        AT.Vehicle_Asset veh_Asset = new AT.Vehicle_Asset();


        //        pol.policy_Holder_Individual = polHI;


        //        P.Policy_Provider pro = new P.Policy_Provider();
        //        polId = pro.Save_New_Policy_Personal(pol);
        //    }
        //    catch (Exception ex)
        //    {

        //        U.ErrorLogger eL = new U.ErrorLogger();
        //        eL.LogErrorInDB(ex, "AddNewPolicy", "SaveBasicPolicyData_Personal");
        //    }
        //    return polId;
        //}

        //private int SaveBasicPolicyData_Business_WithPolicy()
        //{
        //    int polId = 0;
        //    try
        //    {


        //        CP.Policy pol = new CP.Policy();

        //        pol.iInsurance_Company_Id = Convert.ToInt32(ddlInsuranceCompanies.SelectedValue);
        //        pol.vcPolicy_Number = txtPolicy_Number.Text;

        //        pol.iPolicy_Type_Id = Convert.ToInt32(ddlPolicy_Type.SelectedValue);
        //        pol.iPolicy_Payment_Frequency_Type_Id = Convert.ToInt32(ddlPolicy_Payment_Frequency.SelectedValue);


        //        CP.Policy_Holder_Business polBi = new CP.Policy_Holder_Business();

        //        polBi.vcBusiness_Name = txtBusiness_Name.Text;
        //        polBi.vcBusiness_Registration_Number = txtBusiness_Registration_Number.Text;
        //        polBi.vcBusiness_Contact_Fullname = txtBusiness_Contact_Fullname.Text;
        //        polBi.vcBusiness_Contact_Number = txtBusiness_Contact_Number.Text;
        //        polBi.vcBusiness_Contact_Alternative_Number = txtBusiness_Contact_Alternative_Number.Text;
        //        polBi.vcBusiness_Email_Address = txtBusiness_Email_Address.Text;


        //        CCom.Addresses.Phycisal_address addPhy = new CCom.Addresses.Phycisal_address();

        //        addPhy.vcBuilding_Unit = txtBuilding_Unit.Text;
        //        addPhy.vcAddress_Line_1 = txtAddress_Line_1.Text;
        //        addPhy.vcAddress_Line_2 = txtAddress_Line_2.Text;
        //        addPhy.vcSuburb = txtSuburb.Text;
        //        addPhy.vcCity = txtCity.Text;
        //        addPhy.iProvince_Id = Convert.ToInt32(ddlProvince.SelectedValue);
        //        addPhy.vcPostal_Code = txtPostal_Code.Text;

        //        polBi.bPostalAddresSameAsPhysical = chkPostalSameAsPhysical.Checked ? true : false;

        //        CCom.Addresses.Postal_Address addPo = new CCom.Addresses.Postal_Address();
        //        addPo.vcPOBox_Bag = txtPOBox_Bag.Text;
        //        addPo.vcPost_Office_Name = txtPost_Office_Name.Text;
        //        addPo.vcPost_Postal_Code = txtPost_Postal_Code.Text;



        //        polBi.physical_Address = addPhy;
        //        polBi.postal_Address = addPo;


        //        CCom.Addresses.Postal_Address addPos = new CCom.Addresses.Postal_Address();
        //        AT.Vehicle_Asset veh_Asset = new AT.Vehicle_Asset();


        //        pol.policy_Holder_Business = polBi;


        //        P.Policy_Provider pro = new P.Policy_Provider();
        //        polId = pro.Save_New_Policy_Business(pol);
        //    }
        //    catch (Exception ex)
        //    {

        //        U.ErrorLogger eL = new U.ErrorLogger();
        //        eL.LogErrorInDB(ex, "AddNewPolicy", "SaveBasicPolicyData_Business");
        //    }
        //    return polId;
        //}


        private bool SaveAssetData_NoPolicy(int alignmentId, bool notifyCustomer)
        {
            bool saved = false;
            switch (ddlAsset_Type.SelectedValue)
            {


                case "1":

                    saved = ucAddVehicleAsset.SaveVehicleData_Without_Policy(alignmentId);
                    ClearUsercontrolFields(ucAddVehicleAsset);
                    pnlAddVehicleAsset.Visible = false;
                    break;
                case "2":
                    saved = ucAddBuildingAsset.SavePropertyData_Without_Policy(alignmentId);
                    break;

                case "3":
                    saved = ucAddWatercraftAsset.SaveWatercraftAsset_Without_Policy(alignmentId);
                    break;

                case "4":
                    saved = ucAddAviationAsset.SaveAviationAsset_Without_Policy(alignmentId);

                    break;
                case "5":
                    saved = ucAddStockAsset.SaveStockAsset_Without_Policy(alignmentId);

                    break;
                case "6":
                    saved = ucAddAccountReceivableAsset.SaveAccountReceivableAsset_Without_Policy(alignmentId);

                    break;
                case "7":
                    saved = ucAddMachineryAsset.SaveMachineryAsset_Without_Policy(alignmentId);
                    break;
                case "8":
                    saved = ucAddPlantEquipmentAsset.SavePlantEquipmentAsset_Without_Policy(alignmentId);
                    break;
                case "9":
                    saved = ucAddBusinessInterruptionAsset.SaveBusinessInterruptionAsset_Without_Policy(alignmentId);
                    break;
                case "10":
                    saved = ucAddKeymanInsuranceAsset.SaveKeymanInsuranceAsset_Without_Policy(alignmentId);
                    break;
                case "11":
                    saved = ucAddElectronicEquipmentAsset.SaveElectronicEquipmentAsset_Without_Policy(alignmentId);
                    break;


            }
            if (notifyCustomer)
            {
                NotifyCustomer(alignmentId);
            }
            return saved;
        }
        private void AddNewPolicyData(int alnmlId)
        {
            CP.Policy pol = new CP.Policy();
            pol.iInsurance_Company_Id = Convert.ToInt32(ddlInsuranceCompanies.SelectedValue);
            pol.vcPolicy_Number = txtPolicy_Number.Text;

            pol.iPolicy_Type_Id = Convert.ToInt32(ddlPolicy_Type.SelectedValue);
            pol.iPolicy_Payment_Frequency_Type_Id = Convert.ToInt32(ddlPolicy_Payment_Frequency.SelectedValue);

            P.Customer_Provider CP = new P.Customer_Provider();
            CP.Save_New_Policy_For_Alignment(pol, alnmlId, Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue));//Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","))
        }
        private void UpdateExistingPolicyData(int alnmlId)
        {
            P.Policy_Provider p = new P.Policy_Provider();
            if (p.Check_Policy_Exists(Convert.ToInt32(ddlInsuranceCompanies.SelectedValue), txtPolicy_Number.Text))
            {
                int pID = p.Get_Policy_Id(Convert.ToInt32(ddlInsuranceCompanies.SelectedValue), txtPolicy_Number.Text);
                P.Customer_Provider CP = new P.Customer_Provider();
                CP.Save_Existing_Policy_For_Alignment(pID, alnmlId, Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue));//, Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","))

            }
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
        private void NotifyCustomer(int alignmentId)
        {
            CCom.CurrentUser objUser = new CCom.CurrentUser();
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();


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
        private void RemoveFinAncerandCovers(UserControl c)
        {
            CCom.CurrentUser objUser = new CCom.CurrentUser();
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();


            //Remove all Policy  covers
            DropDownList ddlACT = (DropDownList)c.FindControl("ddlAsset_Cover_Type");

            var actItem = ddlACT.Items.FindByText("Unconfirmed");

            ddlACT.Items.Clear();
            ddlACT.Items.Add(actItem);

            //Remove all other Financers
            DropDownList ddlF = (DropDownList)c.FindControl("ddlAsset_Financier");

            var fItem = ddlF.Items.FindByValue(objUser.iPartner_Id.ToString());

            ddlF.Items.Clear();
            ddlF.Items.Add(fItem);

            //Remove all other Financers
            TextBox txtInAm = (TextBox)c.FindControl("txtAsset_Insurance_Value");

            txtInAm.Text = "0";
            txtInAm.Enabled = false;



        }
        private void ShowAssetForm()
        {

            switch (ddlAsset_Type.SelectedValue)
            {
                case "1":
                    ucAddVehicleAsset.GetFormFields();
                    RemoveFinAncerandCovers(ucAddVehicleAsset);
                    pnlAddVehicleAsset.Visible = true;
                    ViewState["SelectedUsercontrol"] = "ucAddVehicleAsset";

                    break;
                case "2":
                    ucAddBuildingAsset.GetFormFields();
                    RemoveFinAncerandCovers(ucAddBuildingAsset);
                    pnlAddBuildingProperty.Visible = true;
                    break;
                case "3":
                    ucAddWatercraftAsset.GetFormFields();
                    RemoveFinAncerandCovers(ucAddWatercraftAsset);
                    pnlAddWatercraftAsset.Visible = true;
                    break;
                case "4":
                    ucAddAviationAsset.GetFormFields();
                    RemoveFinAncerandCovers(ucAddAviationAsset);
                    pnlAddAviationAsset.Visible = true;
                    break;
                case "5":
                    ucAddStockAsset.GetFormFields();
                    RemoveFinAncerandCovers(ucAddStockAsset);
                    pnlAddStockAsset.Visible = true;
                    break;
                case "6":
                    ucAddAccountReceivableAsset.GetFormFields();
                    RemoveFinAncerandCovers(ucAddAccountReceivableAsset);
                    pnlAddAccountReceivableAsset.Visible = true;
                    break;
                case "7":
                    ucAddMachineryAsset.GetFormFields();
                    RemoveFinAncerandCovers(ucAddMachineryAsset);
                    pnlAddMachineryAsset.Visible = true;
                    break;
                case "8":
                    ucAddPlantEquipmentAsset.GetFormFields();
                    RemoveFinAncerandCovers(ucAddPlantEquipmentAsset);
                    pnlAddPlantEquipmentAsset.Visible = true;
                    break;
                case "9":
                    ucAddBusinessInterruptionAsset.GetFormFields();
                    RemoveFinAncerandCovers(ucAddBusinessInterruptionAsset);
                    pnlAddBusinessInterruptionAsset.Visible = true;
                    break;

                case "10":
                    ucAddKeymanInsuranceAsset.GetFormFields();
                    RemoveFinAncerandCovers(ucAddKeymanInsuranceAsset);
                    pnlAddKeymanInsuranceAsset.Visible = true;
                    break;
                case "11":
                    ucAddElectronicEquipmentAsset.GetFormFields();
                    RemoveFinAncerandCovers(ucAddElectronicEquipmentAsset);
                    pnlAddElectronicEquipmentAsset.Visible = true;
                    break;
            }




        }
        #endregion

        //protected void btnCreatePolicy_Click(object sender, EventArgs e)
        //{
        //    if (!CheckAssetExists(1))
        //    {
        //        if (chkInsuranceNotAvailable.Checked)
        //        {
        //            int alignmentId = 0;
        //            if (ddlPolicy_Type.SelectedValue == "1")
        //            {
        //                alignmentId = SaveBasicPolicyData_Personal_NoPolicy();
        //            }
        //            else
        //            {
        //                alignmentId = SaveBasicPolicyData_Business_NoPolicy();
        //            }


        //            if (SaveAssetData_NoPolicy(alignmentId))
        //            {
        //                //pnlSaveButtons.Visible = false;
        //                //pnlSuccess.Visible = true;
        //                ClearFormFields();
        //                StartJourney();
        //                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Asset saved');", true);
        //            }
        //        }
        //        else
        //        {
        //            int polId = 0;
        //            if (ddlPolicy_Type.SelectedValue == "1")
        //            {
        //                polId = SaveBasicPolicyData_Personal_WithPolicy();
        //            }
        //            else
        //            {
        //                polId = SaveBasicPolicyData_Business_WithPolicy();
        //            }


        //            if (SaveAssetData_WithPolicy(polId))
        //            {
        //                //pnlSaveButtons.Visible = false;
        //                //pnlSuccess.Visible = true;
        //                ClearFormFields();
        //                StartJourney();
        //                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Asset saved');", true);
        //            }
        //        }
        //    }
        //}
        protected void btnCreatePolicy_Click(object sender, EventArgs e)
        {
            if (!CheckAssetExists(1))
            {
                int alignmentId = 0;
                if (ddlPolicy_Type.SelectedValue == "1")
                {
                    alignmentId = Save_Personal_NoPolicy();
                }
                else
                {
                    alignmentId = Save_Business_NoPolicy();
                }
                bool notifyCustomer = chkInsuranceNotAvailable.Checked ? true : false;
                if (!notifyCustomer)
                {
                    if (hdNewPolicy.Value == "1")
                    {
                        AddNewPolicyData(alignmentId);
                    }
                    else
                    {
                        UpdateExistingPolicyData(alignmentId);
                    }
                }

                if (SaveAssetData_NoPolicy(alignmentId, notifyCustomer))
                {
                    ClearFormFields();
                    StartJourney();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Asset saved');", true);
                }


            }
        }
        protected void btnCancelCreatePolicy_Click(object sender, EventArgs e)
        {
            pnlStep1.Enabled = true;
            btnContinue1.Enabled = true;
            pnlStep2.Visible = false;

        }
        private void ClearFormFields()
        {
            //pnlPersonalDetails.Visible = false;
            //pnlBusinessDetails.Visible = false;
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

        protected void btnContinue1_Click(object sender, EventArgs e)
        {
            CCom.CurrentUser objUser = new CCom.CurrentUser();
            P.User_Provider uP = new P.User_Provider();
            objUser = uP.GetUserFromSession();
            pnlStep2.Enabled = true;
            pnlStep2.Visible = true;
            pnlStep1.Enabled = false;
            DeterminePackage(objUser.iPartner_Package_Id);
            btnContinue1.Enabled = false;
            ShowAssetForm();
            DeterminePolicyType();






        }

        protected void chkPostalSameAsPhysical_CheckedChanged(object sender, EventArgs e)
        {
            pnlPostalAddress.Visible = chkPostalSameAsPhysical.Checked ? false : true;

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
        private void GetCoverTypes()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.Get_Cover_Types_By_Asset_Type(Convert.ToInt32(ddlAsset_Type.SelectedValue));

            //Clear all DropDownLists
            ddlAsset_Cover_Type.Items.Clear();
            ddlAsset_Cover_Type.Items.Clear();



            //Insert Empty 1st option 
            ddlAsset_Cover_Type.Items.Add(new ListItem("", ""));
            ddlAsset_Cover_Type.Items.Add(new ListItem("", ""));




            //Populate relevant dropdownlists
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlAsset_Cover_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

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
        protected void btnCheckPolicy_Click(object sender, EventArgs e)
        {
            P.Policy_Provider p = new P.Policy_Provider();
            if (p.Check_Policy_Exists(Convert.ToInt32(ddlInsuranceCompanies.SelectedValue), txtPolicy_Number.Text))
            {
                //pnlExistingPolicy.Visible = true;
                pnlPolicyPaymentFrequency.Visible = false;
                hdNewPolicy.Value = "0";
            }
            else
            {
                pnlNewPolicy.Visible = true;
                pnlPolicyPaymentFrequency.Visible = true;
                hdNewPolicy.Value = "1";
            }
            pnlInsuranceStep2.Enabled = false;
            btnCheckPolicy.Visible = false;
            pnlInsurance_Value.Visible = true;
            pnlSaveButtons.Visible = true;
            
        }

        protected void chkInsuranceNotAvailable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInsuranceNotAvailable.Checked)
            {
                ResetInsurancePolicyDetails();
            }
            else
            {
                pnlIsuranceDetails.Visible = true;
                pnlInsuranceStep2.Visible = true;
                pnlSaveButtons.Visible = false;
            }

        }
        private void ResetInsurancePolicyDetails()
        {
            pnlIsuranceDetails.Visible = false;
            pnlSaveButtons.Visible = true;
            pnlInsuranceStep2.Visible = false;
            ddlInsuranceCompanies.ClearSelection();
            ddlInsuranceCompanies.SelectedIndex = 0;

            ddlPolicy_TypeInsurance.ClearSelection();
            ddlPolicy_TypeInsurance.SelectedIndex = 0;

            txtPolicy_Number.Text = "";

            pnlInsurance_Value.Visible = true;
            pnlInsurance_Value.Visible = false;
            ddlAsset_Cover_Type.ClearSelection();
            //ddlAsset_Cover_Type.SelectedIndex = 0;
            pnlInsuranceStep2.Enabled = true;
            pnlNewPolicy.Visible = false;
            pnlInsurance_Value.Visible = false;
            pnlNewPolicy.Visible = false;
        }

        protected void ddlAsset_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetCoverTypes();
        }
    }
}