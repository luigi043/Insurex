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
    public partial class InsurerBulkImport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnImportInsurerPolicies_Click(object sender, EventArgs e)
        {
            if (ddlPolicy_Type.SelectedValue == "1")
            {
                Import_Individual_Policies();
            }
            else
            {
                Import_Business_Policies();
            }


        }

        protected void FileUploadComplete(object sender, EventArgs e)
        {
            string filename = System.IO.Path.GetFileName(fupInsurerPolicies.FileName);
            if (filename != null)
            {
                List<C.Policy.BulkImportFromFinancer> bIItemList = new List<C.Policy.BulkImportFromFinancer>();
                P.Bulk_Import_Provider bP = new P.Bulk_Import_Provider();
                using (var reader = new StreamReader(fupInsurerPolicies.PostedFile.InputStream))
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
        private bool Import_Individual_Policies()
        {
            bool imported = false;
            //    List<C.Policy.BulkImportFromInsurance.GenericItems> giItemList = new List<C.Policy.BulkImportFromInsurance.GenericItems>();
            //    List<C.Policy.BulkImportFromInsurance.IndividualHolder> ihItemList = new List<C.Policy.BulkImportFromInsurance.IndividualHolder>();
            //    List<C.Policy.BulkImportFromInsurance.Phycisal_address> phItemList = new List<C.Policy.BulkImportFromInsurance.Phycisal_address>();
            //    List<C.Policy.BulkImportFromInsurance.Postal_Address> poItemList = new List<C.Policy.BulkImportFromInsurance.Postal_Address>();

            //    P.Bulk_Import_Provider bP = new P.Bulk_Import_Provider();

            //    //fupBankAssets.FileContent))
            //    using (var reader = new StreamReader(fupInsurerPolicies.PostedFile.InputStream))
            //    {
            //        reader.ReadLine();
            //        int i = 0;
            //        while (!reader.EndOfStream)
            //        {

            //            C.Policy.BulkImportFromInsurance.GenericItems giItem = new C.Policy.BulkImportFromInsurance.GenericItems();
            //            C.Policy.BulkImportFromInsurance.IndividualHolder ihItem = new C.Policy.BulkImportFromInsurance.IndividualHolder();
            //            C.Policy.BulkImportFromInsurance.Phycisal_address phItem = new C.Policy.BulkImportFromInsurance.Phycisal_address();
            //            C.Policy.BulkImportFromInsurance.Postal_Address poItem = new C.Policy.BulkImportFromInsurance.Postal_Address();



            //            var line = reader.ReadLine();
            //            var values = line.Split(';');


            //            //Generic data
            //            giItem.iCounterID = i;
            //            giItem.iInsurance_Company_Id = Convert.ToInt32(values[0]);
            //            giItem.vcFinancer_Name = values[1];
            //            giItem.vcFinance_Agrreement_Number = values[2];
            //            giItem.vcPolicy_Number = values[3];
            //            giItem.vcPolicy_Status = values[4];
            //            //giItem.vcPolicy_Type_Description = values[3];
            //            giItem.vcAsset_Type_Description = values[5];
            //            giItem.vcAsset_Sub_Type_Description = values[6];
            //            giItem.vcAsset_Unique_Identifier = values[7];
            //            giItem.vcAsset_Cover_Type_Description = values[8];
            //            giItem.vcPolicy_Payment_Frequency_Type_Description = values[9];

            //            giItemList.Add(giItem);


            //            //Idividual Holder data
            //            ihItem.iCounterID = i;
            //            ihItem.vcIdentification_Type_Desccription = values[10];
            //            ihItem.vcIdentification_Number = values[11];
            //            ihItem.vcPerson_Title_Description = values[12];
            //            ihItem.vcFirst_Names = values[13];
            //            ihItem.vcSurname = values[14];
            //            ihItem.bPostalAddresSameAsPhysical = values[15] == "Y" ? true : false;
            //            ihItem.vcContact_Number = values[16];
            //            ihItem.vcAlternative_Contact_Number = values[17];
            //            ihItem.vcEmail_Address = values[18];
            //            ihItemList.Add(ihItem);
            //            //Physical address data

            //            phItem.iCounterID = i;
            //            phItem.vcBuilding_Unit = values[19];
            //            phItem.vcAddress_Line_1 = values[20];
            //            phItem.vcAddress_Line_2 = values[21];
            //            phItem.vcSuburb = values[22];
            //            phItem.vcCity = values[23];
            //            phItem.vcProvince = values[24];
            //            phItem.vcPostal_Code = values[25];
            //            phItemList.Add(phItem);

            //            //Physical address data
            //            poItem.iCounterID = i;
            //            poItem.vcPOBox_Bag = values[26];
            //            poItem.vcPost_Office_Name = values[27];
            //            poItem.vcPost_Postal_Code = values[28];
            //            poItemList.Add(poItem);

            //            i = i + 1;




            //        }
            //    }
            //    imported = bP.Save_Bulk_Import_From_Insurer_Individual(giItemList, ihItemList, phItemList, poItemList);
            return imported;
        }
        private bool Import_Business_Policies()
        {
            bool imported = false;
            List<C.Policy.BulkImportFromInsurance.GenericItems> giItemList = new List<C.Policy.BulkImportFromInsurance.GenericItems>();
            List<C.Policy.BulkImportFromInsurance.BusinessHolder> biItemList = new List<C.Policy.BulkImportFromInsurance.BusinessHolder>();
            List<C.Policy.BulkImportFromInsurance.Phycisal_address> phItemList = new List<C.Policy.BulkImportFromInsurance.Phycisal_address>();
            List<C.Policy.BulkImportFromInsurance.Postal_Address> poItemList = new List<C.Policy.BulkImportFromInsurance.Postal_Address>();





            P.Bulk_Import_Provider bP = new P.Bulk_Import_Provider();

            //fupBankAssets.FileContent))
            using (var reader = new StreamReader(fupInsurerPolicies.PostedFile.InputStream))
            {
                reader.ReadLine();
                int i = 0;
                while (!reader.EndOfStream)
                {

                    C.Policy.BulkImportFromInsurance.GenericItems giItem = new C.Policy.BulkImportFromInsurance.GenericItems();
                    C.Policy.BulkImportFromInsurance.BusinessHolder biItem = new C.Policy.BulkImportFromInsurance.BusinessHolder();
                    C.Policy.BulkImportFromInsurance.Phycisal_address phItem = new C.Policy.BulkImportFromInsurance.Phycisal_address();
                    C.Policy.BulkImportFromInsurance.Postal_Address poItem = new C.Policy.BulkImportFromInsurance.Postal_Address();



                    var line = reader.ReadLine();
                    var values = line.Split(';');


                    //Generic data
                    giItem.iCounterID = i;
                    giItem.iInsurance_Company_Id = Convert.ToInt32(values[0]);
                    giItem.vcFinancer_Name = values[1];
                    giItem.vcFinance_Agrreement_Number = values[2];
                    giItem.vcPolicy_Number = values[3];
                    giItem.vcPolicy_Status = values[4];
                    //giItem.vcPolicy_Type_Description = values[3];
                    giItem.vcAsset_Type_Description = values[5];
                    giItem.vcAsset_Sub_Type_Description = values[6];
                    giItem.vcAsset_Unique_Identifier = values[7];
                    giItem.vcAsset_Cover_Type_Description = values[8];
                    giItem.vcPolicy_Payment_Frequency_Type_Description = values[9];

                    giItemList.Add(giItem);


                    //Idividual Holder data
                    biItem.iCounterID = i;
                    biItem.vcBusiness_Name = values[10];
                    biItem.vcBusiness_Registration_Number = values[11];
                    biItem.vcBusiness_Contact_Fullname = values[12];
                    biItem.bPostalAddresSameAsPhysical = values[13] == "Y" ? true : false;
                    biItem.vcBusiness_Contact_Number = values[14];
                    biItem.vcBusiness_Contact_Alternative_Number = values[15];
                    biItem.vcBusiness_Email_Address = values[16];

                    biItemList.Add(biItem);
                    //Physical address data

                    phItem.iCounterID = i;
                    phItem.vcBuilding_Unit = values[17];
                    phItem.vcAddress_Line_1 = values[18];
                    phItem.vcAddress_Line_2 = values[19];
                    phItem.vcSuburb = values[20];
                    phItem.vcCity = values[21];
                    phItem.vcProvince = values[22];
                    phItem.vcPostal_Code = values[23];
                    phItemList.Add(phItem);

                    //Physical address data
                    poItem.iCounterID = i;
                    poItem.vcPOBox_Bag = values[24];
                    poItem.vcPost_Office_Name = values[25];
                    poItem.vcPost_Postal_Code = values[26];
                    poItemList.Add(poItem);

                    i = i + 1;




                }
            }
            imported = bP.Save_Bulk_Import_From_Insurer_Business(giItemList, biItemList, phItemList, poItemList);
            return imported;
        }
    }
}