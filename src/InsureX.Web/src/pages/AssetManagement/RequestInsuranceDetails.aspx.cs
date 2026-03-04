using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using AT = IAPR_Data.Classes.AssetTypes;
using CP = IAPR_Data.Classes.Policy;
using CCom = IAPR_Data.Classes.Common;
using P = IAPR_Data.Providers;
using System.Configuration;
using U = IAPR_Data.Utils;

namespace IAPR_Web.AssetManagement
{
    public partial class RequestInsuranceDetails : System.Web.UI.Page
    {
        DataSet ds = null;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnCreatePolicy_Click(object sender, EventArgs e)
        {
            P.Generic_Asset_Provider Ap = new P.Generic_Asset_Provider();
            var dr = Ap.Get_AssetsAwaitingInsurance();
            while (dr.Read())
            {
                NotifyCustomer(Convert.ToInt32(dr["iAsset_Policy_Alignment_Id"].ToString()));
            }
        }

        private void NotifyCustomer(int alignmentId)
        {
            CCom.CurrentUser objUser = new CCom.CurrentUser();
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();


            P.Customer_Provider p = new P.Customer_Provider();
            DataSet ds = p.Get_Customer_Deatils_For_Alignment(alignmentId);
            string Kl = ds.Tables[0].Rows[0][4].ToString();
            string Ai = ds.Tables[0].Rows[0][2].ToString();
            string atype = ds.Tables[0].Rows[0][3].ToString();
            string PhI = ds.Tables[0].Rows[0][8].ToString();

            string link = ConfigurationManager.AppSettings["Application_URL"] + "/AssetToPolicy.aspx?Kl=" + Kl + "&Ai=" + Ai + "&atype=" + atype + "&PhI=" + PhI;

            string customerName = string.Empty;
            customerName = ds.Tables[0].Rows[0][7].ToString() == "1" ? ds.Tables[1].Rows[0][3].ToString() : ds.Tables[1].Rows[0][1].ToString();

            string customerEmail = string.Empty;
            customerEmail = ds.Tables[0].Rows[0][7].ToString() == "1" ? ds.Tables[1].Rows[0][11].ToString() : ds.Tables[1].Rows[0][9].ToString();
            P.Notification_Provider nP = new P.Notification_Provider();
            nP.Customer_Confirm_Policy_Details(customerName, customerEmail, objUser.vcPartner_Name, link, "CustomerConfirmPolicyDetails");

        }
    }
}