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

namespace IAPR_Web
{
    public partial class ProcessNewAssetsByAPI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RunProcess();
        }

        private void RunProcess()
        {
            try
            {

            
            P.Generic_Asset_Provider G = new P.Generic_Asset_Provider();
            DataSet gDs = G.Get_Unprocessed_NewAssetsByAPI();
            
            int alignmentId;
            string Partner;
            foreach (DataRow r in gDs.Tables[0].Rows)
            {
                alignmentId = G.Processed_NewAssetsByAPI(Convert.ToInt32(r[0].ToString()));
                Partner = U.CryptorEngine.GenericDecrypt(r[1].ToString(), true);
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
                nP.Customer_Confirm_Policy_Details(customerName, customerEmail, Partner, link, "CustomerConfirmPolicyDetails");
            }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}