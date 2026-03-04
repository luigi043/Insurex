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
    public partial class AddKeymanInsuranceAsset : System.Web.UI.UserControl
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
            DataSet ds = frmF.GetFormFieldKeymanInsuranceAsset();

            //Clear all DropDownLists


            ddlKeymanInsurance_Asset_Type.Items.Clear();
            ddlIdentification_Type.Items.Clear();


            ddlAsset_Financier.Items.Clear();

            //Insert Empty 1st option 

            ddlAsset_Cover_Type.Items.Add(new ListItem("", ""));


            ddlKeymanInsurance_Asset_Type.Items.Add(new ListItem("", ""));


            ddlAsset_Financier.Items.Add(new ListItem("", ""));
            ddlIdentification_Type.Items.Add(new ListItem("", ""));


            //Populate relevant dropdownlists

            //Asset_Type_Cover
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                ddlAsset_Cover_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            //KeymanInsurance_Asset_Type
            foreach (DataRow row in ds.Tables[6].Rows)
            {
                ddlKeymanInsurance_Asset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
            //Asset_Financier
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                ddlAsset_Financier.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Identification_Types
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                ddlIdentification_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }




        }
        #endregion


        public bool SaveKeymanInsuranceAsset(int policyId)
        {
            bool saved = false;
            if (!Page.IsValid)
            {
                return false;
            }
            try
            {
                P.Generic_Asset_Provider proGen = new P.Generic_Asset_Provider();
                if (!proGen.Check_FinanceNumber_Exists(Convert.ToInt32(ddlAsset_Financier.SelectedValue), txtFinance_Agrreement_Number.Text))
                {
                    AT.KeymanInsurance_Asset ki = new AT.KeymanInsurance_Asset();




                    ki.iPolicy_Id = policyId;
                    ki.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    ki.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    ki.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    ki.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    ki.dtFinance_End_Date = txtFinance_End_Date.Text;
                    ki.iKeymanInsurance_Asset_Type_Id = Convert.ToInt32(ddlKeymanInsurance_Asset_Type.SelectedValue);
                    ki.vcKeyman_Name = txtKeyman_Name.Text;
                    ki.vcKeyman_Surname = txtKeyman_Surname.Text;
                    ki.iKeyman_Identification_type_Id = Convert.ToInt32(ddlIdentification_Type.SelectedValue);
                    ki.vcKeyman_Identity_Number = txtKeyman_Identity_Number.Text;
                    P.KeymanInsurance_Asset_Provider pro = new P.KeymanInsurance_Asset_Provider();
                    pro.Save_New_KeymanInsurance_Asset(ki);
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
                eL.LogErrorInDB(ex, "AddKeymanInsurance-UserControl", "SaveKeymanInsuranceAsset");
            }
            return saved;
        }
        public bool SaveKeymanInsuranceAsset_Without_Policy(int alignmentId)
        {
            bool saved = false;
            if (!Page.IsValid)
            {
                return false;
            }
            try
            {
                P.Generic_Asset_Provider proGen = new P.Generic_Asset_Provider();
                if (!proGen.Check_FinanceNumber_Exists(Convert.ToInt32(ddlAsset_Financier.SelectedValue), txtFinance_Agrreement_Number.Text))
                {
                    AT.KeymanInsurance_Asset ki = new AT.KeymanInsurance_Asset();




                    ki.iPolicy_Id = 0;
                    ki.iAsset_Cover_Type_Id = Convert.ToInt32(ddlAsset_Cover_Type.SelectedValue);
                    ki.iFinancer_Id = Convert.ToInt32(ddlAsset_Financier.SelectedValue);
                    ki.vcFinance_Agrreement_Number = txtFinance_Agrreement_Number.Text;
                    ki.dtFinance_Start_Date = txtFinance_Start_Date.Text;
                    ki.dtFinance_End_Date = txtFinance_End_Date.Text;
                    ki.iKeymanInsurance_Asset_Type_Id = Convert.ToInt32(ddlKeymanInsurance_Asset_Type.SelectedValue);
                    ki.vcKeyman_Name = txtKeyman_Name.Text;
                    ki.vcKeyman_Surname = txtKeyman_Surname.Text;
                    ki.iKeyman_Identification_type_Id = Convert.ToInt32(ddlIdentification_Type.SelectedValue);
                    ki.vcKeyman_Identity_Number = txtKeyman_Identity_Number.Text;
                    P.KeymanInsurance_Asset_Provider pro = new P.KeymanInsurance_Asset_Provider();
                    pro.Save_New_KeymanInsurance_Asset_Without_Policy(ki, alignmentId);
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
                eL.LogErrorInDB(ex, "AddKeymanInsurance-UserControl", "SaveKeymanInsuranceAsset");
            }
            return saved;
        }

    }
}