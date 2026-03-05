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
    public class Watercraft_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());
        public DataSet Check_Watercraft_Details_Exist(string vcFinance_Agrreement_Number, string vcName_Emblem, string vcRegistration_Number)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("dbo.spGet_Check_Watercraft_Details_Exists", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@vcFinance_Agrreement_Number", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcFinance_Agrreement_Number, true);
            cmd.Parameters.Add("@vcName_Emblem", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcName_Emblem, true);
            cmd.Parameters.Add("@vcRegistration_Number", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcRegistration_Number, true);
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;

        }
        public void Save_New_Watercraft_Asset(Classes.AssetTypes.Watercraft_Asset wc)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",wc.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",wc.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",wc.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(wc.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@mAsset_Finance_Value",wc.mAsset_Finance_Value),
                new SqlParameter("@mAsset_Insurance_Value",wc.mAsset_Insurance_Value),
                new SqlParameter("@iWatercraft_Asset_Type_Id",wc.iWatercraft_Asset_Type_Id),
                new SqlParameter("@vcName_Emblem",U.CryptorEngine.GenericEncrypt(wc.vcName_Emblem,true)),
                new SqlParameter("@vcRegistration_Number",U.CryptorEngine.GenericEncrypt(wc.vcRegistration_Number,true)),
                new SqlParameter("@vcClass",wc.vcClass),
                new SqlParameter("@dtFinance_Start_Date",wc.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",wc.dtFinance_End_Date),
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_Watercraft_Asset", parameters);

        }
        public void Save_New_Watercraft_Asset_Without_Policy(Classes.AssetTypes.Watercraft_Asset wc, int iAsset_Policy_Alignment_Id)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",wc.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",wc.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",wc.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt( wc.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@mAsset_Finance_Value",wc.mAsset_Finance_Value),
                new SqlParameter("@mAsset_Insurance_Value",wc.mAsset_Insurance_Value),
                new SqlParameter("@iWatercraft_Asset_Type_Id",wc.iWatercraft_Asset_Type_Id),
                new SqlParameter("@vcName_Emblem",U.CryptorEngine.GenericEncrypt(wc.vcName_Emblem,true)),
                new SqlParameter("@vcRegistration_Number",U.CryptorEngine.GenericEncrypt(wc.vcRegistration_Number,true)),
                new SqlParameter("@vcClass",wc.vcClass),
                new SqlParameter("@dtFinance_Start_Date",wc.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",wc.dtFinance_End_Date),
                 new SqlParameter("@iAsset_Policy_Alignment_Id",iAsset_Policy_Alignment_Id),
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_Watercraft_Asset_Without_Policy", parameters);

        }

        public SqlDataReader Get_Watercraft_Asset_Type_By_Class(int iWatercraft_Class_Type_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iWatercraft_Class_Type_Id",iWatercraft_Class_Type_Id),

        };

            return (SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
                "spGet_Watercraft_Asset_Type_By_Class", parameters));





        }
        public DataSet Get_FormFields_Policy_Update_ChangeCover_Watercraft(int ipolicy_Id, int iWatercraft_Asset_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFields_Policy_Update_ChangeCover_Watercraft", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPolicy_Id", SqlDbType.Int).Value = ipolicy_Id;
            cmd.Parameters.Add("@iWatercraft_Asset_Id", SqlDbType.Int).Value = iWatercraft_Asset_Id;
            cmd.Parameters.Add("@iAsset_Type_Id", SqlDbType.Int).Value = 2;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;



        }
        public bool Save_ChangeCover_Watercraft_Asset(int iPolicy_Id, int iVehicle_Asset_Id, int iPolicy_Cover_Type_Id_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {
            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",iPolicy_Id),
                new SqlParameter("@iWatercraft_Asset_Id",iVehicle_Asset_Id),
                new SqlParameter("@iAsset_Cover_Type_Id_New",iPolicy_Cover_Type_Id_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Policy_ChangeCover_Watercraft_Asset", parameters);
            updated = true;

            return updated;

        }
        public bool Save_ChangeInsuranceValue_Watercraft_Asset(int iWatercraft_Asset_Id, decimal mAsset_Insurance_Value_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {

            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iWatercraft_Asset_Id",iWatercraft_Asset_Id),
                new SqlParameter("@mAsset_Insurance_Value_New",mAsset_Insurance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Asset_Insurance_Value_Watercraft_Asset", parameters);
            updated = true;

            return updated;

        }
        public bool Save_ChangeFinanceValue_Watercraft_Asset(int iWatercraft_Asset_Id, decimal mAsset_Finance_Value_New, string dtDateOfChange)
        {

            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iWatercraft_Asset_Id",iWatercraft_Asset_Id),
                new SqlParameter("@mAsset_Finance_Value_New",mAsset_Finance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Asset_ChangeFianceValue_Watercraft_Asset", parameters);
            updated = true;

            return updated;

        }

    }


}








