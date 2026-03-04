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
using U = IAPR_Data.Utils;
using CCom = IAPR_Data.Classes.Common;
using System.Globalization;
using SysCH = System.Web.UI.DataVisualization.Charting;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using FusionCharts.Charts;

using FusionCharts.Charts;


namespace IAPR_Web
{
    public partial class AdminHome : System.Web.UI.Page
    {
        DataSet ds = null;
        string tbHeaderMonthsFinanceValueEx, tbNonPaymentReInstatedByMonthFinanceValueEx, tbHeaderMonthsAssetCountEx, tbNonPaymentReInstatedByMonthAssetCountEX = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                P.User_Provider uP = new P.User_Provider();
                CCom.CurrentUser objUser = new CCom.CurrentUser();
                CCom.Dashboards.FinancerLandingTableTotals financerLandingTableTotals = new CCom.Dashboards.FinancerLandingTableTotals();

                objUser = uP.GetUserFromSession();

                if (objUser == null)
                {
                    Response.Redirect("/account/login.aspx", false);
                }
                else
                {
                    GetAdminDashboardTable();
                    financerLandingTableTotals = GetAdminDashboardTable();
                    GetAdminDashboardCharts(financerLandingTableTotals);

                }
            }
        }
        private CCom.Dashboards.FinancerLandingTableTotals GetAdminDashboardTable()
        {
            try
            {
                CCom.Dashboards.FinancerLandingTableTotals financerLandingTableTotals = new CCom.Dashboards.FinancerLandingTableTotals();

                financerLandingTableTotals = GetTotals();

                UninsuredAssetsCount(financerLandingTableTotals.PremiumUnpaidAssetCount, financerLandingTableTotals.NoInsuranceAssetCount);


                //lblPremiumUnpaidAssetCount.Text = financerLandingTableTotals.PremiumUnpaidAssetCount.ToString();

                //lblPremiumUnpaidAssetCountPercent.Text = lcPremiumUnpaidAssetCountPercent.ToString() + "%";

                lblPremiumUnpaidAssetTotal.Text = financerLandingTableTotals.PremiumUnpaidAssetTotal.ToString("C", new CultureInfo("en-ZA"));
                lblPremiumUnpaidAssetTotalPercent.Text = financerLandingTableTotals.PremiumUnpaidAssetTotalPercent.ToString() + "%";

                //lblNoInsuranceAssetCount.Text = financerLandingTableTotals.NoInsuranceAssetCount.ToString();

                //lblNoInsuranceAssetCountPercent.Text = lcNoInsuranceAssetCountPercent.ToString() + "%";

                lblNoInsuranceAssetTotal.Text = financerLandingTableTotals.NoInsuranceAssetTotal.ToString("C", new CultureInfo("en-ZA"));


                lblNoInsuranceAssetTotalPercent.Text = financerLandingTableTotals.NoInsuranceAssetTotalPercent.ToString() + "%";

                lblUninsuredTotalAssetCount.Text = financerLandingTableTotals.AllUninsuredCount.ToString();//(financerLandingTableTotals.PremiumUnpaidAssetCount + financerLandingTableTotals.NoInsuranceAssetCount).ToString();
                //lblUninsuredTotalAssetCountPercent.Text = (lcPremiumUnpaidAssetCountPercent + lcNoInsuranceAssetCountPercent).ToString() + "%";

                lblUninsuredTotalAssetTotal.Text = lblUninsuredTotalAssetTotal2.Text = (financerLandingTableTotals.PremiumUnpaidAssetTotal + financerLandingTableTotals.NoInsuranceAssetTotal).ToString("C", new CultureInfo("en-ZA"));
                //lblUninsuredTotalAssetTotalPercent.Text = (lcPremiumUnpaidAssetTotalPercent + lcNoInsuranceAssetTotalPercent).ToString() + "%";


                lblAllAssetCount.Text = financerLandingTableTotals.AllAssetCount.ToString();
                lblAllAssetTotal.Text = financerLandingTableTotals.AllAssetTotal.ToString("C", new CultureInfo("en-ZA"));

                lblInsuredAssetCount.Text = financerLandingTableTotals.InsuredAssetCount.ToString();
                lblInsuredAssetTotal.Text = financerLandingTableTotals.InsuredTotal.ToString("C", new CultureInfo("en-ZA"));
                lblInsuredShortFall.Text = (financerLandingTableTotals.AllAssetTotal - financerLandingTableTotals.InsuredTotal).ToString("C", new CultureInfo("en-ZA"));


                lblAdequatelyInsuredTotal.Text = financerLandingTableTotals.InsuredAdequatelyTotal.ToString("C", new CultureInfo("en-ZA"));
                lblAdequatelyInsuredTotalPercent.Text = financerLandingTableTotals.InsuredAdequatelyTotalPercent.ToString() + "%";

                lblUnderInsuredTotal.Text = financerLandingTableTotals.InsuredUnderInsuredTotal.ToString("C", new CultureInfo("en-ZA"));
                lblUnderInsuredTotalPercent.Text = financerLandingTableTotals.InsuredUnderInsuredTotalPercent.ToString() + "%";

                return financerLandingTableTotals;

            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "FinancerHome.aspx", "GetFinancerDashboardTable");
                return null;
            }

        }
        private CCom.Dashboards.FinancerLandingTableTotals GetTotals()
        {


            CCom.Dashboards.FinancerLandingTableTotals financerLandingTableTotals = new CCom.Dashboards.FinancerLandingTableTotals();
            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_DashboardTable();

            financerLandingTableTotals.PremiumUnpaidAssetCount = Convert.ToInt32(ds.Tables[0].Rows[0]["iNumber_Of_Assets"].ToString());
            financerLandingTableTotals.PremiumUnpaidAssetTotal = Convert.ToDecimal(ds.Tables[0].Rows[0]["dcUninsured_Finance_Value"].ToString());

            financerLandingTableTotals.NoInsuranceAssetCount = Convert.ToInt32(ds.Tables[1].Rows[0]["iNumber_Of_Assets"].ToString());
            financerLandingTableTotals.NoInsuranceAssetTotal = Convert.ToDecimal(ds.Tables[1].Rows[0]["dcUninsured_Finance_Value"].ToString());


            financerLandingTableTotals.AllAssetCount = Convert.ToInt32(ds.Tables[2].Rows[0]["iNumber_Of_Assets"].ToString());
            financerLandingTableTotals.AllAssetTotal = Convert.ToDecimal(ds.Tables[2].Rows[0]["dcFinance_Value"].ToString());


            financerLandingTableTotals.AllUninsuredCount = Convert.ToInt32(ds.Tables[0].Rows[0]["iNumber_Of_Assets"]) + Convert.ToInt32(ds.Tables[1].Rows[0]["iNumber_Of_Assets"]);
            //financerLandingTableTotals.AllUninsuredCountPercent
            financerLandingTableTotals.AllUninsuredTotal = Convert.ToDecimal(ds.Tables[0].Rows[0]["dcUninsured_Finance_Value"]) + Convert.ToDecimal(ds.Tables[1].Rows[0]["dcUninsured_Finance_Value"]);
            //  financerLandingTableTotals.AllUninSuredTotalPercent

            financerLandingTableTotals.InsuredTotal = Convert.ToDecimal(ds.Tables[3].Rows[0]["dcFinance_Value"].ToString());
            financerLandingTableTotals.InsuredAssetCount = Convert.ToInt32(ds.Tables[3].Rows[0]["iNumber_Of_Assets"].ToString());

            financerLandingTableTotals.InsuredAdequatelyTotal = Convert.ToDecimal(ds.Tables[4].Rows[0]["dcFinance_Value"].ToString());
            financerLandingTableTotals.InsuredAdequatelyAssetCount = Convert.ToInt32(ds.Tables[4].Rows[0]["iNumber_Of_Assets"].ToString());

            financerLandingTableTotals.InsuredUnderInsuredTotal = Convert.ToDecimal(ds.Tables[5].Rows[0]["dcFinance_Value"].ToString());
            financerLandingTableTotals.InsuredUnderInsuredAssetCount = Convert.ToInt32(ds.Tables[5].Rows[0]["iNumber_Of_Assets"].ToString());


            financerLandingTableTotals.PremiumUnpaidAssetCountPercent = financerLandingTableTotals.PremiumUnpaidAssetCount > 0 ? Convert.ToInt32((decimal.Divide(financerLandingTableTotals.PremiumUnpaidAssetCount, financerLandingTableTotals.AllAssetCount) * 100)) : 0;
            financerLandingTableTotals.PremiumUnpaidAssetTotalPercent = financerLandingTableTotals.PremiumUnpaidAssetTotal > 0 ? Convert.ToInt32((decimal.Divide(financerLandingTableTotals.PremiumUnpaidAssetTotal, financerLandingTableTotals.AllUninsuredTotal) * 100)) : 0;
            financerLandingTableTotals.NoInsuranceAssetTotalPercent = financerLandingTableTotals.NoInsuranceAssetTotal > 0 ? Convert.ToInt32((decimal.Divide(financerLandingTableTotals.NoInsuranceAssetTotal, financerLandingTableTotals.AllUninsuredTotal) * 100)) : 0;


            financerLandingTableTotals.InsuredAdequatelyTotalPercent = financerLandingTableTotals.InsuredAdequatelyTotal > 0 ? Convert.ToInt32((decimal.Divide(financerLandingTableTotals.InsuredAdequatelyTotal, financerLandingTableTotals.InsuredTotal) * 100)) : 0;
            financerLandingTableTotals.InsuredUnderInsuredTotalPercent = financerLandingTableTotals.InsuredUnderInsuredTotal > 0 ? Convert.ToInt32((decimal.Divide(financerLandingTableTotals.InsuredUnderInsuredTotal, financerLandingTableTotals.InsuredTotal) * 100)) : 0;

            return financerLandingTableTotals;
        }
        private void GetAdminDashboardCharts(CCom.Dashboards.FinancerLandingTableTotals financerLandingTableTotals)
        {
            try
            {
                InsuranceStatusChart(financerLandingTableTotals.AllAssetCount);
                UninsuredAgeAnaylysis();
                UninsuredByFinancerChart();
                NonPaymentReInstatedByMonthChartFinanceValue();
                NonPaymentReInstatedByMonthChartAssetCount();
                
                InsuredAssets(financerLandingTableTotals.InsuredAdequatelyAssetCount, financerLandingTableTotals.InsuredUnderInsuredAssetCount);
                Communications_Current_MonthChart();
                UninsuredByInsurer();
                //GetAdminDashboard_NonPaymentPreviousYear();
                //UninsuresByAssetTypeChart(ds.Tables[3]);


            }
            catch (Exception ex)
            {

                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AdminHome", "GetAdminDashboardCharts");
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastError", "toastError('An error occured, could not retrieve dashboard data!');", true);

            }
        }

        #region OnlineCharts
        private void UninsuredAssetsCount(int unpaid, int noInsurance)
        {
            StringBuilder jsonData = new StringBuilder();
            StringBuilder data = new StringBuilder();
            Dictionary<string, string> chartConfig = new Dictionary<string, string>();

            chartConfig.Add("caption", "Uninsured Assets");
            //chartConfig.Add("subcaption", "All ASSETS");
            chartConfig.Add("xAxisName", "Unpaid Premiums");
            chartConfig.Add("yAxisName", "No Insurance Details");

            chartConfig.Add("captionFont", "sans - serif");
            chartConfig.Add("captionFontColor", "#4e73df");

            chartConfig.Add("showLabels", "0");
            chartConfig.Add("showValues", "0");
            chartConfig.Add("labelFont", "Arial");
            chartConfig.Add("labelFontColor", "0075c2");
            chartConfig.Add("labelFontBold", "1");
            chartConfig.Add("lableFontItalic", "1");
            chartConfig.Add("labelAlpha", "70");

            chartConfig.Add("showpercentvalues", "1");
            chartConfig.Add("defaultcenterlabel", "Total Uninsured Assets: <b>" + (unpaid + noInsurance).ToString() + "</b>");
            chartConfig.Add("centerlabel", "<b>$label: $value</b>");
            chartConfig.Add("plottooltext", "<b>$label</b>: <b>$percentValue</b> ");
            chartConfig.Add("smartLineThickness", "0.8");
            chartConfig.Add("doughnutRadius", "90");
            chartConfig.Add("legendposition", "bottom");
            chartConfig.Add("theme", "fusion");
            ////chartConfig.Add("exportEnabled", "1");
            chartConfig.Add("exportFileName", "_UNINSURED ASSETS_" + String.Format("fileName_{0:yyyy_MM_dd}", DateTime.Now));
            jsonData.Append("{'chart':{");
            foreach (var config in chartConfig)
            {
                jsonData.AppendFormat("'{0}':'{1}',", config.Key, config.Value);
            }
            jsonData.Replace(",", "},", jsonData.Length - 1, 1);
            data.Append("'data':[");


            data.AppendFormat("{{'label':'{0}','value':'{1}'}},", "Unpaid Premiums", unpaid.ToString());
            data.AppendFormat("{{'label':'{0}','value':'{1}'}},", "No Insurance Details", noInsurance.ToString());


            data.Replace(",", "]", data.Length - 1, 1);

            jsonData.Append(data.ToString());
            jsonData.Append("}");


            Chart doughnut2d = new Chart("doughnut2d", "chartUninsuredAssetsCount", "100%", "400", "json", jsonData.ToString());

            chartUninsuredAssetsCount.Text = doughnut2d.Render();


        }
        private void InsuranceStatusChart(int allAssetsCount)
        {
            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_Dashboard_ArrearVsUnconfirmed();
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {

                StringBuilder jsonData = new StringBuilder();
                StringBuilder data = new StringBuilder();
                Dictionary<string, string> chartConfig = new Dictionary<string, string>();

                chartConfig.Add("caption", "Insurance Status");
                chartConfig.Add("subcaption", "All Assets");
                chartConfig.Add("xAxisName", "Unpaid premiums");
                chartConfig.Add("yAxisName", "Unconfirmed cover");
                chartConfig.Add("captionFont", "sans - serif");
                chartConfig.Add("captionFontColor", "#4e73df");

                chartConfig.Add("showLabels", "0");
                chartConfig.Add("showValues", "0");
                chartConfig.Add("labelFont", "Arial");
                chartConfig.Add("labelFontColor", "0075c2");
                chartConfig.Add("labelFontBold", "1");
                chartConfig.Add("lableFontItalic", "1");
                chartConfig.Add("labelAlpha", "70");

                chartConfig.Add("showpercentvalues", "1");
                chartConfig.Add("defaultcenterlabel", "Total Assets: <b>" + allAssetsCount.ToString() + "</b>");
                chartConfig.Add("centerlabel", "<b>$label: $value</b>");
                chartConfig.Add("plottooltext", "<b>$label</b>: <b>$percentValue</b> ");
                chartConfig.Add("smartLineThickness", "0.8");
                chartConfig.Add("doughnutRadius", "75");

                //chartConfig.Add("labelFont", "Arial");
                //chartConfig.Add("labelFontColor", "0075c2");
                //chartConfig.Add("labelFontBold", "1");
                //chartConfig.Add("lableFontItalic", "1");
                //chartConfig.Add("labelAlpha", "70");

                chartConfig.Add("legendposition", "bottom");

                chartConfig.Add("theme", "fusion");
                //chartConfig.Add("exportEnabled", "1");
                chartConfig.Add("exportFileName", "_INSURANCE_STATUS_" + String.Format("{0:yyyy_MM_dd}", DateTime.Now));
                jsonData.Append("{'chart':{");
                foreach (var config in chartConfig)
                {
                    jsonData.AppendFormat("'{0}':'{1}',", config.Key, config.Value);
                }
                jsonData.Replace(",", "},", jsonData.Length - 1, 1);
                data.Append("'data':[");

                //iterate through data table to build data object
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        data.AppendFormat("{{'label':'{0}','value':'{1}'}},", row[0].ToString(), row[1].ToString());
                    }
                }
                data.Replace(",", "]", data.Length - 1, 1);

                jsonData.Append(data.ToString());
                jsonData.Append("}");


                Chart doughnut2d = new Chart("doughnut2d", "UnconfirmedVsUnpaid", "100%", "400", "json", jsonData.ToString());

                chartInsuraceStatus.Text = doughnut2d.Render();
            }
            else
            {
                //divchartInsuraceStatus.Attributes.Add("style", "display:none;");
            }
        }
        private void InsuredAssets(int adequatelyInsured, int underInsured)
        {
            StringBuilder jsonData = new StringBuilder();
            StringBuilder data = new StringBuilder();
            Dictionary<string, string> chartConfig = new Dictionary<string, string>();

            chartConfig.Add("caption", "Insured Assets");
            //chartConfig.Add("subcaption", "All ASSETS");
            chartConfig.Add("xAxisName", "Adequately Insured");
            chartConfig.Add("yAxisName", "Under Insured");

            chartConfig.Add("captionFont", "sans - serif");
            chartConfig.Add("captionFontColor", "#4e73df");

            chartConfig.Add("showLabels", "0");
            chartConfig.Add("showValues", "0");
            chartConfig.Add("labelFont", "Arial");
            chartConfig.Add("labelFontColor", "0075c2");
            chartConfig.Add("labelFontBold", "1");
            chartConfig.Add("lableFontItalic", "1");
            chartConfig.Add("labelAlpha", "70");

            chartConfig.Add("showpercentvalues", "1");
            chartConfig.Add("defaultcenterlabel", "Total Insured Assets: <b>" + (adequatelyInsured + underInsured).ToString() + "</b>");
            chartConfig.Add("centerlabel", "<b>$label: $value</b>");
            chartConfig.Add("plottooltext", "<b>$label</b>: <b>$percentValue</b> ");
            chartConfig.Add("smartLineThickness", "0.8");
            chartConfig.Add("doughnutRadius", "90");
            chartConfig.Add("legendposition", "bottom");
            chartConfig.Add("theme", "fusion");
            //chartConfig.Add("exportEnabled", "1");
            chartConfig.Add("exportFileName", "_INSURED ASSETS_" + String.Format("fileName_{0:yyyy_MM_dd}", DateTime.Now));
            jsonData.Append("{'chart':{");
            foreach (var config in chartConfig)
            {
                jsonData.AppendFormat("'{0}':'{1}',", config.Key, config.Value);
            }
            jsonData.Replace(",", "},", jsonData.Length - 1, 1);
            data.Append("'data':[");


            data.AppendFormat("{{'label':'{0}','value':'{1}'}},", "Adequately Insured", adequatelyInsured.ToString());
            data.AppendFormat("{{'label':'{0}','value':'{1}'}},", "Under Insured", underInsured.ToString());


            data.Replace(",", "]", data.Length - 1, 1);

            jsonData.Append(data.ToString());
            jsonData.Append("}");


            Chart doughnut2d = new Chart("doughnut2d", "chartInsuredAssets", "100%", "400", "json", jsonData.ToString());

            chartInsuredAssets.Text = doughnut2d.Render();


        }
        private void UninsuredByFinancerChart()
        {
            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_Dashboard_UninsuredByFinancer();
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {


                StringBuilder jsonData = new StringBuilder();
                StringBuilder data = new StringBuilder();
                Dictionary<string, string> chartConfig = new Dictionary<string, string>();

                chartConfig.Add("caption", "Uninsured Assets");
                chartConfig.Add("subcaption", "By Lender");
                chartConfig.Add("yaxisname", "Number of assets");
                chartConfig.Add("numvisibleplot", "10");
                chartConfig.Add("exportEnabled", "1");
                chartConfig.Add("exportFileName", "UNINSURED_BY_LENDER_AS_AT_" + DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture) + "_" + String.Format("fileName_{0:yyyy_MM_dd}", DateTime.Now));

                chartConfig.Add("theme", "fusion");

                //chartConfig.Add("bgColor", "#DDDDDD");
                //chartConfig.Add("yAxisValueFontColor ", "#FFF");
                //chartConfig.Add("xAxisNameBgColor", "#4e73df");


                // json data to use as chart data source
                jsonData.Append("{'chart':{");
                foreach (var config in chartConfig)
                {
                    jsonData.AppendFormat("'{0}':'{1}',", config.Key, config.Value);
                }
                jsonData.Replace(",", "},", jsonData.Length - 1, 1);
                data.Append("'categories':[{");
                data.Append("'category':[");

                //iterate through data table to build data object
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<string> financers = (from p in dt.AsEnumerable()
                                              select p.Field<string>("Financer")).Distinct().ToList();
                    List<string> statuses = (from p in dt.AsEnumerable()
                                             select p.Field<string>("Policy_Status")).Distinct().ToList();


                    foreach (string l in financers)
                    {

                        data.Append("{'label': '" + l + "'},");

                    }
                    data.Replace(",", "", data.Length - 1, 1);
                    data.Append(" ]    }  ],");

                    data.Append("'dataset':[");
                    //foreach (string l in financers)
                    //{

                    foreach (string s in statuses)
                    {
                        data.Append("{'seriesname':'" + s + "',");
                        data.Append("'data':[");
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["Policy_Status"].ToString() == s)//row["Financer"].ToString() == l &&
                            {

                                data.Append("{'value': '" + row["iNumber_Of_Assets"].ToString() + "'},");
                            }
                            else
                            {
                                data.Append("{'value': '0'},");
                            }
                        }
                        data.Replace(",", "", data.Length - 1, 1);
                        data.Append("]},");
                        // }
                    }
                    data.Replace(",", "", data.Length - 1, 1);
                    data.Append("]}");


                }
                // data.Replace(",", "]", data.Length - 1, 1);

                jsonData.Append(data.ToString());


                Chart scrollColumn = new Chart("mscolumn2d", "UncoveredByFinancer", "100%", "400", "json", jsonData.ToString());

                chartUninsuredByFinancer.Text = scrollColumn.Render();
            }
            else
            {
                divchartUninsuredByFinancer.Attributes.Add("style", "display:none;");

            }
        }
        private void UninsuredAgeAnaylysis()
        {
            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_Dashboard_Uninsured_Statistics();
            int month = DateTime.Now.Month;
            string name = DateTime.Now.ToString("MMM");



            if (ds.Tables[0].Rows.Count > 0)
            {


                StringBuilder jsonData = new StringBuilder();
                StringBuilder data = new StringBuilder();
                Dictionary<string, string> chartConfig = new Dictionary<string, string>();

                chartConfig.Add("caption", "Unpaid Premiums");
                chartConfig.Add("captionFont", "sans - serif");
                chartConfig.Add("captionFontColor", "#4e73df");

                chartConfig.Add("subCaption", "AGE ANALYSIS"); //+ DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture).ToUpper() + " " + DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture) + "\",");
                chartConfig.Add("yaxisname", "Number of assets");
                chartConfig.Add("syaxisname", "Financed value");
                chartConfig.Add("labeldisplay", "AUTO");
                chartConfig.Add("snumberprefix", "R");
                chartConfig.Add("numvisibleplot", "7");
                chartConfig.Add("theme", "fusion");
                //chartConfig.Add("exportEnabled", "1");
                chartConfig.Add("exportFileName", "_UNPAID_PREMIUM_AGE_ANALYSIS_" + DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture) + "_" + String.Format("fileName_{0:yyyy_MM_dd}", DateTime.Now));

                jsonData.Append("{'chart':{");
                foreach (var config in chartConfig)
                {
                    jsonData.AppendFormat("'{0}':'{1}',", config.Key, config.Value);
                }
                jsonData.Replace(",", "},", jsonData.Length - 1, 1);
                data.Append("'categories':[{");
                data.Append("'category':[");


                List<string> categories = new List<string>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    categories.Add(row["DayCount"].ToString());
                    data.Append("{'label': '" + row["DayCount"].ToString() + "'},");

                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append(" ]    }  ],");

                data.Append("'dataset':[");

                data.Append("{'seriesname':'Number of uninsured assets',");
                data.Append("'plottooltext': 'Number of uninsured assets: $dataValue',");
                data.Append("'data':[");
                foreach (string cat in categories)
                {
                    bool found = false;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["DayCount"].ToString() == cat)
                        {

                            data.Append("{'value': '" + row["AssetCount"].ToString() + "'},");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{'value': '0'},");
                    }
                }
                data.Append("]},");


                data.Append("{'seriesname':'Financed value',");
                data.Append("'parentyaxis': 'S',");
                data.Append("'renderas': 'spline',");
                data.Append("'plottooltext': 'Financed value: $dataValue',");
                data.Append("'showanchors': '1',");
                data.Append("'showvalues': '0',");
                data.Append("'data':[");
                foreach (string pmR in categories)
                {
                    bool found = false;

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["DayCount"].ToString() == pmR)
                        {
                            data.Append("{'value': '" + row["FinancedValue"].ToString() + "'},");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{'value': '0'},");
                    }
                    // }
                }

                data.Append("]},");
                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]}");
                // data.Replace(",", "]", data.Length - 1, 1);

                jsonData.Append(data.ToString());


                Chart scrollcombidy2d = new Chart("scrollcombidy2d", "chartUninsuredStatistics", "100%", "400", "json", jsonData.ToString());

                chartUninsuredStatistics.Text = scrollcombidy2d.Render();
            }
            else
            {
                divchartUninsuredStatistics.Attributes.Add("style", "display:none;");
            }
        }
        private void NonPaymentReInstatedByMonthChartFinanceValue()
        {
            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_Dashboard_NonPayment_History();
            int month = DateTime.Now.Month;
            string name = DateTime.Now.ToString("MMM");
            StringBuilder tableHearders = new StringBuilder();
            StringBuilder graphTable = new StringBuilder();


            if (ds.Tables[0].Rows.Count > 0)
            {


                StringBuilder jsonData = new StringBuilder();
                StringBuilder data = new StringBuilder();
                Dictionary<string, string> chartConfig = new Dictionary<string, string>();

                chartConfig.Add("caption", "Premiums Unpaid vs Re-instated Cover: Financed Value");
                chartConfig.Add("subcaption", "Last Twelve Months");
                chartConfig.Add("yaxisname", "Financed value");
                chartConfig.Add("captionFont", "sans - serif");
                chartConfig.Add("captionFontColor", "#4e73df");

                chartConfig.Add("labeldisplay", "AUTO");
                chartConfig.Add("snumberprefix", "R");
                chartConfig.Add("numvisibleplot", "12");
                chartConfig.Add("theme", "fusion");
                //chartConfig.Add("exportEnabled", "1");
                chartConfig.Add("exportFileName", "PREMIUMS_UNPAID_vs_RE_INSTATED_COVER_FINANCED VALUE_" + String.Format("fileName_{0:yyyy_MM_dd}", DateTime.Now));

                jsonData.Append("{'chart':{");
                foreach (var config in chartConfig)
                {
                    jsonData.AppendFormat("'{0}':'{1}',", config.Key, config.Value);
                }
                jsonData.Replace(",", "},", jsonData.Length - 1, 1);
                data.Append("'categories':[{");
                data.Append("'category':[");


                List<string> previousMonths = new List<string>();

                int i = 0;
                for (int j = 1; j < 13; j++)
                {
                    previousMonths.Add(DateTime.Now.AddMonths(i).ToString("MMM"));
                    data.Append("{'label': '" + DateTime.Now.AddMonths(i).ToString("MMM") + "'},");
                    tableHearders.Append("<th>" + DateTime.Now.AddMonths(i).ToString("MMM") + "</th>");
                    i = i - 1;
                }



                tbHeaderMonthsFinanceValue.InnerHtml = tableHearders.ToString();


                graphTable.Append("<tr>");
                graphTable.Append("<td>Uninsured Value</td>");

                data.Replace(",", "", data.Length - 1, 1);
                data.Append(" ]    }  ],");

                data.Append("'dataset':[");

                data.Append("{'seriesname':'Uninsured Value',");
                data.Append("'plottooltext': 'Uninsured Value: $dataValue',");
                data.Append("'data':[");
                foreach (string mnth in previousMonths)
                {
                    bool found = false;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["Month"].ToString() == mnth)
                        {

                            data.Append("{'value': '" + row["Uninsured Value"].ToString() + "'},");
                            graphTable.Append("<td>R" + CCom.Common.ConvertToMillion(Convert.ToDecimal(row["Uninsured Value"].ToString())).ToString("0.##") + "M</td>");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{'value': '0'},");
                        graphTable.Append("<td>0</td>");
                    }
                }
                data.Append("]},");


                graphTable.Append("</tr>");

                graphTable.Append("<tr>");
                graphTable.Append("<td>Re-Instated Value</td>");
                data.Append("{'seriesname':'Re-Instated Value',");
                //data.Append("'parentyaxis': 'S',");
                data.Append("'renderas': 'spline',");
                data.Append("'plottooltext': 'Re-Instated Value: $dataValue',");
                data.Append("'showanchors': '1',");
                data.Append("'showvalues': '0',");
                data.Append("'data':[");
                foreach (string mnth in previousMonths)
                {
                    bool found = false;

                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        if (row["Month"].ToString() == mnth)
                        {
                            data.Append("{'value': '" + row["Re-Instated Value"].ToString() + "'},");
                            graphTable.Append("<td>R" + CCom.Common.ConvertToMillion(Convert.ToDecimal(row["Re-Instated Value"].ToString())).ToString("0.##") + "M</td>");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{'value': '0'},");
                        graphTable.Append("<td>0</td>");
                    }
                    // }
                }

                data.Append("]},");
                data.Append("]}");
                graphTable.Append("</tr>");

                jsonData.Append(data.ToString());
                tbNonPaymentReInstatedByMonthFinanceValue.InnerHtml = graphTable.ToString();

                Chart scrollcombidy2d = new Chart("msspline", "chartNonPaymentReInstatedByMonthFinanceValue", "100%", "400", "json", jsonData.ToString());

                chartNonPaymentReInstatedByMonthFinanceValue.Text = scrollcombidy2d.Render();
            }
            else
            {
                divchartNonPaymentReInstatedByMonthFinanceValue.Attributes.Add("style", "display:none;");
            }
        }

        private void NonPaymentReInstatedByMonthChartAssetCount()
        {
            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_Dashboard_NonPayment_History();
            int month = DateTime.Now.Month;
            string name = DateTime.Now.ToString("MMM");
            StringBuilder tableHearders = new StringBuilder();
            StringBuilder graphTable = new StringBuilder();


            if (ds.Tables[0].Rows.Count > 0)
            {


                StringBuilder jsonData = new StringBuilder();
                StringBuilder data = new StringBuilder();
                Dictionary<string, string> chartConfig = new Dictionary<string, string>();

                chartConfig.Add("caption", "Premiums Unpaid vs Re-instated Cover: Asset Count");
                chartConfig.Add("subcaption", "Last Twelve Months");
                chartConfig.Add("yaxisname", "Number of assets");
                chartConfig.Add("captionFont", "sans - serif");
                chartConfig.Add("captionFontColor", "#4e73df");

                chartConfig.Add("labeldisplay", "AUTO");

                chartConfig.Add("numvisibleplot", "12");
                chartConfig.Add("theme", "fusion");
                //chartConfig.Add("exportEnabled", "1");
                chartConfig.Add("exportFileName", "PREMIUMS_UNPAID_vs_RE_INSTATED_COVER_ASSET_COUNT_" + String.Format("fileName_{0:yyyy_MM_dd}", DateTime.Now));

                jsonData.Append("{'chart':{");
                foreach (var config in chartConfig)
                {
                    jsonData.AppendFormat("'{0}':'{1}',", config.Key, config.Value);
                }
                jsonData.Replace(",", "},", jsonData.Length - 1, 1);
                data.Append("'categories':[{");
                data.Append("'category':[");


                List<string> previousMonths = new List<string>();

                int i = 0;
                for (int j = 1; j < 13; j++)
                {
                    previousMonths.Add(DateTime.Now.AddMonths(i).ToString("MMM"));
                    data.Append("{'label': '" + DateTime.Now.AddMonths(i).ToString("MMM") + "'},");
                    tableHearders.Append("<th>" + DateTime.Now.AddMonths(i).ToString("MMM") + "</th>");
                    i = i - 1;
                }

                tbHeaderMonthsAssetCount.InnerHtml = tableHearders.ToString();
                graphTable.Append("<tr>");
                graphTable.Append("<td>Uninsured Assets</td>");


                data.Replace(",", "", data.Length - 1, 1);
                data.Append(" ]    }  ],");

                data.Append("'dataset':[");

                data.Append("{'seriesname':'Number of uninsured assets',");
                data.Append("'plottooltext': 'Number of uninsured assets: $dataValue',");
                data.Append("'data':[");
                foreach (string mnth in previousMonths)
                {
                    bool found = false;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["Month"].ToString() == mnth)
                        {

                            data.Append("{'value': '" + row["Unpaid Premiums"].ToString() + "'},");
                            graphTable.Append("<td>" + row["Unpaid Premiums"].ToString() + "</td>");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{'value': '0'},");
                        graphTable.Append("<td>0</td>");
                    }
                }
                data.Append("]},");

                graphTable.Append("</tr>");

                graphTable.Append("<tr>");
                graphTable.Append("<td>Re-Instated Assets</td>");

                data.Append("{'seriesname':'Number of re-instated Assets',");
                //data.Append("'parentyaxis': 'S',");
                data.Append("'renderas': 'spline',");
                data.Append("'plottooltext': 'Number of re-instated Assets: $dataValue',");
                data.Append("'showanchors': '1',");
                data.Append("'showvalues': '0',");
                data.Append("'data':[");
                foreach (string mnth in previousMonths)
                {
                    bool found = false;

                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        if (row["Month"].ToString() == mnth)
                        {
                            data.Append("{'value': '" + row["Re-Instated Assets"].ToString() + "'},");
                            graphTable.Append("<td>" + row["Re-Instated Assets"].ToString() + "</td>");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{'value': '0'},");
                        graphTable.Append("<td>0</td>");
                    }

                }

                data.Append("]},");
                data.Append("]}");

                graphTable.Append("</tr>");
                tbNonPaymentReInstatedByMonthAssetCount.InnerHtml = graphTable.ToString();

                jsonData.Append(data.ToString());


                Chart scrollcombidy2d = new Chart("msline", "chartNonPaymentReInstatedByMonthAssetCount", "100%", "400", "json", jsonData.ToString());

                chartNonPaymentReInstatedByMonthAssetCount.Text = scrollcombidy2d.Render();
            }
            else
            {
                divchartNonPaymentReInstatedByMonthAssetCount.Attributes.Add("style", "display:none;");
            }
        }

        private void Communications_Current_MonthChart()
        {
            DataSet dsReIns = new DataSet();

            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_Dashboard_Communications_History();
            dsReIns = frmF.Get_Admin_Landing_Dashboard_NonPayment_History();
            int month = DateTime.Now.Month;
            string name = DateTime.Now.ToString("MMM");



            if (ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder jsonData = new StringBuilder();
                StringBuilder data = new StringBuilder();
                Dictionary<string, string> chartConfig = new Dictionary<string, string>();

                chartConfig.Add("caption", "Customer Notifications");
                chartConfig.Add("subcaption", "Last Twelve Months");
                chartConfig.Add("yaxisname", "Notifications");
                chartConfig.Add("captionFont", "sans - serif");
                chartConfig.Add("captionFontColor", "#4e73df");
                chartConfig.Add("showsum", "1");

                chartConfig.Add("numvisibleplot", "12");
                chartConfig.Add("drawcrossline", "1");
                chartConfig.Add("theme", "fusion");
                //chartConfig.Add("exportEnabled", "1");
                chartConfig.Add("exportFileName", "CUSTOMER_NOTIFICATIONS" + String.Format("fileName_{0:yyyy_MM_dd}", DateTime.Now));

                jsonData.Append("{'chart':{");
                foreach (var config in chartConfig)
                {
                    jsonData.AppendFormat("'{0}':'{1}',", config.Key, config.Value);
                }
                jsonData.Replace(",", "},", jsonData.Length - 1, 1);
                data.Append("'categories':[{");
                data.Append("'category':[");


                List<string> previousMonths = new List<string>();

                int i = 0;
                for (int j = 1; j < 13; j++)
                {
                    previousMonths.Add(DateTime.Now.AddMonths(i).ToString("MMM"));
                    data.Append("{'label': '" + DateTime.Now.AddMonths(i).ToString("MMM") + "'},");
                    i = i - 1;
                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append(" ]    }  ],");

                data.Append("'dataset':[");

                data.Append("{'seriesname':'Emails',");
                data.Append("'plottooltext': 'Emails: $dataValue',");
                data.Append("'data':[");
                foreach (string mnth in previousMonths)
                {
                    bool found = false;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["Month"].ToString() == mnth)
                        {
                            if (row["Communication Type"].ToString() == "Email")
                            {
                                data.Append("{'value': '" + row["Number Of Notifications"].ToString() + "'},");
                                found = true;
                            }
                        }

                    }
                    if (!found)
                    {
                        data.Append("{'value': '0'},");
                    }
                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]},");

                data.Append("{'seriesname':'SMS',");
                data.Append("'plottooltext': 'SMS: $dataValue',");
                data.Append("'data':[");
                foreach (string mnth in previousMonths)
                {
                    bool found = false;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["Month"].ToString() == mnth)
                        {
                            if (row["Communication Type"].ToString() == "SMS")
                            {
                                data.Append("{'value': '" + row["Number Of Notifications"].ToString() + "'},");
                                found = true;
                            }
                        }

                    }
                    if (!found)
                    {
                        data.Append("{'value': '0'},");
                    }
                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]},");

                data.Append("{'seriesname':'Re-instated Assets',");
                //data.Append("'parentyaxis': 'S',");
                data.Append("'renderas': 'spline',");
                data.Append("'plottooltext': 'Re-instated Assets: $dataValue',");
                data.Append("'showanchors': '1',");
                //data.Append("'showvalues': '0',");
                data.Append("'data':[");
                foreach (string mnth in previousMonths)
                {
                    bool found = false;

                    foreach (DataRow row in dsReIns.Tables[1].Rows)
                    {
                        if (row["Month"].ToString() == mnth)
                        {
                            data.Append("{'value': '" + row["Re-Instated Assets"].ToString() + "'},");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{'value': '0'},");
                    }

                }

                data.Append("]},");

                data.Append("]}");
                jsonData.Append(data.ToString());


                Chart scrollcombidy2d = new Chart("mscolumn2d", "chartCommunicationsByFinancer", "100%", "400", "json", jsonData.ToString());

                chartCommunicationsByFinancer.Text = scrollcombidy2d.Render();
            }
            else
            {
                divchartCommunicationsByFinancer.Attributes.Add("style", "display:none;");
            }
        }

        private void UninsuredByInsurer()
        {
            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_Dashboard_Uninsured_By_Insurer();
            int month = DateTime.Now.Month;
            string name = DateTime.Now.ToString("MMM");

            if (ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder jsonData = new StringBuilder();
                StringBuilder data = new StringBuilder();
                Dictionary<string, string> chartConfig = new Dictionary<string, string>();

                chartConfig.Add("caption", "Uninsured Assets");
                chartConfig.Add("subcaption", "By Insurance Company");
                chartConfig.Add("yaxisname", "Number of assets");
                chartConfig.Add("syaxisname", "Financed value");
                chartConfig.Add("captionFont", "sans - serif");
                chartConfig.Add("captionFontColor", "#4e73df");

                chartConfig.Add("labeldisplay", "AUTO");
                chartConfig.Add("snumberprefix", "R");
                chartConfig.Add("numvisibleplot", "12");
                chartConfig.Add("theme", "fusion");
                //chartConfig.Add("exportEnabled", "1");
                chartConfig.Add("exportFileName", "UNINSURED_BY_INSURANCE_COMPANY_" + String.Format("fileName_{0:yyyy_MM_dd}", DateTime.Now));

                jsonData.Append("{'chart':{");
                foreach (var config in chartConfig)
                {
                    jsonData.AppendFormat("'{0}':'{1}',", config.Key, config.Value);
                }
                jsonData.Replace(",", "},", jsonData.Length - 1, 1);
                data.Append("'categories':[{");
                data.Append("'category':[");


                List<string> insurers = new List<string>();


                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    insurers.Add(row["Ínsurer"].ToString());
                    data.Append("{'label': '" + row["Ínsurer"].ToString() + "'},");

                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append(" ]    }  ],");

                data.Append("'dataset':[");

                data.Append("{'seriesname':'Number of uninsured assets',");
                data.Append("'plottooltext': 'Number of uninsured assets: $dataValue',");
                data.Append("'data':[");
                foreach (string ins in insurers)
                {
                    bool found = false;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["Ínsurer"].ToString() == ins)
                        {

                            data.Append("{'value': '" + row["AssetCount"].ToString() + "'},");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{'value': '0'},");
                    }
                }
                data.Append("]},");


                data.Append("{'seriesname':'Uninsured value',");
                data.Append("'parentyaxis': 'S',");
                data.Append("'renderas': 'spline',");
                data.Append("'plottooltext': 'Uninsured value: $dataValue',");
                data.Append("'showanchors': '1',");
                data.Append("'showvalues': '0',");
                data.Append("'data':[");
                foreach (string ins in insurers)
                {
                    bool found = false;

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["Ínsurer"].ToString() == ins)
                        {
                            data.Append("{'value': '" + row["FinanceAmount"].ToString() + "'},");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{'value': '0'},");
                    }

                }

                data.Append("]},");
                data.Append("]}");
                jsonData.Append(data.ToString());


                Chart scrollcombidy2d = new Chart("mscombidy2d", "chartUninsuredByInsurer", "100%", "400", "json", jsonData.ToString());

                chartUninsuredByInsurer.Text = scrollcombidy2d.Render();
            }
            else
            {
                divchartUninsuredByInsurer.Attributes.Add("style", "display:none;");
            }
        }

        private void UninsuredPercentage(CCom.Dashboards.FinancerLandingTableTotals financerLandingTableTotals)
        {
            int PercUninsuredAssets = 0, PercUninsuredValue = 0;

            //PercUninsuredAssets = financerLandingTableTotals.iNumber_Of_Assets > 0 ? Convert.ToInt32((decimal.Divide(financerLandingTableTotals.iNumber_Of_Uninsued_Assets, financerLandingTableTotals.iNumber_Of_Assets) * 100)) : 0;

            Chart uninsuredPercentage = new Chart();
            uninsuredPercentage.SetChartParameter(Chart.ChartParameter.chartId, "uninsuredPercentage");
            uninsuredPercentage.SetChartParameter(Chart.ChartParameter.chartType, "angulargauge");
            uninsuredPercentage.SetChartParameter(Chart.ChartParameter.chartWidth, "100%");
            uninsuredPercentage.SetChartParameter(Chart.ChartParameter.chartHeight, "400");

            uninsuredPercentage.SetData("{\n  \"chart\": {\n    \"caption\": \"Percentage of Uninsured Assets\",\n    \"caption\": \"Percentage of Uninsured Assets\",\n    \"lowerlimit\": \"0\",\n    \"upperlimit\": \"100\",\n    \"showvalue\": \"1\",\n    \"" + PercUninsuredAssets.ToString() + "\": \"%\",\n    \"theme\": \"fusion\",\n    \"showtooltip\": \"0\"\n,\n    \"exportEnabled\": \"1\"\n,\n    \"exportFileName\": \"" + "_UNINSURED_PERCENTAGE_AS_AT_" + String.Format("fileName_{0:yyyy_MM_dd}", DateTime.Now) + "\"\n  },\n  \"colorrange\": {\n    \"color\": [\n      {\n        \"minvalue\": \"0\",\n        \"maxvalue\": \"50\",\n        \"code\": \"#62B58F\"\n      },\n      {\n        \"minvalue\": \"50\",\n        \"maxvalue\": \"75\",\n        \"code\": \"#FFC533\"\n      },\n      {\n        \"minvalue\": \"75\",\n        \"maxvalue\": \"100\",\n        \"code\": \"#F2726F\"\n      }\n    ]\n  },\n  \"dials\": {\n    \"dial\": [\n      {\n        \"value\": \"" + PercUninsuredAssets.ToString() + "\"\n      }\n    ]\n  }\n}", Chart.DataFormat.json);
            //ChartUninsuredAssetPercentage.Text = uninsuredPercentage.Render();
        }
        //private void Communications_Current_MonthChart(int iFinancer_Id)
        //{
        //    try
        //    {


        //        P.Daschboard_Provider frmF = new P.Daschboard_Provider();
        //        ds = frmF.Get_Financer_Landing_Dashboard_Communications_Current_Month(iFinancer_Id);

        //        DataTable dt = new DataTable();
        //        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            int month = DateTime.Now.Month;
        //            string name = DateTime.Now.ToString("MMM");

        //            dt = ds.Tables[0];
        //            List<string> financers = (from p in dt.AsEnumerable()
        //                                      select p.Field<string>("Financer")).Distinct().ToList();
        //            List<string> communicationType = (from p in dt.AsEnumerable()
        //                                              select p.Field<string>("Communication Type")).Distinct().ToList();


        //            StringBuilder jsonData = new StringBuilder();
        //            StringBuilder data = new StringBuilder();
        //            Dictionary<string, string> chartConfig = new Dictionary<string, string>();

        //            chartConfig.Add("caption", "Notifications");
        //            chartConfig.Add("subcaption", "By Lender");
        //            //chartConfig.Add("yaxisname", "Number of assets");
        //            //chartConfig.Add("numvisibleplot", "6");
        //            //chartConfig.Add("labeldisplay", "auto");
        //            chartConfig.Add("theme", "fusion");
        //            //chartConfig.Add("exportEnabled", "1");
        //            // json data to use as chart data source
        //            jsonData.Append("{'chart':{");
        //            foreach (var config in chartConfig)
        //            {
        //                jsonData.AppendFormat("'{0}':'{1}',", config.Key, config.Value);
        //            }
        //            jsonData.Replace(",", "},", jsonData.Length - 1, 1);
        //            data.Append("'categories':[{");
        //            data.Append("'category':[");

        //            foreach (string f in financers)
        //            {

        //                data.Append("{'label': '" + f + "'},");

        //            }
        //            data.Replace(",", "", data.Length - 1, 1);
        //            data.Append(" ]    }  ],");

        //            data.Append("'dataset':[");
        //            foreach (string comm in communicationType)
        //            {
        //                data.Append("{'seriesname':'" + comm + "',");
        //                data.Append("'data':[");
        //                //foreach (string comm in communicationType)
        //                //{
        //                bool found = false;
        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    if (row["Communication Type"].ToString() == comm)
        //                    {

        //                        data.Append("{'value': '" + row["Number Of Messages"].ToString() + "'},");
        //                        found = true;
        //                    }
        //                }
        //                if (!found)
        //                {
        //                    data.Append("{'value': '0'},");
        //                }
        //                //}
        //                data.Replace(",", "", data.Length - 1, 1);
        //                data.Append("]},");
        //            }


        //            data.Replace(",", "", data.Length - 1, 1);
        //            data.Append("]}");
        //            // data.Replace(",", "]", data.Length - 1, 1);

        //            jsonData.Append(data.ToString());


        //            Chart scrollColumn = new Chart("column2d", "CommunicationsByFinancer", "100%", "400", "json", jsonData.ToString());
        //           
        //            CommunicationsByFinancer.Text = scrollColumn.Render();



        //        }
        //        //else
        //        //{
        //        //    divCommunicationsByFinancer.Attributes.Add("style", "display:none;");
        //        //}
        //    }
        //    catch (Exception ex)
        //    {

        //        U.ErrorLogger eL = new U.ErrorLogger();
        //        eL.LogErrorInDB(ex, "FinancerHome.aspx", "Communications_Current_MonthChart");
        //    }
        //}
        #endregion

        #region Export

        protected void btnDownloadDashboard_Click(object sender, EventArgs e)
        {
            try
            {

                CCom.Dashboards.FinancerLandingTableTotals financerLandingTableTotals = new CCom.Dashboards.FinancerLandingTableTotals();

                string allCharts = string.Empty;
                var path = Server.MapPath("GraphExportResources");
                string templateHTML = File.ReadAllText(path + "\\dashboard-template-financer-Landing.html");
                StringBuilder graphDetails = new StringBuilder();// = File.ReadAllText(path + "\\chart-config-file.json");

                P.User_Provider uP = new P.User_Provider();
                CCom.CurrentUser objUser = new CCom.CurrentUser();
                graphDetails.Append("[");

                objUser = uP.GetUserFromSession();
                if (objUser == null)
                {
                    Response.Redirect("/account/login.aspx", false);
                }
                else
                {

                    financerLandingTableTotals = GetTotals();
                    graphDetails.Append(Export_UninsuredStatistics(objUser.iPartner_Id) + ",");
                    graphDetails.Append(Export_NonPaymentReInstatedByMonthChartFinancedValue() + ",");
                    graphDetails.Append(Export_NonPaymentReInstatedByMonthChartAssetCount() + ",");
                    graphDetails.Append(Export_ArrearVsUnconfirmedChart(financerLandingTableTotals.AllAssetCount) + ",");
                    graphDetails.Append(Export_InsuredAssetsChart(financerLandingTableTotals.InsuredAdequatelyAssetCount, financerLandingTableTotals.InsuredUnderInsuredAssetCount) + ",");
                    graphDetails.Append(Export_UninsuredAssetsCount(financerLandingTableTotals.PremiumUnpaidAssetCount, financerLandingTableTotals.NoInsuranceAssetCount) + ",");
                    graphDetails.Append(Export_Communications_Current_MonthChart() + ",");
                    graphDetails.Append(Export_UninsuredByInsurer() + ",");
                    //graphDetails.Append(Export_UninsuredPercentage(objUser.iPartner_Id) + ",");
                }
                graphDetails.Replace(",", "", graphDetails.Length - 1, 1);
                graphDetails.Append("]");




                int lcPremiumUnpaidAssetCountPercent, lcPremiumUnpaidAssetTotalPercent, lcNoInsuranceAssetCountPercent, lcNoInsuranceAssetTotalPercent = 0;


                lcPremiumUnpaidAssetCountPercent = financerLandingTableTotals.PremiumUnpaidAssetCount > 0 ? Convert.ToInt32((decimal.Divide(financerLandingTableTotals.PremiumUnpaidAssetCount, financerLandingTableTotals.AllAssetCount) * 100)) : 0;
                lcPremiumUnpaidAssetTotalPercent = financerLandingTableTotals.PremiumUnpaidAssetTotal > 0 ? Convert.ToInt32((decimal.Divide(financerLandingTableTotals.PremiumUnpaidAssetTotal, financerLandingTableTotals.AllAssetTotal) * 100)) : 0;
                lcNoInsuranceAssetCountPercent = financerLandingTableTotals.NoInsuranceAssetCount > 0 ? Convert.ToInt32((decimal.Divide(financerLandingTableTotals.NoInsuranceAssetCount, financerLandingTableTotals.AllAssetCount) * 100)) : 0;
                lcNoInsuranceAssetTotalPercent = financerLandingTableTotals.NoInsuranceAssetTotal > 0 ? Convert.ToInt32((decimal.Divide(financerLandingTableTotals.NoInsuranceAssetTotal, financerLandingTableTotals.AllAssetTotal) * 100)) : 0;

                //                templateHTML = templateHTML.Replace("{1}", objUser.vcPartnerLogo.ToString());
                templateHTML = templateHTML.Replace("{2}", DateTime.Now.ToString("dd MMMM yyyy"));
                templateHTML = templateHTML.Replace("{3}", financerLandingTableTotals.PremiumUnpaidAssetCount.ToString());
                templateHTML = templateHTML.Replace("{4}", lcPremiumUnpaidAssetCountPercent.ToString());
                templateHTML = templateHTML.Replace("{5}", financerLandingTableTotals.PremiumUnpaidAssetTotal.ToString("C", new CultureInfo("en-ZA")));
                templateHTML = templateHTML.Replace("{6}", financerLandingTableTotals.PremiumUnpaidAssetTotalPercent.ToString());
                templateHTML = templateHTML.Replace("{7}", financerLandingTableTotals.NoInsuranceAssetCount.ToString());
                templateHTML = templateHTML.Replace("{8}", financerLandingTableTotals.NoInsuranceAssetTotalPercent.ToString());
                templateHTML = templateHTML.Replace("{9}", financerLandingTableTotals.NoInsuranceAssetTotal.ToString("C", new CultureInfo("en-ZA")));
                templateHTML = templateHTML.Replace("{10}", lcNoInsuranceAssetTotalPercent.ToString());
                templateHTML = templateHTML.Replace("{11}", (financerLandingTableTotals.PremiumUnpaidAssetCount + financerLandingTableTotals.NoInsuranceAssetCount).ToString());
                templateHTML = templateHTML.Replace("{12}", (lcPremiumUnpaidAssetCountPercent + lcNoInsuranceAssetCountPercent).ToString());
                templateHTML = templateHTML.Replace("{13}", (financerLandingTableTotals.PremiumUnpaidAssetTotal + financerLandingTableTotals.NoInsuranceAssetTotal).ToString("C", new CultureInfo("en-ZA")));
                templateHTML = templateHTML.Replace("{14}", (lcPremiumUnpaidAssetTotalPercent + lcNoInsuranceAssetTotalPercent).ToString());
                templateHTML = templateHTML.Replace("{15}", financerLandingTableTotals.AllAssetCount.ToString());
                templateHTML = templateHTML.Replace("{16}", financerLandingTableTotals.AllAssetTotal.ToString("C", new CultureInfo("en-ZA")));
                templateHTML = templateHTML.Replace("{17}", (financerLandingTableTotals.AllAssetCount - (financerLandingTableTotals.PremiumUnpaidAssetCount + financerLandingTableTotals.NoInsuranceAssetCount)).ToString());
                templateHTML = templateHTML.Replace("{18}", financerLandingTableTotals.InsuredTotal.ToString("C", new CultureInfo("en-ZA")));
                templateHTML = templateHTML.Replace("{19}", (financerLandingTableTotals.AllAssetTotal - financerLandingTableTotals.InsuredTotal).ToString("C", new CultureInfo("en-ZA")));

                templateHTML = templateHTML.Replace("{20}", financerLandingTableTotals.InsuredAdequatelyTotal.ToString("C", new CultureInfo("en-ZA")));

                templateHTML = templateHTML.Replace("{21}", financerLandingTableTotals.InsuredAdequatelyTotalPercent.ToString());

                templateHTML = templateHTML.Replace("{22}", financerLandingTableTotals.InsuredUnderInsuredTotal.ToString("C", new CultureInfo("en-ZA")));

                templateHTML = templateHTML.Replace("{23}", financerLandingTableTotals.InsuredUnderInsuredTotalPercent.ToString());



                templateHTML = templateHTML.Replace("{tbHeaderMonthsFinanceValue}", tbHeaderMonthsFinanceValueEx);
                templateHTML = templateHTML.Replace("{tbNonPaymentReInstatedByMonthFinanceValue}", tbNonPaymentReInstatedByMonthFinanceValueEx);

                templateHTML = templateHTML.Replace("{tbHeaderMonthsAssetCount}", tbHeaderMonthsAssetCountEx);
                templateHTML = templateHTML.Replace("{tbNonPaymentReInstatedByMonthAssetCount}", tbNonPaymentReInstatedByMonthAssetCountEX);





                Stream fIleStream = IAPR_Web.Reporting.BulkExport.GetGraphStream(graphDetails.ToString(), templateHTML);

                var buffer = new byte[fIleStream.Length];

                HttpContext context = HttpContext.Current;

                MemoryStream _ms = new MemoryStream();
                fIleStream.CopyTo(_ms);

                context.Response.Clear();
                context.Response.ContentType = "Application/pdf";
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + objUser.vcPartner_Name + "_" + DateTime.Now.ToString("dd/MMM/yyyy") + ".pdf");
                context.Response.BinaryWrite(_ms.ToArray());
                context.Response.Flush();
                context.Response.Close();






            }
            catch (Exception exc)
            {
                if (exc.Source == "Fusion")
                {
                    U.ErrorLogger eL = new U.ErrorLogger();
                    eL.LogErrorInDB(exc, "FinancerHome.aspx", "btnDownloadDashboard_Click");
                    return;
                }
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
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
        private string Export_UninsuredAssetsCount(int unpaid, int noInsurance)
        {
            StringBuilder data = new StringBuilder();

            int month = DateTime.Now.Month;
            string name = DateTime.Now.ToString("MMM");




            Dictionary<string, string> chartConfig = new Dictionary<string, string>();

            data.Append("{");
            data.Append("\"type\": \"doughnut2d\",");
            data.Append("\"renderAt\": \"divchartUninsuredAssetsCount\",");
            data.Append("\"width\": \"95%\",");
            data.Append("\"height\": \"400\",");
            data.Append("\"dataFormat\": \"json\",");
            data.Append("\"dataSource\": {");
            data.Append("\"chart\": {");
            data.Append("\"caption\": \"Uninsured Assets\",");

            data.Append("\"captionFont\": \"sans - serif\",");
            data.Append("\"captionFontColor\": \"#4e73df\",");



            data.Append("\"defaultcenterlabel\": \"Total Uninsured Assets: <b>" + (unpaid + noInsurance).ToString() + "</b>\",");

            data.Append("\"showpercentvalues\": \"0\",");
            data.Append("\"showLabels\": \"0\",");
            data.Append("\"showvalues\": \"1\",");
            //data.Append("\"labelFont\": \"Arial\",");
            //data.Append("\"labelFontColor\": \"0075c2\",");
            //data.Append("\"labelFontBold\": \"1\",");
            //data.Append("\"lableFontItalic\": \"1\",");
            //data.Append("\"labelAlpha\": \"70\",");


            data.Append("\"smartLineThickness\": \"0.8\",");
            data.Append("\"doughnutRadius\": \"85\",");
            data.Append("\"labelFont\": \"Arial\",");
            data.Append("\"labelFontColor\": \"#000000\",");
            data.Append("\"labelAlpha\": \"100\",");

            data.Append("\"showlegend\": \"1\",");
            data.Append("\"theme\": \"fusion\"");
            data.Append("},");



            data.Append("\"data\": [");


            data.AppendFormat("{{\"label\":\"{0}\",\"value\":\"{1}\"}},", "Unpaid Premiums", unpaid.ToString());
            data.AppendFormat("{{\"label\":\"{0}\",\"value\":\"{1}\"}},", "No Insurance Details", noInsurance.ToString());

            data.Replace(",", "]", data.Length - 1, 1);
            data.Append("}}");



            return data.ToString();
        }
        private string Export_ArrearVsUnconfirmedChart(int allAssetCount)
        {
            StringBuilder data = new StringBuilder();
            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_Dashboard_ArrearVsUnconfirmed();


            int month = DateTime.Now.Month;
            string name = DateTime.Now.ToString("MMM");



            if (ds.Tables[0].Rows.Count > 0)
            {

                Dictionary<string, string> chartConfig = new Dictionary<string, string>();

                data.Append("{");
                data.Append("\"type\": \"doughnut2d\",");
                data.Append("\"renderAt\": \"divchartInsuraceStatus\",");
                data.Append("\"width\": \"95%\",");
                data.Append("\"height\": \"400\",");
                data.Append("\"dataFormat\": \"json\",");
                data.Append("\"dataSource\": {");
                data.Append("\"chart\": {");
                data.Append("\"caption\": \"Insurance Status\",");
                data.Append("\"captionFont\": \"sans - serif\",");
                data.Append("\"captionFontColor\": \"#4e73df\",");

                data.Append("\"showLabels\": \"0\",");
                data.Append("\"showvalues\": \"1\",");
                data.Append("\"showpercentvalues\": \"0\",");
                data.Append("\"defaultcenterlabel\": \"Total Assets: <b>" + allAssetCount.ToString() + "</b>\",");

                data.Append("\"smartLineThickness\": \"0.8\",");
                data.Append("\"doughnutRadius\": \"85\",");

                data.Append("\"labelFont\": \"Arial\",");
                data.Append("\"labelFontColor\": \"#000000\",");
                data.Append("\"labelAlpha\": \"100\",");




                data.Append("\"theme\": \"fusion\"");
                data.Append("},");



                data.Append("\"data\": [");

                //iterate through data table to build data object


                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    data.AppendFormat("{{\"label\":\"{0}\",\"value\":\"{1}\"}},", row[0].ToString(), row[1].ToString());
                }

                data.Replace(",", "]", data.Length - 1, 1);
                data.Append("}}");

            }

            return data.ToString();
        }
        private string Export_InsuredAssetsChart(int adequatelyInsured, int underInsured)
        {
            StringBuilder data = new StringBuilder();

            int month = DateTime.Now.Month;
            string name = DateTime.Now.ToString("MMM");





            Dictionary<string, string> chartConfig = new Dictionary<string, string>();

            data.Append("{");
            data.Append("\"type\": \"doughnut2d\",");
            data.Append("\"renderAt\": \"divchartInsuredAssetsCount\",");
            data.Append("\"width\": \"95%\",");
            data.Append("\"height\": \"400\",");
            data.Append("\"dataFormat\": \"json\",");
            data.Append("\"dataSource\": {");
            data.Append("\"chart\": {");
            data.Append("\"caption\": \"Insured Assets\",");

            data.Append("\"captionFont\": \"sans - serif\",");
            data.Append("\"captionFontColor\": \"#4e73df\",");



            data.Append("\"defaultcenterlabel\": \"Total Insured Assets: <b>" + (adequatelyInsured + underInsured).ToString() + "</b>\",");

            data.Append("\"showpercentvalues\": \"0\",");
            data.Append("\"showLabels\": \"0\",");
            data.Append("\"showvalues\": \"1\",");
            //data.Append("\"labelFont\": \"Arial\",");
            //data.Append("\"labelFontColor\": \"0075c2\",");
            //data.Append("\"labelFontBold\": \"1\",");
            //data.Append("\"lableFontItalic\": \"1\",");
            //data.Append("\"labelAlpha\": \"70\",");


            data.Append("\"smartLineThickness\": \"0.8\",");
            data.Append("\"doughnutRadius\": \"85\",");
            data.Append("\"labelFont\": \"Arial\",");
            data.Append("\"labelFontColor\": \"#000000\",");
            data.Append("\"labelAlpha\": \"100\",");

            data.Append("\"showlegend\": \"1\",");
            data.Append("\"theme\": \"fusion\"");
            data.Append("},");



            data.Append("\"data\": [");


            data.AppendFormat("{{\"label\":\"{0}\",\"value\":\"{1}\"}},", "Adequately Insured", adequatelyInsured.ToString());
            data.AppendFormat("{{\"label\":\"{0}\",\"value\":\"{1}\"}},", "Under Insured", underInsured.ToString());

            data.Replace(",", "]", data.Length - 1, 1);
            data.Append("}}");



            return data.ToString();
        }
        private string Export_UninsuredStatistics(int financer_Id)
        {

            StringBuilder data = new StringBuilder();
            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_Dashboard_Uninsured_Statistics();

            int month = DateTime.Now.Month;
            string name = DateTime.Now.ToString("MMM");



            if (ds.Tables[0].Rows.Count > 0)
            {
                Dictionary<string, string> chartConfig = new Dictionary<string, string>();

                data.Append("{");
                data.Append("\"type\": \"scrollcombidy2d\",");
                data.Append("\"renderAt\": \"divchartUninsuredStatistics\",");
                data.Append("\"width\": \"100%\",");
                data.Append("\"height\": \"400\",");
                data.Append("\"dataFormat\": \"json\",");
                data.Append("\"dataSource\": {");
                data.Append("\"chart\": {");
                data.Append("\"caption\": \"Unpaid Premiums\",");
                data.Append("\"snumberprefix\": \"R\",");
                data.Append("\"subCaption\": \"Age Analysis \","); //+ DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture).ToUpper() + " " + DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture) + "\",");
                data.Append("\"captionFont\": \"sans - serif\",");
                data.Append("\"captionFontColor\": \"#4e73df\",");

                data.Append("\"xAxisNameFont\": \"Arial\",");
                data.Append("\"xAxisNameFontSize\": \"12\",");
                data.Append("\"xAxisNameFontColor\": \"#993300\",");
                //data.Append("\"xAxisNameFontBold\": \"1\",");
                data.Append("\"xAxisNameFontItalic\": \"1\",");

                data.Append("\"theme\": \"fusion\"");
                data.Append("},");

                data.Append("\"categories\":[{");
                data.Append("\"category\":[");


                List<string> categories = new List<string>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    categories.Add(row["DayCount"].ToString());
                    data.Append("{\"label\": \"" + row["DayCount"].ToString() + "\"},");

                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append(" ]    }  ],");

                data.Append("\"dataset\":[");

                data.Append("{\"seriesname\":\"Number of uninsured assets\",");
                data.Append("\"plottooltext\": \"Number of uninsured assets: $dataValue\", ");
                data.Append("\"data\":[");
                foreach (string cat in categories)
                {
                    bool found = false;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["DayCount"].ToString() == cat)
                        {

                            data.Append("{\"value\": \"" + row["AssetCount"].ToString() + "\"},");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{\"value\": \"0\"},");
                    }
                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]},");


                data.Append("{\"seriesname\":\"Financed value\",");
                data.Append("\"parentyaxis\": \"S\",");
                data.Append("\"renderas\": \"spline\",");
                data.Append("\"plottooltext\": \"Financed value: $dataValue\",");
                data.Append("\"showanchors\": \"1\",");
                data.Append("\"showvalues\": \"1\",");
                data.Append("\"data\":[");
                foreach (string pmR in categories)
                {
                    bool found = false;

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["DayCount"].ToString() == pmR)
                        {
                            data.Append("{\"value\": \"" + row["FinancedValue"].ToString() + "\"},");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{\"value\": \"0\"},");
                    }

                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]},");
                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]}}");



            }


            return data.ToString();
        }
        private string Export_NonPaymentReInstatedByMonthChartFinancedValue()
        {
            StringBuilder jsonData = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder tableHearders = new StringBuilder();
            StringBuilder graphTable = new StringBuilder();

            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_Dashboard_NonPayment_History();

            int month = DateTime.Now.Month;
            string name = DateTime.Now.ToString("MMM");



            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtUnpaid = new DataTable();
                DataTable dtReInstated = new DataTable();

                dtUnpaid = ds.Tables[0];
                dtReInstated = ds.Tables[1];


                Dictionary<string, string> chartConfig = new Dictionary<string, string>();





                data.Append("{");
                data.Append("\"type\": \"msspline\",");
                data.Append("\"renderAt\": \"divchartNonPaymentReInstatedByMonthFinancedValue\",");
                data.Append("\"width\": \"100%\",");
                data.Append("\"height\": \"400\",");
                data.Append("\"dataFormat\": \"json\",");
                data.Append("\"dataSource\": {");
                data.Append("\"chart\": {");
                data.Append("\"caption\": \"Premiums Unpaid vs Re-instated Cover: Financed Value\",");
                data.Append("\"subCaption\": \"Last Twelve Months\",");
                data.Append("\"captionFont\": \"sans - serif\",");
                data.Append("\"captionFontColor\": \"#4e73df\",");

                data.Append("\"numberprefix\": \"R\",");
                data.Append("\"theme\": \"fusion\"");
                data.Append("},");



                data.Append("\"categories\":[{");
                data.Append("\"category\":[");



                List<string> previousMonths = new List<string>();

                int i = 0;
                for (int j = 1; j < 13; j++)
                {
                    previousMonths.Add(DateTime.Now.AddMonths(i).ToString("MMM"));
                    data.Append("{\"label\": \"" + DateTime.Now.AddMonths(i).ToString("MMM") + "\"},");
                    tableHearders.Append("<th>" + DateTime.Now.AddMonths(i).ToString("MMM") + "</th>");
                    i = i - 1;
                }
                tbHeaderMonthsFinanceValueEx = tableHearders.ToString();
                graphTable.Append("<tr>");
                graphTable.Append("<td>Premiums Unpaid</td>");

                data.Replace(",", "", data.Length - 1, 1);
                data.Append(" ]    }  ],");

                data.Append("\"dataset\":[");

                data.Append("{\"seriesname\":\"Premiums Unpaid\",");
                data.Append("\"showanchors\": \"1\",");
                data.Append("\"showvalues\": \"0\",");
                data.Append("\"data\":[");
                foreach (string mnth in previousMonths)
                {
                    bool found = false;
                    foreach (DataRow row in dtUnpaid.Rows)
                    {
                        if (row["Month"].ToString() == mnth)
                        {

                            data.Append("{\"value\": \"" + row["Uninsured Value"].ToString() + "\"},");
                            graphTable.Append("<td>R " + CCom.Common.ConvertToMillion(Convert.ToDecimal(row["Uninsured Value"].ToString())).ToString("0.##") + "M</td>");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{\"value\": \"0\"},");
                        graphTable.Append("<td>0</td>");
                    }
                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]},");

                graphTable.Append("</tr>");

                graphTable.Append("<tr>");
                graphTable.Append("<td>Re-instated Cover</td>");


                data.Append("{\"seriesname\":\"Re-Instated Value\",");
                data.Append("\"renderas\": \"spline\",");
                //data.Append("\"plottooltext\": \"Re-Instated Value: $dataValue\",");
                data.Append("\"showanchors\": \"1\",");
                data.Append("\"showvalues\": \"0\",");
                data.Append("\"data\":[");


                foreach (string pmR in previousMonths)
                {
                    bool found = false;

                    foreach (DataRow row in dtReInstated.Rows)
                    {
                        if (row["Month"].ToString() == pmR)
                        {
                            data.Append("{\"value\": \"" + row["Re-Instated Value"].ToString() + "\"},");
                            graphTable.Append("<td>R " + CCom.Common.ConvertToMillion(Convert.ToDecimal(row["Re-Instated Value"].ToString())).ToString("0.##") + "M</td>");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{\"value\": \"0\"},");
                        graphTable.Append("<td>0</td>");
                    }

                }

                graphTable.Append("</tr>");
                tbNonPaymentReInstatedByMonthFinanceValueEx = graphTable.ToString();

                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]},");

                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]}}");

            }
            return data.ToString();
        }
        private string Export_NonPaymentReInstatedByMonthChartAssetCount()
        {
            StringBuilder jsonData = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder tableHearders = new StringBuilder();
            StringBuilder graphTable = new StringBuilder();


            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_Dashboard_NonPayment_History();

            int month = DateTime.Now.Month;
            string name = DateTime.Now.ToString("MMM");



            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtUnpaid = new DataTable();
                DataTable dtReInstated = new DataTable();

                dtUnpaid = ds.Tables[0];
                dtReInstated = ds.Tables[1];


                Dictionary<string, string> chartConfig = new Dictionary<string, string>();





                data.Append("{");
                data.Append("\"type\": \"msline\",");
                data.Append("\"renderAt\": \"divchartNonPaymentReInstatedByMonthAssetCount\",");
                data.Append("\"width\": \"100%\",");
                data.Append("\"height\": \"400\",");
                data.Append("\"dataFormat\": \"json\",");
                data.Append("\"dataSource\": {");
                data.Append("\"chart\": {");
                data.Append("\"caption\": \"Premiums Unpaid vs Re-instated Cover: Asset Count\",");
                data.Append("\"subCaption\": \"Last Twelve Months\",");
                data.Append("\"captionFont\": \"sans - serif\",");
                data.Append("\"captionFontColor\": \"#4e73df\",");

                //data.Append("\"snumberprefix\": \"R\",");
                data.Append("\"theme\": \"fusion\"");
                data.Append("},");



                data.Append("\"categories\":[{");
                data.Append("\"category\":[");



                List<string> previousMonths = new List<string>();

                int i = 0;
                for (int j = 1; j < 13; j++)
                {
                    previousMonths.Add(DateTime.Now.AddMonths(i).ToString("MMM"));
                    data.Append("{\"label\": \"" + DateTime.Now.AddMonths(i).ToString("MMM") + "\"},");
                    tableHearders.Append("<th>" + DateTime.Now.AddMonths(i).ToString("MMM") + "</th>");
                    i = i - 1;
                }
                tbHeaderMonthsAssetCountEx = tableHearders.ToString();
                graphTable.Append("<tr>");
                graphTable.Append("<td>Premiums Unpaid</td>");

                data.Replace(",", "", data.Length - 1, 1);
                data.Append(" ]    }  ],");

                data.Append("\"dataset\":[");

                data.Append("{\"seriesname\":\"Premiums Unpaid\",");
                data.Append("\"showanchors\": \"1\",");
                data.Append("\"showvalues\": \"0\",");
                data.Append("\"data\":[");
                foreach (string mnth in previousMonths)
                {
                    bool found = false;
                    foreach (DataRow row in dtUnpaid.Rows)
                    {
                        if (row["Month"].ToString() == mnth)
                        {

                            data.Append("{\"value\": \"" + row["Unpaid Premiums"].ToString() + "\"},");
                            graphTable.Append("<td>" + row["Unpaid Premiums"].ToString() + "</td>");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{\"value\": \"0\"},");
                        graphTable.Append("<td>0</td>");
                    }
                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]},");

                graphTable.Append("</tr>");

                graphTable.Append("<tr>");
                graphTable.Append("<td>Re-instated Cover:</td>");

                data.Append("{\"seriesname\":\"Re-instated Cover:\",");
                //data.Append("\"parentyaxis\": \"S\",");
                data.Append("\"renderas\": \"spline\",");
                //data.Append("\"plottooltext\": \"Re-Instated Assets: $dataValue\",");
                data.Append("\"showanchors\": \"1\",");
                data.Append("\"showvalues\": \"0\",");
                data.Append("\"data\":[");


                foreach (string pmR in previousMonths)
                {
                    bool found = false;

                    foreach (DataRow row in dtReInstated.Rows)
                    {
                        if (row["Month"].ToString() == pmR)
                        {
                            data.Append("{\"value\": \"" + row["Re-Instated Assets"].ToString() + "\"},");
                            graphTable.Append("<td>" + row["Re-Instated Assets"].ToString() + "</td>");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{\"value\": \"0\"},");
                        graphTable.Append("<td>0</td>");
                    }

                }

                graphTable.Append("</tr>");
                tbNonPaymentReInstatedByMonthAssetCountEX = graphTable.ToString();


                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]},");
                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]}}");

            }
            return data.ToString();
        }

        private string Export_Communications_Current_MonthChart()
        {
            DataSet dsReIns = new DataSet();

            StringBuilder jsonData = new StringBuilder();
            StringBuilder data = new StringBuilder();
            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_Dashboard_Communications_History();
            dsReIns = frmF.Get_Admin_Landing_Dashboard_NonPayment_History();

            int month = DateTime.Now.Month;
            string name = DateTime.Now.ToString("MMM");




            if (ds.Tables[0].Rows.Count > 0)
            {

                Dictionary<string, string> chartConfig = new Dictionary<string, string>();

                //chartConfig.Add("caption", "CUSTOMER NOTIFICATIONS");
                //chartConfig.Add("subcaption", "Last Twelve Months");
                //chartConfig.Add("yaxisname", "Notifications");

                //chartConfig.Add("showsum", "1");

                //chartConfig.Add("numvisibleplot", "12");
                //chartConfig.Add("drawcrossline", "1");
                //chartConfig.Add("theme", "fusion");

                data.Append("{");
                data.Append("\"type\": \"mscolumn2d\",");
                data.Append("\"renderAt\": \"divchartCustomerNotifications\",");
                data.Append("\"width\": \"100%\",");
                data.Append("\"height\": \"400\",");
                data.Append("\"dataFormat\": \"json\",");
                data.Append("\"dataSource\": {");
                data.Append("\"chart\": {");
                data.Append("\"caption\": \"Customer Notifications\",");
                data.Append("\"subCaption\": \"Last Twelve Months\",");
                data.Append("\"captionFont\": \"sans - serif\",");
                data.Append("\"captionFontColor\": \"#4e73df\",");

                data.Append("\"showsum\": \"1\",");
                //data.Append("\"snumberprefix\": \"R\",");
                data.Append("\"theme\": \"fusion\"");
                data.Append("},");



                data.Append("\"categories\":[{");
                data.Append("\"category\":[");



                List<string> previousMonths = new List<string>();

                int i = 0;
                for (int j = 1; j < 13; j++)
                {
                    previousMonths.Add(DateTime.Now.AddMonths(i).ToString("MMM"));
                    data.Append("{\"label\": \"" + DateTime.Now.AddMonths(i).ToString("MMM") + "\"},");
                    i = i - 1;
                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append(" ]    }  ],");

                data.Append("\"dataset\":[");

                data.Append("{\"seriesname\":\"Emails\",");
                data.Append("\"showanchors\": \"1\",");

                data.Append("\"data\":[");
                foreach (string mnth in previousMonths)
                {
                    bool found = false;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["Month"].ToString() == mnth)
                        {
                            if (row["Communication Type"].ToString() == "Email")
                            {
                                data.Append("{\"value\": \"" + row["Number Of Notifications"].ToString() + "\"},");

                                found = true;
                            }
                        }

                    }
                    if (!found)
                    {
                        data.Append("{\"value\": \"0\"},");
                    }
                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]},");

                data.Append("{\"seriesname\":\"SMS\",");
                data.Append("\"data\":[");
                foreach (string mnth in previousMonths)
                {
                    bool found = false;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["Month"].ToString() == mnth)
                        {
                            if (row["Communication Type"].ToString() == "SMS")
                            {
                                data.Append("{\"value\": \"" + row["Number Of Notifications"].ToString() + "\"},");

                                found = true;
                            }
                        }

                    }
                    if (!found)
                    {
                        data.Append("{\"value\": \"0\"},");
                    }
                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]},");


                data.Append("{\"seriesname\":\"Re-Instated Assets\",");
                data.Append("\"data\":[");
                foreach (string mnth in previousMonths)
                {
                    bool found = false;
                    foreach (DataRow row in dsReIns.Tables[1].Rows)
                    {
                        if (row["Month"].ToString() == mnth)
                        {
                            data.Append("{\"value\": \"" + row["Re-Instated Assets"].ToString() + "\"},");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{\"value\": \"0\"},");
                    }
                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]},");

                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]}}");

            }
            return data.ToString();
        }
        private string Export_UninsuredByInsurer()
        {
            StringBuilder jsonData = new StringBuilder();
            StringBuilder data = new StringBuilder();
            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_Dashboard_Uninsured_By_Insurer();

            int month = DateTime.Now.Month;
            string name = DateTime.Now.ToString("MMM");



            if (ds.Tables[0].Rows.Count > 0)
            {

                Dictionary<string, string> chartConfig = new Dictionary<string, string>();





                data.Append("{");
                data.Append("\"type\": \"mscombidy2d\",");
                data.Append("\"renderAt\": \"divchartUninsuredByInsurer\",");
                data.Append("\"width\": \"100%\",");
                data.Append("\"height\": \"400\",");
                data.Append("\"dataFormat\": \"json\",");
                data.Append("\"dataSource\": {");
                data.Append("\"chart\": {");
                data.Append("\"caption\": \"Uninsured Assets\",");
                data.Append("\"subCaption\": \"By Insurance Company\",");
                data.Append("\"captionFont\": \"sans - serif\",");
                data.Append("\"captionFontColor\": \"#4e73df\",");

                data.Append("\"snumberprefix\": \"R\",");
                data.Append("\"theme\": \"fusion\"");
                data.Append("},");



                data.Append("\"categories\":[{");
                data.Append("\"category\":[");



                List<string> insurers = new List<string>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    insurers.Add(row["Ínsurer"].ToString());

                    data.Append("{\"label\": \"" + row["Ínsurer"].ToString() + "\"},");
                }

                data.Replace(",", "", data.Length - 1, 1);
                data.Append(" ]    }  ],");

                data.Append("\"dataset\":[");

                data.Append("{\"seriesname\":\"Number of uninsured assets\",");
                data.Append("\"showanchors\": \"1\",");
                data.Append("\"showvalues\": \"1\",");
                data.Append("\"data\":[");
                foreach (string mnth in insurers)
                {
                    bool found = false;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["Ínsurer"].ToString() == mnth)
                        {

                            data.Append("{\"value\": \"" + row["AssetCount"].ToString() + "\"},");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{\"value\": \"0\"},");
                    }
                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]},");

                data.Append("{\"seriesname\":\"Uninsured value\",");
                data.Append("\"parentyaxis\": \"S\",");
                data.Append("\"renderas\": \"line\",");
                //data.Append("\"plottooltext\": \"Uninsured value: $dataValue\",");
                //data.Append("\"showanchors\": \"1\",");
                data.Append("\"showvalues\": \"1\",");
                data.Append("\"data\":[");
                foreach (string pmR in insurers)
                {
                    bool found = false;

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["Ínsurer"].ToString() == pmR)
                        {
                            data.Append("{\"value\": \"" + row["FinanceAmount"].ToString() + "\"},");
                            found = true;
                        }

                    }
                    if (!found)
                    {
                        data.Append("{\"value\": \"0\"},");
                    }

                }
                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]},");

                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]}}");

            }
            return data.ToString();
        }
        private string Export_UninsuredPercentage(int iPartnerId)
        {
            string chartDetails = string.Empty;
            CCom.Dashboards.FinancerLandingTableTotals financerLandingTableTotals = new CCom.Dashboards.FinancerLandingTableTotals();

            financerLandingTableTotals = GetTotals();

            int PercUninsuredAssets = 0;

            PercUninsuredAssets = financerLandingTableTotals.AllUninsuredCount > 0 ? Convert.ToInt32((decimal.Divide(financerLandingTableTotals.AllUninsuredCount, financerLandingTableTotals.AllAssetCount) * 100)) : 0;

            StringBuilder data = new StringBuilder();
            Dictionary<string, string> chartConfig = new Dictionary<string, string>();

            data.Append("{");
            data.Append("\"type\": \"angulargauge\",");
            data.Append("\"renderAt\": \"divChartUninsuredAssetPercentage\",");
            data.Append("\"width\": \"100%\",");
            data.Append("\"height\": \"400\",");
            data.Append("\"dataFormat\": \"json\",");
            data.Append("\"dataSource\": {");

            data.Append("\"chart\": {");
            data.Append("\"" + PercUninsuredAssets + "\": \"%\",");
            data.Append("\"caption\": \"Percentage of Uninsured Assets\",");
            data.Append("\"captionFont\": \"sans - serif\",");
            data.Append("\"captionFontColor\": \"#4e73df\",");

            data.Append("\"lowerlimit\": \"0\",");
            data.Append("\"upperlimit\": \"100\",");
            data.Append("\"showvalue\": \"1\",");
            data.Append("\"theme\": \"fusion\",");
            data.Append("\"showtooltip\": \"0\"");
            data.Append("},");
            data.Append("\"colorrange\": {");
            data.Append("\"color\": [");
            data.Append("{ ");
            data.Append("\"minvalue\": \"0\",");
            data.Append("\"maxvalue\": \"50\",");
            data.Append("\"code\": \"#62B58F\"");
            data.Append("},");
            data.Append("{");
            data.Append("\"minvalue\": \"50\",");
            data.Append("\"maxvalue\": \"75\",");
            data.Append("\"code\": \"#FFC533\"");
            data.Append("},");
            data.Append("{");

            data.Append("\"minvalue\": \"75\",");
            data.Append("\"maxvalue\": \"100\",");
            data.Append("\"code\": \"#F2726F\"");
            data.Append("}");
            data.Append("]");
            data.Append("},");
            data.Append("\"dials\": {");
            data.Append("\"dial\": [");
            data.Append("{");
            data.Append("\"value\": \"" + PercUninsuredAssets + "\"");
            data.Append("}");
            data.Append("]");
            data.Append("}");
            data.Append("}");
            data.Append("}");
            return data.ToString();
        }
        #endregion

    }
}

