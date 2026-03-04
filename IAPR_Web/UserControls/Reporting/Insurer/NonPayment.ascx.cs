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

namespace IAPR_Web.UserControls.Reporting.Insurer
{
    public partial class NonPayment : System.Web.UI.UserControl
    {
        CCom.CurrentUser objUser = new CCom.CurrentUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["SortOrder"] = "ASC";
                ViewState["SortField"] = "PolicyNumber";
                ddlYear.Items.Add(new ListItem("", ""));
                GetMonths();
                GetYears();
                P.User_Provider uP = new P.User_Provider();

                objUser = uP.GetUserFromSession();
                
                if (objUser.iUser_Type_Id == 1 || objUser.iUser_Type_Id == 2)
                {
                    pnlPartner.Visible = true;
                    Getformfields();
                }
            }




        }
        public void StartFormLoad()
        {
            ViewState["SortOrder"] = "ASC";
            ViewState["SortField"] = "PolicyNumber";
            ddlYear.Items.Add(new ListItem("", ""));
            GetMonths();
            GetYears();
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();
            
            if (objUser.iUser_Type_Id == 1 || objUser.iUser_Type_Id == 2)
            {
                pnlPartner.Visible = true;
                rptNonPayment.DataSource = null;
                rptNonPayment.DataBind();
                pnlNonPaymnet.Visible = false;

                Getformfields();
            }
        }
        private void GetMonths()
        {
            ddlPeriod.Items.Clear();
            ddlPeriod.Items.Add(new ListItem("", ""));

            var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            for (int i = 0; i < months.Length; i++)
            {
                ddlPeriod.Items.Add(new ListItem(months[i], (i + 1).ToString()));
            }

        }
        private void GetYears()
        {
            ddlYear.Items.Clear();
            ddlYear.Items.Add(new ListItem("", ""));
            for (int row = DateTime.Now.Year; row > DateTime.Now.Year - 2; row--)
            {
                ddlYear.Items.Add(new ListItem(row.ToString(), row.ToString()));
            }

        }
        protected void btnShowMonthlyNonPayment_Click(object sender, EventArgs e)
        {
            CCom.CurrentUser objUser = new CCom.CurrentUser();
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();
            
            if (objUser.iUser_Type_Id == 1 || objUser.iUser_Type_Id == 2)
            {
                GetNonPaymentReport(Convert.ToInt32(ddlPartner.SelectedValue));
            }
            else
            {
                GetNonPaymentReport(objUser.iPartner_Id);
            }
            lblPeriod.Text = ddlPeriod.SelectedItem.Text + " " + ddlYear.SelectedItem.Text;
            pnlNonPaymnet.Visible = true;
        }

        public void Getformfields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsGeneric();
            ddlPartner.Items.Clear();
            ddlPartner.Items.Add(new ListItem("", ""));
            //Asset_Financier
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlPartner.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
        }

        protected void btnSendReport_Click(object sender, EventArgs e)
        {
            try
            {

                rptNonPayment.DataSource = null;
                rptNonPayment.DataBind();

                P.Report_Provider frmF = new P.Report_Provider();
                DataSet ds = frmF.Get_Policy_NonPayment_By_Financier_By_Period(2, Convert.ToInt32(ddlPeriod.SelectedValue), Convert.ToInt32(ddlYear.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {

                    rptNonPayment.DataSource = ds.Tables[0];
                    rptNonPayment.DataBind();
                    rptNonPayment.Visible = true;
                    rptNonPayment.Visible = true;

                    P.Notification_Provider n = new P.Notification_Provider();

                    //n.Send_Financier_NonPayment_By_Period_By_Year(ds, "Investec", "January", "2021");

                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        private void GetNonPaymentReport(int iPartnerId)
        {
            try
            {

                rptNonPayment.DataSource = null;
                rptNonPayment.DataBind();

                P.Report_Provider frmF = new P.Report_Provider();
                DataSet ds = frmF.Get_Policy_NonPayment_By_Insurer_By_Period(iPartnerId, Convert.ToInt32(ddlPeriod.SelectedValue), Convert.ToInt32(ddlYear.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    btnSendReport.Visible = true;
                    rptNonPayment.DataSource = ds.Tables[0];

                    rptNonPayment.DataBind();
                    rptNonPayment.Visible = true;
                    rptNonPayment.Visible = true;

                    P.Notification_Provider n = new P.Notification_Provider();

                }
                else
                {
                    btnSendReport.Visible = false;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void rptNonPayment_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName == "Finance Value")
                {



                }
            }
        }
    }
}