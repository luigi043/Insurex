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

namespace IAPR_Web.AssetManagement
{
    public partial class FinancerFindAsset : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CCom.CurrentUser objUser = new CCom.CurrentUser();
                P.User_Provider uP = new P.User_Provider();
                objUser = uP.GetUserFromSession();
                GetFinancerAssetTypes(objUser.iPartner_Id);
            }
        }

        private void GetFormFields()
        {

        }
        private void GetFinancerAssetTypes(int iFinancer_Id)
        {
            P.GetFormFields_Provider p = new P.GetFormFields_Provider();
            DataSet ds = p.GetFormFieldsAssetsFinancedByFinancer(iFinancer_Id);

            ddlAsset_Type.Items.Clear();
            ddlAsset_Type.Items.Add(new ListItem("", ""));
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlAsset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
        }
        protected void btnFindAsset_Click(object sender, EventArgs e)
        {
            CCom.CurrentUser objUser = new CCom.CurrentUser();
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();


            P.Generic_Asset_Provider pro = new P.Generic_Asset_Provider();
            int iAsset_Id = pro.Get_Asset_ID_By_Finance_Number(txtFinanceNumber.Text, Convert.ToInt32(ddlAsset_Type.SelectedValue), objUser.iPartner_Id);
            if (iAsset_Id > 0)
            {
                GetAllAssetDetails(iAsset_Id);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Asset not found');", true);
            }
        }

        private void GetAllAssetDetails(int iAsset_Id)
        {
            P.Generic_Asset_Provider pro = new P.Generic_Asset_Provider();
            DataSet ds = pro.Get_Asset_All_Details_By_Asset_ID(Convert.ToInt32(ddlAsset_Type.SelectedValue), iAsset_Id);
            System.Text.StringBuilder s = new System.Text.StringBuilder();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                foreach (DataColumn c in ds.Tables[0].Columns)
                {
                    s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                }
            }
            divAssetDetails.InnerHtml = s.ToString();

            s.Clear();
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
            divPolicyDetails.InnerHtml = s.ToString();
            s.Clear();
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                foreach (DataColumn c in ds.Tables[2].Columns)
                {
                    s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                }
            }

            divCustomerDeatils.InnerHtml = s.ToString();
            s.Clear();
            foreach (DataRow row in ds.Tables[3].Rows)
            {
                foreach (DataColumn c in ds.Tables[3].Columns)
                {
                    s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                }
            }

            divPhysicalAddress.InnerHtml = s.ToString();
            s.Clear();
            foreach (DataRow row in ds.Tables[4].Rows)
            {
                foreach (DataColumn c in ds.Tables[4].Columns)
                {
                    s.Append(c.ColumnName + ": " + row[c] + "<br /><br />");
                }
            }
            divPostalAddress.InnerHtml = s.ToString();
            pnlAllDetails.Visible = true;
        }
    }
}