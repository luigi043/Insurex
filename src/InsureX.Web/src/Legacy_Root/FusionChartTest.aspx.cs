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

using FusionCharts.Charts;

namespace IAPR_Web
{
    public partial class FusionChartTest : System.Web.UI.Page
    {
        DataSet ds = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            // This page demonstrates the ease of generating charts using FusionCharts.
            // For this chart, we've used a pre-defined Data.xml (contained in /Data/ folder)
            // Ideally, you would NOT use a physical data file. Instead you'll have
            // your own ASP.NET scripts virtually relay the XML data document.
            // FusionCharts supports various data format, please comment the code for
            // current data format (Chart.DataFormat.xmlurl) and uncomment the required format to view respective examples.
            // For a head-start, we've kept this example very simple.

            // Create the chart - scrollstackedbar2d Chart with data from Data/Data.xml
            Chart sales = new Chart();

            // Setting chart id
            sales.SetChartParameter(Chart.ChartParameter.chartId, "myChart");

            // Setting chart type to scrollstackedbar2d chart
            sales.SetChartParameter(Chart.ChartParameter.chartType, "scrollstackedbar2d");

            // Setting chart width to 600px
            sales.SetChartParameter(Chart.ChartParameter.chartWidth, "600");

            // Setting chart height to 350px
            sales.SetChartParameter(Chart.ChartParameter.chartHeight, "350");

