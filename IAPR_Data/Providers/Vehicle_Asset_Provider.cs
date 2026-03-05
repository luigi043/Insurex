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
    public class Vehicle_Asset_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());
        public DataSet Check_Vehicles_Details_Exist(string vcFinance_Agrreement_Number, string vcVin_Number, string vcRegistration_Number)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("dbo.spGet_Check_Vehicle_Details_Exists", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@vcFinance_Agrreement_Number", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcFinance_Agrreement_Number, true);
            cmd.Parameters.Add("@vcVin_Number", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcVin_Number, true);
            cmd.Parameters.Add("@vcRegistration_Number", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcRegistration_Number, true);
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;

        }
        public void Save_New_Vehicle_Asset(Classes.AssetTypes.Vehicle_Asset va)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",va.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",va.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",va.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number", U.CryptorEngine.GenericEncrypt(va.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@mAsset_Finance_Value",va.mAsset_Finance_Value),
                new SqlParameter("@mAsset_Insurance_Value",va.mAsset_Insurance_Value),
                new SqlParameter("@iAsset_Type_Id",va.iAsset_Type_Id),
                new SqlParameter("@iVehicle_Asset_Type_Id",va.iVehicle_Asset_Type_Id),
                //new SqlParameter("@iVehicle_Asset_Licence_Type_Id",va.iVehicle_Asset_Licence_Type_Id),
                new SqlParameter("@iAsset_Usage_Type_Id",va.iAsset_Usage_Type_Id),
                new SqlParameter("@iAsset_Condition_Id",va.iAsset_Condition_Id),
                new SqlParameter("@iVehicle_Model_Id",va.iVehicle_Model_Id),
                new SqlParameter("@iVehicle_Make_Id ",va.iVehicle_Make_Id),
                new SqlParameter("@iVehicle_Model_Variant_Id ",va.iVehicle_Model_Variant_Id),
                new SqlParameter("@vcVin_Number",U.CryptorEngine.GenericEncrypt(va.vcVin_Number,true)),
                new SqlParameter("@vcRegistration_Number",U.CryptorEngine.GenericEncrypt(va.vcRegistration_Number,true)),
                new SqlParameter("@iModel_Year",va.iModel_Year),
                new SqlParameter("@dtFinance_Start_Date",va.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",va.dtFinance_End_Date),
                // new SqlParameter("@vcVehicle_Color",va.vcVehicle_Color)


            };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_Vehicle_Asset", parameters);



        }

        public void Save_New_Vehicle_Asset_Without_Policy(Classes.AssetTypes.Vehicle_Asset va, int iAsset_Policy_Alignment_Id)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",va.iPolicy_Id),

                new SqlParameter("@iFinancer_Id",va.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(va.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@mAsset_Finance_Value",va.mAsset_Finance_Value),
                new SqlParameter("@mAsset_Insurance_Value",va.mAsset_Insurance_Value),
                new SqlParameter("@iAsset_Cover_Type_Id",va.iAsset_Cover_Type_Id),
                new SqlParameter("@iAsset_Type_Id",va.iAsset_Type_Id),
                new SqlParameter("@iVehicle_Asset_Type_Id",va.iVehicle_Asset_Type_Id),
                //new SqlParameter("@iVehicle_Asset_Licence_Type_Id",va.iVehicle_Asset_Licence_Type_Id),
                new SqlParameter("@iAsset_Usage_Type_Id",va.iAsset_Usage_Type_Id),
                new SqlParameter("@iAsset_Condition_Id",va.iAsset_Condition_Id),
                new SqlParameter("@iVehicle_Model_Id",va.iVehicle_Model_Id),
                new SqlParameter("@iVehicle_Make_Id ",va.iVehicle_Make_Id),
                new SqlParameter("@iVehicle_Model_Variant_Id ",va.iVehicle_Model_Variant_Id),
                new SqlParameter("@vcVin_Number",U.CryptorEngine.GenericEncrypt(va.vcVin_Number,true)),
                new SqlParameter("@vcRegistration_Number",U.CryptorEngine.GenericEncrypt(va.vcRegistration_Number,true)),
                new SqlParameter("@iModel_Year",va.iModel_Year),
                new SqlParameter("@dtFinance_Start_Date",va.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",va.dtFinance_End_Date),
               new SqlParameter("@iAsset_Policy_Alignment_Id",iAsset_Policy_Alignment_Id), 
                // new SqlParameter("@vcVehicle_Color",va.vcVehicle_Color)


            };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_Vehicle_Asset_Without_Policy", parameters);



        }

        public DataSet Get_FormFields_Policy_Update_ChangeCover_Vehicle(int ipolicy_Id, int iVehicle_Asset_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFields_Policy_Update_ChangeCover_Vehicle", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPolicy_Id", SqlDbType.Int).Value = ipolicy_Id;
            cmd.Parameters.Add("@iVehicle_Asset_Id", SqlDbType.Int).Value = iVehicle_Asset_Id;
            cmd.Parameters.Add("@iAsset_Type_Id", SqlDbType.Int).Value = C.Common.Common.Asset_Type.Vehicle;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }
        public DataSet GetPolicy_Vehicles_Assets(string ipolicy_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_Policy_Vehicle_Assets", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPolicy_Id", SqlDbType.Int).Value = Convert.ToInt32(ipolicy_Id);
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;
        }

        //public DataSet Get_Non_Payment_Transactions(int iFinancer_Id, int iPeriod, int iYear)
        //{
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter da = new SqlDataAdapter();

        //    try
        //    {


        //        SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Monthly_NonPayment", sqlConn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@iFinancer_Id", SqlDbType.Int).Value = iFinancer_Id;
        //        cmd.Parameters.Add("@iPeriod_Id", SqlDbType.Int).Value = iPeriod;
        //        cmd.Parameters.Add("@iAffected_Year", SqlDbType.Int).Value = iYear;
        //        sqlConn.Open();
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(ds);
        //        sqlConn.Close();

        //        return ds;

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}

        public SqlDataReader Get_Vehicle_Assset_Models_By_Make_Type(int iVehicle_Make_Id, int iVehicle_Asset_Type_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iVehicle_Make_Id",iVehicle_Make_Id),
                new SqlParameter("@iVehicle_Asset_Type_Id",iVehicle_Asset_Type_Id),
        };

            return (SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
                "spGet_Vehicle_Asset_By_Make_And_Type", parameters));



        }
        public SqlDataReader Get_Vehicle_Assset_Models_Variant(int iVehicle_Model_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@iVehicle_Model_Id",iVehicle_Model_Id),
            };

            return (SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
                "spGet_Vehicle_Asset_Variant_By_Model", parameters));





        }



        public bool Save_ChangeCover_Vehicle_Asset(int iPolicy_Id, int iVehicle_Asset_Id, int iPolicy_Cover_Type_Id_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {
            int iPolicy_Transaction_Id = 0;
            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",iPolicy_Id),
                new SqlParameter("@iVehicle_Asset_Id",iVehicle_Asset_Id),
                new SqlParameter("@iAsset_Cover_Type_Id_New",iPolicy_Cover_Type_Id_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spUpd_Policy_ChangeCover_Vehicle_Asset", parameters);
            updated = true;

            return updated;

        }
        public bool Save_ChangeInsuranceValue_Vehicle_Asset(int iVehicle_Asset_Id, decimal mAsset_Insurance_Value_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {

            bool updated = false;
            SqlParameter[] parameters = new SqlParameter[]
             {
                new SqlParameter("@iVehicle_Asset_Id",iVehicle_Asset_Id),
                new SqlParameter("@mAsset_Insurance_Value_New",mAsset_Insurance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
             };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spUpd_Asset_Insurance_Value_Vehicle_Asset", parameters);
            updated = true;

            return updated;

        }
        public bool Save_ChangeFinanceValue_Vehicle_Asset(int iVehicle_Asset_Id, decimal mAsset_Finance_Value_New, string dtDateOfChange)
        {

            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iVehicle_Asset_Id",iVehicle_Asset_Id),
                new SqlParameter("@mAsset_Finance_Value_New",mAsset_Finance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spUpd_Asset_ChangeFianceValue_Vehicle_Asset", parameters);
            updated = true;

            return updated;

        }

        #region API_Related
        public C.vehicleDetailsResponse GetVehicle_Finance_Details(string vcAPI_Source_Identifier, string vcPolicy_Number, string vcVin_Number)
        {
            C.vehicleDetailsResponse res = new vehicleDetailsResponse();
            C.VehicleFinanceDeatils vd = new VehicleFinanceDeatils();

            SqlParameter[] arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@vcVin_Number", U.CryptorEngine.GenericEncrypt(vcVin_Number, true));
            arParams[1] = new SqlParameter("@vcPolicy_Number", U.CryptorEngine.GenericEncrypt(vcPolicy_Number, true));
            arParams[2] = new SqlParameter("@vcAPI_Source_Identifier", U.CryptorEngine.ValidationEncrypt(vcAPI_Source_Identifier, true));
            using (
     var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(),
     CommandType.StoredProcedure, "spGet_Asset_Finance_Details_Vehicle_API", arParams))
                while (dr.Read())
                {
                    vd.financer = dr["Financer"] != null ? dr["Financer"].ToString() : null;
                    vd.financeagrreementnumber = dr["Finance agrreement number"] != null ? dr["Finance agrreement number"].ToString() : null;

                    vd.assetTypeDescription = dr["Asset type description"] != null ? dr["Asset type description"].ToString() : null;
                    vd.assetSubTypeDescription = dr["Asset sub-type description"] != null ? dr["Asset sub-type description"].ToString() : null;
                    vd.make = dr["Make"] != null ? dr["Make"].ToString() : null;
                    vd.model = dr["Model"] != null ? dr["Model"].ToString() : null;
                    vd.modelVariant = dr["Model Variant"] != null ? dr["Model Variant"].ToString() : null;
                    vd.assetFinanceValue = dr["Asset Finance Value"] != null ? dr["Asset Finance Value"].ToString() : null;
                    vd.financeStartDate = dr["Finance Start Date"] != null ? dr["Finance Start Date"].ToString() : null;
                    vd.financeEndDate = dr["Finance End Date"] != null ? dr["Finance End Date"].ToString() : null;
                }
            res.vehicleFinanceDetails = vd;


            return res;

        }
        public C.Response Import_New_Vehicles(C.AssetTypes.API_NewAssets.addVehicleAssetsRequest newVehicles, int iPartnerId)
        {
            Financer_Provider fP = new Financer_Provider();
            C.Policy.Policy pol = new C.Policy.Policy();
            //C.AssetTypes.API_NewAssets.addVehicleAssetsRequest.ConsumerVehicles consumerDetails pH = new C.AssetTypes.API_NewAssets.addVehicleAssetsRequest.ConsumerDetails();
            string linkGuid = Guid.NewGuid().ToString();

            var dtConsumerCustomer =
               C.Common.ConvertToDataTable.ConvertTODataTable_Consumer_Customers(iPartnerId, newVehicles.vehicleAssets.consumerVehicles, newVehicles.trasactionId);

            var dtBusinessCustomer =
               C.Common.ConvertToDataTable.ConvertTODataTable_Business_Customers(iPartnerId, newVehicles.vehicleAssets.businessVehicles, newVehicles.trasactionId);

            var dtConsumerVehicles =
                C.Common.ConvertToDataTable.ConvertTODataTable_NewAssetByAPI_Vehicle_Consumer(iPartnerId, newVehicles.vehicleAssets.consumerVehicles, newVehicles.trasactionId);

            var dtBusinessVehicles =
                C.Common.ConvertToDataTable.ConvertTODataTable_NewAssetByAPI_Vehicle_Business(iPartnerId, newVehicles.vehicleAssets.businessVehicles, newVehicles.trasactionId);


            SqlParameter[] arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@dtConsumerCustomerByAPI", dtConsumerCustomer);
            arParams[1] = new SqlParameter("@dtConsumerVehicles", dtConsumerVehicles);
            arParams[2] = new SqlParameter("@dtBusinessCustomerByAPI", dtBusinessCustomer);
            arParams[3] = new SqlParameter("@dtBusinessVehicles", dtBusinessVehicles);

            using (
                    var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRAPI"].ToString(),
                    CommandType.StoredProcedure, "spIns_Import_BulkNewAsset_Vehicle_By_API", arParams))
                return null;
        }


        #endregion
    }
}








