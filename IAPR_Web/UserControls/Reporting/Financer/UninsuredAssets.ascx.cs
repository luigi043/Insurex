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
namespace IAPR_Web.UserControls.Reporting.Financer
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
                if (objUser.iUser_Type_Id == 1 || objUser.iUser_Type_Id == 2)
                {
                    pnlPartner.Visible = true;
                    Getformfields();

                }
                else
                {
                    hdPartnerID.Value = IAPR_Data.Utils.CryptorEngine.GenericEncrypt(objUser.iPartner_Id.ToString(), true);
                    GetAssetsFinanced(objUser.iPartner_Id);
                    pnlAssetType.Visible = true;
                    GetUninsuredAssets(objUser.iPartner_Id, ViewState["SortField"].ToString(), "All");
                }
                hdAssetTypeFilter.Value = "ALL";

            }
        }
        protected void rptUninsuredAssets_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                string[] param = e.CommandArgument.ToString().Split(new Char[] { ';' });
                GetAllAssetDetails(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]));

                DataSet ds = new DataSet();
                System.Text.StringBuilder s = new System.Text.StringBuilder();

                P.Report_Provider frmF = new P.Report_Provider();
                ds = frmF.Get_Asset_Communications(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]), param[2].ToString());

                int i = 1;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    divNotifications.InnerHtml = s.ToString();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        s.Append(i + ". " + r[0].ToString() + ": " + String.Format("{0:m}", r[1].ToString() + "<br /><br />"));
                        i = i + 1;
                    }
                    divNotifications.InnerHtml = s.ToString();
                    divDateSinceUninsured.InnerHtml = param[2].ToString();
                }
                else
                {
                    //divNotifications.InnerHtml = "None";
                }
            }
        }

        protected void rptUninsuredAssets_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                C.AssetTypes.Uninsured_Assets res = e.Item.DataItem as C.AssetTypes.Uninsured_Assets;
                if (res != null)
                {
                    DataSet ds = new DataSet();
                    P.Report_Provider frmF = new P.Report_Provider();
                    ds = frmF.Get_Asset_Communications(res.iAsset_Id, res.iAsset_Type_Id, res.dtDate_since_Unisured);

                    Label lblNotificationsSent = (Label)e.Item.FindControl("lblNotificationsSent");
                    lblNotificationsSent.Text = ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows.Count.ToString() : "0";
                }
            }
        }
        public void Getformfields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsGeneric();
            ddlPartner.Items.Clear();
            ddlPartner.Items.Add(new ListItem("", ""));
            //Asset_Financier
            foreach (DataRow row in ds.Tables[13].Rows)
            {
                ddlPartner.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
        }
        public void GetAssetsFinanced(int iPartner_Id)
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsAssetsFinancedByFinancer(iPartner_Id);
            ddlAsset_type.ClearSelection();
            ddlAsset_type.Items.Clear();
            ddlAsset_type.Items.Add(new ListItem("", ""));
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
                uninsured_AssetsL = frmF.Get_Uninsured_Assets_Financer(iPartner_Id);

                if (assetTypeFilter != "All")
                {
                    uninsured_AssetsByType = uninsured_AssetsL.Where(x => x.iAsset_Type_Id == Convert.ToInt32(ddlAsset_type.SelectedValue)).ToList();
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
                    //if (c.ColumnName.ToString() == "Asset description" 
                    //    || c.ColumnName.ToString() == "Asset sub-type description"
                    //    || c.ColumnName.ToString() == "Make"
                    //    || c.ColumnName.ToString() == "Model"
                    //    || c.ColumnName.ToString() == "Model Variant"
                    //    || c.ColumnName.ToString() == "Finance value"
                    //    || c.ColumnName.ToString() == "Class"
                    //    //|| c.ColumnName.ToString() == "Name/Emblem"
                    //    //|| c.ColumnName.ToString() == "Tail number"
                    //    //|| c.ColumnName.ToString() == "Description"
                    //    //|| c.ColumnName.ToString() == "Serial number"
                    //    )
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
            pnlUninsuredAssets.Visible = false;
            pnlAllDetails.Visible = false;
            GetAssetsFinanced(Convert.ToInt32(ddlPartner.SelectedValue));
            //GetUninsuredAssets(Convert.ToInt32(ddlPartner.SelectedValue), ViewState["SortField"].ToString(), "All");
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
            pnlUninsuredAssets.Visible = true;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlAssetType.Visible = true;
            pnlUninsuredAssets.Visible = true;
            pnlAllDetails.Visible = false;
        }
        protected void btnSendReport_Click(object sender, EventArgs e)
        {
            try
            {
                string ParnerName = string.Empty;

                string csv = string.Empty;
                DataSet ds;
                CCom.CurrentUser objUser = new CCom.CurrentUser();
                P.User_Provider uP = new P.User_Provider();
                P.Report_Provider frmF = new P.Report_Provider();
                List<C.AssetTypes.Uninsured_Assets> uninsured_AssetsL = new List<C.AssetTypes.Uninsured_Assets>();
                objUser = uP.GetUserFromSession();

                if (objUser.iUser_Type_Id == 1 || objUser.iUser_Type_Id == 2)
                {
                    ds = frmF.Get_Uninsured_Assets_Financer_Report_Download(Convert.ToInt32(ddlPartner.SelectedValue));
                    ParnerName = ddlPartner.SelectedItem.Text;
                }
                else
                {
                    ds = frmF.Get_Uninsured_Assets_Financer_Report_Download(objUser.iPartner_Id);
                    ParnerName = objUser.vcPartner_Name;
                }

                if (ds.Tables[0].Rows.Count > 0)
                {

                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    DataTable dtExcel = ds.Tables[0];
                    foreach (DataColumn column in dtExcel.Columns)
                    {
                        context.Response.Write(column.ColumnName + ",");
                    }
                    context.Response.Write(Environment.NewLine);
                    foreach (DataRow row in dtExcel.Rows)
                    {
                        for (int i = 0; i < dtExcel.Columns.Count; i++)
                        {
                            context.Response.Write(row[i].ToString().Replace(",", string.Empty) + ",");
                        }
                        context.Response.Write(Environment.NewLine);
                    }
                    //context.Response.ContentType = "text/csv";
                    context.Response.AppendHeader("Content-Type", "application/vnd.ms-excel");
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + ParnerName + "_Uninsured_Assets_" + DateTime.Now.ToString("dd/MMM/yyyy") + ".csv"); //+ DateTime.Now.ToString("dd/MMM/yyyy HH:mm") 
                    //context.ApplicationInstance.CompleteRequest();
                    //context.Response.End();
                    // File.WriteAllText(@"C:\Codingvila.csv", sb.ToString());

                    //HttpContext.Current.Response.Write(Data);
                }
            }
            catch (Exception exc) { }
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
    }
}