            // Setting chart data as JSON String (Uncomment below line
            sales.SetData("{\n  \"chart\": {\n    \"caption\": \"Video Games Sales - 2018\",\n    \"subcaption\": \"Across different markets (In Million USD)\",\n    \"numbersuffix\": \"M\",\n    \"plottooltext\": \"<b>$seriesName</b><hr>$label: <b>$dataValue</b>\",\n    \"numvisibleplot\": \"5\",\n    \"theme\": \"fusion\"\n  },\n  \"categories\": [\n    {\n      \"category\": [\n        {\n          \"label\": \"Wii Sports\"\n        },\n        {\n          \"label\": \"Super Mario Bros\"\n        },\n        {\n          \"label\": \"Mario Kart Wii\"\n        },\n        {\n          \"label\": \"Wii Sports Resort\"\n        },\n        {\n          \"label\": \"Pokemon Red/Blue\"\n        },\n        {\n          \"label\": \"Tetris\"\n        },\n        {\n          \"label\": \"New Super Mario Bros\"\n        },\n        {\n          \"label\": \"Wii Play\"\n        },\n        {\n          \"label\": \"New Super Mario Bros. Wii\"\n        },\n        {\n          \"label\": \"Duck Hunt\"\n        },\n        {\n          \"label\": \"Nintendogs\"\n        },\n        {\n          \"label\": \"Mario Kart DS\"\n        },\n        {\n          \"label\": \"Pokemon Gold/Silver\"\n        },\n        {\n          \"label\": \"Wii Fit\"\n        },\n        {\n          \"label\": \"Wii Fit Plus\"\n        },\n        {\n          \"label\": \"Kinect Adventures\"\n        },\n        {\n          \"label\": \"Grand Theft Auto V\"\n        },\n        {\n          \"label\": \"GTA: San Andreas\"\n        },\n        {\n          \"label\": \"Super Mario World\"\n        },\n        {\n          \"label\": \"Brain Age\"\n        },\n        {\n          \"label\": \"Pokemon Diamond/Pearl\"\n        },\n        {\n          \"label\": \"Super Mario Land\"\n        },\n        {\n          \"label\": \"Super Mario Bros. 3\"\n        },\n        {\n          \"label\": \"Grand Theft Auto V\"\n        },\n        {\n          \"label\": \"GTA: Vice City\"\n        }\n      ]\n    }\n  ],\n  \"dataset\": [\n    {\n      \"seriesname\": \"North America\",\n      \"data\": [\n        {\n          \"value\": \"41.49\"\n        },\n        {\n          \"value\": \"29.08\"\n        },\n        {\n          \"value\": \"15.85\"\n        },\n        {\n          \"value\": \"15.75\"\n        },\n        {\n          \"value\": \"11.27\"\n        },\n        {\n          \"value\": \"23.2\"\n        },\n        {\n          \"value\": \"11.38\"\n        },\n        {\n          \"value\": \"14.03\"\n        },\n        {\n          \"value\": \"14.59\"\n        },\n        {\n          \"value\": \"26.93\"\n        },\n        {\n          \"value\": \"9.07\"\n        },\n        {\n          \"value\": \"9.81\"\n        },\n        {\n          \"value\": \"9\"\n        },\n        {\n          \"value\": \"8.94\"\n        },\n        {\n          \"value\": \"9.09\"\n        },\n        {\n          \"value\": \"14.97\"\n        },\n        {\n          \"value\": \"7.01\"\n        },\n        {\n          \"value\": \"9.43\"\n        },\n        {\n          \"value\": \"12.78\"\n        },\n        {\n          \"value\": \"4.75\"\n        },\n        {\n          \"value\": \"6.42\"\n        },\n        {\n          \"value\": \"10.83\"\n        },\n        {\n          \"value\": \"9.54\"\n        },\n        {\n          \"value\": \"9.63\"\n        },\n        {\n          \"value\": \"8.41\"\n        }\n      ]\n    },\n    {\n      \"seriesname\": \"Europe \",\n      \"data\": [\n        {\n          \"value\": \"29.02\"\n        },\n        {\n          \"value\": \"3.58\"\n        },\n        {\n          \"value\": \"12.88\"\n        },\n        {\n          \"value\": \"11.01\"\n        },\n        {\n          \"value\": \"8.89\"\n        },\n        {\n          \"value\": \"2.26\"\n        },\n        {\n          \"value\": \"9.23\"\n        },\n        {\n          \"value\": \"9.2\"\n        },\n        {\n          \"value\": \"7.06\"\n        },\n        {\n          \"value\": \"0.63\"\n        },\n        {\n          \"value\": \"11\"\n        },\n        {\n          \"value\": \"7.57\"\n        },\n        {\n          \"value\": \"6.18\"\n        },\n        {\n          \"value\": \"8.03\"\n        },\n        {\n          \"value\": \"8.59\"\n        },\n        {\n          \"value\": \"4.94\"\n        },\n        {\n          \"value\": \"9.27\"\n        },\n        {\n          \"value\": \"0.4\"\n        },\n        {\n          \"value\": \"3.75\"\n        },\n        {\n          \"value\": \"9.26\"\n        },\n        {\n          \"value\": \"4.52\"\n        },\n        {\n          \"value\": \"2.71\"\n        },\n        {\n          \"value\": \"3.44\"\n        },\n        {\n          \"value\": \"5.31\"\n        },\n        {\n          \"value\": \"5.49\"\n        }\n      ]\n    },\n    {\n      \"seriesname\": \"Japan\",\n      \"data\": [\n        {\n          \"value\": \"3.77\"\n        },\n        {\n          \"value\": \"6.81\"\n        },\n        {\n          \"value\": \"3.79\"\n        },\n        {\n          \"value\": \"3.28\"\n        },\n        {\n          \"value\": \"10.22\"\n        },\n        {\n          \"value\": \"4.22\"\n        },\n        {\n          \"value\": \"6.5\"\n        },\n        {\n          \"value\": \"2.93\"\n        },\n        {\n          \"value\": \"4.7\"\n        },\n        {\n          \"value\": \"0.28\"\n        },\n        {\n          \"value\": \"1.93\"\n        },\n        {\n          \"value\": \"4.13\"\n        },\n        {\n          \"value\": \"7.2\"\n        },\n        {\n          \"value\": \"3.6\"\n        },\n        {\n          \"value\": \"2.53\"\n        },\n        {\n          \"value\": \"0.24\"\n        },\n        {\n          \"value\": \"0.97\"\n        },\n        {\n          \"value\": \"0.41\"\n        },\n        {\n          \"value\": \"3.54\"\n        },\n        {\n          \"value\": \"4.16\"\n        },\n        {\n          \"value\": \"6.04\"\n        },\n        {\n          \"value\": \"4.18\"\n        },\n        {\n          \"value\": \"3.84\"\n        },\n        {\n          \"value\": \"0.06\"\n        },\n        {\n          \"value\": \"0.47\"\n        }\n      ]\n    },\n    {\n      \"seriesname\": \" Rest of the world\",\n      \"data\": [\n        {\n          \"value\": \"8.46\"\n        },\n        {\n          \"value\": \"0.77\"\n        },\n        {\n          \"value\": \"3.31\"\n        },\n        {\n          \"value\": \"2.96\"\n        },\n        {\n          \"value\": \"1\"\n        },\n        {\n          \"value\": \"0.58\"\n        },\n        {\n          \"value\": \"2.9\"\n        },\n        {\n          \"value\": \"2.85\"\n        },\n        {\n          \"value\": \"2.26\"\n        },\n        {\n          \"value\": \"0.47\"\n        },\n        {\n          \"value\": \"2.75\"\n        },\n        {\n          \"value\": \"1.92\"\n        },\n        {\n          \"value\": \"0.71\"\n        },\n        {\n          \"value\": \"2.15\"\n        },\n        {\n          \"value\": \"1.79\"\n        },\n        {\n          \"value\": \"1.67\"\n        },\n        {\n          \"value\": \"4.14\"\n        },\n        {\n          \"value\": \"10.57\"\n        },\n        {\n          \"value\": \"0.55\"\n        },\n        {\n          \"value\": \"2.05\"\n        },\n        {\n          \"value\": \"1.37\"\n        },\n        {\n          \"value\": \"0.42\"\n        },\n        {\n          \"value\": \"0.46\"\n        },\n        {\n          \"value\": \"1.38\"\n        },\n        {\n          \"value\": \"1.78\"\n        }\n      ]\n    }\n  ]\n}", Chart.DataFormat.json);