//Oldcode
// UnconfirmedUnpaid ByFinancer
//if (dt.Rows.Count > 0)
//{
//    List<string> statuses = (from p in dt.AsEnumerable()
//                             select p.Field<string>("Policy_Status")).Distinct().ToList();


//    //Loop through the statuses.
//    foreach (string status in statuses)
//    {

//        //Get the Year for each Country.
//        string[] x = (from p in dt.AsEnumerable()
//                      where p.Field<string>("Policy_Status") == status
//                      orderby p.Field<int>("iFinancer_Id") ascending
//                      select p.Field<string>("Financer")).ToArray();

//        //Get the Total of Orders for each Country.
//        int[] y = (from p in dt.AsEnumerable()
//                   where p.Field<string>("Policy_Status") == status
//                   orderby p.Field<int>("iFinancer_Id") ascending
//                   select p.Field<int>("iNumber_Of_Assets")).ToArray();

//        //Add Series to the Chart.
//        chUninsuredByFinancer.Series.Add(new SysCH.Series(status));
//        chUninsuredByFinancer.Series[status].IsValueShownAsLabel = true;
//        chUninsuredByFinancer.Series[status].ChartType = SysCH.SeriesChartType.Column;
//        chUninsuredByFinancer.Series[status].LabelForeColor = System.Drawing.Color.White;
//        //chUninsuredByFinancer.Series[status].Font="
//        chUninsuredByFinancer.Series[status].Points.DataBindXY(x, y);
//        chUninsuredByFinancer.Series[status]["PixelPointWidth"] = "25";
//    }

