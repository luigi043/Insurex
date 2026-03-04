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
    public partial class AddWatercraft : System.Web.UI.UserControl
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
            DataSet ds = frmF.GetFormFieldsWaterCraftAsset();

            //Clear all DropDownLists

            ddlModel_Year.Items.Clear();
            ddlAsset_Usage_Type.Items.Clear();
            ddlWatercraft_Asset_Type.Items.Clear();

            ddlAsset_Condition.Items.Clear();

            ddlAsset_Financier.Items.Clear();
            ddlWatercraft_Class_Type.Items.Clear();
            //Insert Empty 1st option 

            ddlAsset_Cover_Type.Items.Add(new ListItem("", ""));

            ddlModel_Year.Items.Add(new ListItem("", ""));
            ddlAsset_Usage_Type.Items.Add(new ListItem("", ""));
            ddlWatercraft_Class_Type.Items.Add(new ListItem("", ""));
            ddlWatercraft_Asset_Type.Items.Add(new ListItem("", ""));
            // ddlVehicle_Asset_Licence_Type.Items.Add(new ListItem("", ""));
            ddlAsset_Condition.Items.Add(new ListItem("", ""));

            ddlAsset_Financier.Items.Add(new ListItem("", ""));



            //Populate relevant dropdownlists

            //Asset_Type_Cover
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                ddlAsset_Cover_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            //Watercraft_Asset_Type
            //foreach (DataRow row in ds.Tables[6].Rows)
            //{
            //    ddlWatercraft_Asset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            //}

            //Asset_Usage
            foreach (DataRow row in ds.Tables[7].Rows)
            {
                ddlAsset_Usage_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

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
            //Watercraft class
            foreach (DataRow row in ds.Tables[15].Rows)
            {
                ddlWatercraft_Class_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Mdel_year from current year to -20
            for (int row = DateTime.Now.Year; row > DateTime.Now.Year - 20; row--)
            {
                ddlModel_Year.Items.Add(new ListItem(row.ToString(), row.ToString()));
            }


        }
        public bool CheckWatercraftDetailsExists()
        {
            bool exists = false;
            P.Watercraft_Provider pro = new P.Watercraft_Provider();
            DataSet ds = pro.Check_Watercraft_Details_Exist(txtFinance_Agrreement_Number.Text, txtName_Emblem.Text, txtHull_Identification_Number.Text);
            foreach (DataTable t in ds.Tables)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Finance agreement number already exists');", true);
                    exists = true;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Name/Emblem number already exists');", true);
                    exists = true;
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Registration  Number already exists');", true);
                    exists = true;
                }

            }

            return exists;
        }
        #endregion


        public bool SaveWatercraftAsset(int policyId)
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
                    AT.Watercraft_Asset wc = new AT.Watercraft_Asset();




                    wc.iPolicy_Id = policyId;
                    wc.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);

                    wc.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);

                    wc.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    wc.mAsset_Finance_Value = Convert.ToDecimal(txtAsset_Finance_Value.Text.Replace(",", "").Replace(".", ","));
                    wc.mAsset_Insurance_Value = Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","));
                    wc.iWatercraft_Asset_Type_Id = Convert.ToInt32(ddlWatercraft_Asset_Type.SelectedValue);
                    wc.vcName_Emblem = txtName_Emblem.Text;
                    wc.vcRegistration_Number = txtHull_Identification_Number.Text;
                    wc.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    wc.dtFinance_End_Date = txtFinance_End_Date.Text;

                    P.Watercraft_Provider pro = new P.Watercraft_Provider();
                    pro.Save_New_Watercraft_Asset(wc);
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
                eL.LogErrorInDB(ex, "AddWatercraft-UserControl", "SaveWatercraftAsset");
            }
            return saved;
        }
        public bool SaveWatercraftAsset_Without_Policy(int alignmentId)
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
                    AT.Watercraft_Asset wc = new AT.Watercraft_Asset();




                    wc.iPolicy_Id = 0;
                    wc.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);

                    wc.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);

                    wc.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    wc.mAsset_Finance_Value = Convert.ToDecimal(txtAsset_Finance_Value.Text.Replace(",", "").Replace(".", ","));
                    wc.mAsset_Insurance_Value = Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","));
                    wc.iWatercraft_Asset_Type_Id = Convert.ToInt32(ddlWatercraft_Asset_Type.SelectedValue);
                    wc.vcName_Emblem = txtName_Emblem.Text;
                    wc.vcRegistration_Number = txtHull_Identification_Number.Text;
                    wc.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    wc.dtFinance_End_Date = txtFinance_End_Date.Text;

                    P.Watercraft_Provider pro = new P.Watercraft_Provider();
                    pro.Save_New_Watercraft_Asset_Without_Policy(wc, alignmentId);
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
                eL.LogErrorInDB(ex, "AddWatercraft-UserControl", "SaveWatercraftAsset");
            }
            return saved;
        }

        protected void ddlWatercraft_Class_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlWatercraft_Class_Type.SelectedIndex > 0)
            {
                ddlWatercraft_Asset_Type.ClearSelection();
                ddlWatercraft_Asset_Type.Items.Clear();
                ddlWatercraft_Asset_Type.Items.Add(new ListItem("", ""));
                P.Watercraft_Provider frmF = new P.Watercraft_Provider();
                SqlDataReader dr = frmF.Get_Watercraft_Asset_Type_By_Class(Convert.ToInt32(ddlWatercraft_Class_Type.SelectedValue));
                while (dr.Read())
                {
                    ddlWatercraft_Asset_Type.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                }
                litWatercraftClass.Text = "";
            }
            else
            {

                litWatercraftClass.Text = "<label for='" + ddlWatercraft_Class_Type.ClientID + "' class='txtnamevalidation erroMessage'>Please select a type</label>"; ;//= "Please select a type";
                ddlWatercraft_Asset_Type.ClearSelection();
                ddlWatercraft_Asset_Type.SelectedIndex = 0;
            }
        }
    }
}