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
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using C = IAPR_Data.Classes;
using U = IAPR_Data.Utils;


namespace IAPR_Data.Providers
{
    public class PlantEquipment_Asset_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());
        public DataSet Check_PlantEquipment_Details_Exist(string vcFinance_Agrreement_Number, string vcSerial_Number, string vcRegistration_Number)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("dbo.spGet_Check_PlantEquipment_Details_Exists", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@vcFinance_Agrreement_Number", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcFinance_Agrreement_Number, true);
            cmd.Parameters.Add("@vcSerial_Number", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcSerial_Number, true);
            cmd.Parameters.Add("@vcRegistration_Number", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcRegistration_Number, true);
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;

        }
        public void Save_New_PlantEquipment_Asset(Classes.AssetTypes.PlantEquipment_Asset pe)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",pe.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",pe.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",pe.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(pe.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@mAsset_Finance_Value",pe.mAsset_Finance_Value),
                new SqlParameter("@mAsset_Insurance_Value",pe.mAsset_Insurance_Value),
                new SqlParameter("@iPlantEquipment_Asset_Type_Id",pe.iPlantEquipment_Asset_Type_Id),
                new SqlParameter("@vcRegistration_Number",U.CryptorEngine.GenericEncrypt(pe.vcRegistration_Number,true)),
                new SqlParameter("@vcSerial_Number",U.CryptorEngine.GenericEncrypt(pe.vcSerial_Number,true)),
                new SqlParameter("@dtFinance_Start_Date",pe.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",pe.dtFinance_End_Date),
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_PlantEquipment_Asset", parameters);

        }
        public void Save_New_PlantEquipment_Asset_Without_Policy(Classes.AssetTypes.PlantEquipment_Asset pe, int iAsset_Policy_Alignment_Id)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",pe.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",pe.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",pe.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(pe.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@mAsset_Finance_Value",pe.mAsset_Finance_Value),
                new SqlParameter("@mAsset_Insurance_Value",pe.mAsset_Insurance_Value),
                new SqlParameter("@iPlantEquipment_Asset_Type_Id",pe.iPlantEquipment_Asset_Type_Id),
                new SqlParameter("@vcRegistration_Number",U.CryptorEngine.GenericEncrypt(pe.vcRegistration_Number,true)),
                new SqlParameter("@vcSerial_Number",U.CryptorEngine.GenericEncrypt(pe.vcSerial_Number,true)),
                new SqlParameter("@dtFinance_Start_Date",pe.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",pe.dtFinance_End_Date),
                new SqlParameter("@iAsset_Policy_Alignment_Id",iAsset_Policy_Alignment_Id),
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_PlantEquipment_Asset_Without_Policy", parameters);

        }
        public DataSet Get_FormFields_Policy_Update_ChangeCover_PlantEquipment(int ipolicy_Id, int iPlantEquipment_Asset_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFields_Policy_Update_ChangeCover_PlantEquipment", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPolicy_Id", SqlDbType.Int).Value = ipolicy_Id;
            cmd.Parameters.Add("@iPlantEquipment_Asset_Id", SqlDbType.Int).Value = iPlantEquipment_Asset_Id;
            cmd.Parameters.Add("@iAsset_Type_Id", SqlDbType.Int).Value = 2;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;

        }
        public bool Save_ChangeCover_PlantEquipment_Asset(int iPolicy_Id, int iVehicle_Asset_Id, int iPolicy_Cover_Type_Id_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {
            bool updated = false;
            SqlParameter[] parameters = new SqlParameter[]
               {

                new SqlParameter("@iPolicy_Id",iPolicy_Id),
                new SqlParameter("@iPlantEquipment_Asset_Id",iVehicle_Asset_Id),
                new SqlParameter("@iAsset_Cover_Type_Id_New",iPolicy_Cover_Type_Id_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
               };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Policy_ChangeCover_PlantEquipment_Asset", parameters);
            updated = true;

            return updated;

        }
        public bool Save_ChangeInsuranceValue_PlantEquipment_Asset(int iPlantEquipment_Asset_Id, decimal mAsset_Insurance_Value_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {

            bool updated = false;
            SqlParameter[] parameters = new SqlParameter[]
             {
                new SqlParameter("@iPlantEquipment_Asset_Id",iPlantEquipment_Asset_Id),
                new SqlParameter("@mAsset_Insurance_Value_New",mAsset_Insurance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
             };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Asset_Insurance_Value_PlantEquipment_Asset", parameters);
            updated = true;

            return updated;

        }
        public bool Save_ChangeFinanceValue_PlantEquipment_Asset(int iPlantEquipment_Asset_Id, decimal mAsset_Finance_Value_New, string dtDateOfChange)
        {

            bool updated = false;
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@iPlantEquipment_Asset_Id",iPlantEquipment_Asset_Id),
                new SqlParameter("@mAsset_Finance_Value_New",mAsset_Finance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
              };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Asset_ChangeFianceValue_PlantEquipment_Asset", parameters);
            updated = true;

            return updated;
        }
    }
}
