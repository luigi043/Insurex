using IAPR_Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAPR_Data.Classes;
using System.Configuration;
using System.Net;
using System.Text;
using System.IO;
using System.Data;
using Microsoft.Data.SqlClient;

using IAPR_Data.Interfaces;
using C = IAPR_Data.Classes;
using U = IAPR_Data.Utils;
namespace IAPR_Data.Providers
{
    public class Policy_Provider //: iPolicy_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());

        public int Save_New_Policy_Personal(Classes.Policy.Policy p)
        {
            int newPolicy_Id = 0;

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlParameter[] parameters = new SqlParameter[]
            {
                 new SqlParameter("@iInsurance_Company_Id",p.iInsurance_Company_Id),
                 new SqlParameter("@vcPolicy_Number",U.CryptorEngine.GenericEncrypt(p.vcPolicy_Number, true)),

                 new SqlParameter("@iPolicy_Type_Id",p.iPolicy_Type_Id),
                 new SqlParameter("@iPolicy_Payment_Frequency_Type_Id", p.iPolicy_Payment_Frequency_Type_Id),

                 new SqlParameter("@iIdentification_Type_Id",p.policy_Holder_Individual.iIdentification_Type_Id),
                 new SqlParameter("@iPerson_Title_Id",p.policy_Holder_Individual.iPerson_Title_Id),
                 new SqlParameter("@vcFirst_Names", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.vcFirst_Names,true)),
                 new SqlParameter("@vcSurname", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.vcSurname,true)),
                 new SqlParameter("@vcIdentification_Number", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.vcIdentification_Number,true)),

                 new SqlParameter("@vcContact_Number", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.vcContact_Number,true)),
                 new SqlParameter("@vcAlternative_Contact_Number", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.vcAlternative_Contact_Number,true)),
                 new SqlParameter("@vcEmail_Address", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.vcEmail_Address,true)),

                 new SqlParameter("@vcBuilding_Unit", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcBuilding_Unit,true)),
                 new SqlParameter("@vcAddress_Line_1", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcAddress_Line_1,true)),
                 new SqlParameter("@vcAddress_Line_2", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcAddress_Line_2,true)),
                 new SqlParameter("@vcSuburb", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcSuburb,true)),
                 new SqlParameter("@vcCity", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcCity,true)),
                 new SqlParameter("@iProvince_Id",p.policy_Holder_Individual.physical_Address.iProvince_Id),
                 new SqlParameter("@vcPostal_Code", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcPostal_Code, true)),


                 new SqlParameter( "@vcPOBox_Bag", (!p.policy_Holder_Individual.bPostalAddresSameAsPhysical) ?  U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.postal_Address.vcPOBox_Bag, true) : null),
                 new SqlParameter("@vcPost_Office_Name",(!p.policy_Holder_Individual.bPostalAddresSameAsPhysical) ?  U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.postal_Address.vcPost_Office_Name, true) : null),
                 new SqlParameter("@vcPost_Postal_Code",(!p.policy_Holder_Individual.bPostalAddresSameAsPhysical) ?  U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.postal_Address.vcPost_Postal_Code, true) : null),
                 new SqlParameter("@bPostalAddresSameAsPhysical",p.policy_Holder_Individual.bPostalAddresSameAsPhysical),
            };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spIns_Save_New_Policy_Personal", parameters);

            while (dr.Read())
            {
                newPolicy_Id = Convert.ToInt32(dr["New_Policy_Id"].ToString());
            }
            return newPolicy_Id;
        }
        public int Save_New_Policy_Business(Classes.Policy.Policy p)
        {
            int newPolicy_Id = 0;

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iInsurance_Company_Id",p.iInsurance_Company_Id),
                new SqlParameter("@vcPolicy_Number",U.CryptorEngine.GenericEncrypt(p.vcPolicy_Number,true)),

                new SqlParameter("@iPolicy_Type_Id",p.iPolicy_Type_Id),
                new SqlParameter("@iPolicy_Payment_Frequency_Type_Id", p.iPolicy_Payment_Frequency_Type_Id),

                new SqlParameter("@vcBusiness_Name",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.vcBusiness_Name,true)),
                new SqlParameter("@vcBusiness_Registration_Number",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.vcBusiness_Registration_Number,true)),
                new SqlParameter("@Business_Contact_Fullname",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.vcBusiness_Contact_Fullname,true)),
                new SqlParameter("@vcBusiness_Contact_Number",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.vcBusiness_Contact_Number,true)),
                new SqlParameter("@vcBusiness_Contact_Alternative_Number",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.vcBusiness_Contact_Alternative_Number,true)),
                new SqlParameter("@vcBusiness_Email_Address",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.vcBusiness_Email_Address,true)),

                new SqlParameter("@vcBuilding_Unit",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.physical_Address.vcBuilding_Unit,true)),
                new SqlParameter("@vcAddress_Line_1",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.physical_Address.vcAddress_Line_1,true)),
                new SqlParameter("@vcAddress_Line_2",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.physical_Address.vcAddress_Line_2,true)),
                new SqlParameter("@vcSuburb",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.physical_Address.vcSuburb,true)),
                new SqlParameter("@vcCity",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.physical_Address.vcCity,true)),
                new SqlParameter("@iProvince_Id",p.policy_Holder_Business.physical_Address.iProvince_Id),
                new SqlParameter("@vcPostal_Code",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.physical_Address.vcPostal_Code,true)),


                new SqlParameter( "@vcPOBox_Bag", (!p.policy_Holder_Business.bPostalAddresSameAsPhysical) ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.postal_Address.vcPOBox_Bag,true) : null),
                new SqlParameter("@vcPost_Office_Name",(!p.policy_Holder_Business.bPostalAddresSameAsPhysical) ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.postal_Address.vcPost_Office_Name,true) : null),
                new SqlParameter("@vcPost_Postal_Code",(!p.policy_Holder_Business.bPostalAddresSameAsPhysical) ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.postal_Address.vcPost_Postal_Code,true) : null),
                new SqlParameter("@bPostalAddresSameAsPhysical",p.policy_Holder_Business.bPostalAddresSameAsPhysical),

        };
            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
