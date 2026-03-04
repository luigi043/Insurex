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
    public partial class AddBusinessInterruptionAsset : System.Web.UI.UserControl
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
            DataSet ds = frmF.GetFormFieldBusinessInterruptionAsset();

            //Clear all DropDownLists


            ddlBusinessInterruption_Asset_Type.Items.Clear();


            ddlAsset_Financier.Items.Clear();

            //Insert Empty 1st option 

            ddlAsset_Cover_Type.Items.Add(new ListItem("", ""));


            ddlBusinessInterruption_Asset_Type.Items.Add(new ListItem("", ""));


            ddlAsset_Financier.Items.Add(new ListItem("", ""));



            //Populate relevant dropdownlists

            //Asset_Type_Cover
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                ddlAsset_Cover_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            //BusinessInterruption_Asset_Type
            foreach (DataRow row in ds.Tables[6].Rows)
            {
                ddlBusinessInterruption_Asset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }



            //Asset_Financier
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                ddlAsset_Financier.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
        }
        #endregion


        public bool SaveBusinessInterruptionAsset(int policyId)
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
                    AT.BusinessInterruption_Asset bi = new AT.BusinessInterruption_Asset();




                    bi.iPolicy_Id = policyId;
                    bi.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    bi.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    bi.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    bi.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    bi.dtFinance_End_Date = txtFinance_End_Date.Text;
                    bi.iBusinessInterruption_Asset_Type_Id = Convert.ToInt32(ddlBusinessInterruption_Asset_Type.SelectedValue);
                    bi.vcDescription = txtDescription.Text;
                    P.BusinessInterruption_Asset_Provider pro = new P.BusinessInterruption_Asset_Provider();
                    pro.Save_New_BusinessInterruption_Asset(bi);
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
                eL.LogErrorInDB(ex, "AddBusinessInterruption-UserControl", "SaveBusinessInterruptionAsset");
            }
            return saved;
        }
        public bool SaveBusinessInterruptionAsset_Without_Policy(int alignmentId)
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
                    AT.BusinessInterruption_Asset bi = new AT.BusinessInterruption_Asset();




                    bi.iPolicy_Id = 0;
                    bi.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    bi.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    bi.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    bi.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    bi.dtFinance_End_Date = txtFinance_End_Date.Text;
                    bi.iBusinessInterruption_Asset_Type_Id = Convert.ToInt32(ddlBusinessInterruption_Asset_Type.SelectedValue);
                    bi.vcDescription = txtDescription.Text;
                    P.BusinessInterruption_Asset_Provider pro = new P.BusinessInterruption_Asset_Provider();
                    pro.Save_New_BusinessInterruption_Asset_Without_Policy(bi, alignmentId);
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
                eL.LogErrorInDB(ex, "AddBusinessInterruption-UserControl", "SaveBusinessInterruptionAsset");
            }
            return saved;
        }

    }
}