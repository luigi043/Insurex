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

namespace IAPR_Web.AssetManagement
{
    public partial class AddNewAsset : System.Web.UI.UserControl
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


            ddlIdentification_Type.Items.Clear();
            ddlPerson_Title.Items.Clear();
            ddlProvince.Items.Clear();
            ddlAsset_Type.Items.Clear();
            ddlPolicy_Type.Items.Clear();
            //Insert Empty 1st option 

            ddlIdentification_Type.Items.Add(new ListItem("", ""));
            ddlPerson_Title.Items.Add(new ListItem("", ""));
            ddlProvince.Items.Add(new ListItem("", ""));
            ddlAsset_Type.Items.Add(new ListItem("", ""));
            ddlPolicy_Type.Items.Add(new ListItem("", ""));

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











        }

        private int Financer_Asset_Holder_Personal()
        {
            CP.Policy pol = new CP.Policy();

            CP.Policy_Holder_Personal polHI = new CP.Policy_Holder_Personal();

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

        private int Financer_Asset_Holder_Business()
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




        private bool SaveAssetData(int alignmentId)
        {
            bool saved = false;
            switch (ddlAsset_Type.SelectedValue)
            {


                case "1":

                    saved = ucAddVehicleAsset.SaveVehicleDataWithoutPolicy(alignmentId);
                    break;
                case "2":
                    saved = ucAddBuildingAsset.SavePropertyData(alignmentId);
                    break;

                case "3":
                    saved = ucAddWatercraftAsset.SaveWatercraftAsset(alignmentId);
                    break;

                case "4":
                    saved = ucAddAviationAsset.SaveAviationAsset(alignmentId);

                    break;
                case "5":
                    saved = ucAddStockAsset.SaveStockAsset(alignmentId);

                    break;
                case "6":
                    saved = ucAddAccountReceivableAsset.SaveAccountReceivableAsset(alignmentId);

                    break;
                case "7":
                    saved = ucAddMachineryAsset.SaveMachineryAsset(alignmentId);
                    break;
                case "8":
                    saved = ucAddPlantEquipmentAsset.SavePlantEquipmentAsset(alignmentId);
                    break;
                case "9":
                    saved = ucAddBusinessInterruptionAsset.SaveBusinessInterruptionAsset(alignmentId);
                    break;
                case "10":
                    saved = ucAddKeymanInsuranceAsset.SaveKeymanInsuranceAsset(alignmentId);
                    break;
                case "11":
                    saved = ucAddElectronicEquipmentAsset.SaveElectronicEquipmentAsset(alignmentId);
                    break;


            }

            return saved;


        }
        private void ShowAssetForm()
        {

            switch (ddlAsset_Type.SelectedValue)
            {


                case "1":
                    pnlAddVehicleAsset.Visible = true;
                    break;
                case "2":
                    pnlAddBuildingProperty.Visible = true;
                    break;
                case "3":
                    pnlAddWatercraftAsset.Visible = true;
                    break;
                case "4":
                    pnlAddAviationAsset.Visible = true;
                    break;
                case "5":
                    pnlAddStockAsset.Visible = true;
                    break;
                case "6":
                    pnlAddAccountReceivableAsset.Visible = true;
                    break;
                case "7":
                    pnlAddMachineryAsset.Visible = true;
                    break;
                case "8":
                    pnlAddPlantEquipmentAsset.Visible = true;
                    break;
                case "9":
                    pnlAddBusinessInterruptionAsset.Visible = true;
                    break;

                case "10":
                    pnlAddKeymanInsuranceAsset.Visible = true;
                    break;
                case "11":
                    pnlAddElectronicEquipmentAsset.Visible = true;
                    break;
            }




        }
        #endregion

        protected void btnCreatePolicy_Click(object sender, EventArgs e)
        {
            int polId = 0;
            if (ddlPolicy_Type.SelectedValue == "1")
            {
                polId = Financer_Asset_Holder_Personal();
            }
            else
            {
                polId = Financer_Asset_Holder_Business();
            }


            if (SaveAssetData(polId))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Policy has been created successfully');", true);
            }
        }



        protected void btnContinue1_Click(object sender, EventArgs e)
        {

            pnlStep2.Enabled = true;
            pnlStep2.Visible = true;
            pnlStep1.Enabled = false;
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



    }
}