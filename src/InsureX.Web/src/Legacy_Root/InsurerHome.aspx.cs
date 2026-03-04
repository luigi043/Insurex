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
namespace IAPR_Web
{
    public partial class InsurerHome : System.Web.UI.Page
    {
        DataSet ds = null;
        CCom.CurrentUser objUser = new CCom.CurrentUser();
        P.User_Provider uP = new P.User_Provider();

        string[] pointColours = { "#4e73df", "#fd7e14", "#f6c23e", "#1cc88a", "#858796", "#1cc88a", "#5a5c69", "#36b9cc", "#e74a3b", "#e83e8c", "#fff" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                P.User_Provider uP = new P.User_Provider();


                objUser = uP.GetUserFromSession();

                if (objUser == null)
                {
                    Response.Redirect("/account/login.aspx", false);
                }
                else
                {
                    GetInsurerLandingUninsuredTable();
                    GetInsurerLandingAsseTotals();
                }
            }
        }
        private void GetInsurerLandingUninsuredTable()
        {


            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            //DataSet ds = frmF.Get_Financer_Landing_Dashboard(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true));
            ds = frmF.Get_Insurer_Landing_Uninsured_Table(Convert.ToInt32(objUser.iPartner_Id));
            if (ds.Tables[0].Rows.Count > 0)
            {
                rptCurrentlyUninsured.DataSource = ds.Tables[0];
                rptCurrentlyUninsured.DataBind();
                rptCurrentlyUninsured.Visible = true;
                rptCurrentlyUninsured.Visible = true;
            }
        }
        private void GetPolicyAssets(int vcPolicy_Id)
        {
            try
            {

                pnlAssetList.Visible = true;

                P.Policy_Provider frmF = new P.Policy_Provider();
                DataSet ds = frmF.GetPolicy_All_Assets(vcPolicy_Id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    rptAssetList.DataSource = ds.Tables[0];
                    rptAssetList.DataBind();
                    pnlAssetList.Visible = true;
                    rptAssetList.Visible = true;
                }

            }
            catch (Exception)
            {

                throw;
            }


        }
        protected void rptCurrentlyUninsured_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName == "ViewPolicies")
                {

                    string[] param = e.CommandArgument.ToString().Split(new Char[] { ';' });
                    GetPolicyAssets(Convert.ToInt32(param[0]));
                    plInsurerTotals.Visible = false;
                    pnlCurrentlyUninsured.Visible = false;
                    pnlAssetList.Visible = true;
                }
            }
        }
        protected void rptCurrentlyUninsured_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RepeaterItem item = e.Item;
                //decimal SumInsurance = Convert.ToDecimal((item.FindControl("lblSumInsurance") as Label).Text);
                //decimal Finance_Value = Convert.ToDecimal((item.FindControl("lblFinance_Value") as Label).Text);
                string policyNumber = (item.FindControl("lblPolicy_Number") as Label).Text;
                decimal Finance_Value = 0;
                decimal SumInsurance = 0;
                int NumberOfAssets = 0;
                foreach (DataRow r in ds.Tables[1].Rows)
                {
                    if (r[0].ToString() == policyNumber)
                    {
                        Finance_Value = Convert.ToDecimal(r[1].ToString());
                        SumInsurance = Convert.ToDecimal(r[2].ToString());
                        NumberOfAssets = Convert.ToInt32(r[3].ToString());
                    }
                }
                Label lblSumInsurance = (Label)e.Item.FindControl("lblSumInsurance");
                //Label lblSumFinance = (Label)e.Item.FindControl("lblSumFinance");
                Label lblNumberOfAssets = (Label)e.Item.FindControl("lblNumberOfAssets");

                lblSumInsurance.Text = "R " + SumInsurance.ToString("N", new CultureInfo("en-US")); //SumInsurance.ToString("0.##");
                //lblSumFinance.Text = "R " + Finance_Value.ToString("N", new CultureInfo("en-US")); //Finance_Value.ToString("0.##");
                lblNumberOfAssets.Text = NumberOfAssets.ToString();

            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            plInsurerTotals.Visible = true;
            pnlCurrentlyUninsured.Visible = true;
            pnlAssetList.Visible = false;
        }



        private void GetInsurerLandingAsseTotals()
        {

            objUser = uP.GetUserFromSession();
            P.Daschboard_Provider frmF = new P.Daschboard_Provider();
            CCom.Dashboards.InsurerLandingTableTotals insurerLandingTableTotals = new CCom.Dashboards.InsurerLandingTableTotals();

            ds = frmF.Get_Insurer_Landing_Asset_Totals(Convert.ToInt32(objUser.iPartner_Id));


            if (ds.Tables[0].Rows.Count > 0)
            {

                insurerLandingTableTotals.iNumber_Of_Uninsured_Assets = Convert.ToInt32(ds.Tables[0].Rows[0]["iNumber_Of_Uninsured_Assets"].ToString());
                insurerLandingTableTotals.dcInsurance_Uninsured_Value = Convert.ToDecimal(ds.Tables[0].Rows[0]["dcInsurance_Uninsured_Value"].ToString());

            }
            else
            {
                insurerLandingTableTotals.iNumber_Of_Uninsured_Assets = 0;
                insurerLandingTableTotals.dcInsurance_Uninsured_Value = 0;
            }


            if (ds.Tables[1].Rows.Count > 0)
            {

                insurerLandingTableTotals.iTotal_Number_Of_Assets = Convert.ToInt32(ds.Tables[1].Rows[0]["iTotal_Number_Of_Assets"].ToString());
                insurerLandingTableTotals.dcTotal_Insurance_Value = Convert.ToDecimal(ds.Tables[1].Rows[0]["dcTotal_Insurance_Value"].ToString());

            }
            else
            {
                insurerLandingTableTotals.iTotal_Number_Of_Assets = 0;
                insurerLandingTableTotals.dcTotal_Insurance_Value = 0;
            }

            lblUninsuredAssetsTotal.Text = insurerLandingTableTotals.iNumber_Of_Uninsured_Assets.ToString();
            lblUninsuredValue.Text = insurerLandingTableTotals.dcInsurance_Uninsured_Value.ToString("C", new CultureInfo("en-ZA")); //Convert.ToDecimal(fv)).ToString("C", new CultureInfo("en-ZA")))
            lblTotaAssets.Text = insurerLandingTableTotals.iTotal_Number_Of_Assets.ToString();
            lblTotalFinanceValue.Text = insurerLandingTableTotals.dcTotal_Insurance_Value.ToString("C", new CultureInfo("en-ZA"));
            int PercUninsuredAssets = 0, PercUninsuredValue = 0;


            PercUninsuredAssets = insurerLandingTableTotals.iTotal_Number_Of_Assets > 0 ? Convert.ToInt32((decimal.Divide(insurerLandingTableTotals.iNumber_Of_Uninsured_Assets, insurerLandingTableTotals.iTotal_Number_Of_Assets) * 100)) : 0;
            lblPercUninsuredAssets.Text = PercUninsuredAssets.ToString() + "%";
            divUninsuredAssetsPerc.Style.Clear();
            divUninsuredAssetsPerc.Style.Add("width", PercUninsuredAssets + "%");

            if (PercUninsuredAssets > 0)
            {
                divUninsuredAssetsPerc.Attributes.Add("class", "progress-bar bg-danger");
                divUninsured_Assest.Attributes.Add("class", "card border-left-danger shadow h-100 py-2 dashBackground");
                divUninsured_Value.Attributes.Add("class", "card border-left-danger shadow h-100 py-2 dashBackground");
            }
            else
            {
                divUninsuredAssetsPerc.Attributes.Add("class", "progress-bar bg-success");
            }


            PercUninsuredValue = insurerLandingTableTotals.dcTotal_Insurance_Value > 0 ? Convert.ToInt32((decimal.Divide(insurerLandingTableTotals.dcInsurance_Uninsured_Value, insurerLandingTableTotals.dcTotal_Insurance_Value) * 100)) : 0;

            lblPercUninsuredValue.Text = PercUninsuredValue.ToString() + "%";
            divUninsuredValuePerc.Style.Clear();
            divUninsuredValuePerc.Style.Add("width", PercUninsuredValue + "%");
            if (PercUninsuredValue > 0 && PercUninsuredValue <= 100)
            {
                divUninsuredValuePerc.Attributes.Add("class", "progress-bar bg-danger");
            }
            else
            {
                divUninsuredValuePerc.Attributes.Add("class", "progress-bar bg-success");
                divUninsured_Assets_Perc.Attributes.Add("class", "card border-left-success shadow h-100 py-2 dashBackground");
                divUninsured_Value_Perc.Attributes.Add("class", "card border-left-success shadow h-100 py-2 dashBackground");
            }

            ////if (ds.Tables[0].Rows.Count > 0)
            ////{
            //rptInsurerTotals.DataSource = insurerLandingTableTotals;// ds.Tables[0];
            //rptInsurerTotals.DataBind();
            //rptInsurerTotals.Visible = true;

            //}
        }
        protected void rptInsurerTotals_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RepeaterItem item = e.Item;

                int NumberInsurerAssets = 0;// Convert.ToInt32((item.FindControl("lblNumberInsurerAssets") as Label).Text);
                decimal TotaInsurerValue = 0;// Convert.ToDecimal((item.FindControl("lblTotaInsurerValue") as Label).Text);

                //int Total_Number_Of_Assets = 0;
                //decimal Total_Finance_Value = 0;
                //decimal Total_Insurance_Value = 0;

                foreach (DataRow r in ds.Tables[1].Rows)
                {
                    NumberInsurerAssets = Convert.ToInt32(r["iNumber_Of_Assets"].ToString());
                    TotaInsurerValue = Convert.ToDecimal(r["dcInsurance_Value"].ToString());

                }

                Label lblNumberUninsuredAssets = (Label)e.Item.FindControl("lblNumberUninsuresAssets");
                Label lblTotaUninsuredValue = (Label)e.Item.FindControl("lblTotaUninsuredValue");

                int NumberUninsuredAssets = Convert.ToInt32(lblNumberUninsuredAssets.Text);
                decimal TotaUninsuredValue = Convert.ToDecimal(lblTotaUninsuredValue.Text);

                Label lblNumberInsurerAssets = (Label)e.Item.FindControl("lblNumberInsurerAssets");
                Label lblTotaInsurerValue = (Label)e.Item.FindControl("lblTotaInsurerValue");

                Label lblPercNumberUninsuredAssets = (Label)e.Item.FindControl("lblPercNumberUninsuredAssets");
                Label lblPercTotalInsuranceValueAsset = (Label)e.Item.FindControl("lblPercTotalInsuranceValueAsset");


                lblNumberInsurerAssets.Text = NumberInsurerAssets.ToString();
                lblTotaInsurerValue.Text = "R " + TotaInsurerValue.ToString("N", new CultureInfo("en-US"));//.ToString("#,##0.##");


                if (NumberInsurerAssets > 0)
                {
                    lblPercNumberUninsuredAssets.Text = (decimal.Divide(NumberUninsuredAssets, NumberInsurerAssets) * 100).ToString("0.##");
                }
                if (TotaUninsuredValue > 0)
                {
                    lblPercTotalInsuranceValueAsset.Text = (decimal.Divide(TotaUninsuredValue, TotaInsurerValue) * 100).ToString("0.##");
                }
                //Label lblFinance_Value = (Label)e.Item.FindControl("lblFinance_Value");
                //lblFinance_Value.Text = "R " + Finance_Value.ToString("N", new CultureInfo("en-US"));//.ToString("#,##0.##");
            }
        }
    }
}