"spIns_Save_New_Policy_Business", parameters);

            while (dr.Read())
            {
                newPolicy_Id = Convert.ToInt32(dr["New_Policy_Id"].ToString());
            }
            return newPolicy_Id;
        }

        public int Save_Single_Policy_NonPayment(int iPolicy_Id, int iAffected_Period_Id, int iAffected_Year, string dtDateOfNonPayment)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {
            int iPolicy_Transaction_Id = 0;


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",iPolicy_Id),
                new SqlParameter("@iAffected_Period_Id",iAffected_Period_Id),
                new SqlParameter("@iAffected_Year",iAffected_Year),
                new SqlParameter("@dtDateOfNonPayment",dtDateOfNonPayment),
            };
            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
    "spIns_Save_Policy_Transaction_NonPayment", parameters);
            while (dr.Read())
            {
                iPolicy_Transaction_Id = Convert.ToInt32(dr["iPolicy_Transaction_Id"].ToString());
            }



            return iPolicy_Transaction_Id;

        }
        public bool Save_PolicyStatus(int iPolicy_Id, int iPolicy_Status_New_Id, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {
            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",iPolicy_Id),
                new SqlParameter("@iPolicy_Status_New_Id",iPolicy_Status_New_Id),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spUpd_PolicyStatus", parameters);
            updated = true;

            return updated;

        }

        public int Get_Policy_Id(int iInsurance_Company_Id, string vcPolicy_Number)
        {
            int Policy_Id = 0;

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlParameter[] parameters = new SqlParameter[]
            {

                    new SqlParameter("@iInsurance_Company_Id",iInsurance_Company_Id),
                    new SqlParameter("@vcPolicy_Number",U.CryptorEngine.GenericEncrypt(vcPolicy_Number,true)),

            };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spGet_Policy_Id", parameters);

            while (dr.Read())
            {
                Policy_Id = Convert.ToInt32(dr["iPolicy_Id"].ToString());
            }


            return Policy_Id;
        }
        public SqlDataReader Get_Individual_Policy_Details(int iInsurance_Company_Id, string vcPolicy_Number)
        {


            //var dtPackageEntries = GetPackageTable(cartItems.PackageEntries);
            //var dsLotterEntries = GetLotteryEntries(cartItems.LotteryEntries);
            //var dtLotteryMainInfo = dsLotterEntries.Tables[0];
            //var dtLotteryCombinations = dsLotterEntries.Tables[1];

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iInsurance_Company_Id",iInsurance_Company_Id),
                
                //new SqlParameter("@iPolicy_Type",iPolicy_Type),
                new SqlParameter("@vcPolicy_Number", U.CryptorEngine.GenericEncrypt(vcPolicy_Number,true)),



        };

            return (SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
                "spGet_Policy_PaymentFrequency_Details", parameters));





        }
        public int Get_Policy_Status(int iPolicy_Id)
        {
            int status = 0;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iPolicy_Id",iPolicy_Id),
            };

            SqlDataReader dr = (SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
             "spGet_Policy_Status", parameters));
            while (dr.Read())
            {
                status = Convert.ToInt32(dr["iPolicy_Status_Id"].ToString());
            }
            return status;
        }

        public bool Update_Policy_Resume_Cover(int iPolicy_Id)
        {
            bool updated = false;


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",iPolicy_Id),

            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
             "spUpd_Policy_Resume_Cover", parameters);
            updated = true;
            return updated;
        }
        public bool Update_Policy_Address(int iPolicy_Id, Classes.Policy.Policy p)
        {
            bool updated = false;

            //var dtPackageEntries = GetPackageTable(cartItems.PackageEntries);
            //var dsLotterEntries = GetLotteryEntries(cartItems.LotteryEntries);
            //var dtLotteryMainInfo = dsLotterEntries.Tables[0];
            //var dtLotteryCombinations = dsLotterEntries.Tables[1];

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {


                new SqlParameter("@iPolicy_Id",iPolicy_Id),
                new SqlParameter("@bPostalAddresSameAsPhysical",p.policy_Holder_Individual.bPostalAddresSameAsPhysical),
                new SqlParameter("@vcBuilding_Unit", p.policy_Holder_Individual.physical_Address.vcBuilding_Unit != null ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcBuilding_Unit, true) : null),
                new SqlParameter("@vcAddress_Line_1",p.policy_Holder_Individual.physical_Address.vcAddress_Line_1 != null ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcAddress_Line_1, true) : null),
                new SqlParameter("@vcAddress_Line_2",p.policy_Holder_Individual.physical_Address.vcAddress_Line_2 != null ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcAddress_Line_2, true) : null),
                new SqlParameter("@vcSuburb",p.policy_Holder_Individual.physical_Address.vcSuburb != null ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcSuburb, true) : null),
                new SqlParameter("@vcCity",p.policy_Holder_Individual.physical_Address.vcCity != null ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcCity, true) : null),
                 new SqlParameter("@iProvince_Id",p.policy_Holder_Individual.physical_Address.iProvince_Id),
                new SqlParameter("@vcPostal_Code",p.policy_Holder_Individual.physical_Address.vcPostal_Code != null ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcPostal_Code, true) : null),


                new SqlParameter("@vcPOBox_Bag", (!p.policy_Holder_Individual.bPostalAddresSameAsPhysical) ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.postal_Address.vcPOBox_Bag,true):null),
                new SqlParameter("@vcPost_Office_Name",(!p.policy_Holder_Individual.bPostalAddresSameAsPhysical) ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.postal_Address.vcPost_Office_Name,true):null),
                new SqlParameter("@vcPost_Postal_Code",(!p.policy_Holder_Individual.bPostalAddresSameAsPhysical) ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.postal_Address.vcPost_Postal_Code,true):null),


        };
            //sqlConn.Open();
            //da = new SqlDataAdapter(cmd);
            //da.Fill(ds);
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
                "spUpd_Policy_Address", parameters);
            updated = true;
            return updated;
        }
        public bool Check_Policy_Exists(int iInsurance_Company_Id, string vcPolicy_Number)
        {
            bool Policy_exists = false;


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iInsurance_Company_Id",iInsurance_Company_Id),
                new SqlParameter("@vcPolicy_Number", U.CryptorEngine.GenericEncrypt(vcPolicy_Number,true)),
         };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spGet_CheckPolicyExists", parameters);

            while (dr.Read())
            {
                Policy_exists = Convert.ToBoolean(dr["Exists"].ToString());
            }



            return Policy_exists;
        }
        //private DataTable GetPolicy_NonPayment(IEnumerable<C.Policy.PolicyNonPaymentRequest.Policy_details> policy_details)
        //{
        //    var dataT = new DataTable();
        //    foreach (var p in policy_details)
        //    {
        //        dataT.Merge(ConvertPolicy_detailsToDatatable(p));
        //    }

        //    return dataT;
        //}

        //private DataTable ConvertPolicy_detailsToDatatable(IEnumerable<C.Policy.PolicyNonPaymentRequest.Policy_details> pD)
        //{

        //    var dataTable = new DataTable();

        //    foreach (var info in typeof(C.Policy.PolicyNonPaymentRequest.Policy_details).GetProperties())
        //    {

        //            dataTable.Columns.Add(info.Name, info.PropertyType);
        //    }
        //    //dataTable.Columns.Add("iEventId", typeof(Int32));

        //    dataTable.AcceptChanges();


        //    foreach (var item in packageEntryParticipant)
        //    {
        //        for (var i = 0; i < item.Number_of_Entries; i++)
        //        {
        //            var datos = new PackageEntryParticipant();
        //            datos = item;
        //            DataRow row = dataTable.NewRow();

        //            foreach (var property in datos.GetType().GetProperties())
        //            {
        //                if (property.Name != "Number_of_Entries")
        //                    row[property.Name] = property.GetValue(datos, null);
        //            }

        //            row["iEventId"] = eventId;
        //            dataTable.Rows.Add(row);
        //        }
        //    }

        //    return dataTable;
        //}
        public DataSet GetPolicy_All_Assets(int ipolicy_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();




            SqlCommand cmd = new SqlCommand("dbo.spGet_Policy_All_Assets", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPolicy_Id", SqlDbType.Int).Value = ipolicy_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new int[] { 5, 13 });
            return ds;



        }

        public DataSet Get_Policy_Holder_All_Assets(int ipolicy_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();




            SqlCommand cmd = new SqlCommand("dbo.spGet_Policy_Holder_All_Assets", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPolicy_Id", SqlDbType.Int).Value = ipolicy_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Insurer", "Policy number" });

            if (ds.Tables[0].Rows[0]["iPolicy_Type_Id"].ToString() == "1")
            {
                ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 1, new string[] { "Identification number", "Firstname", "Surname", "Contact number", "Alternative contact number", "Email address" });
            }
            if (ds.Tables[0].Rows[0]["iPolicy_Type_Id"].ToString() == "2")
            {
                ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 1, new string[] { "Business name", "Registration number", "Contact fullname", "Contact number", "Alternative contact number", "Email address" });
            }
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 2, new string[] { "Unit/Building", "Address line 1", "Address line 2", "Suburb", "City", "Postal code" });

            if (ds.Tables[3].Rows.Count > 0)
            {
                ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 3, new string[] { "P O Box", "Post office", "Postal code", "Contact number", "Alternative contact number", "Email address" });
            }

            if (ds.Tables[4].Rows.Count > 0)
            {
                ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 4, new string[] { "Financer", "Asset Identifier" });
            }

            return ds;


        }


        #region API-related

        public C.Response Save_Bulk_Policy_NonPayment(C.Policy.PolicyNonPaymentRequest policyNonPaymentRequest, int sourceIdentifier)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {
            C.Response res = new Response();
            try
            {


                var dtPolicyDetails = C.Common.ConvertToDataTable.ConvertTODataTable_Policy_Nonpayment(policyNonPaymentRequest.policyDetails, policyNonPaymentRequest.trasactionId);
                SqlParameter[] arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@dtPolicyDetails", dtPolicyDetails);
                arParams[1] = new SqlParameter("@sourceIdentifier", sourceIdentifier);
                using (
                        var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRAPI"].ToString(),
                        CommandType.StoredProcedure, "spIns_Import_BulkPolicy_NonPayment", arParams))
                    res.statusCode = 0;
            }
            catch (Exception)
            {

                res.statusCode = 1;
                res.statusMessage = "Error";
                res.supportMessages[0] = "Error occurd saving ";
            }
            return res;

        }
        public C.Response Save_Bulk_Policy_Status(C.Policy.PolicyStatusRequest policyStatusRequest, int sourceIdentifier)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {

            var dtPolicyDetails = C.Common.ConvertToDataTable.ConvertTODataTable_Policy_Status(policyStatusRequest.policyDetails, policyStatusRequest.trasactionId);


            SqlParameter[] arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@dtPolicyDetails", dtPolicyDetails);
            arParams[1] = new SqlParameter("@sourceIdentifier", sourceIdentifier);
            using (
                    var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRAPI"].ToString(),
                    CommandType.StoredProcedure, "spIns_Import_Policy_Status", arParams))
            {
                //return null;//dr.HasRows ? true : false;
            }

            return null;

        }
        public C.Response Save_Bulk_RemoveAsset(C.Policy.RemoveAssetRequest updateAssetCoverRequest, int iPartnerId)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {

            var dtVehicleAssets = C.Common.ConvertToDataTable.ConvertTODataTable_RemoveAsset_Vehicle(updateAssetCoverRequest.vehicleAssets, updateAssetCoverRequest.trasactionId);//, updateAssetFinanceValueRequest.trasactionId);
            var dtPropertyAssets = C.Common.ConvertToDataTable.ConvertTODataTable_RemoveAsset_Property(updateAssetCoverRequest.propertyAssets, updateAssetCoverRequest.trasactionId);
            var dtWatercraftAssets = C.Common.ConvertToDataTable.ConvertTODataTable_RemoveAsset_Watercraft(updateAssetCoverRequest.watercraftAssets, updateAssetCoverRequest.trasactionId);
            var dtAviationAssets = C.Common.ConvertToDataTable.ConvertTODataTable_RemoveAsset_Aviation(updateAssetCoverRequest.aviationtAssets, updateAssetCoverRequest.trasactionId);
            var dtMachineryAssets = C.Common.ConvertToDataTable.ConvertTODataTable_RemoveAsset_Machinery(updateAssetCoverRequest.machineryAssets, updateAssetCoverRequest.trasactionId);
            var dtPlantEquipmentAssets = C.Common.ConvertToDataTable.ConvertTODataTable_RemoveAsset_PlantEquipment(updateAssetCoverRequest.plantEquipmentAssets, updateAssetCoverRequest.trasactionId);
            var dtElectronicEquipmentAssets = C.Common.ConvertToDataTable.ConvertTODataTable_RemoveAsset_ElectronicEquipment(updateAssetCoverRequest.electronicEquipmentAssets, updateAssetCoverRequest.trasactionId);


            SqlParameter[] arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@iPartner_Id", iPartnerId);
            arParams[1] = new SqlParameter("@dtVehicleAssets", dtVehicleAssets);
            arParams[2] = new SqlParameter("@dtPropertyAssets", dtPropertyAssets);
            arParams[3] = new SqlParameter("@dtWatercraftAssets", dtWatercraftAssets);
            arParams[4] = new SqlParameter("@dtAviationAssets", dtAviationAssets);
            arParams[5] = new SqlParameter("@dtMachineryAssets", dtMachineryAssets);
            arParams[6] = new SqlParameter("@dtPlantEquipmentAssets", dtPlantEquipmentAssets);
            arParams[7] = new SqlParameter("@dtElectronicEquipmentAssets", dtElectronicEquipmentAssets);

            using (
                    var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRAPI"].ToString(),
                    CommandType.StoredProcedure, "spIns_Import_Asset_Remove_Assets_From_Cover", arParams))
            {
                //return null;//dr.HasRows ? true : false;
            }

            return null;

        }

        public int Remove_Asset_From_Policy(int iAsset_Id, int @iAsset_Type_Id, int iPolicy_Id, string vcRemoval_Reason, string dtDateOfRemoval, string vcLinkKey)
        {

            int alignmentId = 0;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iAsset_Id",iAsset_Id),
                new SqlParameter("@iAsset_Type_Id",iAsset_Type_Id),
                new SqlParameter("@iPolicy_Id",iPolicy_Id),
                new SqlParameter("@vcRemoval_Reason",vcRemoval_Reason),
                new SqlParameter("@dtDateOfRemoval",dtDateOfRemoval),
                new SqlParameter("@vcLinkKey", vcLinkKey),
            };
            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spUpd_Remove_Asset_From_Policy", parameters);
            while (dr.Read())
            {
                alignmentId = Convert.ToInt32(dr["iAsset_Policy_Alignment_Id"].ToString());
            }
            return alignmentId;

        }

        public SqlDataReader Get_Asset_Communications_Awaiting_Processing()
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            return (SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spGet_Asset_Communications_Awaiting_Processing"));
        }

        public void Close_Asset_Communications_Awaiting_Processing(long id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id",id),
            };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spUpd_Close_Asset_Communications_Awaiting_Processing", parameters);
        }

        #endregion
        #region Shims for Modernized API
        public DataSet Get_Policy_Transactions(int partnerId)
        {
             return new DataSet(); // Return empty for now
        }
        public DataSet Get_Pending_Policy_Confirmations(int partnerId)
        {
             return new DataSet(); // Return empty for now
        }
        public void Confirm_Policy_Cover(int policyId)
        {
             Save_PolicyStatus(policyId, 1, DateTime.Now.ToString("yyyy-MM-dd")); // 1 = Active/Confirmed
        }
        public void Reject_Policy_Cover(int policyId)
        {
             Save_PolicyStatus(policyId, 2, DateTime.Now.ToString("yyyy-MM-dd")); // 2 = Rejected/Cancelled
        }
        #endregion
        #region private
        #endregion
    }
}