            Literal1.Text = sales.Render();
            GetAdminDashboardCharts();
        }

        private void GetAdminDashboardTable()
        {
            try
            {
                P.Daschboard_Provider frmF = new P.Daschboard_Provider();
                //List<CCom.Dashboards.FinancerLandingTableTotals> financerLandingTableTotals = new List<CCom.Dashboards.FinancerLandingTableTotals>();

                ds = frmF.Get_Admin_Landing_DashboardTable();
                CCom.Dashboards.FinancerLandingTableTotals financerLandingTableTotals = new CCom.Dashboards.FinancerLandingTableTotals();

                //financerLandingTableTotals.iNumber_Of_Uninsued_Assets = Convert.ToInt32(ds.Tables[0].Rows[0]["iNumber_Of_Uninsued_Assets"].ToString());
                //financerLandingTableTotals.dcUninsured_Finance_Value = Convert.ToDecimal(ds.Tables[0].Rows[0]["dcUninsured_Finance_Value"].ToString());
                //financerLandingTableTotals.dcUninsured_Insurance_Value = Convert.ToDecimal(ds.Tables[0].Rows[0]["dcUninsured_Insurance_Value"].ToString());

                //financerLandingTableTotals.iNumber_Of_Assets = Convert.ToInt32(ds.Tables[1].Rows[0]["iNumber_Of_Assets"].ToString());
                //financerLandingTableTotals.dcFinance_Value = Convert.ToDecimal(ds.Tables[1].Rows[0]["dcFinance_Value"].ToString());
                //financerLandingTableTotals.dcInsurance_Value = Convert.ToDecimal(ds.Tables[1].Rows[0]["dcInsurance_Value"].ToString());


                //lblUninsuredAssetsTotal.Text = financerLandingTableTotals.iNumber_Of_Uninsued_Assets.ToString();
                //lblUninsuredValue.Text = financerLandingTableTotals.dcUninsured_Finance_Value.ToString("C", new CultureInfo("en-ZA")); //Convert.ToDecimal(fv)).ToString("C", new CultureInfo("en-ZA")))
                //lblTotaAssets.Text = financerLandingTableTotals.iNumber_Of_Assets.ToString();
                //lblTotalFinanceValue.Text = financerLandingTableTotals.dcFinance_Value.ToString("C", new CultureInfo("en-ZA"));
                //int PercUninsuredAssets = 0, PercUninsuredValue = 0;

                //PercUninsuredAssets = financerLandingTableTotals.iNumber_Of_Assets > 0 ? Convert.ToInt32((decimal.Divide(financerLandingTableTotals.iNumber_Of_Uninsued_Assets, financerLandingTableTotals.iNumber_Of_Assets) * 100)) : 0;
                //// lblPercUninsuredAssets.Text = PercUninsuredAssets.ToString() + "%";
                //UninsuredPercentage(financerLandingTableTotals);

                ////divUninsuredAssetsPerc.Style.Clear();
                ////divUninsuredAssetsPerc.Style.Add("width", PercUninsuredAssets + "%");

                //if (PercUninsuredAssets > 0)
                //{
                //    // divUninsuredAssetsPerc.Attributes.Add("class", "progress-bar bg-danger");
                //    divUninsured_Assest.Attributes.Add("class", "card border-left-danger shadow h-100 py-2 dashBackground");
                //    divUninsured_Value.Attributes.Add("class", "card border-left-danger shadow h-100 py-2 dashBackground");
                //}
                //else
                //{
                //    // divUninsuredAssetsPerc.Attributes.Add("class", "progress-bar bg-success");
                //}



                //PercUninsuredValue = financerLandingTableTotals.dcFinance_Value > 0 ? Convert.ToInt32((decimal.Divide(financerLandingTableTotals.dcUninsured_Finance_Value, financerLandingTableTotals.dcFinance_Value) * 100)) : 0;



            }
            catch (Exception ex)
            {

                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AdminHome", "GetAdminDashboardTable");
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastError", "toastError('An error occured!');", true);

            }
        }
        private void GetAdminDashboardCharts()
        {
            try
            {
                UninsuredByFinancerChart();
                NonPaymentReInstatedByMonthChart();
                ArrearVsUnconfirmedChart();
                Communications_Current_MonthChart();

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
                //chartConfig.Add("labeldisplay", "slant");
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
                //render chart
                chartUninsuredByFinancer.Text = scrollColumn.Render();
            }
        }
        private void NonPaymentReInstatedByMonthChart()
        {
            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_Dashboard_NonPayment_History();
            DataTable dtUnpaid = new DataTable();
            DataTable dtReInstated = new DataTable();

            int month = DateTime.Now.Month;
            string name = DateTime.Now.ToString("MMM");
            dtUnpaid = ds.Tables[0];
            dtReInstated = ds.Tables[1];


            //if (dt.Rows.Count > 0)
            //{


            StringBuilder jsonData = new StringBuilder();
            StringBuilder data = new StringBuilder();
            Dictionary<string, string> chartConfig = new Dictionary<string, string>();

            chartConfig.Add("caption", "Premiums Unpaid vs Re-Instated Cover");
            chartConfig.Add("subcaption", "Previous year");
            chartConfig.Add("yaxisname", "Number of assets");
            chartConfig.Add("numvisibleplot", "12");
            chartConfig.Add("labeldisplay", "auto");
            chartConfig.Add("theme", "fusion");

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

            List<string> previousMonths = new List<string>();

            int i = 11;
            for (int j = 1; j < 13; j++)
            {
                previousMonths.Add(DateTime.Now.AddMonths(-i).ToString("MMM"));
                data.Append("{'label': '" + DateTime.Now.AddMonths(-i).ToString("MMM") + "'},");
                i = i - 1;
            }
            data.Replace(",", "", data.Length - 1, 1);
            data.Append(" ]    }  ],");

            data.Append("'dataset':[");
            //foreach (string l in financers)
            //{
            data.Append("{'seriesname':'Unpaid Premiums',");
            data.Append("'data':[");
            foreach (string pmU in previousMonths)
            {
                bool found = false;
                foreach (DataRow row in dtUnpaid.Rows)
                {
                    if (row["vcMonthName"].ToString() == pmU)//row["Financer"].ToString() == l &&
                    {

                        data.Append("{'value': '" + row["Unpaid Insurance Premiums"].ToString() + "'},");
                        found = true;
                    }
                    //else
                    //{
                    //    data.Append("{'value': '0'},");
                    //}
                }
                if (!found)
                {
                    data.Append("{'value': '0'},");
                }
            }
            data.Append("]},");


            data.Append("{'seriesname':'Re-Instated Cover',");
            data.Append("'data':[");
            foreach (string pmR in previousMonths)
            {
                bool found = false;

                foreach (DataRow row in dtReInstated.Rows)
                {
                    if (row["vcMonthName"].ToString() == pmR)//row["Financer"].ToString() == l &&
                    {
                        data.Append("{'value': '" + row["Re-instated Cover"].ToString() + "'},");
                        found = true;
                    }
                    //else
                    //{
                    //    data.Append("{'value': '0'},");
                    //}
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


            Chart scrollColumn = new Chart("msspline", "NonPaymentReInstatedByMonthChart", "100%", "400", "json", jsonData.ToString());
            //render chart
            chartNonPaymentReInstatedByMonth.Text = scrollColumn.Render();
            // }

        }
        private void ArrearVsUnconfirmedChart()
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

                chartConfig.Add("caption", "Unconfimed Cover vs Premium Unpaid");
                chartConfig.Add("xAxisName", "Unpaid premiums");
                chartConfig.Add("yAxisName", "Unconfirmed cover");
                //chartConfig.Add("numberSuffix", "k");
                chartConfig.Add("showpercentvalues", "1");
                chartConfig.Add("usedataplotcolorforlabels", "1");
                chartConfig.Add("showLabels", "0");
                chartConfig.Add("showValues", "0");
                //chartConfig.Add("centerlabel", "$label: $value");
                chartConfig.Add("labelFontColor", "#000000");
                chartConfig.Add("theme", "fusion");

                // json data to use as chart data source
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
                //render chart
                chartUninsuredvsUnconfirmed.Text = doughnut2d.Render();
            }
        }

