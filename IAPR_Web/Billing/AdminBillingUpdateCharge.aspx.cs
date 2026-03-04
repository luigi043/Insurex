using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using C = IAPR_Data.Classes;
using AT = IAPR_Data.Classes.AssetTypes;
using P = IAPR_Data.Providers;
using U = IAPR_Data.Utils;

namespace IAPR_Web.Billing
{
    public partial class AdminBillingUpdateCharge : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetPartnerCharges();
            }
        }

        protected void btnFind_Charge_Click(object sender, EventArgs e)
        {
            GetChargeCurrentDetails();
            pnlStep1.Enabled = false;
            pnlStep2.Visible = true;
        }
        private void GetPartnerCharges()
        {
            P.Billing_Provider frmF = new P.Billing_Provider();
            DataSet ds = frmF.GetPartnerChargeTypes();

            //Clear all DropDownLists
            ddlCharge_Type.Items.Clear();
            ddlCharge_Type.Items.Add(new ListItem("", ""));

            //Populate relevant dropdownlists
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlCharge_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
        }
        private void GetChargeCurrentDetails()
        {
            try
            {
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                P.Billing_Provider frmF = new P.Billing_Provider();
                DataSet ds = frmF.GetPartnerChargeDetails(Convert.ToInt32(ddlCharge_Type.SelectedValue));
                divPartnerChargeDetails.InnerHtml = "";
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    foreach (DataColumn c in ds.Tables[0].Columns)
                    {
                        s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");

                    }
                }
                divPartnerChargeDetails.InnerHtml = s.ToString();
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AdminBillingNewCharge", "btnAddNewCharge_Click");
            }

        }
        protected void btnSaveUpdateCharge_Click(object sender, EventArgs e)
        {
            try
            {


                P.Billing_Provider aB = new P.Billing_Provider();
                aB.Update_Partner_Charge(Convert.ToInt32(ddlCharge_Type.SelectedValue), Convert.ToDecimal(txtChargeAmount.Text.Replace(",", "").Replace(".", ",")),
                    txtCharge_Start_Date.Text, txtCharge_End_Date.Text);
                txtCharge_End_Date.Text = "";
                txtCharge_Start_Date.Text = "";
                txtChargeAmount.Text = "";
                pnlStep1.Enabled = true;
                pnlStep2.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Charge fee updated successfully');", true);

            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AdminBillingNewCharge", "btnAddNewCharge_Click");
            }
        }

        protected void btnCancelUpdateCharge_Click(object sender, EventArgs e)
        {
            pnlStep1.Enabled = true;
            pnlStep2.Visible = false;
            txtChargeAmount.Text = txtCharge_End_Date.Text = txtCharge_Start_Date.Text = "";

        }
    }
}