//    chUninsuredByFinancer.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
//    chUninsuredByFinancer.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
//    chUninsuredByFinancer.ChartAreas[0].AxisX.IsLabelAutoFit = true;
//    chUninsuredByFinancer.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
//    chUninsuredByFinancer.ChartAreas[0].AxisX.LabelStyle.Enabled = true;
//    chUninsuredByFinancer.Series[0].IsValueShownAsLabel = true;
//    chUninsuredByFinancer.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
//    chUninsuredByFinancer.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
//    chUninsuredByFinancer.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Gray;
//    chUninsuredByFinancer.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Gray;
//    chUninsuredByFinancer.Series[0]["PixelPointWidth"] = "25";
//    //            chUninsuredByFinancer.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Nunito' -apple-system BlinkMacSystemFont 'Segoe UI' Roboto 'Helvetica Neue' Arial sans-serif 'Apple Color Emoji' 'Segoe UI Emoji' 'Segoe UI Symbol' 'Noto Color Emoji, 11.2px, style=Bold", 2.25F, System.Drawing.FontStyle.Bold);
//    chUninsuredByFinancer.Legends[0].Enabled = true;
//}

//private void GetAdminDashboard_NonPaymentPreviousYear()
//{
//    try
//    {
//        //P.User_Provider uP = new P.User_Provider();
//        //CCom.CurrentUser objUser = new CCom.CurrentUser();

