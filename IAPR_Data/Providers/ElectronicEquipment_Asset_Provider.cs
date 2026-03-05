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
    public class ElectronicEquipment_Asset_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());
        public DataSet Check_ElectronicEquipment_Details_Exist(string vcFinance_Agrreement_Number, string vcSerial_Number)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("dbo.spGet_Check_ElectronicEquipment_Details_Exists", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@vcFinance_Agrreement_Number", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcFinance_Agrreement_Number, true);
            cmd.Parameters.Add("@vcSerial_Number", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcSerial_Number, true);

            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;

        }
        public void Save_New_ElectronicEquipment_Asset(Classes.AssetTypes.ElectronicEquipment_Asset ee)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",ee.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",ee.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",ee.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(ee.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@mAsset_Finance_Value",ee.mAsset_Finance_Value),
                new SqlParameter("@mAsset_Insurance_Value",ee.mAsset_Insurance_Value),
                new SqlParameter("@iElectronicEquipment_Asset_Type_Id",ee.iElectronicEquipment_Asset_Type_Id),
                new SqlParameter("@vcSerial_Number",U.CryptorEngine.GenericEncrypt(ee.vcSerial_Number,true)),
                new SqlParameter("@iElectronicEquipment_Make_Id",ee.iElectronicEquipment_Make_Id),
                new SqlParameter("@iElectronicEquipment_Model_Id",ee.iElectronicEquipment_Model_Id),
                new SqlParameter("@dtFinance_Start_Date",ee.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",ee.dtFinance_End_Date),
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_ElectronicEquipment_Asset", parameters);

        }
        public void Save_New_ElectronicEquipment_Asset_Without_Policy(Classes.AssetTypes.ElectronicEquipment_Asset ee, int iAsset_Policy_Alignment_Id)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",ee.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",ee.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",ee.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(ee.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@mAsset_Finance_Value",ee.mAsset_Finance_Value),
                new SqlParameter("@mAsset_Insurance_Value",ee.mAsset_Insurance_Value),
                new SqlParameter("@iElectronicEquipment_Asset_Type_Id",ee.iElectronicEquipment_Asset_Type_Id),
                new SqlParameter("@vcSerial_Number",U.CryptorEngine.GenericEncrypt(ee.vcSerial_Number,true)),
                new SqlParameter("@iElectronicEquipment_Make_Id",ee.iElectronicEquipment_Make_Id),
                new SqlParameter("@iElectronicEquipment_Model_Id",ee.iElectronicEquipment_Model_Id),
                new SqlParameter("@dtFinance_Start_Date",ee.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",ee.dtFinance_End_Date),
                new SqlParameter("@iAsset_Policy_Alignment_Id",iAsset_Policy_Alignment_Id),
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_ElectronicEquipment_Asset_Without_Policy", parameters);

        }
        public SqlDataReader Get_ElectronicEquipment_Assset_Models_By_Make_Type(int iElectronicEquipment_Make_Id, int iElectronicEquipment_Asset_Type_Id)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iElectronicEquipment_Make_Id",iElectronicEquipment_Make_Id),
                new SqlParameter("@iElectronicEquipment_Asset_Type_Id",iElectronicEquipment_Asset_Type_Id),
        };

            return (SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
                "spGet_ElectronicEquipment_Asset_By_Make_And_Type", parameters));





        }
        public DataSet Get_FormFields_Policy_Update_ChangeCover_ElectronicEquipment(int ipolicy_Id, int iElectronicEquipment_Asset_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();




            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFields_Policy_Update_ChangeCover_ElectronicEquipment", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPolicy_Id", SqlDbType.Int).Value = ipolicy_Id;
            cmd.Parameters.Add("@iElectronicEquipment_Asset_Id", SqlDbType.Int).Value = iElectronicEquipment_Asset_Id;
            cmd.Parameters.Add("@iAsset_Type_Id", SqlDbType.Int).Value = 2;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;



        }
        public bool Save_ChangeCover_ElectronicEquipment_Asset(int iPolicy_Id, int iVehicle_Asset_Id, int iPolicy_Cover_Type_Id_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {
            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",iPolicy_Id),
                new SqlParameter("@iElectronicEquipment_Asset_Id",iVehicle_Asset_Id),
                new SqlParameter("@iAsset_Cover_Type_Id_New",iPolicy_Cover_Type_Id_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Policy_ChangeCover_ElectronicEquipment_Asset", parameters);
            updated = true;

            return updated;

        }
        public bool Save_ChangeInsuranceValue_ElectronicEquipment_Asset(int iElectronicEquipment_Asset_Id, decimal mAsset_Insurance_Value_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {

            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iElectronicEquipment_Asset_Id",iElectronicEquipment_Asset_Id),
                new SqlParameter("@mAsset_Insurance_Value_New",mAsset_Insurance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Asset_Insurance_Value_ElectronicEquipment_Asset", parameters);
            updated = true;



            return updated;

        }
        public bool Save_ChangeFinanceValue_ElectronicEquipment_Asset(int iElectronicEquipment_Asset_Id, decimal mAsset_Finance_Value_New, string dtDateOfChange)
        {

            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iElectronicEquipment_Asset_Id",iElectronicEquipment_Asset_Id),
                new SqlParameter("@mAsset_Finance_Value_New",mAsset_Finance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
    "spUpd_Asset_ChangeFianceValue_ElectronicEquipment_Asset", parameters);
            updated = true;

            return updated;

        }

    }
}








