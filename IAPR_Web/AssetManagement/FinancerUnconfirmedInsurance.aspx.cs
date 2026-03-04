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
using System.Configuration;
namespace IAPR_Web.AssetManagement
{
    public partial class FinancerUnconfirmedInsurance : System.Web.UI.Page
    {
        CCom.CurrentUser objUser = new CCom.CurrentUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                P.User_Provider uP = new P.User_Provider();

                objUser = uP.GetUserFromSession();
                ViewState["SortField"] = "vcFinancer_Name";
                ViewState["SortOrder"] = "ASC";
                GetUnconfirmedAssets(objUser.iPartner_Id, ViewState["SortField"].ToString(), "All");
                GetAssetsFinanced(objUser.iPartner_Id);
            }
        }

        protected void ddlAsset_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();

            GetUnconfirmedAssets(objUser.iPartner_Id, ViewState["SortField"].ToString(), ddlAsset_type.SelectedItem.Text);

            hdAssetTypeFilter.Value = ddlAsset_type.SelectedItem.Text;
        }

        protected void rptUnconfirmedAssets_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                string[] param = e.CommandArgument.ToString().Split(new Char[] { ';' });
                GetAllAssetDetails(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]));
                if (!Convert.ToBoolean(param[2]))
                {
                    hdAlgnmtID.Value = U.CryptorEngine.GenericEncrypt(param[3], true);
                    btnResendConfirm.Visible = true;
                }
            }
        }
        private void GetUnconfirmedAssets(int iPartner_Id, string sortField, string assetTypeFilter)
        {

            try
            {
                var sort = sortField;
                var asset = assetTypeFilter;
                var sortItem = typeof(C.AssetTypes.Uninsured_Assets).GetProperty(sort);
                var assetItem = typeof(C.AssetTypes.Uninsured_Assets).GetProperty(asset);

                rptUnconfirmedAssets.DataSource = null;
                rptUnconfirmedAssets.DataBind();

                List<C.AssetTypes.Uninsured_Assets> uninsured_AssetsL = new List<C.AssetTypes.Uninsured_Assets>();
                List<C.AssetTypes.Uninsured_Assets> uninsured_AssetsByType = new List<C.AssetTypes.Uninsured_Assets>();

                P.Generic_Asset_Provider frmF = new P.Generic_Asset_Provider();
                uninsured_AssetsL = frmF.Get_Unconfirmed_Assets_Financer(iPartner_Id);

                if (assetTypeFilter != "All")
                {
                    uninsured_AssetsByType = uninsured_AssetsL.Where(x => x.vcAsset_Type_Description == assetTypeFilter).ToList();
                }
                else
                {
                    uninsured_AssetsByType = uninsured_AssetsL.ToList();
                }



                if (uninsured_AssetsByType.Count > 0)
                {
                    if (ViewState["SortOrder"].ToString() == "ASC")
                    {
                        rptUnconfirmedAssets.DataSource = uninsured_AssetsByType.OrderBy(x => sortItem.GetValue(x, null)).ToList();

                    }
                    else
                    {
                        rptUnconfirmedAssets.DataSource = uninsured_AssetsByType.OrderByDescending(x => sortItem.GetValue(x, null)).ToList();

                    }
                    rptUnconfirmedAssets.DataBind();
                    rptUnconfirmedAssets.Visible = true;

                }
                ViewState["SortField"] = sortField;

            }
            catch (Exception)
            {

                throw;
            }


        }
        private void GetAllAssetDetails(int iAsset_Id, int iAsset_Type_Id)
        {
            P.Generic_Asset_Provider pro = new P.Generic_Asset_Provider();
            DataSet ds = pro.Get_Asset_All_Details_By_Asset_ID(iAsset_Type_Id, iAsset_Id);
            System.Text.StringBuilder s = new System.Text.StringBuilder();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                foreach (DataColumn c in ds.Tables[0].Columns)
                {
                    //if (c.ColumnName.ToString() == "Asset description" || c.ColumnName.ToString() == "Asset sub-type description"
                    //    || c.ColumnName.ToString() == "Make" || c.ColumnName.ToString() == "Model"
                    //    || c.ColumnName.ToString() == "Model Variant"
                    //    || c.ColumnName.ToString() == "Finance value")
                    //{
                    if (c.ColumnName.ToString() == "Finance value")
                    {
                        string fv = row[c].ToString();
                        s.Append(c.ColumnName + ": " + (Convert.ToDecimal(fv)).ToString("C", new CultureInfo("en-ZA")));// string.Format("{0,C}", fv) + "<br /><br />");
                    }
                    else
                    {
                        s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                    }
                    //}
                    //else
                    //{
                    //    s.Append(c.ColumnName + ": " + U.CryptorEngine.GenericDecrypt(row[c].ToString(), true) + "<br /><br />");
                    //}
                }
            }
            divAssetDetails.InnerHtml = s.ToString();

            s.Clear();
            if (ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    foreach (DataColumn c in ds.Tables[1].Columns)
                    {
                        if (c.ColumnName != "Policy status")
                        {
                            s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                        }
                        else
                        {
                            if (row[c].ToString() == "Active")
                            {
                                s.Append(c.ColumnName + ": <span style='color: Green; font-weight: bold;'>" + row[c] + "</span><br /><br />");
                            }
                            else
                            {
                                s.Append(c.ColumnName + ": <span style='color: Red;font-weight: bold;'>" + row[c] + "</span><br /><br />");
                            }

                        }
                    }
                }
            }
            else
            {
                s.Append("Policy details unconfirmed<br /><br />");
            }

            divPolicyDetails.InnerHtml = s.ToString();


            s.Clear();
            if (ds.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    foreach (DataColumn c in ds.Tables[2].Columns)
                    {
                        s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                    }
                }
            }
            else
            {
                s.Append("Customer details unconfirmed<br /><br />");
            }
            divCustomerDeatils.InnerHtml = s.ToString();


            s.Clear();
            if (ds.Tables[3].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[3].Rows)
                {
                    foreach (DataColumn c in ds.Tables[3].Columns)
                    {
                        s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                    }
                }
            }
            else
            {
                s.Append("Details unconfirmed<br /><br />");
            }
            divPhysicalAddress.InnerHtml = s.ToString();


            s.Clear();

            if (ds.Tables[4].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[4].Rows)
                {
                    foreach (DataColumn c in ds.Tables[4].Columns)
                    {
                        s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                    }
                }
            }
            else
            {
                s.Append("Details unconfirmed<br /><br />");
            }
            divPostalAddress.InnerHtml = s.ToString();
            pnlAssetType.Visible = false;
            pnlUnconfirmedAssets.Visible = false;
            pnlAllDetails.Visible = true;
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
        public void GetAssetsFinanced(int iPartner_Id)
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsAssetsFinancedByFinancer(iPartner_Id);
            ddlAsset_type.ClearSelection();
            ddlAsset_type.Items.Clear();
            //ddlAsset_type.Items.Add(new ListItem("", ""));
            ddlAsset_type.Items.Add(new ListItem("All", "0"));
            //Asset_Financier
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlAsset_type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
        }
        protected void btnResendConfirm_Click(object sender, EventArgs e)
        {
            try
            {


                NotifyCustomer(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAlgnmtID.Value, true)));
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Request sent');", true);
                btnResendConfirm.Visible = false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlAssetType.Visible = true;
            pnlUnconfirmedAssets.Visible = true;
            pnlAllDetails.Visible = false;
        }
    }
}