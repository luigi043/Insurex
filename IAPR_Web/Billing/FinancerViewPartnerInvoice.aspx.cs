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
    public partial class FinancerViewPartnerInvoice : System.Web.UI.Page
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



            //Insert Empty 1st option 


            //Populate relevant dropdownlists
            //Insurance companies

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

        private void GetInvoiceTotalsForPartnerForPeriod()
        {
            P.User_Provider uP = new P.User_Provider();
            CCom.CurrentUser objUser = new CCom.CurrentUser();
            objUser = uP.GetUserFromSession();

            System.Text.StringBuilder s = new System.Text.StringBuilder();
            P.Billing_Provider frmF = new P.Billing_Provider();
            DataSet ds = frmF.GetInvoiceTotalsForPartnerForPeriod(objUser.iPartner_Id, objUser.iPartner_Type_Id, Convert.ToInt32(ddlInvoiceMonth.SelectedValue), Convert.ToInt32(ddlInvoiceYear.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                pnlStep1.Enabled = false;
                pnlStep2.Visible = true;
                lblPartnerName.Text = objUser.vcPartner_Name;
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
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('No invoice found for this period ');", true);
            }
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            GetInvoiceTotalsForPartnerForPeriod();
            // txtPartnerName.Text = "test";
            //  pnlStep1.Enabled = false;

            
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