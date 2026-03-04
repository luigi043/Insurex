using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using C = IAPR_Data.Classes;
using P = IAPR_Data.Providers;
using U = IAPR_Data.Utils;
using CCom = IAPR_Data.Classes.Common;
using System.Globalization;
using System.Web.Script.Serialization;

namespace IAPR_Web
{
    public partial class ServicePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [WebMethod]
        public static object getAdminTable()
        {
            //DataSet ds = null;
            //CCom.Dashboards.FinancerLandingTableTotals financerLandingTableTotals = new CCom.Dashboards.FinancerLandingTableTotals();
            //try
            //{
            //    P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            //    //List<CCom.Dashboards.FinancerLandingTableTotals> financerLandingTableTotals = new List<CCom.Dashboards.FinancerLandingTableTotals>();

            //    ds = frmF.Get_Admin_Landing_DashboardTable();
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        financerLandingTableTotals.iNumber_Of_Uninsued_Assets = Convert.ToInt32(ds.Tables[0].Rows[0]["iNumber_Of_Uninsued_Assets"].ToString());
            //        financerLandingTableTotals.dcUninsured_Finance_Value = Convert.ToInt32(ds.Tables[0].Rows[0]["dcUninsured_Finance_Value"].ToString());
            //        financerLandingTableTotals.dcUninsured_Insurance_Value = Convert.ToInt32(ds.Tables[0].Rows[0]["dcUninsured_Insurance_Value"].ToString());


            //    }
            //    else
            //    {
            //        financerLandingTableTotals.iNumber_Of_Uninsued_Assets = 0;
            //        financerLandingTableTotals.dcUninsured_Finance_Value = 0;
            //        financerLandingTableTotals.dcUninsured_Insurance_Value = 0;


            //    }
            //}
            //catch (Exception ex)
            //{

            //    U.ErrorLogger eL = new U.ErrorLogger();
            //    eL.LogErrorInDB(ex, "ServicePage", "GetAdminDashboardTable");


            //}
            //JavaScriptSerializer JSS = new JavaScriptSerializer();
            //var json = JSS.Serialize(financerLandingTableTotals);
            return "test";// json;


        }
    }
}