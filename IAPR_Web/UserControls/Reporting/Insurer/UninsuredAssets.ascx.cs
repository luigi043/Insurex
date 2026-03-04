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

namespace IAPR_Web.UserControls.Reporting.Insurer
{
    public partial class UninsuredAssets : System.Web.UI.UserControl
    {
        CCom.CurrentUser objUser = new CCom.CurrentUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["SortOrder"] = "ASC";
                ViewState["SortField"] = "vcFinance_Agrreement_Number";
                P.User_Provider uP = new P.User_Provider();

                objUser = uP.GetUserFromSession();


                if (objUser == null)
                {
                    Response.Redirect("/account/login.aspx", false);
                }
                else
                {
                    if (objUser.iUser_Type_Id == 1 || objUser.iUser_Type_Id == 2)
                    {
                        pnlPartner.Visible = true;
                        Getformfields();

                    }
                    else
                    {
                        hdPartnerID.Value = IAPR_Data.Utils.CryptorEngine.GenericEncrypt(objUser.iPartner_Id.ToString(), true);
                        //GetAssetsFinanced(objUser.iPartner_Id);
                        Getformfields();
                        pnlAssetType.Visible = true;
                        GetUninsuredAssets(objUser.iPartner_Id, ViewState["SortField"].ToString(), "All");
                    }
                    hdAssetTypeFilter.Value = "ALL";
                }
            }
        }
        protected void rptUninsuredAssets_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                string[] param = e.CommandArgument.ToString().Split(new Char[] { ';' });
                GetAllAssetDetails(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]));
            }
        }
        public void Getformfields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsGeneric();
            ddlPartner.Items.Clear();
            ddlPartner.Items.Add(new ListItem("", ""));
            //Asset_Financier
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlPartner.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            //All Asset Types
            ddlAsset_type.Items.Clear();
            ddlAsset_type.Items.Add(new ListItem("All", "0"));
            foreach (DataRow row in ds.Tables[12].Rows)
            {
                ddlAsset_type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
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

        private void GetUninsuredAssets(int iPartner_Id, string sortField, string assetTypeFilter)
        {

            try
            {
                var sort = sortField;
                var asset = assetTypeFilter;
                var sortItem = typeof(C.AssetTypes.Uninsured_Assets).GetProperty(sort);
                var assetItem = typeof(C.AssetTypes.Uninsured_Assets).GetProperty(asset);

                rptUninsuredAssets.DataSource = null;
                rptUninsuredAssets.DataBind();

                List<C.AssetTypes.Uninsured_Assets> uninsured_AssetsL = new List<C.AssetTypes.Uninsured_Assets>();
                List<C.AssetTypes.Uninsured_Assets> uninsured_AssetsByType = new List<C.AssetTypes.Uninsured_Assets>();

                P.Report_Provider frmF = new P.Report_Provider();
                uninsured_AssetsL = frmF.Get_Uninsured_Assets_Insurer(iPartner_Id);

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
                        rptUninsuredAssets.DataSource = uninsured_AssetsByType.OrderBy(x => sortItem.GetValue(x, null)).ToList();

                    }
                    else
                    {
                        rptUninsuredAssets.DataSource = uninsured_AssetsByType.OrderByDescending(x => sortItem.GetValue(x, null)).ToList();

                    }
                    rptUninsuredAssets.DataBind();
                    rptUninsuredAssets.Visible = true;

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
            pnlUninsuredAssets.Visible = false;
            pnlAllDetails.Visible = true;
        }
        protected void ddlPartner_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlAssetType.Visible = true;
            ddlAsset_type.Visible = true;
            //GetAssetsFinanced(Convert.ToInt32(ddlPartner.SelectedValue));
            GetUninsuredAssets(Convert.ToInt32(ddlPartner.SelectedValue), ViewState["SortField"].ToString(), "All");
        }

        protected void ddlAsset_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();

            if (objUser.iUser_Type_Id == 1 || objUser.iUser_Type_Id == 2)
            {
                GetUninsuredAssets(Convert.ToInt32(ddlPartner.SelectedValue), ViewState["SortField"].ToString(), ddlAsset_type.SelectedItem.Text);
            }
            else
            {
                GetUninsuredAssets(objUser.iPartner_Id, ViewState["SortField"].ToString(), ddlAsset_type.SelectedItem.Text);
            }
            hdAssetTypeFilter.Value = ddlAsset_type.SelectedItem.Text;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlAssetType.Visible = true;
            pnlUninsuredAssets.Visible = true;
            pnlAllDetails.Visible = false;
        }
    }
}