//        //objUser = uP.GetUserFromSession();

//        //if (objUser == null)
//        //{
//        //    Response.Redirect("/account/login.aspx", false);
//        //}
//        //ds = null;
//        //P.Daschboard_Provider frmF = new P.Daschboard_Provider();

//        //ds = frmF.Get_Admin_Landing_Dashboard_NonPayment_Annual_Chart();
//        //DataTable dt;
//        //for (int i = 0; i < ds.Tables.Count; i++)
//        //{
//        //    if (ds.Tables[i].Rows.Count > 0)
//        //    {
//        //        dt = ds.Tables[i];
//        //        string[] XPointMember = new string[dt.Rows.Count];
//        //        int[] YPointMember = new int[dt.Rows.Count];


//        //        for (int count = 0; count < dt.Rows.Count; count++)
//        //        {
//        //            XPointMember[count] = dt.Rows[count]["vcMonthName"].ToString();
//        //            //storing values for Y Axis  
//        //            YPointMember[count] = Convert.ToInt32(dt.Rows[count]["iNumber_Of_Assets"]);

//        //        }
//        //        chNonpaymentPreviousYearAsset.Series[0].Points.DataBindXY(XPointMember, YPointMember);
//        //        chNonpaymentPreviousYearAsset.Series[0].BorderWidth = 10;

