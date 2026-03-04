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
namespace IAPR_Web
{
    public partial class AddNewPolicy_Old : System.Web.UI.Page
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
            ddlAsset_Cover_Type.Items.Clear();
            ddlIdentification_Type.Items.Clear();
            ddlPerson_Title.Items.Clear();
            ddlProvince.Items.Clear();
            ddlModel_Year.Items.Clear();
            ddlAsset_Usage_Type.Items.Clear();
            ddlVehicle_Asset_Type.Items.Clear();
            ddlVehicle_Asset_Licence_Type.Items.Clear();
            ddlAsset_Condition.Items.Clear();
            ddlPolicy_Payment_Frequency.Items.Clear();
            ddlAsset_Financier.Items.Clear();
            ddlVehicle_Model.Items.Clear();
            ddlVehicle_Make.Items.Clear();
            //Insert Empty 1st option 
            ddlInsuranceCompanies.Items.Add(new ListItem("", ""));
            ddlPolicy_Type.Items.Add(new ListItem("", ""));
            ddlAsset_Cover_Type.Items.Add(new ListItem("", ""));
            ddlIdentification_Type.Items.Add(new ListItem("", ""));
            ddlPerson_Title.Items.Add(new ListItem("", ""));
            ddlProvince.Items.Add(new ListItem("", ""));
            ddlModel_Year.Items.Add(new ListItem("", ""));
            ddlAsset_Usage_Type.Items.Add(new ListItem("", ""));
            ddlVehicle_Asset_Type.Items.Add(new ListItem("", ""));
            ddlVehicle_Asset_Licence_Type.Items.Add(new ListItem("", ""));
            ddlAsset_Condition.Items.Add(new ListItem("", ""));
            ddlPolicy_Payment_Frequency.Items.Add(new ListItem("", ""));
            ddlAsset_Financier.Items.Add(new ListItem("", ""));
            ddlVehicle_Make.Items.Add(new ListItem("", ""));
            ddlVehicle_Model.Items.Add(new ListItem("", ""));
            
            
            //Populate relevant dropdownlists
            //Insurance companies
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlInsuranceCompanies.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Asset_Type_Cover
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                ddlAsset_Cover_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
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


            //Vehicle_Asset_Types
            foreach (DataRow row in ds.Tables[6].Rows)
            {
                ddlVehicle_Asset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Asset_Usage
            foreach (DataRow row in ds.Tables[7].Rows)
            {
                ddlAsset_Usage_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Vehicle_Asset_Licence_Type
            foreach (DataRow row in ds.Tables[8].Rows)
            {
                ddlVehicle_Asset_Licence_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }


            //Asset_Condition
            foreach (DataRow row in ds.Tables[9].Rows)
            {
                ddlAsset_Condition.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }


            //Policy_Payment_Frequency
            foreach (DataRow row in ds.Tables[10].Rows)
            {
                ddlPolicy_Payment_Frequency.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Asset_Financier
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                ddlAsset_Financier.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Vehicle_Make
            foreach (DataRow row in ds.Tables[13].Rows)
            {
                ddlVehicle_Make.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Mdel_year from current year to -20
            for (int row = DateTime.Now.Year - 20; row < DateTime.Now.Year + 1; row++)
            {
                ddlModel_Year.Items.Add(new ListItem(row.ToString(), row.ToString()));
            }


            

            


           

            
           

           
        }

        private int SaveBasicPolicyData()
        {
            CP.Policy pol = new CP.Policy();

            pol.iInsurance_Company_Id = Convert.ToInt32(ddlInsuranceCompanies.SelectedValue);
            pol.vcPolicy_Number = txtPolicy_Number.Text;

            pol.iPolicy_Type_Id = Convert.ToInt32(ddlPolicy_Type.SelectedValue);
            pol.iPolicy_Payment_Frequency_Type_Id = Convert.ToInt32(ddlPolicy_Payment_Frequency.SelectedValue);


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


            P.Policy_Provider pro = new P.Policy_Provider();
            return (pro.Save_New_Policy_Personal(pol));
        }

        private void SaveVehicleData(int policyId)
        {
            AT.Vehicle_Asset veh = new AT.Vehicle_Asset();



            veh.iPolicy_Id = policyId;
            veh.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
            veh.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
            veh.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
            veh.iAsset_Type_Id = 1;
            veh.iVehicle_Asset_Type_Id = Convert.ToInt32(ddlVehicle_Asset_Type.SelectedValue);
            veh.iVehicle_Asset_Licence_Type_Id = Convert.ToInt32(ddlVehicle_Asset_Licence_Type.SelectedValue);
            veh.iAsset_Usage_Type_Id = Convert.ToInt32(ddlAsset_Usage_Type.SelectedValue);
            veh.iAsset_Condition_Id = Convert.ToInt32(ddlAsset_Condition.SelectedValue);
            veh.iVehicle_Model_Id = Convert.ToInt32(ddlVehicle_Model.SelectedValue);
            veh.iVehicle_Make_Id = Convert.ToInt32(ddlVehicle_Make.SelectedValue);
            veh.vcVin_Number = txtVin_Number.Text;
            veh.vcRegistration_Number = txtRegistration_Number.Text;
            veh.iModel_Year = Convert.ToInt32(ddlModel_Year.SelectedValue);
            veh.dtFinance_Start_Date = txtFinance_Start_Date.Text;
            veh.dtFinance_End_Date = txtFinance_End_Date.Text;
            veh.vcVehicle_Color = txtVehicle_Color.Text;


            P.Vehicle_Asset_Provider pro = new P.Vehicle_Asset_Provider();
            pro.Save_New_Vehicle_Asset(veh);
        }
        #endregion

        protected void btnCreatePolicy_Click(object sender, EventArgs e)
        {
            int polId = SaveBasicPolicyData();
            SaveVehicleData(polId);
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Policy has been created successfully');", true);
        }

        protected void btnContinue1_Click(object sender, EventArgs e)
        {
            P.Policy_Provider p = new P.Policy_Provider();
            if (p.Check_Policy_Exists(Convert.ToInt32(ddlInsuranceCompanies.SelectedValue), txtPolicy_Number.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('A policy with these details already exists.');", true);
            }
            else
            {
                btnContinue1.Enabled = false;
                pnlStep2.Enabled = true;
                pnlStep2.Visible = true;
                pnlStep1.Enabled = false;

            }

        }

        protected void chkPostalSameAsPhysical_CheckedChanged(object sender, EventArgs e)
        {
            pnlPostalAddress.Visible = chkPostalSameAsPhysical.Checked ? false : true;

        }

        protected void ddlVehicle_Make_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVehicle_Asset_Type.SelectedIndex > 0)
            {
                P.Vehicle_Asset_Provider frmF = new P.Vehicle_Asset_Provider();
                SqlDataReader dr = frmF.Get_Vehicle_Assset_Models_By_Make_Type(Convert.ToInt32(ddlVehicle_Make.SelectedValue), Convert.ToInt32(ddlVehicle_Asset_Type.SelectedValue));
                while (dr.Read())
                {
                    ddlVehicle_Model.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

                }
            }
            else
            {
                litVehicle_Asset_Type.Text = "Please select a type";
                ddlVehicle_Make.ClearSelection();
                ddlVehicle_Make.SelectedIndex = 0;
            }

        }
    }
}