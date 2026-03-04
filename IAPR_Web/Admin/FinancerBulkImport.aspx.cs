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
using System.IO;
namespace IAPR_Web.Admin
{
    public partial class FinancerBulkImport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //UpdatePanel updatePanel = Page.Master.FindControl("UpdatePanel1") as UpdatePanel;
            //UpdatePanelControlTrigger trigger = new PostBackTrigger();
            //trigger.ControlID = btnImportBankAssest.UniqueID;
            //updatePanel.Triggers.Add(trigger);
            if (!IsPostBack)
            {
                Page.Form.Attributes.Add("enctype", "multipart/form-data");
                GetFormFields();
            }
        }
        private void GetFormFields()
        {
            P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
            DataSet ds = frmF.GetFormFieldsGeneric();

            //Clear all DropDownLists


            ddlAsset_Financier.Items.Clear();
            //Insert Empty 1st option 


            ddlAsset_Financier.Items.Add(new ListItem("", ""));


            foreach (DataRow row in ds.Tables[13].Rows)
            {
                ddlAsset_Financier.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }

        }
        protected void FileUploadComplete(object sender, EventArgs e)
        {
            string filename = System.IO.Path.GetFileName(fuBankAssets.FileName);
            if (filename != null)
            {
                List<C.Policy.BulkImportFromFinancer> bIItemList = new List<C.Policy.BulkImportFromFinancer>();
                P.Bulk_Import_Provider bP = new P.Bulk_Import_Provider();
                using (var reader = new StreamReader(fuBankAssets.PostedFile.InputStream))
                {
                    reader.ReadLine();
                    int i = 0;
                    while (!reader.EndOfStream)
                    {
                        C.Policy.BulkImportFromFinancer bIItem = new C.Policy.BulkImportFromFinancer();
                        var line = reader.ReadLine();
                        var values = line.Split(';');
                        bIItem.iCounterID = i;
                        bIItem.iFinancier_Id = Convert.ToInt32(values[0]);
                        bIItem.vcFinance_Number = values[1];
                        bIItem.vcID_Business_Number = values[2];
                        bIItem.vcCustomer_Type_Description = values[3];
                        bIItem.vcInsurance_Company = values[4];
                        bIItem.vcPolicy_Number = values[5];
                        bIItem.vcAsset_Type_Description = values[6];
                        bIItem.vcAsset_Sub_Type_Description = values[7];
                        bIItem.vcAsset_Unique_Identifier = values[8];
                        bIItem.dtFinancing_StartDate = values[9];
                        bIItem.dtFinancing_EndDate = values[10];

                        bIItemList.Add(bIItem);
                        i = i + 1;
                    }
                }
                bP.Save_Bulk_Import_From_Financer(bIItemList);
            }
            // }

        }
        protected void afu_OnUploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs file)
        {
            using (var stream = file.GetStreamContents())
            {
                // Read the stream...
            }
        }
        protected void btnImportBankAssest_Click(object sender, EventArgs e)
        {
            // //if (fupBankAssets.FileName.Length > 0)
            // //{
            //     List<C.Policy.BulkImportFromFinancer> bIItemList = new List<C.Policy.BulkImportFromFinancer>();
            //     P.Bulk_Import_Provider bP = new P.Bulk_Import_Provider();

            //     //fupBankAssets.FileContent))
            //     using (var reader = new StreamReader(@"C:\Mapoza\Insured_Assest_Protection_Register\Documentation\Test files\ImportBankAssets.csv"))
            //     {
            //         reader.ReadLine();
            //         int i = 0;
            //         while (!reader.EndOfStream)
            //         {
            //             C.Policy.BulkImportFromFinancer bIItem = new C.Policy.BulkImportFromFinancer();
            //             var line = reader.ReadLine();
            //             var values = line.Split(';');
            //             bIItem.iCounterID = i;
            //             bIItem.iFinancier_Id = Convert.ToInt32(values[0]);
            //             bIItem.vcFinance_Number = values[1];
            //             bIItem.vcID_Business_Number = values[2];
            //             bIItem.vcCustomer_Type_Description = values[3];
            //             bIItem.vcInsurance_Company = values[4];
            //             bIItem.vcPolicy_Number = values[5];
            //             bIItem.vcAsset_Type_Description = values[6];
            //             bIItem.vcAsset_Sub_Type_Description = values[7];
            //             bIItem.vcAsset_Unique_Identifier = values[8];
            //             bIItem.dtFinancing_StartDate = values[9];
            //             bIItem.dtFinancing_EndDate = values[10];

            //             bIItemList.Add(bIItem);
            //             i = i + 1;
            //         }
            //     }
            //     bP.Save_Bulk_Import_From_Financer(bIItemList);
            //// }

        }

        protected void ddlAsset_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        //private void GetAssetFinancer()
        //{
        //    P.GetFormFields_Provider frmF = new P.GetFormFields_Provider();
        //    DataSet ds = frmF.GetFormFieldsAssetFinancerByAssetType(Convert.ToInt32(ddlAsset_Type.SelectedValue));

        //    //Clear all DropDownLists

        //    ddlAsset_Financier.Items.Clear();

        //    //Insert Empty 1st option 

        //    ddlAsset_Financier.Items.Add(new ListItem("", ""));


        //    //Asset_Financier
        //    foreach (DataRow row in ds.Tables[0].Rows)
        //    {
        //        ddlAsset_Financier.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
        //    }


        //}
        private bool ImportFinancerAssets()
        {
            bool imported = false;



            return imported;
        }

        protected void ddlAsset_Financier_SelectedIndexChanged(object sender, EventArgs e)
        {
            P.GetFormFields_Provider p = new P.GetFormFields_Provider();
            DataSet ds = p.GetFormFieldsAssetsFinancedByFinancer(Convert.ToInt32(ddlAsset_Financier.SelectedValue));

            ddlAsset_Type.Items.Clear();
            ddlAsset_Type.Items.Add(new ListItem("", ""));
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlAsset_Type.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
            }
        }

        protected void chkCorrectIputs_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkCorrectIputs.Checked)
                btnImportBankAssest.Enabled = chkCorrectIputs.Checked ? true : false;
        }
    }
}