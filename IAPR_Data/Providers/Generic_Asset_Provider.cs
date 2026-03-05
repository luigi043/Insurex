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

using C = IAPR_Data.Classes;
using U = IAPR_Data.Utils;

namespace IAPR_Data.Providers
{
    public class Generic_Asset_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());
        public int Get_Asset_ID_By_Finance_Number(string vcFinance_Agrreement_Number, int iAsset_Type_Id, int iPartner_id)
        {
            int iAsset_Id = 0;


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@iAsset_Type_Id",iAsset_Type_Id),
                    new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(vcFinance_Agrreement_Number,true)),
                    new SqlParameter("@iPartner_id",iPartner_id)
            };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spGet_Asset_ID_By_Finance_Number", parameters);

            while (dr.Read())
            {
                if (dr["iAsset_Id"].ToString() != "")
                {
                    iAsset_Id = Convert.ToInt32(dr["iAsset_Id"].ToString());
                }
            }





            return iAsset_Id;
        }
        public DataSet Get_Asset_All_Details_By_Asset_ID(int iAsset_Type_Id, int iAsset_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();




            SqlCommand cmd = new SqlCommand("dbo.spGet_Asset_All_Details_By_Asset_ID", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iAsset_Type_Id", SqlDbType.Int).Value = iAsset_Type_Id;
            cmd.Parameters.Add("@iAsset_Id", SqlDbType.Int).Value = iAsset_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();


            //ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance agrreement number" });
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 1, new string[] { "Insurer", "Policy number" });

            switch (ds.Tables[5].Rows[0]["iAsset_Type_Id"].ToString())
            {
                case "1":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Vin number" });
                    break;
                case "2":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "EFR Number", "Sectional Title Number", "Sectional Title Name" });
                    break;
                case "3":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Name/Emblem", "Hull/Registration number" });
                    break;
                case "4":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Tail number" });
                    break;
                case "5":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Stock description" });
                    break;
                case "6":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Description" });
                    break;
                case "7":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Serial number" });
                    break;
                case "8":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Serial number" });
                    break;
                case "9":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Description" });
                    break;
                case "10":
                    //ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Asset description", "Asset sub-type description", "Make", "Model", "Model Variant" });
                    break;
                case "11":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Serial number" });
                    break;

            }


            if (ds.Tables[1].Rows.Count > 0)
            {
                if (ds.Tables[1].Rows[0]["Policy type"].ToString() == "Personal")
                {
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 2, new string[] { "Identification number", "First names", "Surname", "Contact number", "Alternative contact number", "Email address" });
                }
                else
                {
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 2, new string[] { "Business name", "Registration number", "Contact fullname", "Contact number", "Alternative contact number", "Email address" });
                }
            }

            if (ds.Tables[3].Rows.Count > 0)
            {
                ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 3, new string[] { "Unit/Building", "Address line 1", "Address line 2", "Suburb", "City", "Postal code" });
            }

            if (ds.Tables[4].Rows.Count > 0)
            {
                ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 4, new string[] { "P O Box", "Post office", "Postal code" });
            }

            return ds;
        }
        public DataSet Get_Unprocessed_NewAssetsByAPI()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("dbo.spGet_UnProcessed_NewAssetsByAPI_Id", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            return ds;
        }
        public int Processed_NewAssetsByAPI(int iEnstry_Id)
        {
            int alignmentId = 0;


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@iEntry_Id",iEnstry_Id)

            };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spUpd_Process_NewIndividualVehicleByAPI", parameters);

            while (dr.Read())
            {
                if (dr["iAsset_Policy_Alignment_Id"].ToString() != "")
                {
                    alignmentId = Convert.ToInt32(dr["iAsset_Policy_Alignment_Id"].ToString());
                }
            }





            return alignmentId;
        }

        public List<C.AssetTypes.Uninsured_Assets> Get_Unconfirmed_Assets_Financer(int iFinancier_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Unconfirmed_Assets_By_Financier", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancier_Id", SqlDbType.Int).Value = iFinancier_Id;

            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            string dateSinceUninsured = string.Empty;
            List<C.AssetTypes.Uninsured_Assets> uninsured_AssetsL = new List<C.AssetTypes.Uninsured_Assets>();
            uninsured_AssetsL = (from DataRow dr in ds.Tables[0].Rows
                                 select new C.AssetTypes.Uninsured_Assets()
                                 {
                                     // iPolicy_Id = dr["iPolicy_Id"] != DBNull.Value ? Convert.ToInt32(dr["iPolicy_Id"].ToString()) : 0,
                                     iAsset_Id = dr["iAsset_Id"] != DBNull.Value ? Convert.ToInt32(dr["iAsset_Id"].ToString()) : 0,

                                     iAsset_Type_Id = dr["iAsset_Type_Id"] != DBNull.Value ? Convert.ToInt32(dr["iAsset_Type_Id"].ToString()) : 0,
                                     iFinancer_Id = dr["iFinancer_Id"] != DBNull.Value ? Convert.ToInt32(dr["iFinancer_Id"].ToString()) : 0,
                                     vcFinancer_Name = dr["Financer"] != DBNull.Value ? U.CryptorEngine.GenericDecrypt(dr["Financer"].ToString(), true) : "",
                                     vcFinance_Agrreement_Number = dr["Finance Agrreement Number"] != DBNull.Value ? U.CryptorEngine.GenericDecrypt(dr["Finance Agrreement Number"].ToString(), true) : "",
                                     vcInsurance_Company_Name = dr["Insurer Company"] != DBNull.Value && dr["Insurer Company"].ToString() != "None" ? U.CryptorEngine.GenericDecrypt(dr["Insurer Company"].ToString(), true) : "None",
                                     vcPolicy_Number = dr["Insurance Policy Number"] != DBNull.Value && dr["Insurance Policy Number"].ToString() != "Unconfirmed" ? U.CryptorEngine.GenericDecrypt(dr["Insurance Policy Number"].ToString(), true) : "Unconfirmed",
                                     vcAsset_Type_Description = dr["Asset Type"] != DBNull.Value ? dr["Asset Type"].ToString() : "",
                                     vcAsset_SubType_Description = dr["Asset Description"] != DBNull.Value ? dr["Asset Description"].ToString() : "",
                                     mAsset_Finance_Value = dr["Finance Value"] != DBNull.Value ? string.Format("{0:c}", dr["Finance Value"]) : string.Format("{0:C}", 0),
                                     dtDate_since_Unisured = dr["Date since Unisured"] != DBNull.Value ? string.Format("{0:dd/MMM/yyyy}", dr["Date since Unisured"].ToString()) : "",
                                     bPolicy_Holder_Confirmed = dr["Customer Policy ID"] != DBNull.Value && Convert.ToInt32(dr["Customer Policy ID"]) > 0 ? true : false,
                                     iAlighnment_Id = dr["iAlighnment_ID"] != DBNull.Value ? Convert.ToInt32(dr["iAlighnment_ID"].ToString()) : 0,
                                 }).ToList();


            return uninsured_AssetsL;


        }

        public bool Check_FinanceNumber_Exists(int iFinancer_Id, string vcFinance_Agrreement_Number)
        {
            bool Policy_exists = false;


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iFinancer_Id",iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",vcFinance_Agrreement_Number),
         };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spGet_CheckFinanceNumberExists", parameters);

            while (dr.Read())
            {
                Policy_exists = Convert.ToBoolean(dr["Exists"].ToString());
            }



            return Policy_exists;
        }
        public SqlDataReader Get_AssetsAwaitingInsurance()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            return SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
                "spGet_AssetsAwaitingInsurance");



        }

        #region API-related
        public C.Response Save_Bulk_UpdateAssetFinanceValue(C.AssetTypes.UpdateAssetFinanceValueRequest updateAssetFinanceValueRequest, int iFanancerId)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {

            var dtUpdateAssetFinanceValue = C.Common.ConvertToDataTable.ConvertTODataTable_Asset_Finance_Value(updateAssetFinanceValueRequest.assetDetails, updateAssetFinanceValueRequest.trasactionId);


            SqlParameter[] arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@dtUpdateAssetFinanceValue", dtUpdateAssetFinanceValue);
            arParams[1] = new SqlParameter("@iFanancerId", iFanancerId);
            using (
                    var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRAPI"].ToString(),
                    CommandType.StoredProcedure, "spIns_Import_Asset_Finance_Value_Updates", arParams))
            {
                //return null;//dr.HasRows ? true : false;
            }

            return null;

        }
        public C.Response Save_Bulk_UpdateAssetInsuredValue(C.AssetTypes.UpdateAssetInsuredValueRequest updateAssetInsuredValueRequest, int iPartnerId)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {

            var dtVehicleAssets = C.Common.ConvertToDataTable.ConvertTODataTable_InsuredRequest_Vehicle(updateAssetInsuredValueRequest.vehicleAssets, updateAssetInsuredValueRequest.trasactionId);//, updateAssetFinanceValueRequest.trasactionId);
            var dtPropertyAssets = C.Common.ConvertToDataTable.ConvertTODataTable_InsuredRequest_Property(updateAssetInsuredValueRequest.propertyAssets, updateAssetInsuredValueRequest.trasactionId);
            var dtWatercraftAssets = C.Common.ConvertToDataTable.ConvertTODataTable_InsuredRequest_Watercraft(updateAssetInsuredValueRequest.watercraftAssets, updateAssetInsuredValueRequest.trasactionId);
            var dtAviationAssets = C.Common.ConvertToDataTable.ConvertTODataTable_InsuredRequest_Aviation(updateAssetInsuredValueRequest.aviationtAssets, updateAssetInsuredValueRequest.trasactionId);
            var dtMachineryAssets = C.Common.ConvertToDataTable.ConvertTODataTable_InsuredRequest_Machinery(updateAssetInsuredValueRequest.machineryAssets, updateAssetInsuredValueRequest.trasactionId);
            var dtPlantEquipmentAssets = C.Common.ConvertToDataTable.ConvertTODataTable_InsuredRequest_PlantEquipment(updateAssetInsuredValueRequest.plantEquipmentAssets, updateAssetInsuredValueRequest.trasactionId);
            var dtElectronicEquipmentAssets = C.Common.ConvertToDataTable.ConvertTODataTable_InsuredRequest_ElectronicEquipment(updateAssetInsuredValueRequest.electronicEquipmentAssets, updateAssetInsuredValueRequest.trasactionId);


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
                    CommandType.StoredProcedure, "spIns_Import_Asset_Insured_Value_Updates", arParams))
            {
                //return null;//dr.HasRows ? true : false;
            }

            return null;

        }

        public C.Response Save_Bulk_UpdateAssetCover(C.AssetTypes.UpdateAssetCoverRequest updateAssetCoverRequest, int iPartnerId)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {

            var dtVehicleAssets = C.Common.ConvertToDataTable.ConvertTODataTable_CoverRequest_Vehicle(updateAssetCoverRequest.vehicleAssets, updateAssetCoverRequest.trasactionId);//, updateAssetFinanceValueRequest.trasactionId);
            var dtPropertyAssets = C.Common.ConvertToDataTable.ConvertTODataTable_CoverRequest_Property(updateAssetCoverRequest.propertyAssets, updateAssetCoverRequest.trasactionId);
            var dtWatercraftAssets = C.Common.ConvertToDataTable.ConvertTODataTable_CoverRequest_Watercraft(updateAssetCoverRequest.watercraftAssets, updateAssetCoverRequest.trasactionId);
            var dtAviationAssets = C.Common.ConvertToDataTable.ConvertTODataTable_CoverRequest_Aviation(updateAssetCoverRequest.aviationtAssets, updateAssetCoverRequest.trasactionId);
            var dtMachineryAssets = C.Common.ConvertToDataTable.ConvertTODataTable_CoverRequest_Machinery(updateAssetCoverRequest.machineryAssets, updateAssetCoverRequest.trasactionId);
            var dtPlantEquipmentAssets = C.Common.ConvertToDataTable.ConvertTODataTable_CoverRequest_PlantEquipment(updateAssetCoverRequest.plantEquipmentAssets, updateAssetCoverRequest.trasactionId);
            var dtElectronicEquipmentAssets = C.Common.ConvertToDataTable.ConvertTODataTable_CoverRequest_ElectronicEquipment(updateAssetCoverRequest.electronicEquipmentAssets, updateAssetCoverRequest.trasactionId);


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
                    CommandType.StoredProcedure, "spIns_Import_Asset_Cover_Updates", arParams))
            {
                //return null;//dr.HasRows ? true : false;
            }

            return null;

        }
        #endregion

    }
}
//private DataTable ConvertAssetDetailsTODataTable(IEnumerable<C.AssetTypes.UpdateAssetFinanceValueRequest.AssetDetails> policy_details, int trasactionId)
//{

//    var dtPolicyDetails = new DataTable();
//    dtPolicyDetails.Columns.Add("iCounterID", typeof(Int32));
//    dtPolicyDetails.Columns.Add("iTransadtionID", typeof(String));
//    dtPolicyDetails.Columns.Add("iAssetTypeId", typeof(int));
//    dtPolicyDetails.Columns.Add("vcFinanceAgreementNumber", typeof(String));
//    dtPolicyDetails.Columns.Add("dcFinanceValue", typeof(decimal));
//    int c = 1;
//    foreach (C.AssetTypes.UpdateAssetFinanceValueRequest.AssetDetails i in policy_details)
//    {
//        DataRow workRow = dtPolicyDetails.NewRow();
//        workRow["iCounterID"] = c;
//        workRow["iTransadtionID"] = trasactionId;
//        workRow["iassetTypeId"] = i.assetTypeId;
//        workRow["vcFinanceAgreementNumber"] = i.financeAgreementNumber;
//        workRow["dcFinanceValue"] = i.financeValue;
//        dtPolicyDetails.Rows.Add(workRow);

//        c++;
//    }

//    return dtPolicyDetails;
//}








