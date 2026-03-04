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
namespace IAPR_Web.UserControls.AssetTypes
{
    public partial class AddBuilding_PropertyAsset : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        #region private
        public void GetFormFields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsPropertyAsset();

            //Clear all DropDownLists
            ddlAsset_Financier.Items.Clear();
            ddlAsset_Cover_Type.Items.Clear();
            ddlProperty_Asset_Type.Items.Clear();
            //Insert Empty 1st option 

            ddlAsset_Cover_Type.Items.Add(new ListItem("", ""));
            ddlAsset_Financier.Items.Add(new ListItem("", ""));
            ddlProperty_Asset_Type.Items.Add(new ListItem("", ""));


            //Populate relevant dropdownlists

            //Asset_Type_Cover
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                ddlAsset_Cover_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            //Property_Type
            foreach (DataRow row in ds.Tables[15].Rows)
            {
                ddlProperty_Asset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }



            //Asset_Financier
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                ddlAsset_Financier.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

        }
        public bool CheckPropertyDetailsExists()
        {
            bool exists = false;
            P.Property_Asset_Provider pro = new P.Property_Asset_Provider();
            DataSet ds = pro.Check_Property_Details_Exist(txtFinance_Agrreement_Number.Text, txtStand_ERF_Number.Text, txtSectionalTitleNumber.Text, txtSectionalTitleName.Text);
            foreach (DataTable t in ds.Tables)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Finance agreement number already exists');", true);
                    exists = true;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Stand/ERF number already exists');", true);
                    exists = true;
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Sectional Title Number already exists');", true);
                    exists = true;
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Sectional Title Name already exists');", true);
                    exists = true;
                }
            }

            return exists;
        }
        #endregion


        public bool SavePropertyData(int policyId)
        {
            bool saved = false;
            try
            {
                P.Generic_Asset_Provider proGen = new P.Generic_Asset_Provider();
                if (!proGen.Check_FinanceNumber_Exists(Convert.ToInt32(ddlAsset_Financier.SelectedValue), txtFinance_Agrreement_Number.Text))
                {
                    AT.Property_Asset pro = new AT.Property_Asset();

                    pro.iPolicy_Id = policyId;
                    pro.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    pro.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    pro.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    pro.mAsset_Finance_Value = Convert.ToDecimal(txtAsset_Finance_Value.Text.Replace(",", "").Replace(".", ","));
                    pro.mAsset_Insurance_Value = Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","));
                    pro.iAsset_Type_Id = 1;
                    pro.iProperty_Asset_Type_Id = Convert.ToInt32(ddlProperty_Asset_Type.SelectedValue);
                    pro.vcStand_ERF_Number = txtStand_ERF_Number.Text;
                    pro.vcSectionalTitleNumber = txtSectionalTitleNumber.Text;
                    pro.vcSectionalTitleName = txtSectionalTitleName.Text;
                    pro.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    pro.dtFinance_End_Date = txtFinance_End_Date.Text;




                    P.Property_Asset_Provider prv = new P.Property_Asset_Provider();
                    prv.Save_New_Property_Asset(pro);
                    saved = true;
                }
                else
                {
                    litFinanceNumberExists.Text = "<label for='" + txtFinance_Agrreement_Number.ClientID + "' class='txtnamevalidation erroMessage'>Finance number already exists</label>";
                }
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddVehicleAsset-UserControl", "SaveVehicleData");
            }
            return saved;
        }
        public bool SavePropertyData_Without_Policy(int alignmentId)
        {
            bool saved = false;
            try
            {
                P.Generic_Asset_Provider proGen = new P.Generic_Asset_Provider();
                if (!proGen.Check_FinanceNumber_Exists(Convert.ToInt32(ddlAsset_Financier.SelectedValue), txtFinance_Agrreement_Number.Text))
                {
                    AT.Property_Asset pro = new AT.Property_Asset();

                    pro.iPolicy_Id = 0;
                    pro.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    pro.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    pro.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    pro.mAsset_Finance_Value = Convert.ToDecimal(txtAsset_Finance_Value.Text.Replace(",", "").Replace(".", ","));
                    pro.mAsset_Insurance_Value = Convert.ToDecimal(txtAsset_Insurance_Value.Text.Replace(",", "").Replace(".", ","));
                    pro.iAsset_Type_Id = 1;
                    pro.iProperty_Asset_Type_Id = Convert.ToInt32(ddlProperty_Asset_Type.SelectedValue);
                    pro.vcStand_ERF_Number = txtStand_ERF_Number.Text;
                    pro.vcSectionalTitleNumber = txtSectionalTitleNumber.Text;
                    pro.vcSectionalTitleName = txtSectionalTitleName.Text;
                    pro.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    pro.dtFinance_End_Date = txtFinance_End_Date.Text;




                    P.Property_Asset_Provider prv = new P.Property_Asset_Provider();
                    prv.Save_New_Property_Asset_Without_Policy(pro, alignmentId);
                    saved = true;
                }
                else
                {
                    litFinanceNumberExists.Text = "<label for='" + txtFinance_Agrreement_Number.ClientID + "' class='txtnamevalidation erroMessage'>Finance number already exists</label>";
                }
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddVehicleAsset-UserControl", "SaveVehicleData");
            }
            return saved;
        }
    }
}