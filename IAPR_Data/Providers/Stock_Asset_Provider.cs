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
    public class Stock_Asset_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());
        public void Save_New_Stock_Asset(Classes.AssetTypes.Stock_Asset st)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",st.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",st.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",st.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(st.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@mAsset_Finance_Value",st.mAsset_Finance_Value),
                new SqlParameter("@mAsset_Insurance_Value",st.mAsset_Insurance_Value),
                new SqlParameter("@iStock_Asset_Type_Id",st.iStock_Asset_Type_Id),
                new SqlParameter("@vcStock_Description",U.CryptorEngine.GenericEncrypt(st.vcStock_Description,true)),
                new SqlParameter("@vcStock_Value",st.vcStock_Value),
                new SqlParameter("@dtFinance_Start_Date",st.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",st.dtFinance_End_Date),
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_Stock_Asset", parameters);

        }
        public void Save_New_Stock_Asset_Without_Policy(Classes.AssetTypes.Stock_Asset st, int iAsset_Policy_Alignment_Id)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",st.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",st.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",st.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(st.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@mAsset_Finance_Value",st.mAsset_Finance_Value),
                new SqlParameter("@mAsset_Insurance_Value",st.mAsset_Insurance_Value),
                new SqlParameter("@iStock_Asset_Type_Id",st.iStock_Asset_Type_Id),
                new SqlParameter("@vcStock_Description",U.CryptorEngine.GenericEncrypt(st.vcStock_Description,true)),
                new SqlParameter("@vcStock_Value",st.vcStock_Value),
                new SqlParameter("@dtFinance_Start_Date",st.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",st.dtFinance_End_Date),
                 new SqlParameter("@iAsset_Policy_Alignment_Id",iAsset_Policy_Alignment_Id),
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_Stock_Asset_Without_Policy", parameters);

        }
        public DataSet Get_FormFields_Policy_Update_ChangeCover_Stock(int ipolicy_Id, int iStock_Asset_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFields_Policy_Update_ChangeCover_Stock", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPolicy_Id", SqlDbType.Int).Value = ipolicy_Id;
            cmd.Parameters.Add("@iStock_Asset_Id", SqlDbType.Int).Value = iStock_Asset_Id;
            cmd.Parameters.Add("@iAsset_Type_Id", SqlDbType.Int).Value = 2;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;



        }
        public bool Save_ChangeCover_Stock_Asset(int iPolicy_Id, int iVehicle_Asset_Id, int iPolicy_Cover_Type_Id_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {
            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",iPolicy_Id),
                new SqlParameter("@iStock_Asset_Id",iVehicle_Asset_Id),
                new SqlParameter("@iAsset_Cover_Type_Id_New",iPolicy_Cover_Type_Id_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Policy_ChangeCover_Stock_Asset", parameters);
            updated = true;

            return updated;

        }
        public bool Save_ChangeInsuranceValue_Stock_Asset(int iStock_Asset_Id, decimal mAsset_Insurance_Value_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {

            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iStock_Asset_Id",iStock_Asset_Id),
                new SqlParameter("@mAsset_Insurance_Value_New",mAsset_Insurance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Asset_Insurance_Value_Stock_Asset", parameters);
            updated = true;

            return updated;

        }
        public bool Save_ChangeFinanceValue_Stock_Asset(int iStock_Asset_Id, decimal mAsset_Finance_Value_New, string dtDateOfChange)
        {

            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iStock_Asset_Id",iStock_Asset_Id),
                new SqlParameter("@mAsset_Finance_Value_New",mAsset_Finance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Asset_ChangeFianceValue_Stock_Asset", parameters);
            updated = true;

            return updated;

        }
    }
}








