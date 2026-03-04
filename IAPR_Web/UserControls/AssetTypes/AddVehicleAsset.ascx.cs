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
    public partial class AddVehicleAsset : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "anything", "AssignSelect();", true);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DropDownSearch()", true);
            if (!IsPostBack)
            {

            }
        }

        #region private
        public void GetFormFields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsVehicleAsset();

            //Clear all DropDownLists

            ddlModel_Year.Items.Clear();
            ddlAsset_Usage_Type.Items.Clear();
            ddlVehicle_Asset_Type.Items.Clear();
            //ddlVehicle_Asset_Licence_Type.Items.Clear();
            ddlAsset_Condition.Items.Clear();

            ddlAsset_Financier.Items.Clear();
            ddlVehicle_Model.Items.Clear();
            ddlVehicle_Make.Items.Clear();
            //Insert Empty 1st option 

            ddlAsset_Cover_Type.Items.Add(new ListItem("", ""));

            ddlModel_Year.Items.Add(new ListItem("", ""));
            ddlAsset_Usage_Type.Items.Add(new ListItem("", ""));
            ddlVehicle_Asset_Type.Items.Add(new ListItem("", ""));
            // ddlVehicle_Asset_Licence_Type.Items.Add(new ListItem("", ""));
            ddlAsset_Condition.Items.Add(new ListItem("", ""));

            ddlAsset_Financier.Items.Add(new ListItem("", ""));
            ddlVehicle_Make.Items.Add(new ListItem("", ""));
            ddlVehicle_Model.Items.Add(new ListItem("", ""));


            //Populate relevant dropdownlists

            //Asset_Type_Cover
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                ddlAsset_Cover_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
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
            //foreach (DataRow row in ds.Tables[8].Rows)
            //{
            //    ddlVehicle_Asset_Licence_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            //}


            //Asset_Condition
            foreach (DataRow row in ds.Tables[9].Rows)
            {
                ddlAsset_Condition.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
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
            for (int row = DateTime.Now.Year; row > DateTime.Now.Year - 20; row--)
            {
                ddlModel_Year.Items.Add(new ListItem(row.ToString(), row.ToString()));
            }

            ddlModelVariant.ClearSelection();
        }

        public bool CheckVehicleDetailsExists()
        {
            bool exists = false;
            P.Vehicle_Asset_Provider pro = new P.Vehicle_Asset_Provider();
            DataSet ds = pro.Check_Vehicles_Details_Exist(txtFinance_Agrreement_Number.Text, txtVin_Number.Text, txtRegistration_Number.Text);
            foreach (DataTable t in ds.Tables)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Finance number already exists');", true);
                    litFinanceNumberExists.Text = "<label for='" + txtFinance_Agrreement_Number.ClientID + "' class='txtnamevalidation erroMessage'>Finance number already exists</label>";
                    exists = true;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('VIN number already exists');", true);
                    litVinNumber.Text = "<label for='" + txtVin_Number.ClientID + "' class='txtnamevalidation erroMessage'>VIN number already exists</label>";
                    exists = true;
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Registration number already exists');", true);
                    litRegistrationNUmber.Text = "<label for='" + txtRegistration_Number.ClientID + "' class='txtnamevalidation erroMessage'>Registration number already exists</label>";
                    exists = true;
                }

            }

            return exists;
        }
        #endregion


        public bool SaveVehicleData(int policyId)
        {
            bool saved = false;
            if (!Page.IsValid)
            {
                return false;
            }
            try
            {
                P.Generic_Asset_Provider proGen = new P.Generic_Asset_Provider();
                if (!CheckVehicleDetailsExists())
                {
                    AT.Vehicle_Asset veh = new AT.Vehicle_Asset();

                    veh.iPolicy_Id = policyId;
                    veh.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    veh.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    veh.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    veh.mAsset_Finance_Value = Convert.ToDecimal(txtAsset_Finance_Value.Text.Replace(",", "").Replace(".", ","));
                    veh.mAsset_Insurance_Value = Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","));
                    veh.iAsset_Type_Id = 1;
                    veh.iVehicle_Asset_Type_Id = Convert.ToInt32(ddlVehicle_Asset_Type.SelectedValue);
                    // veh.iVehicle_Asset_Licence_Type_Id = Convert.ToInt32(ddlVehicle_Asset_Licence_Type.SelectedValue);
                    veh.iAsset_Usage_Type_Id = Convert.ToInt32(ddlAsset_Usage_Type.SelectedValue);
                    veh.iAsset_Condition_Id = Convert.ToInt32(ddlAsset_Condition.SelectedValue);
                    veh.iVehicle_Model_Id = Convert.ToInt32(ddlVehicle_Model.SelectedValue);
                    veh.iVehicle_Make_Id = Convert.ToInt32(ddlVehicle_Make.SelectedValue);
                    veh.iVehicle_Model_Variant_Id = Convert.ToInt32(ddlModelVariant.SelectedValue);
                    veh.vcVin_Number = txtVin_Number.Text;
                    veh.vcRegistration_Number = txtRegistration_Number.Text;
                    veh.iModel_Year = Convert.ToInt32(ddlModel_Year.SelectedValue);
                    veh.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    veh.dtFinance_End_Date = txtFinance_End_Date.Text;
                    // veh.vcVehicle_Color = txtVehicle_Color.Text;


                    P.Vehicle_Asset_Provider pro = new P.Vehicle_Asset_Provider();
                    pro.Save_New_Vehicle_Asset(veh);
                    saved = true;
                }
                //else
                //{
                //    litFinanceNumberExists.Text = "<label for='" + txtFinance_Agrreement_Number.ClientID + "' class='txtnamevalidation erroMessage'>Finance number already exists</label>";
                //}
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddVehicleAsset-UserControl", "SaveVehicleData");
            }
            return saved;
        }
        public bool SaveVehicleData_Without_Policy(int alignmentId)
        {
            bool saved = false;
            try
            {
                P.Generic_Asset_Provider proGen = new P.Generic_Asset_Provider();
                if (!CheckVehicleDetailsExists())
                {
                    AT.Vehicle_Asset veh = new AT.Vehicle_Asset();

                    veh.iPolicy_Id = 0;
                    veh.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    veh.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    veh.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    veh.mAsset_Finance_Value = Convert.ToDecimal(txtAsset_Finance_Value.Text.Replace(",", "").Replace(".", ","));
                    veh.mAsset_Insurance_Value = Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","));
                    veh.iAsset_Type_Id = 1;
                    veh.iVehicle_Asset_Type_Id = Convert.ToInt32(ddlVehicle_Asset_Type.SelectedValue);
                    // veh.iVehicle_Asset_Licence_Type_Id = Convert.ToInt32(ddlVehicle_Asset_Licence_Type.SelectedValue);
                    veh.iAsset_Usage_Type_Id = Convert.ToInt32(ddlAsset_Usage_Type.SelectedValue);
                    veh.iAsset_Condition_Id = Convert.ToInt32(ddlAsset_Condition.SelectedValue);
                    veh.iVehicle_Model_Id = Convert.ToInt32(ddlVehicle_Model.SelectedValue);
                    veh.iVehicle_Make_Id = Convert.ToInt32(ddlVehicle_Make.SelectedValue);
                    veh.iVehicle_Model_Variant_Id = Convert.ToInt32(ddlModelVariant.SelectedValue);
                    veh.vcVin_Number = txtVin_Number.Text;
                    veh.vcRegistration_Number = txtRegistration_Number.Text;
                    veh.iModel_Year = Convert.ToInt32(ddlModel_Year.SelectedValue);
                    veh.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    veh.dtFinance_End_Date = txtFinance_End_Date.Text;
                    // veh.vcVehicle_Color = txtVehicle_Color.Text;


                    P.Vehicle_Asset_Provider pro = new P.Vehicle_Asset_Provider();
                    pro.Save_New_Vehicle_Asset_Without_Policy(veh, alignmentId);
                    saved = true;
                    ClearControls(this);
                }
                else
                {
                    litFinanceNumberExists.Text = "<label for='" + txtFinance_Agrreement_Number.ClientID + "' class='txtnamevalidation erroMessage'>Finance number already exists</label>";
                }
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddVehicleAsset-UserControl", "SaveVehicleData");
            }
            return saved;
            // ScriptManager.RegisterStartupScript(this, typeof(Page), "<script>showpop5('Asset saved')</script>", true);
        }

        protected void ddlVehicle_Make_SelectedIndexChanged(object sender, EventArgs e)
        {
            litVehicle_Make.Text = "";

            if (ddlVehicle_Asset_Type.SelectedIndex > 0)
            {
                ddlVehicle_Model.ClearSelection();
                ddlVehicle_Model.Items.Clear();
                ddlModelVariant.ClearSelection();
                ddlModelVariant.Items.Clear();
                ddlVehicle_Model.Items.Add(new ListItem("", ""));
                P.Vehicle_Asset_Provider frmF = new P.Vehicle_Asset_Provider();
                SqlDataReader dr = frmF.Get_Vehicle_Assset_Models_By_Make_Type(Convert.ToInt32(ddlVehicle_Make.SelectedValue), Convert.ToInt32(ddlVehicle_Asset_Type.SelectedValue));
                while (dr.Read())
                {
                    ddlVehicle_Model.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

                }
            }
            else
            {
                litVehicle_Asset_Type.Text = "<label for='" + ddlVehicle_Asset_Type.ClientID + "' class='txtnamevalidation erroMessage'>Please select a type</label>";
                //litVehicle_Asset_Type.Text = "Please select a type";
                ddlVehicle_Make.ClearSelection();
                ddlVehicle_Make.SelectedIndex = 0;
            }
        }

        protected void TextBoxCurrency_TextChanged(object sender, EventArgs e)

        {
            double cur;

            double.TryParse(txtAsset_Finance_Value.Text, out cur);



            txtAsset_Finance_Value.Text = cur.ToString("c");

        }

        protected void ddlVehicle_Asset_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            litVehicle_Asset_Type.Text = "";
        }

        protected void ddlVehicle_Model_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVehicle_Make.SelectedIndex > 0)
            {
                ddlModelVariant.ClearSelection();
                ddlModelVariant.Items.Clear();
                ddlModelVariant.Items.Add(new ListItem("", ""));
                P.Vehicle_Asset_Provider frmF = new P.Vehicle_Asset_Provider();
                SqlDataReader dr = frmF.Get_Vehicle_Assset_Models_Variant(Convert.ToInt32(ddlVehicle_Model.SelectedValue));
                while (dr.Read())
                {
                    ddlModelVariant.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));

                }
            }
            else
            {
                litVehicle_Make.Text = "<label for='" + ddlVehicle_Make.ClientID + "' class='txtnamevalidation erroMessage'>Please select a vehicle make</label>";
                //litVehicle_Asset_Type.Text = "Please select a type";
                ddlVehicle_Model.ClearSelection();
                ddlVehicle_Model.SelectedIndex = 0;
            }
        }

        public void ClearControls(Control parent)
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
        }
    }
}