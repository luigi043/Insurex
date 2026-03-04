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
    public partial class AddAviationAsset : System.Web.UI.UserControl
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
            DataSet ds = frmF.GetFormFieldAviationAsset();

            //Clear all DropDownLists

            ddlModel_Year.Items.Clear();
            ddlAsset_Usage_Type.Items.Clear();
            ddlAviation_Asset_Type.Items.Clear();

            ddlAsset_Condition.Items.Clear();

            ddlAsset_Financier.Items.Clear();

            //Insert Empty 1st option 

            ddlAsset_Cover_Type.Items.Add(new ListItem("", ""));

            ddlModel_Year.Items.Add(new ListItem("", ""));
            ddlAsset_Usage_Type.Items.Add(new ListItem("", ""));
            ddlAviation_Asset_Type.Items.Add(new ListItem("", ""));

            ddlAsset_Condition.Items.Add(new ListItem("", ""));

            ddlAsset_Financier.Items.Add(new ListItem("", ""));



            //Populate relevant dropdownlists

            //Asset_Type_Cover
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                ddlAsset_Cover_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            //Aviation_Asset_Type
            foreach (DataRow row in ds.Tables[6].Rows)
            {
                ddlAviation_Asset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

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


            //Mdel_year from current year to -20
            for (int row = DateTime.Now.Year; row > DateTime.Now.Year - 20; row--)
            {
                ddlModel_Year.Items.Add(new ListItem(row.ToString(), row.ToString()));
            }


        }
        public bool CheckAviationDetailsExists()
        {
            bool exists = false;
            P.Aviation_Asset_Provider pro = new P.Aviation_Asset_Provider();
            DataSet ds = pro.Check_Aviation_Details_Exist(txtFinance_Agrreement_Number.Text, txtTail_Number.Text);
            foreach (DataTable t in ds.Tables)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Finance agreement number already exists');", true);
                    exists = true;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Tail number already exists');", true);
                    exists = true;
                }


            }

            return exists;
        }
        #endregion


        public bool SaveAviationAsset(int policyId)
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
                    AT.Aviation_Asset wc = new AT.Aviation_Asset();




                    wc.iPolicy_Id = policyId;
                    wc.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    wc.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    wc.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    wc.mAsset_Finance_Value = Convert.ToDecimal(txtAsset_Finance_Value.Text.Replace(",", "").Replace(".", ","));
                    wc.mAsset_Insurance_Value = Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","));
                    wc.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    wc.dtFinance_End_Date = txtFinance_End_Date.Text;
                    wc.iAviation_Asset_Type_Id = Convert.ToInt32(ddlAviation_Asset_Type.SelectedValue);
                    wc.vcTail_Number = txtTail_Number.Text;
                    P.Aviation_Asset_Provider pro = new P.Aviation_Asset_Provider();
                    pro.Save_New_Aviation_Asset(wc);
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
                eL.LogErrorInDB(ex, "AddAviation-UserControl", "SaveAviationAsset");
            }
            return saved;
        }
        public bool SaveAviationAsset_Without_Policy(int alignmentId)
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
                    AT.Aviation_Asset wc = new AT.Aviation_Asset();




                    wc.iPolicy_Id = 0;
                    wc.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    wc.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    wc.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    wc.mAsset_Finance_Value = Convert.ToDecimal(txtAsset_Finance_Value.Text.Replace(",", "").Replace(".", ","));
                    wc.mAsset_Insurance_Value = Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","));
                    wc.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    wc.dtFinance_End_Date = txtFinance_End_Date.Text;
                    wc.iAviation_Asset_Type_Id = Convert.ToInt32(ddlAviation_Asset_Type.SelectedValue);
                    wc.vcTail_Number = txtTail_Number.Text;
                    P.Aviation_Asset_Provider pro = new P.Aviation_Asset_Provider();
                    pro.Save_New_Aviation_Asset_Without_Policy(wc, alignmentId);
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
                eL.LogErrorInDB(ex, "AddAviation-UserControl", "SaveAviationAsset");
            }
            return saved;
        }

    }
}