        private void UninsuredPercentage(CCom.Dashboards.FinancerLandingTableTotals financerLandingTableTotals)
        {
            int PercUninsuredAssets = 0, PercUninsuredValue = 0;

            //PercUninsuredAssets = financerLandingTableTotals.iNumber_Of_Assets > 0 ? Convert.ToInt32((decimal.Divide(financerLandingTableTotals.iNumber_Of_Uninsued_Assets, financerLandingTableTotals.iNumber_Of_Assets) * 100)) : 0;

            //Chart uninsuredPercentage = new Chart();
            //uninsuredPercentage.SetChartParameter(Chart.ChartParameter.chartId, "uninsuredPercentage");
            //uninsuredPercentage.SetChartParameter(Chart.ChartParameter.chartType, "angulargauge");
            //uninsuredPercentage.SetChartParameter(Chart.ChartParameter.chartWidth, "100%");
            //uninsuredPercentage.SetChartParameter(Chart.ChartParameter.chartHeight, "400");
            //uninsuredPercentage.SetData("{\n  \"chart\": {\n    \"caption\": \"Current Percentage of Uninsured Assets\",\n    \"lowerlimit\": \"0\",\n    \"upperlimit\": \"100\",\n    \"showvalue\": \"1\",\n    \"" + PercUninsuredAssets.ToString() + "\": \"%\",\n    \"theme\": \"fusion\",\n    \"showtooltip\": \"0\"\n  },\n  \"colorrange\": {\n    \"color\": [\n      {\n        \"minvalue\": \"0\",\n        \"maxvalue\": \"50\",\n        \"code\": \"#62B58F\"\n      },\n      {\n        \"minvalue\": \"50\",\n        \"maxvalue\": \"75\",\n        \"code\": \"#FFC533\"\n      },\n      {\n        \"minvalue\": \"75\",\n        \"maxvalue\": \"100\",\n        \"code\": \"#F2726F\"\n      }\n    ]\n  },\n  \"dials\": {\n    \"dial\": [\n      {\n        \"value\": \"" + PercUninsuredAssets.ToString() + "\"\n      }\n    ]\n  }\n}", Chart.DataFormat.json);
            //ChartUninsuredAssetPercentage.Text = uninsuredPercentage.Render();
        }

