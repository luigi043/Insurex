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
    public class BusinessInterruption_Asset_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());
        public void Save_New_BusinessInterruption_Asset(Classes.AssetTypes.BusinessInterruption_Asset bi)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",bi.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",bi.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",bi.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(bi.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@iBusinessInterruption_Asset_Type_Id",bi.iBusinessInterruption_Asset_Type_Id),
                new SqlParameter("@vcDescription",U.CryptorEngine.GenericEncrypt(bi.vcDescription,true)),
                new SqlParameter("@dtFinance_Start_Date",bi.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",bi.dtFinance_End_Date),

        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_BusinessInterruption_Asset", parameters);

        }
        public void Save_New_BusinessInterruption_Asset_Without_Policy(Classes.AssetTypes.BusinessInterruption_Asset bi, int iAsset_Policy_Alignment_Id)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",bi.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",bi.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",bi.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(bi.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@iBusinessInterruption_Asset_Type_Id",bi.iBusinessInterruption_Asset_Type_Id),
                new SqlParameter("@vcDescription",U.CryptorEngine.GenericEncrypt(bi.vcDescription,true)),
                new SqlParameter("@dtFinance_Start_Date",bi.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",bi.dtFinance_End_Date),
                new SqlParameter("@iAsset_Policy_Alignment_Id",iAsset_Policy_Alignment_Id),

        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_BusinessInterruption_Asset_Without_Policy", parameters);

        }
        public DataSet Get_FormFields_Policy_Update_ChangeCover_BusinessInterruption(int ipolicy_Id, int iBusinessInterruption_Asset_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFields_Policy_Update_ChangeCover_BusinessInterruption", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPolicy_Id", SqlDbType.Int).Value = ipolicy_Id;
            cmd.Parameters.Add("@iBusinessInterruption_Asset_Id", SqlDbType.Int).Value = iBusinessInterruption_Asset_Id;
            cmd.Parameters.Add("@iAsset_Type_Id", SqlDbType.Int).Value = 2;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }
        public bool Save_ChangeCover_BusinessInterruption_Asset(int iPolicy_Id, int iVehicle_Asset_Id, int iPolicy_Cover_Type_Id_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {
            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",iPolicy_Id),
                new SqlParameter("@iBusinessInterruption_Asset_Id",iVehicle_Asset_Id),
                new SqlParameter("@iAsset_Cover_Type_Id_New",iPolicy_Cover_Type_Id_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Policy_ChangeCover_BusinessInterruption_Asset", parameters);
            updated = true;

            return updated;

        }
        public bool Save_ChangeInsuranceValue_BusinessInterruption_Asset(int iBusinessInterruption_Asset_Id, decimal mAsset_Insurance_Value_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {

            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iBusinessInterruption_Asset_Id",iBusinessInterruption_Asset_Id),
                new SqlParameter("@mAsset_Insurance_Value_New",mAsset_Insurance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Asset_Insurance_Value_BusinessInterruption_Asset", parameters);
            updated = true;

            return updated;

        }
        public bool Save_ChangeFinanceValue_BusinessInterruption_Asset(int iBusinessInterruption_Asset_Id, decimal mAsset_Finance_Value_New, string dtDateOfChange)
        {

            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iBusinessInterruption_Asset_Id",iBusinessInterruption_Asset_Id),
                new SqlParameter("@mAsset_Finance_Value_New",mAsset_Finance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Asset_ChangeFianceValue_BusinessInterruption_Asset", parameters);
            updated = true;

            return updated;

        }

    }
}