//        //        //setting Chart type   
//        //        chNonpaymentPreviousYearAsset.Series[0].ChartType = SeriesChartType.Column;
//        //        chNonpaymentPreviousYearAsset.Series[0].Points[0].Color = System.Drawing.ColorTranslator.FromHtml("#4e73df");
//        //        chNonpaymentPreviousYearAsset.Series[0]["PixelPointWidth"] = "10";

//        //    }

//        //}

//        //for (int i = 0; i < ds.Tables.Count; i++)
//        //{
//        //    if (ds.Tables[i].Rows.Count > 0)
//        //    {
//        //        dt = ds.Tables[i];
//        //        string[] XPointMember = new string[dt.Rows.Count];
//        //        int[] YPointMember = new int[dt.Rows.Count];


//        //        for (int count = 0; count < dt.Rows.Count; count++)
//        //        {
//        //            XPointMember[count] = dt.Rows[count]["vcMonthName"].ToString();
//        //            //storing values for Y Axis  
//        //            YPointMember[count] = Convert.ToInt32(dt.Rows[count]["Assets_Finance_Total"]);

//        //        }
//        //        chNonpaymentPreviousYearFinanceValue.Series[0].Points.DataBindXY(XPointMember, YPointMember);
//        //        chNonpaymentPreviousYearFinanceValue.Series[0].BorderWidth = 1;

