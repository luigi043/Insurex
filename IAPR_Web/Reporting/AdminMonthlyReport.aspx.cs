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

namespace IAPR_Web.Reporting
{
    public partial class AdminMonthlyReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            CloseAll();
            switch (ddlReport.SelectedValue)
            {
                case "1":
                    NonPayment.StartFormLoad();
                    pnlNonpayment.Visible = true;

                    break;
                case "2":
                    ChangeInsuranceCover.StartFormLoad();
                    pnlChangeInsuranceCover.Visible = true;
                    break;
                case "3":
                    ChangeInsuranceValue.StartFormLoad();
                    pnlChangeInsuranceValue.Visible = true;
                    break;
                case "5":
                    AssetRemoval.StartFormLoad();
                    pnlAssetRemoval.Visible = true;
                    break;
                case "7":
                    Notifications.StartFormLoad();
                    pnlCustomerNotifications.Visible = true;
                    break;
            }
        }

        private void CloseAll()
        {
            pnlNonpayment.Visible = false;
            pnlChangeInsuranceCover.Visible = false;
            pnlChangeInsuranceValue.Visible = false;
            pnlAssetRemoval.Visible = false;
        }
    }
}