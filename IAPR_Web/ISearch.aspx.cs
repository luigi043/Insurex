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


namespace IAPR_Web
{
    public partial class ISearch : System.Web.UI.Page
    {
        DataSet ds = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CheckQueryStrings())
                {
                    GetSearchForPolicy(Request.QueryString["pSearch"].ToString());
                }
            }
        }
        private bool CheckQueryStrings()
        {
            bool exists = true;
            string pSearch = Request.QueryString["pSearch"] as string;
            if (pSearch == null)
            {
                exists = false;
            }




            return exists;

        }
        private void GetSearchForPolicy(string vcPolicyNumber)
        {
            P.User_Provider uP = new P.User_Provider();
            CCom.CurrentUser objUser = new CCom.CurrentUser();

            objUser = uP.GetUserFromSession();

            if (objUser == null)
            {
                Response.Redirect("/account/login.aspx", false);
            }

            P.Search_Provider frmF = new P.Search_Provider();
            //DataSet ds = frmF.Get_Financer_Landing_Dashboard(U.CryptorEngine.GenericDecrypt(hdPolicyId.Value, true));
            ds = frmF.Get_Search_Insurer_By_PolicyNumber(Convert.ToInt32(objUser.iPartner_Id), vcPolicyNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                rptPolicies.DataSource = ds.Tables[0];
                rptPolicies.DataBind();
                rptPolicies.Visible = true;

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

        protected void rptPolicies_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName == "ViewPolicies")
                {
                    string[] param = e.CommandArgument.ToString().Split(new Char[] { ';' });
                    GetPolicyAssets(Convert.ToInt32(param[0]));
                    pnlPolicyList.Visible = false;
                    pnlAssetList.Visible = true;
                }
            }
        }
        protected void rptPolicies_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                Label lblSumFinance = (Label)e.Item.FindControl("lblSumFinance");
                Label lblNumberOfAssets = (Label)e.Item.FindControl("lblNumberOfAssets");

                lblSumInsurance.Text = SumInsurance.ToString("0,00.##");
                lblSumFinance.Text = Finance_Value.ToString("0,00.##");
                lblNumberOfAssets.Text = NumberOfAssets.ToString();

            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlPolicyList.Visible = true;
            pnlAssetList.Visible = false;
        }
    }
}