//private DataTable ConvertPolicyDetailsTODataTable(IEnumerable<C.Policy.PolicyNonPaymentRequest.PolicyDetails> policy_details, int trasactionId)
//{

//    var dtPolicyDetails = new DataTable();
//    dtPolicyDetails.Columns.Add("iCounterID", typeof(Int32));
//    dtPolicyDetails.Columns.Add("iTransadtionID", typeof(Int32));
//    dtPolicyDetails.Columns.Add("vcPolicy_Number", typeof(String));
//    dtPolicyDetails.Columns.Add("iAffected_Year", typeof(Int32));
//    dtPolicyDetails.Columns.Add("vcAffected_Period", typeof(String));
//    int c = 1;
//    foreach (C.Policy.PolicyNonPaymentRequest.PolicyDetails i in policy_details)
//    {
//        DataRow workRow = dtPolicyDetails.NewRow();
//        workRow["iCounterID"] = c;
//        workRow["iTransadtionID"] = trasactionId;
//        workRow["vcPolicy_Number"] = i.policyNumber;

//        workRow["iAffected_Year"] = i.affectedYear;
//        workRow["vcAffected_Period"] = i.affectedPeriod;
//        dtPolicyDetails.Rows.Add(workRow);

//        c++;
//    }

//    return dtPolicyDetails;
//}
//