//        //        //setting Chart type   
//        //        chNonpaymentPreviousYearFinanceValue.Series[0].ChartType = SeriesChartType.Column;
//        //        chNonpaymentPreviousYearFinanceValue.Series[0].Points[0].Color = System.Drawing.ColorTranslator.FromHtml("#1cc88a");
//        //        chNonpaymentPreviousYearFinanceValue.Series[0]["PixelPointWidth"] = "10";
//        //    }

//        //}
//    }
//    catch (Exception ex)
//    {

//        U.ErrorLogger eL = new U.ErrorLogger();
//        eL.LogErrorInDB(ex, "AdminHome", "GetAdminDashboardCharts");
//        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastError", "toastError('An error occured, could not retrieve dashboard data!');", true);

//    }
//}
//private void Communications_Current_MonthChart()
//{
//    P.Daschboard_Provider frmF = new P.Daschboard_Provider();
//    ds = frmF.Get_Admin_Landing_Dashboard_Communications_Current_Month();

//    DataTable dt = new DataTable();
//    if (ds.Tables[0].Rows.Count > 0)
//    {
//        int month = DateTime.Now.Month;
//        string name = DateTime.Now.ToString("MMM");

//        dt = ds.Tables[0];
//        List<string> financers = (from p in dt.AsEnumerable()
//                                  select p.Field<string>("Financer")).Distinct().ToList();
//        List<string> communicationType = (from p in dt.AsEnumerable()
//                                          select p.Field<string>("Communication Type")).Distinct().ToList();


