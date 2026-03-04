using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using C = IAPR_Data.Classes;
using P = IAPR_Data.Providers;
using System.Globalization;
using CCom = IAPR_Data.Classes.Common;
using U = IAPR_Data.Utils;

namespace IAPR_Web.Billing
{
    public partial class AdminBillingNewCharge : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetFormFields();
            }
        }
        private void GetFormFields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsGeneric();

            //Clear all DropDownLists
            ddlPartnerType.Items.Clear();
            ddlPartnerPackage.Items.Clear();
            //chkAssetsFinanced.Items.Clear();
            //ddlDivisionAssets.Items.Clear();

            //Insert Empty 1st option 
            ddlPartnerType.Items.Add(new ListItem("", ""));
            ddlPartnerPackage.Items.Add(new ListItem("", ""));
            //ddlDivisionAssets.Items.Add(new ListItem("", ""));

            //Populate relevant dropdownlists
            //Partner Types
            ddlPartnerType.Items.Add(new ListItem("None", "0"));

            foreach (DataRow row in ds.Tables[11].Rows)
            {
                ddlPartnerType.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }


            //Partner Packages
            ddlPartnerPackage.Items.Add(new ListItem("None", "0"));
            foreach (DataRow row in ds.Tables[16].Rows)
            {
                ddlPartnerPackage.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

        }
        protected void btnAddNewCharge_Click(object sender, EventArgs e)
        {
            try
            {


                P.Billing_Provider aB = new P.Billing_Provider();
                bool bIs_Applicable_Monthly = rblMonthlyCharge.SelectedValue == "Yes" ? true : false;

                aB.Save_New_Partner_Charge(txtChargeTitle.Text, txtDescription.Text,
                    Convert.ToDecimal(txtChargeAmount.Text.Replace(",", "").Replace(".", ","))
                    , bIs_Applicable_Monthly, Convert.ToInt32(ddlPartnerType.SelectedValue)
                    , Convert.ToInt32(ddlPartnerPackage.SelectedValue), txtCharge_Start_Date.Text, txtCharge_End_Date.Text);
                txtChargeTitle.Text = "";
                txtDescription.Text = "";
                txtCharge_End_Date.Text = "";
                txtCharge_Start_Date.Text = "";
                txtChargeAmount.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Charge added successfully');", true);

            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AdminBillingNewCharge", "btnAddNewCharge_Click");
            }
        }
    }
}