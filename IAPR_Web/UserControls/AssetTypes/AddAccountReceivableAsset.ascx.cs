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
    public partial class AddAccountReceivableAsset : System.Web.UI.UserControl
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
            DataSet ds = frmF.GetFormFieldAccountReceivableAsset();

            //Clear all DropDownLists


            ddlAccountReceivable_Asset_Type.Items.Clear();


            ddlAsset_Financier.Items.Clear();

            //Insert Empty 1st option 

            ddlAsset_Cover_Type.Items.Add(new ListItem("", ""));


            ddlAccountReceivable_Asset_Type.Items.Add(new ListItem("", ""));



            ddlAsset_Financier.Items.Add(new ListItem("", ""));



            //Populate relevant dropdownlists

            //Asset_Type_Cover
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                ddlAsset_Cover_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            //AccountReceivable_Asset_Type
            foreach (DataRow row in ds.Tables[6].Rows)
            {
                ddlAccountReceivable_Asset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }




            //Asset_Financier
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                ddlAsset_Financier.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }





        }
        #endregion


        public bool SaveAccountReceivableAsset(int policyId)
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
                    AT.AccountReceivable_Asset ar = new AT.AccountReceivable_Asset();




                    ar.iPolicy_Id = policyId;
                    ar.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    ar.iFinancer_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    ar.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    ar.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    ar.dtFinance_End_Date = txtFinance_End_Date.Text;
                    ar.iAccountReceivable_Asset_Type_Id = Convert.ToInt32(ddlAccountReceivable_Asset_Type.SelectedValue);
                    ar.vcAccountReceivable_Description = txtvcAccountReceivable_Description.Text;
                    P.AccountReceivable_Asset_Provider pro = new P.AccountReceivable_Asset_Provider();
                    pro.Save_New_AccountReceivable_Asset(ar);
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
                eL.LogErrorInDB(ex, "AddAccountReceivable-UserControl", "SaveAccountReceivableAsset");
            }
            return saved;
        }
        public bool SaveAccountReceivableAsset_Without_Policy(int alignmentId)
        {
            bool saved = false;
            if (!Page.IsValid)
            {
                return false;
            }
            try
            {
                AT.AccountReceivable_Asset ar = new AT.AccountReceivable_Asset();




                ar.iPolicy_Id = 0;
                ar.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                ar.iFinancer_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                ar.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                ar.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                ar.dtFinance_End_Date = txtFinance_End_Date.Text;
                ar.iAccountReceivable_Asset_Type_Id = Convert.ToInt32(ddlAccountReceivable_Asset_Type.SelectedValue);
                ar.vcAccountReceivable_Description = txtvcAccountReceivable_Description.Text;
                P.AccountReceivable_Asset_Provider pro = new P.AccountReceivable_Asset_Provider();
                pro.Save_New_AccountReceivable_Asset_Without_Policy(ar, alignmentId);
                saved = true;
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddAccountReceivable-UserControl", "SaveAccountReceivableAsset");
            }
            return saved;
        }

    }
}