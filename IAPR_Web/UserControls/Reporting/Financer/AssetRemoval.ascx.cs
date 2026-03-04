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
    public partial class AssetRemoval : System.Web.UI.UserControl
    {
        CCom.CurrentUser objUser = new CCom.CurrentUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

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
                Getformfields();
            }
        }

        protected void btnShowAssetRemoval_Click(object sender, EventArgs e)
        {
            CCom.CurrentUser objUser = new CCom.CurrentUser();
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();

            if (objUser.iUser_Type_Id == 1 || objUser.iUser_Type_Id == 2)
            {
                GetAssetRemovalReport(Convert.ToInt32(ddlPartner.SelectedValue));
            }
            else
            {
                GetAssetRemovalReport(objUser.iPartner_Id);
            }
            lblPeriod.Text = ddlPeriod.SelectedItem.Text + " " + ddlYear.SelectedItem.Text;
            pnlAssetRemoval.Visible = true;
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
                
                
                P.Report_Provider frmF = new P.Report_Provider();
                DataSet ds = frmF.Get_Policy_NonPayment_By_Financier_By_Period(2, Convert.ToInt32(ddlPeriod.SelectedValue), Convert.ToInt32(ddlYear.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {

                    rptAssetRemoval.DataSource = ds.Tables[0];
                    rptAssetRemoval.DataBind();
                    rptAssetRemoval.Visible = true;
                    rptAssetRemoval.Visible = true;

                    P.Notification_Provider n = new P.Notification_Provider();

                    //n.Send_Financier_NonPayment_By_Period_By_Year(ds, "Investec", "January", "2021");

                }
               

            }
            catch (Exception)
            {

                throw;
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
        private void GetAssetRemovalReport(int iPartnerId)
        {
            try
            {

                rptAssetRemoval.DataSource = null;
                rptAssetRemoval.DataBind();

                P.Report_Provider frmF = new P.Report_Provider();
                DataSet ds = frmF.Get_Asset_Removal_By_Financier_By_Period(iPartnerId, Convert.ToInt32(ddlPeriod.SelectedValue), Convert.ToInt32(ddlYear.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    btnSendReport.Visible = true;
                    rptAssetRemoval.DataSource = ds.Tables[0];

                    rptAssetRemoval.DataBind();
                    rptAssetRemoval.Visible = true;
                    rptAssetRemoval.Visible = true;

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

        private void GetAllAssetDetails(int iAsset_Id, int iAsset_Type_Id)
        {
            P.Generic_Asset_Provider pro = new P.Generic_Asset_Provider();
            DataSet ds = pro.Get_Asset_All_Details_By_Asset_ID(iAsset_Type_Id, iAsset_Id);
            System.Text.StringBuilder s = new System.Text.StringBuilder();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                foreach (DataColumn c in ds.Tables[0].Columns)
                {
                    s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                }
            }
            divAssetDetails.InnerHtml = s.ToString();

            s.Clear();
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                foreach (DataColumn c in ds.Tables[1].Columns)
                {
                    if (c.ColumnName != "Policy status")
                    {
                        s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                    }
                    else
                    {
                        if (row[c].ToString() == "Active")
                        {
                            s.Append(c.ColumnName + ": <span style='color: Green; font-weight: bold;'>" + row[c] + "</span><br /><br />");
                        }
                        else
                        {
                            s.Append(c.ColumnName + ": <span style='color: Red;font-weight: bold;'>" + row[c] + "</span><br /><br />");
                        }

                    }
                }
            }
            divPolicyDetails.InnerHtml = s.ToString();
            s.Clear();
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                foreach (DataColumn c in ds.Tables[2].Columns)
                {
                    s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                }
            }

            divCustomerDeatils.InnerHtml = s.ToString();
            s.Clear();
            foreach (DataRow row in ds.Tables[3].Rows)
            {
                foreach (DataColumn c in ds.Tables[3].Columns)
                {
                    s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                }
            }

            divPhysicalAddress.InnerHtml = s.ToString();
            s.Clear();
            foreach (DataRow row in ds.Tables[4].Rows)
            {
                foreach (DataColumn c in ds.Tables[4].Columns)
                {
                    s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                }
            }
            divPostalAddress.InnerHtml = s.ToString();
            pnlAllDetails.Visible = true;
        }
        protected void rptAssetRemoval_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName == "ViewHistory")
                {
                    string[] param = e.CommandArgument.ToString().Split(new Char[] { ';' });

                    GetAllAssetDetails(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]));
                    //GetAssetRemovalAssetHistory(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]));
                    pnlAssetRemoval.Visible = false;
                    pnlAllDetails.Visible = true;
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlAssetRemoval.Visible = true;
            pnlAllDetails.Visible = false;
        }
    }
}