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
    public partial class AddStockAsset : System.Web.UI.UserControl
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
            DataSet ds = frmF.GetFormFieldStockAsset();

            //Clear all DropDownLists


            ddlStock_Asset_Type.Items.Clear();


            ddlAsset_Financier.Items.Clear();

            //Insert Empty 1st option 

            ddlAsset_Cover_Type.Items.Add(new ListItem("", ""));


            ddlStock_Asset_Type.Items.Add(new ListItem("", ""));
            // ddlVehicle_Asset_Licence_Type.Items.Add(new ListItem("", ""));
            ddlAsset_Financier.Items.Add(new ListItem("", ""));
            //Populate relevant dropdownlists

            //Asset_Type_Cover
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                ddlAsset_Cover_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            //Stock_Asset_Type
            foreach (DataRow row in ds.Tables[6].Rows)
            {
                ddlStock_Asset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }





            //Asset_Financier
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                ddlAsset_Financier.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }





        }
        #endregion


        public bool SaveStockAsset(int policyId)
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
                    AT.Stock_Asset st = new AT.Stock_Asset();




                    st.iPolicy_Id = policyId;
                    st.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    st.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    st.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    st.mAsset_Finance_Value = Convert.ToDecimal(txtAsset_Finance_Value.Text.Replace(",", "").Replace(".", ","));
                    st.mAsset_Insurance_Value = Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","));
                    st.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    st.dtFinance_End_Date = txtFinance_End_Date.Text;
                    st.iStock_Asset_Type_Id = Convert.ToInt32(ddlStock_Asset_Type.SelectedValue);
                    st.vcStock_Description = txtStock_Description.Text;
                    st.vcStock_Value = txtStock_Value.Text;





                    P.Stock_Asset_Provider pro = new P.Stock_Asset_Provider();
                    pro.Save_New_Stock_Asset(st);
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
                eL.LogErrorInDB(ex, "AddStock-UserControl", "SaveStockAsset");
            }
            return saved;
        }
        public bool SaveStockAsset_Without_Policy(int alignmentId)
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
                    AT.Stock_Asset st = new AT.Stock_Asset();




                    st.iPolicy_Id = 0;
                    st.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    st.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    st.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    st.mAsset_Finance_Value = Convert.ToDecimal(txtAsset_Finance_Value.Text.Replace(",", "").Replace(".", ","));
                    st.mAsset_Insurance_Value = Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","));
                    st.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    st.dtFinance_End_Date = txtFinance_End_Date.Text;
                    st.iStock_Asset_Type_Id = Convert.ToInt32(ddlStock_Asset_Type.SelectedValue);
                    st.vcStock_Description = txtStock_Description.Text;
                    st.vcStock_Value = txtStock_Value.Text;





                    P.Stock_Asset_Provider pro = new P.Stock_Asset_Provider();
                    pro.Save_New_Stock_Asset_Without_Policy(st, alignmentId);
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
                eL.LogErrorInDB(ex, "AddStock-UserControl", "SaveStockAsset");
            }
            return saved;
        }
    }
}