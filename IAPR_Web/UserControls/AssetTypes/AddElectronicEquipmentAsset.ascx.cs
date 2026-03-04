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
namespace IAPR_Web.UserControls.AssetTypes
{
    public partial class AddElectronicEquipmentAsset : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

            }

        }

        #region private
        public void GetFormFields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldElectronicEquipmentAsset();

            //Clear all DropDownLists


            ddlElectronicEquipment_Asset_Type.Items.Clear();
            ddlElectronicEquipment_Make.Items.Clear();
            ddlAsset_Financier.Items.Clear();

            //Insert Empty 1st option 

            ddlAsset_Cover_Type.Items.Add(new ListItem("", ""));
            ddlElectronicEquipment_Make.Items.Add(new ListItem("", ""));

            ddlElectronicEquipment_Asset_Type.Items.Add(new ListItem("", ""));


            ddlAsset_Financier.Items.Add(new ListItem("", ""));



            //Populate relevant dropdownlists

            //Asset_Type_Cover
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                ddlAsset_Cover_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            //ElectronicEquipment_Asset_Type
            foreach (DataRow row in ds.Tables[6].Rows)
            {
                ddlElectronicEquipment_Asset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Asset_Financier
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                ddlAsset_Financier.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            //ElectronicEquipment_Make
            foreach (DataRow row in ds.Tables[13].Rows)
            {
                ddlElectronicEquipment_Make.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

        }
        public bool CheckElectronicEquipmentDetailsExists()
        {
            bool exists = false;
            P.ElectronicEquipment_Asset_Provider pro = new P.ElectronicEquipment_Asset_Provider();
            DataSet ds = pro.Check_ElectronicEquipment_Details_Exist(txtFinance_Agrreement_Number.Text, txtSerial_Number.Text);
            foreach (DataTable t in ds.Tables)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Finance agreement number already exists');", true);
                    exists = true;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Serial number already exists');", true);
                    exists = true;
                }


            }

            return exists;
        }
        #endregion


        public bool SaveElectronicEquipmentAsset(int policyId)
        {
            bool saved = false;
            if (!Page.IsValid)
            {
                return false;
            }
            try
            {
                P.Generic_Asset_Provider proGen = new P.Generic_Asset_Provider();
                if (!proGen.Check_FinanceNumber_Exists(Convert.ToInt32(ddlAsset_Financier.SelectedValue), txtFinance_Agrreement_Number.Text))
                {
                    AT.ElectronicEquipment_Asset ee = new AT.ElectronicEquipment_Asset();




                    ee.iPolicy_Id = policyId;
                    ee.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    ee.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    ee.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    ee.mAsset_Finance_Value = Convert.ToDecimal(txtAsset_Finance_Value.Text.Replace(",", "").Replace(".", ","));
                    ee.mAsset_Insurance_Value = Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","));
                    ee.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    ee.dtFinance_End_Date = txtFinance_End_Date.Text;
                    ee.iElectronicEquipment_Asset_Type_Id = Convert.ToInt32(ddlElectronicEquipment_Asset_Type.SelectedValue);
                    ee.vcSerial_Number = txtSerial_Number.Text;
                    ee.iElectronicEquipment_Make_Id = Convert.ToInt32(ddlElectronicEquipment_Make.SelectedValue);
                    ee.iElectronicEquipment_Model_Id = Convert.ToInt32(ddlElectronicEquipment_Model.SelectedValue);

                    P.ElectronicEquipment_Asset_Provider pro = new P.ElectronicEquipment_Asset_Provider();
                    pro.Save_New_ElectronicEquipment_Asset(ee);
                    saved = true;
                }
                else
                {
                    litFinanceNumberExists.Text = "<label for='" + txtFinance_Agrreement_Number.ClientID + "' class='txtnamevalidation erroMessage'>Finance number already exists</label>";
                }
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddElectronicEquipment-UserControl", "SaveElectronicEquipmentAsset");
            }
            return saved;
        }
        public bool SaveElectronicEquipmentAsset_Without_Policy(int alignmentId)
        {
            bool saved = false;
            if (!Page.IsValid)
            {
                return false;
            }
            try
            {
                P.Generic_Asset_Provider proGen = new P.Generic_Asset_Provider();
                if (!proGen.Check_FinanceNumber_Exists(Convert.ToInt32(ddlAsset_Financier.SelectedValue), txtFinance_Agrreement_Number.Text))
                {
                    AT.ElectronicEquipment_Asset ee = new AT.ElectronicEquipment_Asset();




                    ee.iPolicy_Id = 0;
                    ee.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    ee.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    ee.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    ee.mAsset_Finance_Value = Convert.ToDecimal(txtAsset_Finance_Value.Text.Replace(",", "").Replace(".", ","));
                    ee.mAsset_Insurance_Value = Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","));
                    ee.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    ee.dtFinance_End_Date = txtFinance_End_Date.Text;
                    ee.iElectronicEquipment_Asset_Type_Id = Convert.ToInt32(ddlElectronicEquipment_Asset_Type.SelectedValue);
                    ee.vcSerial_Number = txtSerial_Number.Text;
                    ee.iElectronicEquipment_Make_Id = Convert.ToInt32(ddlElectronicEquipment_Make.SelectedValue);
                    ee.iElectronicEquipment_Model_Id = Convert.ToInt32(ddlElectronicEquipment_Model.SelectedValue);

                    P.ElectronicEquipment_Asset_Provider pro = new P.ElectronicEquipment_Asset_Provider();
                    pro.Save_New_ElectronicEquipment_Asset_Without_Policy(ee, alignmentId);
                    saved = true;
                }
                else
                {
                    litFinanceNumberExists.Text = "<label for='" + txtFinance_Agrreement_Number.ClientID + "' class='txtnamevalidation erroMessage'>Finance number already exists</label>";
                }
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddElectronicEquipment-UserControl", "SaveElectronicEquipmentAsset");
            }
            return saved;
        }
        protected void ddlElectronicEquipment_Make_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlElectronicEquipment_Asset_Type.SelectedIndex > 0)
            {
                ddlElectronicEquipment_Model.ClearSelection();
                ddlElectronicEquipment_Model.Items.Clear();
                ddlElectronicEquipment_Model.Items.Add(new ListItem("", ""));
                P.ElectronicEquipment_Asset_Provider frmF = new P.ElectronicEquipment_Asset_Provider();
                SqlDataReader dr = frmF.Get_ElectronicEquipment_Assset_Models_By_Make_Type(Convert.ToInt32(ddlElectronicEquipment_Make.SelectedValue), Convert.ToInt32(ddlElectronicEquipment_Asset_Type.SelectedValue));
                while (dr.Read())
                {
                    ddlElectronicEquipment_Model.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

                }
            }
            else
            {
                litElectronicEquipment_Asset_Type.Text = "Please select a type";
                ddlElectronicEquipment_Make.ClearSelection();
                ddlElectronicEquipment_Make.SelectedIndex = 0;
            }
        }

        protected void ddlElectronicEquipment_Asset_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlElectronicEquipment_Make.ClearSelection();
            ddlElectronicEquipment_Make.SelectedIndex = 0;
            ddlElectronicEquipment_Model.ClearSelection();
            ddlElectronicEquipment_Model.SelectedIndex = 0;
            litElectronicEquipment_Asset_Type.Text = "";
        }
    }
}