//        StringBuilder jsonData = new StringBuilder();
//        StringBuilder data = new StringBuilder();
//        Dictionary<string, string> chartConfig = new Dictionary<string, string>();

//        chartConfig.Add("caption", "Notifications");
//        chartConfig.Add("subcaption", "By Lender");
//        //chartConfig.Add("yaxisname", "Number of assets");
//        chartConfig.Add("numvisibleplot", "6");
//        chartConfig.Add("labeldisplay", "auto");
//        chartConfig.Add("theme", "fusion");

//        // json data to use as chart data source
//        jsonData.Append("{'chart':{");
//        foreach (var config in chartConfig)
//        {
//            jsonData.AppendFormat("'{0}':'{1}',", config.Key, config.Value);
//        }
//        jsonData.Replace(",", "},", jsonData.Length - 1, 1);
//        data.Append("'categories':[{");
//        data.Append("'category':[");

//        foreach (string f in financers)
//        {

//            data.Append("{'label': '" + f + "'},");

//        }
//        data.Replace(",", "", data.Length - 1, 1);
//        data.Append(" ]    }  ],");

//        data.Append("'dataset':[");
//        foreach (string comm in communicationType)
//        {
//            data.Append("{'seriesname':'" + comm + "',");
//            data.Append("'data':[");
//            //foreach (string comm in communicationType)
//            //{
//            bool found = false;
//            foreach (DataRow row in dt.Rows)
//            {
//                if (row["Communication Type"].ToString() == comm)
//                {

//                    data.Append("{'value': '" + row["Number Of Messages"].ToString() + "'},");
//                    found = true;
//                }
//            }
//            if (!found)
//            {
//                data.Append("{'value': '0'},");
//            }
//            //}
//            data.Replace(",", "", data.Length - 1, 1);
//            data.Append("]},");
//        }


//        data.Replace(",", "", data.Length - 1, 1);
//        data.Append("]}");
//        // data.Replace(",", "]", data.Length - 1, 1);

//        jsonData.Append(data.ToString());


//        Chart scrollColumn = new Chart("scrollstackedcolumn2d", "CommunicationsByFinancer", "100%", "400", "json", jsonData.ToString());
//       
//        CommunicationsByFinancer.Text = scrollColumn.Render();
//    }
//    else
//    {
//        divCommunicationsByFinancer.Attributes.Add("style", "display:none;");
//    }

//}
