using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using AT = IAPR_Data.Classes;
using CP = IAPR_Data.Classes.Policy;
using CCom = IAPR_Data.Classes.Common;
using P = IAPR_Data.Providers;
using U = IAPR_Data.Utils;
using System.Configuration;
using System.Globalization;
namespace IAPR_Web.Billing
{
    public partial class AdminViewPartnerInvoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetFormFields();
            }
        }

        public void GetFormFields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsGeneric();

            //Clear all DropDownLists
            ddlPartnerType.Items.Clear();


            //Insert Empty 1st option 
            ddlPartnerType.Items.Add(new ListItem("", ""));

            //Populate relevant dropdownlists
            //Insurance companies
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                ddlPartnerType.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            ddlInvoiceMonth.Items.Add(new ListItem("", ""));
            var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            for (int i = 0; i < months.Length; i++)
            {
                ddlInvoiceMonth.Items.Add(new ListItem(months[i], (i + 1).ToString()));
            }
            ddlInvoiceYear.Items.Add(new ListItem("", ""));
            for (int i = DateTime.Now.Year; i > 2020; i--)
            {
                ddlInvoiceYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }


        }
        protected void ddlPartnerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsGeneric();

            //Clear all DropDownLists
            ddlPartners.Items.Clear();

            //Insert Empty 1st option 
            ddlPartners.Items.Add(new ListItem("", ""));


            //Populate relevant dropdownlists
            //Insurance companies
            if (Convert.ToInt32(ddlPartnerType.SelectedValue) == Convert.ToInt32(CCom.Common.Partner_types.Insurance_provider))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ddlPartners.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                }
            }

            //Finance Companies
            if (Convert.ToInt32(ddlPartnerType.SelectedValue) == Convert.ToInt32(CCom.Common.Partner_types.Lender))
            {
                foreach (DataRow row in ds.Tables[13].Rows)
                {
                    ddlPartners.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                }
            }
        }
        protected void ddlPartners_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private void GetInvoiceTotalsForPartnerForPeriod()
        {

            System.Text.StringBuilder s = new System.Text.StringBuilder();
            P.Billing_Provider frmF = new P.Billing_Provider();
            DataSet ds = frmF.GetInvoiceTotalsForPartnerForPeriod(Convert.ToInt32(ddlPartners.SelectedValue), Convert.ToInt32(ddlPartnerType.SelectedValue), Convert.ToInt32(ddlInvoiceMonth.SelectedValue), Convert.ToInt32(ddlInvoiceYear.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                pnlStep1.Enabled = false;
                pnlStep2.Visible = true;
                lblPartnerName.Text = ddlPartners.SelectedItem.Text;
                lblPartnerType.Text = ds.Tables[0].Rows[0]["vcPartner_Type_Description"].ToString();
                lblInvoiceNumber.Text = ds.Tables[0].Rows[0]["vcInvoice_Number"].ToString();
                lblInvoiceStatus.Text = ds.Tables[0].Rows[0]["vcPartner_Invoice_Status_Description"].ToString();
                lblInvoicingMonth.Text = ds.Tables[0].Rows[0]["iInvoicing_Month"].ToString();
                lblInvoiceYear.Text = ds.Tables[0].Rows[0]["iInvoicing_Year"].ToString();
                lblInvoiceTotalFee.Text = ds.Tables[0].Rows[0]["InvoiceTotalFee"].ToString();
                lblNumberOfTransactions.Text = ds.Tables[0].Rows[0]["InvoiceransactionCount"].ToString();

                rptTransactionTypeTotals.DataSource = null;
                rptTransactionTypeTotals.DataSource = ds.Tables[1];
                rptTransactionTypeTotals.DataBind();


            }
            else
            {
                pnlStep1.Enabled = true;
                pnlStep2.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('No invoice found for this partner and period ');", true);
            }
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            GetInvoiceTotalsForPartnerForPeriod();            
        }

        protected void btnDownLoadTransactions_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancelUpdateCharge_Click(object sender, EventArgs e)
        {
            pnlStep1.Enabled = true;
            pnlStep2.Visible = false;
            lblPartnerName.Text = "";
            lblPartnerType.Text = "";
            lblInvoiceNumber.Text = "";
            lblInvoiceStatus.Text = "";
            lblInvoicingMonth.Text = "";
            lblInvoiceYear.Text = "";
            lblInvoiceTotalFee.Text = "";
            lblNumberOfTransactions.Text = "";
        }
    }
}