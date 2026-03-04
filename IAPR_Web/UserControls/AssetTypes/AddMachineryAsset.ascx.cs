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
    public partial class AddMachineryAsset : System.Web.UI.UserControl
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
            DataSet ds = frmF.GetFormFieldMachineryAsset();

            //Clear all DropDownLists


            ddlMachinery_Asset_Type.Items.Clear();

            ddlAsset_Financier.Items.Clear();

            //Insert Empty 1st option 

            ddlAsset_Cover_Type.Items.Add(new ListItem("", ""));


            ddlMachinery_Asset_Type.Items.Add(new ListItem("", ""));


            ddlAsset_Financier.Items.Add(new ListItem("", ""));

            //Populate relevant dropdownlists

            //Asset_Type_Cover
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                ddlAsset_Cover_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            //Machinery_Asset_Type
            foreach (DataRow row in ds.Tables[6].Rows)
            {
                ddlMachinery_Asset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Asset_Financier
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                ddlAsset_Financier.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }




        }
        public bool CheckMachineryDetailsExists()
        {
            bool exists = false;
            P.Machinery_Asset_Provider pro = new P.Machinery_Asset_Provider();
            DataSet ds = pro.Check_Machinery_Details_Exist(txtFinance_Agrreement_Number.Text, txtSerial_Number.Text);
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


        public bool SaveMachineryAsset(int policyId)
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
                    AT.Machinery_Asset ma = new AT.Machinery_Asset();




                    ma.iPolicy_Id = policyId;
                    ma.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    ma.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    ma.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    ma.mAsset_Finance_Value = Convert.ToDecimal(txtAsset_Finance_Value.Text.Replace(",", "").Replace(".", ","));
                    ma.mAsset_Insurance_Value = Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","));
                    ma.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    ma.dtFinance_End_Date = txtFinance_End_Date.Text;
                    ma.iMachinery_Asset_Type_Id = Convert.ToInt32(ddlMachinery_Asset_Type.SelectedValue);
                    ma.vcSerial_Number = txtSerial_Number.Text;

                    P.Machinery_Asset_Provider pro = new P.Machinery_Asset_Provider();
                    pro.Save_New_Machinery_Asset(ma);
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
                eL.LogErrorInDB(ex, "AddMachinery-UserControl", "SaveMachineryAsset");
            }
            return saved;
        }
        public bool SaveMachineryAsset_Without_Policy(int alignmentId)
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
                    AT.Machinery_Asset ma = new AT.Machinery_Asset();




                    ma.iPolicy_Id = 0;
                    ma.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    ma.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    ma.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    ma.mAsset_Finance_Value = Convert.ToDecimal(txtAsset_Finance_Value.Text.Replace(",", "").Replace(".", ","));
                    ma.mAsset_Insurance_Value = Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","));
                    ma.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    ma.dtFinance_End_Date = txtFinance_End_Date.Text;
                    ma.iMachinery_Asset_Type_Id = Convert.ToInt32(ddlMachinery_Asset_Type.SelectedValue);
                    ma.vcSerial_Number = txtSerial_Number.Text;

                    P.Machinery_Asset_Provider pro = new P.Machinery_Asset_Provider();
                    pro.Save_New_Machinery_Asset_Without_Policy(ma, alignmentId);
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
                eL.LogErrorInDB(ex, "AddMachinery-UserControl", "SaveMachineryAsset");
            }
            return saved;
        }
    }
}