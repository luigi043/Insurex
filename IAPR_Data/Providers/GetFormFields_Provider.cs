using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using C = IAPR_Data.Classes;
namespace IAPR_Data.Providers
{
    public class GetFormFields_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());
        public DataSet GetFormFieldsGeneric()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFieldsGeneric", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 1);
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 13, 1);
            return ds;



        }
        public DataSet GetFormFieldsAssetFinancerByAssetType(int iAsset_Type_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

          
                SqlCommand cmd = new SqlCommand("dbo.spGet_AssetFinancer_By_Asset_Type", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@iAsset_Type_Id", SqlDbType.Int).Value = @iAsset_Type_Id;
                sqlConn.Open();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                sqlConn.Close();

                return ds;


        }
        public DataSet GetFormFieldsAssetsFinancedByFinancer(int iFinancer_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();




            SqlCommand cmd = new SqlCommand("dbo.spGet_Assets_Financed_By_Financer", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancer_Id", SqlDbType.Int).Value = iFinancer_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;



        }
        public DataSet GetFormFieldsVehicleAsset()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFieldsVehiclePolicy", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@vcCartGUID", SqlDbType.NVarChar).Value = CartRefID;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            //ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 2, new int[] { 2, 3, 4 });
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 1);
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 11, 1);
            return ds;


        }
        public DataSet GetFormFieldsPropertyAsset()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFieldsPropertyPolicy", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@vcCartGUID", SqlDbType.NVarChar).Value = CartRefID;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 1);
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 11, 1);
            return ds;


        }
        public DataSet GetFormFieldsWaterCraftAsset()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFieldsWatercraftPolicy", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@vcCartGUID", SqlDbType.NVarChar).Value = CartRefID;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 1);
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 11, 1);
            return ds;



        }
        public DataSet GetFormFieldAviationAsset()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFieldsAviationPolicy", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@vcCartGUID", SqlDbType.NVarChar).Value = CartRefID;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 1);
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 11, 1);
            return ds;


        }

        public DataSet GetFormFieldStockAsset()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFieldsStockPolicy", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@vcCartGUID", SqlDbType.NVarChar).Value = CartRefID;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 1);
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 11, 1);
            return ds;


        }
        public DataSet GetFormFieldAccountReceivableAsset()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFieldsAccountReceivablePolicy", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@vcCartGUID", SqlDbType.NVarChar).Value = CartRefID;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 1);
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 11, 1);
            return ds;


        }
        public DataSet GetFormFieldMachineryAsset()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFieldsMachineryPolicy", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@vcCartGUID", SqlDbType.NVarChar).Value = CartRefID;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 1);
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 11, 1);
            return ds;


        }
        public DataSet GetFormFieldPlantEquipmentAsset()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFieldsPlantEquipmentPolicy", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@vcCartGUID", SqlDbType.NVarChar).Value = CartRefID;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 1);
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 11, 1);
            return ds;


        }
        public DataSet GetFormFieldBusinessInterruptionAsset()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFieldsBusinessInterruptionPolicy", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@vcCartGUID", SqlDbType.NVarChar).Value = CartRefID;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 1);
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 11, 1);
            return ds;


        }
        public DataSet GetFormFieldKeymanInsuranceAsset()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFieldsKeymanInsurancePolicy", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@vcCartGUID", SqlDbType.NVarChar).Value = CartRefID;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 1);
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 11, 1);
            return ds;


        }
        public DataSet GetFormFieldElectronicEquipmentAsset()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFieldsElectronicEquipmentPolicy", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@vcCartGUID", SqlDbType.NVarChar).Value = CartRefID;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 1);
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 11, 1);
            return ds;


        }
        public DataSet GetFormFields_Policy_Update_NonPayment(string ipolicy_Payment_Frequency_Type_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFields_Policy_Update_NonPayment", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ipolicy_Payment_Frequency_Type_Id", SqlDbType.Int).Value = Convert.ToInt32(ipolicy_Payment_Frequency_Type_Id);
                sqlConn.Open();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                sqlConn.Close();

                return ds;

           
        }
        public DataSet GetFormFields_Policy_Update_Status(string ipolicy_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFields_Policy_Update_ChangeStatus", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPolicy_Id", SqlDbType.Int).Value = Convert.ToInt32(ipolicy_Id);
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }

        public DataSet Get_Cover_Types_By_Asset_Type(int iAsset_Type_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_Asset_Cover_Types_By_Asset_Type", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iAsset_Type_Id", SqlDbType.Int).Value = iAsset_Type_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }
        public DataSet Get_Cities_By_Province(int iProvince_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_CitiesBYProvince", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iProvince_Id", SqlDbType.Int).Value = iProvince_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }
        public DataSet Get_Surburbs_By_City(int iCity_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_CurburbByCity", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iCity_Id", SqlDbType.Int).Value = iCity_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }

    }
}
