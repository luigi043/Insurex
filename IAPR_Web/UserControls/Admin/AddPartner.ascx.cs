using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using AT = IAPR_Data.Classes;
using CP = IAPR_Data.Classes.Policy;
using CCom = IAPR_Data.Classes.Common;
using P = IAPR_Data.Providers;
using U = IAPR_Data.Utils;
using System.Configuration;
using System.IO;
namespace IAPR_Web.UserControls.Admin
{
    public partial class AddPartner : System.Web.UI.UserControl
    {
        CCom.CurrentUser objUser = new CCom.CurrentUser();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetFormFields();

            }
        }
        private void GetFormFields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsGeneric();

            //Clear all DropDownLists
            ddlPartnerType.Items.Clear();
            ddlProvince.Items.Clear();
            //chkAssetsFinanced.Items.Clear();
            //ddlDivisionAssets.Items.Clear();

            //Insert Empty 1st option 
            ddlPartnerType.Items.Add(new ListItem("", ""));
            ddlProvince.Items.Add(new ListItem("", ""));
            //ddlDivisionAssets.Items.Add(new ListItem("", ""));

            //Populate relevant dropdownlists
            //Insurance companies
            foreach (DataRow row in ds.Tables[11].Rows)
            {
                ddlPartnerType.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }


            //Pronvinces
            foreach (DataRow row in ds.Tables[5].Rows)
            {
                ddlProvince.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

            //Asset Types
            //foreach (DataRow row in ds.Tables[12].Rows)
            //{
            //    chkAssetsFinanced.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            //    ddlDivisionAssets.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            //}

            ViewState["PackageAssets"] = ds.Tables[15];
            ViewState["PackageDetails"] = ds.Tables[16];
        }
        private void ApplyPackagessets()
        {
            DataTable dtPackageAssets = null;
            DataTable dtPackageDetails = null;
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            string packageDetail = string.Empty;

            if (ViewState["PackageAssets"] != null)
            {
                dtPackageAssets = (DataTable)ViewState["PackageAssets"];
                dtPackageDetails = (DataTable)ViewState["PackageDetails"];
                foreach (DataRow row in dtPackageDetails.Rows)
                {
                    if (row[0].ToString() == rblPackages.SelectedValue)
                    {
                        packageDetail = row[2].ToString();
                    }
                }
                s.Append(packageDetail + "<br /><br />");
                s.Append(rblPackages.SelectedItem.Text + " package includes the following assets types:<br /><br />");
                foreach (DataRow row in dtPackageAssets.Rows)
                {
                    if (row[0].ToString() == rblPackages.SelectedValue)
                    {
                        s.Append(row[3] + "<br />");
                    }
                }
                divPackageAssets.InnerHtml = s.ToString();
            }
        }
        protected void chkPostalSameAsPhysical_CheckedChanged(object sender, EventArgs e)
        {
            pnlPostalAddress.Visible = chkPostalSameAsPhysical.Checked ? false : true;

        }

        protected void btnAddPartner_Click(object sender, EventArgs e)
        {
            P.User_Provider uP = new P.User_Provider();
            objUser = uP.GetUserFromSession();

            P.Admin_Provider pro = new P.Admin_Provider();
            try
            {
                bool validLogo = false;
                bool fileSelected = false;
                
                //if (LogoUpload.HasFile)
                //{
                //if (AsyncFileUpload1.HasFile)
                //{
                //    fileSelected = true;
                //    if (!CheckFileType())
                //    {
                //        return;
                //    }
                //}
                //else
                //{
                //    return;
                //}


                if (!pro.Check_Username(txtEmail_Address.Text))
                {
                    if (ddlPartnerType.SelectedValue == "1")
                    {
                        SaveInsurer(objUser.iUser_Id);

                    }
                    else
                    {
                        SaveFinancer(objUser.iUser_Id);
                    }

                    litUserExists.Text = "<label for='" + txtEmail_Address.ClientID + "' class=''></label>";
                    //pnlSaveButtons.Visible = false;
                    //pnlSuccess.Visible = true;
                    ClearForm();
                    this.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastSuccess", "toastSuccess('Partner saved');", true);
                }
                else
                {
                    litUserExists.Text = "<label for='" + txtEmail_Address.ClientID + "' class='txtnamevalidation erroMessage'>Username already exists</label>";
                }

            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddPartner-UserControl", "SaveInsurer");
            }
        }

        public bool SaveFinancer(int iUser_Id)
        {
            bool saved = false;
            if (!Page.IsValid)
            {
                return false;
            }

            AT.Partners.Financer fi = new AT.Partners.Financer();
            List<AT.Partners.Financed_Assets> lsfinanced_Assets = new List<AT.Partners.Financed_Assets>();

            CCom.CurrentUser user = new CCom.CurrentUser();
            fi.iPackage_Id = Convert.ToInt32(rblPackages.SelectedValue);
            fi.vcFinancer_Name = txtPartnerName.Text;
            fi.vcBusiness_registration_Number = txtBusinessNumber.Text;
            fi.vcVat_Registration_Number = txtVatRegistrationNumber.Text;
            fi.vcBuilding_Unit = txtBuilding_Unit.Text;
            fi.vcAddress_Line_1 = txtAddress_Line_1.Text;
            fi.vcAddress_Line_2 = txtAddress_Line_2.Text;
            fi.vcSuburb = txtSuburb.Text;
            fi.vcCity = txtCity.Text;
            fi.vcPostal_Code = txtPostal_Code.Text;
            fi.iProvince_Id = Convert.ToInt32(ddlProvince.SelectedValue);
            fi.bPostalAddresSameAsPhysical = chkPostalSameAsPhysical.Checked ? true : false;
            fi.vcPOBox_Bag = txtPOBox_Bag.Text;
            fi.vcPost_Office_Name = txtPost_Office_Name.Text;
            fi.vcPost_Office_Postal_Code = txtPost_Postal_Code.Text;
            fi.vcContact_Number = txtCompanyContactNumber.Text;

            user.vcName = txtFirst_Names.Text;
            user.vcSurname = txtSurname.Text;
            user.vcUsername = txtEmail_Address.Text;
            user.vcPosition_Title = txtPosition_Title.Text;
            user.vcPassword = "";
            user.vcContactNumber = txtContact_Number.Text;
            user.bUserReceiveNotifications = rblNotifications.SelectedValue == "Yes" ? true : false;

            P.Admin_Provider pro = new P.Admin_Provider();
            Dictionary<string, string> NewPArtner = new Dictionary<string, string>();

            NewPArtner = pro.Save_New_Financer(fi, user, iUser_Id);//lsDivisions, iUser_Division
            SendNewUserNotification(NewPArtner["PW"]);

            //SavePartnerLogo(Convert.ToInt32(NewPArtner["iPartnerId"]), Convert.ToInt32(CCom.Common.Partner_types.Lender));
            saved = true;

            return saved;
        }
        public bool SaveInsurer(int iUser_Id)
        {
            bool saved = false;
            if (!Page.IsValid)
            {
                return false;
            }

            AT.Partners.Insurer inS = new AT.Partners.Insurer();
            CCom.CurrentUser user = new CCom.CurrentUser();

            inS.vcInsurance_Company_Name = txtPartnerName.Text;
            inS.vcBusiness_registration_Number = txtBusinessNumber.Text;
            inS.vcVat_Registration_Number = txtVatRegistrationNumber.Text;
            inS.vcBuilding_Unit = txtBuilding_Unit.Text;
            inS.vcAddress_Line_1 = txtAddress_Line_1.Text;
            inS.vcAddress_Line_2 = txtAddress_Line_2.Text;
            inS.vcSuburb = txtSuburb.Text;
            inS.vcCity = txtCity.Text;
            inS.vcPostal_Code = txtPostal_Code.Text;
            inS.iProvince_Id = Convert.ToInt32(ddlProvince.SelectedValue);
            inS.vcPOBox_Bag = txtPOBox_Bag.Text;
            inS.vcPost_Office_Name = txtPost_Office_Name.Text;
            inS.vcPost_Office_Postal_Code = txtPost_Postal_Code.Text;
            inS.bPostalAddresSameAsPhysical = chkPostalSameAsPhysical.Checked ? true : false;
            inS.vcContact_Number = txtCompanyContactNumber.Text;

            user.vcName = txtFirst_Names.Text;
            user.vcSurname = txtSurname.Text;
            user.vcUsername = txtEmail_Address.Text;
            user.vcPosition_Title = txtPosition_Title.Text;
            user.vcContactNumber = txtContact_Number.Text;
            user.vcPassword = "";
            user.bUserReceiveNotifications = rblNotifications.SelectedValue == "Yes" ? true : false;

            P.Admin_Provider pro = new P.Admin_Provider();
            Dictionary<string, string> NewPArtner = new Dictionary<string, string>();

            NewPArtner = pro.Save_New_Insurer(inS, user, iUser_Id);

            //P.Notification_Provider nP = new P.Notification_Provider();
            //nP.NotifyNewUser(user.vcName, user.vcUsername, txtPartnerName.Text, ConfigurationManager.AppSettings["Application_URL"], pw, "NewUser");
            

            SendNewUserNotification(NewPArtner["PW"]);
            //SavePartnerLogo(Convert.ToInt32(NewPArtner["iPartnerId"]), Convert.ToInt32(CCom.Common.Partner_types.Insurance_provider));
            saved = true;



            return saved;
        }
        private void SavePartnerLogo(int partnerId, int partnerType)
        {
            bool validLogo = false;
            bool fileSelected = false;


            if (LogoUpload.HasFile)
            {
                string extension = System.IO.Path.GetExtension(LogoUpload.FileName);
                string fileName = string.Empty;
                switch (partnerType)
                {
                    case 1:
                        fileName = "I" + partnerId + extension;
                        break;
                    case 2:
                        fileName = "L" + partnerId + extension;
                        break;
                    default:
                        fileName = "Default.png";
                        break;
                }

                LogoUpload.PostedFile.SaveAs(Server.MapPath("~/ClientLogos/") + fileName);
                P.Admin_Provider pro = new P.Admin_Provider();
                pro.Update_Partner_Logo(partnerId, partnerType, fileName);

            }
        }
        private bool CheckFileType()
        {
            string[] validFileTypes = ConfigurationManager.AppSettings["ValidImageExtensions"].Split(',');// { "bmp", "gif", "png", "jpg", "jpeg", "doc", "xls" };
            string ext = System.IO.Path.GetExtension(AsyncFileUpload1.FileName);// LogoUpload.PostedFile.FileName);
            
            bool isValidFile = false;

            for (int i = 0; i < validFileTypes.Length; i++)

            {
                if (ext == "." + validFileTypes[i])

                {
                    isValidFile = true;

                }
            }

            if (!isValidFile)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Invalid File. Please upload an image file!');", true);
            }

            return isValidFile;
        }
        protected void ddlPartnerType_SelectedIndexChanged(object sender, EventArgs e)
        {

            pnlAssetsFinanced.Visible = Convert.ToInt32(ddlPartnerType.SelectedValue) == Convert.ToInt32(CCom.Common.Partner_types.Lender) ? true : false;

        }


        protected void btnAddDivision_Click(object sender, EventArgs e)
        {
            //DataTable dtDivisions = new DataTable();
            //dtDivisions = GetDivision();

            //if (ViewState["DivisionTable"] != null)
            //{
            //    dtDivisions = (DataTable)ViewState["DivisionTable"];


            //    rptDivisions.DataSource = dtDivisions;
            //    rptDivisions.DataBind();
            //    rptDivisions.Visible = true;

            //    ddlUser_Division.Items.Clear();

            //    ddlUser_Division.Items.Add(new ListItem("", ""));
            //    foreach (DataRow r in dtDivisions.Rows)
            //    {
            //        ddlUser_Division.Items.Add(new ListItem(r["vcDivision_Name"].ToString(), r["iDivision_Id"].ToString()));
            //    }
            //    pnlUserDivisions.Visible = true;

            //}
            //else
            //{
            //    rptDivisions.DataSource = null;
            //    rptDivisions.DataBind();
            //    rptDivisions.Visible = false;
            //    ddlUser_Division.Items.Clear();
            //    pnlUserDivisions.Visible = false;
            //}



            //txtDivisionName.Text = string.Empty;
            //ddlDivisionAssets.ClearSelection();

        }

        //private DataTable GetDivision()
        //{

        //    DataTable dtDivisions = null;
        //    if (ViewState["iDivision_Id"] != null)
        //    {
        //        int iDivision_Id = Convert.ToInt32((ViewState["iDivision_Id"]));
        //        iDivision_Id++;
        //        ViewState["iDivision_Id"] = iDivision_Id;
        //    }
        //    else
        //    {
        //        ViewState["iDivision_Id"] = 1;
        //    }

        //    if (ViewState["DivisionTable"] == null)
        //    {
        //        dtDivisions = new DataTable("DivisionTable");
        //        dtDivisions.Columns.Add(new DataColumn("iDivision_Id", typeof(int)));
        //        dtDivisions.Columns.Add(new DataColumn("vcDivision_Name", typeof(string)));
        //        dtDivisions.Columns.Add(new DataColumn("iAssociated_Asset_Type_Id", typeof(int)));
        //        dtDivisions.Columns.Add(new DataColumn("vcAssociated_Asset_Type", typeof(string)));


        //        ViewState["DivisionTable"] = dtDivisions;
        //    }
        //    else
        //    {
        //        dtDivisions = (DataTable)ViewState["DivisionTable"];
        //    }
        //    DataRow dtRow = dtDivisions.NewRow();

        //    dtRow["iDivision_Id"] = Convert.ToInt32(ViewState["iDivision_Id"]);
        //    dtRow["vcDivision_Name"] = txtDivisionName.Text.Trim();
        //    dtRow["iAssociated_Asset_Type_Id"] = ddlDivisionAssets.SelectedValue;
        //    dtRow["vcAssociated_Asset_Type"] = ddlDivisionAssets.SelectedItem.Text.Trim();

        //    dtDivisions.Rows.Add(dtRow);
        //    ViewState["DivisionTable"] = dtDivisions;
        //    return dtDivisions;
        //}
        private DataTable RemoveDivision(int id)
        {

            DataTable dtDivisions = null;

            dtDivisions = (DataTable)ViewState["DivisionTable"];

            DataRow[] dtr = dtDivisions.Select("iDivision_Id=" + id);
            foreach (var drow in dtr)
            {
                drow.Delete();
            }

            dtDivisions.AcceptChanges();
            ViewState["DivisionTable"] = dtDivisions;
            return dtDivisions;
        }
        //protected void rptDivisions_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        int id = Convert.ToInt32(e.CommandArgument.ToString());
        //        DataTable dtDivisions = null;
        //        dtDivisions = RemoveDivision(id);
        //        if (dtDivisions.Rows.Count > 0)
        //        {
        //            dtDivisions = (DataTable)ViewState["DivisionTable"];


        //            rptDivisions.DataSource = dtDivisions;
        //            rptDivisions.DataBind();
        //            rptDivisions.Visible = true;

        //            ddlUser_Division.Items.Clear();

        //            ddlUser_Division.Items.Add(new ListItem("", ""));
        //            foreach (DataRow r in dtDivisions.Rows)
        //            {
        //                ddlUser_Division.Items.Add(new ListItem(r["vcDivision_Name"].ToString(), r["iDivision_Id"].ToString()));
        //            }
        //            pnlUserDivisions.Visible = true;

        //        }
        //        else
        //        {
        //            rptDivisions.DataSource = null;
        //            rptDivisions.DataBind();
        //            rptDivisions.Visible = false;
        //            ddlUser_Division.Items.Clear();
        //            pnlUserDivisions.Visible = false;
        //        }
        //    }
        //}

        //protected void rblDivisionsRequired_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    pnlAddDivisions.Visible = rblDivisionsRequired.SelectedIndex == 0 ? true : false;
        //}
        private void SendNewUserNotification(string pw)
        {
            try
            {
                P.Notification_Provider nP = new P.Notification_Provider();
                nP.NotifyNewUser(txtFirst_Names.Text, txtEmail_Address.Text, txtPartnerName.Text, ConfigurationManager.AppSettings["Application_URL"], pw, "NewUser");
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "AddPartner-UserControl", "SendNewUserNotification");
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "toastWarning", "toastWarning('Failure sending email notification');", true);
            }
        }

        private void ClearForm()
        {
            ddlPartnerType.ClearSelection();
            ddlPartnerType.SelectedIndex = 0;
            txtPartnerName.Text = "";
            txtBusinessNumber.Text = "";
            txtVatRegistrationNumber.Text = "";
            rblPackages.ClearSelection();
            divPackageAssets.InnerHtml = "";
            txtBuilding_Unit.Text = "";
            txtAddress_Line_1.Text = "";
            txtAddress_Line_2.Text = "";
            txtSuburb.Text = "";
            txtCity.Text = "";
            txtPostal_Code.Text = "";
            ddlProvince.ClearSelection();
            ddlProvince.SelectedIndex = 0;
            chkPostalSameAsPhysical.Checked = false;
            txtPOBox_Bag.Text = "";
            txtPost_Office_Name.Text = "";
            txtPost_Postal_Code.Text = "";
            txtCompanyContactNumber.Text = "";

            txtFirst_Names.Text = "";
            txtSurname.Text = "";
            txtEmail_Address.Text = "";
            txtPosition_Title.Text = "";

            txtContact_Number.Text = "";
            rblNotifications.ClearSelection();
        }
        protected void rblPackages_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyPackagessets();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        protected void FileUploadComplete(object sender, EventArgs e)
        {
            string filename = System.IO.Path.GetFileName(AsyncFileUpload1.FileName);
           // AsyncFileUpload1.SaveAs(Server.MapPath("Uploads/") + filename);
        }
    }
}