using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IAPR_Web.Reporting
{
    public partial class InsurerMonthlyReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            CloseAll();
            switch (ddlReport.SelectedValue)
            {
                case "1":
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