        private void Communications_Current_MonthChart()
        {
            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            ds = frmF.Get_Admin_Landing_Dashboard_Communications_Current_Month();

            DataTable dt = new DataTable();

            int month = DateTime.Now.Month;
            string name = DateTime.Now.ToString("MMM");

            dt = ds.Tables[0];
            List<string> financers = (from p in dt.AsEnumerable()
                                      select p.Field<string>("Financer")).Distinct().ToList();
            List<string> communicationType = (from p in dt.AsEnumerable()
                                              select p.Field<string>("Communication Type")).Distinct().ToList();


            StringBuilder jsonData = new StringBuilder();
            StringBuilder data = new StringBuilder();
            Dictionary<string, string> chartConfig = new Dictionary<string, string>();

            chartConfig.Add("caption", "Communications Sent");
            chartConfig.Add("subcaption", "By Lender");
            //chartConfig.Add("yaxisname", "Number of assets");
            chartConfig.Add("numvisibleplot", "6");
            chartConfig.Add("labeldisplay", "auto");
            chartConfig.Add("theme", "fusion");

            // json data to use as chart data source
            jsonData.Append("{'chart':{");
            foreach (var config in chartConfig)
            {
                jsonData.AppendFormat("'{0}':'{1}',", config.Key, config.Value);
            }
            jsonData.Replace(",", "},", jsonData.Length - 1, 1);
            data.Append("'categories':[{");
            data.Append("'category':[");

            foreach (string f in financers)
            {

                data.Append("{'label': '" + f + "'},");

            }
            data.Replace(",", "", data.Length - 1, 1);
            data.Append(" ]    }  ],");

            data.Append("'dataset':[");
            foreach (string comm in communicationType)
            {
                data.Append("{'seriesname':'" + comm + "',");
                data.Append("'data':[");
                //foreach (string comm in communicationType)
                //{
                bool found = false;
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Communication Type"].ToString() == comm)
                    {

                        data.Append("{'value': '" + row["Number Of Messages"].ToString() + "'},");
                        found = true;
                    }
                }
                if (!found)
                {
                    data.Append("{'value': '0'},");
                }
                //}
                data.Replace(",", "", data.Length - 1, 1);
                data.Append("]},");
            }


            data.Replace(",", "", data.Length - 1, 1);
            data.Append("]}");
            // data.Replace(",", "]", data.Length - 1, 1);

            jsonData.Append(data.ToString());


            Chart scrollColumn = new Chart("scrollstackedcolumn2d", "CommunicationsByFinancer", "100%", "400", "json", jsonData.ToString());
            //render chart
            CommunicationsByFinancer.Text = scrollColumn.Render();


        }
    }
}