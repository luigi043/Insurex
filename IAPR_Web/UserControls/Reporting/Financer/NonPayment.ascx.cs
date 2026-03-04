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
namespace IAPR_Web.UserControls.Reporting.Financer
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
            foreach (DataRow row in ds.Tables[13].Rows)
            {
                ddlPartner.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
        }

        protected void btnSendReport_Click(object sender, EventArgs e)
        {
            try
            {
                string ParnerName = string.Empty;

                string csv = string.Empty;
                DataSet ds;
                CCom.CurrentUser objUser = new CCom.CurrentUser();
                P.User_Provider uP = new P.User_Provider();
                P.Report_Provider frmF = new P.Report_Provider();

                objUser = uP.GetUserFromSession();

                if (objUser.iUser_Type_Id == 1 || objUser.iUser_Type_Id == 2)
                {
                    ds = frmF.Get_Policy_NonPayment_By_Financier_By_Period(Convert.ToInt32(ddlPartner.SelectedValue), Convert.ToInt32(ddlPeriod.SelectedValue), Convert.ToInt32(ddlYear.SelectedValue));
                    ParnerName = ddlPartner.SelectedItem.Text;
                }
                else
                {
                    ds = frmF.Get_Policy_NonPayment_By_Financier_By_Period(objUser.iPartner_Id, Convert.ToInt32(ddlPeriod.SelectedValue), Convert.ToInt32(ddlYear.SelectedValue));
                    ParnerName = objUser.vcPartner_Name;
                }

                if (ds.Tables[0].Rows.Count > 0)
                {

                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    DataTable dtExcel = ds.Tables[0];
                    foreach (DataColumn column in dtExcel.Columns)
                    {
                        context.Response.Write(column.ColumnName + ",");
                    }
                    context.Response.Write(Environment.NewLine);
                    foreach (DataRow row in dtExcel.Rows)
                    {
                        for (int i = 0; i < dtExcel.Columns.Count; i++)
                        {
                            context.Response.Write(row[i].ToString().Replace(",", string.Empty) + ",");
                        }
                        context.Response.Write(Environment.NewLine);
                    }
                    //context.Response.ContentType = "text/csv";
                    context.Response.AppendHeader("Content-Type", "application/vnd.ms-excel");
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + ParnerName + "_Unpaid-Premiums_" + ddlPeriod.SelectedItem.Text + "_" + ddlYear.SelectedItem.Text + ".csv"); //+ DateTime.Now.ToString("dd/MMM/yyyy HH:mm") 
                    //context.ApplicationInstance.CompleteRequest();
                    //context.Response.End();
                    // File.WriteAllText(@"C:\Codingvila.csv", sb.ToString());

                    //HttpContext.Current.Response.Write(Data);
                }
            }
            catch (Exception exc) { }
            finally
            {
                try
                {
                    //stop processing the script and return the current result
                    HttpContext.Current.Response.End();
                }
                catch (Exception ex) { }
                finally
                {
                    //Sends the response buffer
                    HttpContext.Current.Response.Flush();
                    // Prevents any other content from being sent to the browser
                    HttpContext.Current.Response.SuppressContent = true;
                    //Directs the thread to finish, bypassing additional processing
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //Suspends the current thread
                    System.Threading.Thread.Sleep(1);
                }
            }
        }

        private void GetNonPaymentReport(int iPartnerId)
        {
            try
            {

                rptNonPayment.DataSource = null;
                rptNonPayment.DataBind();

                P.Report_Provider frmF = new P.Report_Provider();
                DataSet ds = frmF.Get_Policy_NonPayment_By_Financier_By_Period(iPartnerId, Convert.ToInt32(ddlPeriod.SelectedValue), Convert.ToInt32(ddlYear.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    btnSendReport.Visible = true;
                    rptNonPayment.DataSource = ds.Tables[0];

                    rptNonPayment.DataBind();
                    rptNonPayment.Visible = true;
                    rptNonPayment.Visible = true;

                    P.Notification_Provider n = new P.Notification_Provider();

                    //n.Send_Financier_NonPayment_By_Period_By_Year(ds, "Investec", "January", "2021");

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