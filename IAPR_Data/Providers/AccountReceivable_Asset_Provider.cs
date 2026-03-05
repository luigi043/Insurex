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
    public class AccountReceivable_Asset_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());
        public void Save_New_AccountReceivable_Asset(Classes.AssetTypes.AccountReceivable_Asset ar)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",ar.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",ar.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",ar.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(ar.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@iAccountReceivable_Asset_Type_Id",ar.iAccountReceivable_Asset_Type_Id),
                new SqlParameter("@vcAccountReceivable_Description",U.CryptorEngine.GenericEncrypt(ar.vcAccountReceivable_Description,true)),
                new SqlParameter("@dtFinance_Start_Date",ar.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",ar.dtFinance_End_Date),
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_AccountReceivable_Asset", parameters);

        }
        public void Save_New_AccountReceivable_Asset_Without_Policy(Classes.AssetTypes.AccountReceivable_Asset ar, int iAsset_Policy_Alignment_Id)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",ar.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",ar.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",ar.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(ar.vcFinance_Agrreement_Number, true)),
                new SqlParameter("@iAccountReceivable_Asset_Type_Id",ar.iAccountReceivable_Asset_Type_Id),
                new SqlParameter("@vcAccountReceivable_Description",U.CryptorEngine.GenericEncrypt(ar.vcAccountReceivable_Description,true)),
                new SqlParameter("@dtFinance_Start_Date",ar.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",ar.dtFinance_End_Date),
                new SqlParameter("@iAsset_Policy_Alignment_Id",iAsset_Policy_Alignment_Id),
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_AccountReceivable_Asset_Without_Policy", parameters);

        }
        public DataSet Get_FormFields_Policy_Update_ChangeCover_AccountReceivable(int ipolicy_Id, int iAccountReceivable_Asset_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            


                SqlCommand cmd = new SqlCommand("dbo.spGet_FormFields_Policy_Update_ChangeCover_AccountReceivable", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@iPolicy_Id", SqlDbType.Int).Value = ipolicy_Id;
                cmd.Parameters.Add("@iAccountReceivable_Asset_Id", SqlDbType.Int).Value = iAccountReceivable_Asset_Id;
                cmd.Parameters.Add("@iAsset_Type_Id", SqlDbType.Int).Value = 2;
                sqlConn.Open();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                sqlConn.Close();

                return ds;

            
        }
        public bool Save_ChangeInsuranceValue_AccountReceivable_Asset(int iAccountReceivable_Asset_Id, decimal mAsset_Insurance_Value_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {

            bool updated = false;
           
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@iAccountReceivable_Asset_Id",iAccountReceivable_Asset_Id),
                new SqlParameter("@mAsset_Insurance_Value_New",mAsset_Insurance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
                };
                SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
     "spUpd_Asset_Insurance_Value_AccountReceivable_Asset", parameters);
                updated = true;
            
            return updated;

        }

        public bool Save_ChangeCover_AccountsReceivable_Asset(int iPolicy_Id, int iVehicle_Asset_Id, int iPolicy_Cover_Type_Id_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {
            bool updated = false;
           
                SqlParameter[] parameters = new SqlParameter[]
                {

                new SqlParameter("@iPolicy_Id",iPolicy_Id),
                new SqlParameter("@iAccountsReceivable_Asset_Id",iVehicle_Asset_Id),
                new SqlParameter("@iAsset_Cover_Type_Id_New",iPolicy_Cover_Type_Id_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
                };
                SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
     "spUpd_Policy_ChangeCover_AccountsReceivable_Asset", parameters);
                updated = true;
            
            return updated;

        }

    }
}








