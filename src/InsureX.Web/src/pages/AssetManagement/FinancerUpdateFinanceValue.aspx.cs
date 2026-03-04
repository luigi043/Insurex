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
using U = IAPR_Data.Utils;
namespace IAPR_Web.AssetManagement
{
    public partial class FinancerUpdateFinanceValue : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetFormFields();
                CCom.CurrentUser objUser = new CCom.CurrentUser();
                P.User_Provider uP = new P.User_Provider();

                objUser = uP.GetUserFromSession();
                GetFinancerAssetTypes(objUser.iPartner_Id);

            }
        }

        private void GetFormFields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsVehicleAsset();

            //Clear all DropDownLists



            ddlAsset_Type.Items.Clear();

            //Insert Empty 1st option 


            ddlAsset_Type.Items.Add(new ListItem("", ""));


            //Populate relevant dropdownlists

            //Asset_Type

            foreach (DataRow row in ds.Tables[14].Rows)
            {
                ddlAsset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
        }

        protected void btnFindAsset_Click(object sender, EventArgs e)
        {
            CCom.CurrentUser objUser = new CCom.CurrentUser();
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();


            if (objUser == null)
            {
                Response.Redirect("/account/login.aspx", false);
            }
            P.Generic_Asset_Provider pro = new P.Generic_Asset_Provider();
            int iAsset_Id = pro.Get_Asset_ID_By_Finance_Number(txtFinanceNumber.Text, Convert.ToInt32(ddlAsset_Type.SelectedValue), objUser.iPartner_Id);
            if (iAsset_Id > 0)
            {
                GetAllAssetDetails(iAsset_Id);
                pnlChangeFinanceValueDetails.Visible = true;
                hdAssetId.Value = U.CryptorEngine.GenericEncrypt(iAsset_Id.ToString(), true);
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
        protected void btnSaveChangeFinanceValue_Click(object sender, EventArgs e)
        {
            bool iPolicy_Transaction_Id = false;
            P.Vehicle_Asset_Provider pV = new P.Vehicle_Asset_Provider();
            P.Property_Asset_Provider pP = new P.Property_Asset_Provider();
            P.Watercraft_Provider pW = new P.Watercraft_Provider();
            P.Aviation_Asset_Provider pA = new P.Aviation_Asset_Provider();
            P.Stock_Asset_Provider pS = new P.Stock_Asset_Provider();
            P.AccountReceivable_Asset_Provider pAR = new P.AccountReceivable_Asset_Provider();
            P.Machinery_Asset_Provider pM = new P.Machinery_Asset_Provider();
            P.PlantEquipment_Asset_Provider pPE = new P.PlantEquipment_Asset_Provider();
            P.BusinessInterruption_Asset_Provider pBI = new P.BusinessInterruption_Asset_Provider();
            P.KeymanInsurance_Asset_Provider pKI = new P.KeymanInsurance_Asset_Provider();
            P.ElectronicEquipment_Asset_Provider pEE = new P.ElectronicEquipment_Asset_Provider();

            switch (ddlAsset_Type.SelectedValue)
            {
                case "1":
                    iPolicy_Transaction_Id = pV.Save_ChangeFinanceValue_Vehicle_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewFinanceValue.Text.Replace(",", "").Replace(".", ",")), txtDateOfChangeFinanceValue.Text);
                    break;
                case "2":
                    iPolicy_Transaction_Id = pP.Save_ChangeFinanceValue_Property_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewFinanceValue.Text.Replace(",", "").Replace(".", ",")), txtDateOfChangeFinanceValue.Text);
                    break;
                case "3":
                    iPolicy_Transaction_Id = pW.Save_ChangeFinanceValue_Watercraft_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewFinanceValue.Text.Replace(",", "").Replace(".", ",")), txtDateOfChangeFinanceValue.Text);
                    break;
                case "4":
                    iPolicy_Transaction_Id = pA.Save_ChangeFinanceValue_Aviation_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewFinanceValue.Text.Replace(",", "").Replace(".", ",")), txtDateOfChangeFinanceValue.Text);
                    break;
                case "5":
                    iPolicy_Transaction_Id = pS.Save_ChangeFinanceValue_Stock_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewFinanceValue.Text.Replace(",", "").Replace(".", ",")), txtDateOfChangeFinanceValue.Text);
                    break;
                //case "6":
                //    iPolicy_Transaction_Id = pAR.Save_ChangeFinanceValue_AccountsReceivable_Asset( Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewFinanceValue.Text.Replace(",", "").Replace(".", ",")), txtDateOfChangeFinanceValue.Text);
                //    break;
                case "7":
                    iPolicy_Transaction_Id = pM.Save_ChangeFinanceValue_Machinery_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewFinanceValue.Text.Replace(",", "").Replace(".", ",")), txtDateOfChangeFinanceValue.Text);
                    break;
                case "8":
                    iPolicy_Transaction_Id = pPE.Save_ChangeFinanceValue_PlantEquipment_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewFinanceValue.Text.Replace(",", "").Replace(".", ",")), txtDateOfChangeFinanceValue.Text);
                    break;
                case "9":
                    iPolicy_Transaction_Id = pBI.Save_ChangeFinanceValue_BusinessInterruption_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewFinanceValue.Text.Replace(",", "").Replace(".", ",")), txtDateOfChangeFinanceValue.Text);
                    break;
                case "10":
                    iPolicy_Transaction_Id = pKI.Save_ChangeFinanceValue_KeymanInsurance_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewFinanceValue.Text.Replace(",", "").Replace(".", ",")), txtDateOfChangeFinanceValue.Text);
                    break;
                case "11":
                    iPolicy_Transaction_Id = pEE.Save_ChangeFinanceValue_ElectronicEquipment_Asset(Convert.ToInt32(U.CryptorEngine.GenericDecrypt(hdAssetId.Value, true)), Convert.ToDecimal(txtNewFinanceValue.Text.Replace(",", "").Replace(".", ",")), txtDateOfChangeFinanceValue.Text);
                    break;
            }
            txtNewFinanceValue.Text = "";
            txtDateOfChangeFinanceValue.Text = "";
            pnlAllDetails.Visible = false;
            pnlChangeFinanceValueDetails.Visible = false;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Updated');", true);

        }
        protected void btnCancelChangeFinanceValue_Click(object sender, EventArgs e)
        {
            txtNewFinanceValue.Text = "";
            txtDateOfChangeFinanceValue.Text = "";
            pnlChangeFinanceValueDetails.Visible = false;
            pnlAllDetails.Visible = false;
            pnlChangeFinanceValueDetails.Visible = false;
        }
    }
}