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

namespace IAPR_Web.UserControls.Reporting.Financer
{
    public partial class Notifications : System.Web.UI.UserControl
    {
        CCom.CurrentUser objUser = new CCom.CurrentUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["SortOrder"] = "ASC";
                ViewState["SortField"] = "vcFinance_Agrreement_Number";
                P.User_Provider uP = new P.User_Provider();

                objUser = uP.GetUserFromSession();
                if (objUser.iUser_Type_Id == 1 || objUser.iUser_Type_Id == 2)
                {
                    pnlPartner.Visible = true;
                    Getformfields();

                }
                else
                {
                    hdPartnerID.Value = IAPR_Data.Utils.CryptorEngine.GenericEncrypt(objUser.iPartner_Id.ToString(), true);

                }
                hdAssetTypeFilter.Value = "ALL";

            }
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

        private void GetCustomerNotifications(int iPartner_Id, int affectedPeriod, int affectedYear)
        {

            try
            {

                rptCustomerNotifications.DataSource = null;
                rptCustomerNotifications.DataBind();
                
                P.Report_Provider frmF = new P.Report_Provider();
                DataSet ds = frmF.Get_Asset_Comminications_Financer(iPartner_Id, affectedPeriod, affectedYear);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    btnSendReport.Visible = true;
                    rptCustomerNotifications.DataSource = ds.Tables[0];

                    rptCustomerNotifications.DataBind();
                    pnlCustomerNotifications.Visible = true;

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

        protected void ddlPartner_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlCustomerNotifications.Visible = false;
        }
        protected void btnSendReport_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {


            }
        }
        protected void btnShowCustomerNotifications_Click(object sender, EventArgs e)
        {
            CCom.CurrentUser objUser = new CCom.CurrentUser();
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();

            if (objUser.iUser_Type_Id == 1 || objUser.iUser_Type_Id == 2)
            {
                GetCustomerNotifications(Convert.ToInt32(ddlPartner.SelectedValue), Convert.ToInt32(ddlPeriod.SelectedValue), Convert.ToInt32(ddlYear.SelectedValue));
            }
            else
            {
                GetCustomerNotifications(objUser.iPartner_Id, Convert.ToInt32(ddlPeriod.SelectedValue), Convert.ToInt32(ddlYear.SelectedValue));
            }
            lblPeriod.Text = ddlPeriod.SelectedItem.Text + " " + ddlYear.SelectedItem.Text;

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
    }
}