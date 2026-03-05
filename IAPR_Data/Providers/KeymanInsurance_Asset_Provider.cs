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
    public class KeymanInsurance_Asset_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());
        public void Save_New_KeymanInsurance_Asset(Classes.AssetTypes.KeymanInsurance_Asset ki)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",ki.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",ki.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",ki.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(ki.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@iKeymanInsurance_Asset_Type_Id",ki.iKeymanInsurance_Asset_Type_Id),
                new SqlParameter("@vcKeyman_Name",U.CryptorEngine.GenericEncrypt(ki.vcKeyman_Name,true)),
                new SqlParameter("@vcKeyman_Surname",U.CryptorEngine.GenericEncrypt(ki.vcKeyman_Surname,true)),
                new SqlParameter("@iKeyman_Identification_type_Id",ki.iKeyman_Identification_type_Id),
                new SqlParameter("@vcKeyman_Identity_Number",U.CryptorEngine.GenericEncrypt(ki.vcKeyman_Identity_Number,true)),
                new SqlParameter("@dtFinance_Start_Date",ki.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",ki.dtFinance_End_Date),
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_KeymanInsurance_Asset", parameters);

        }
        public void Save_New_KeymanInsurance_Asset_Without_Policy(Classes.AssetTypes.KeymanInsurance_Asset ki, int iAsset_Policy_Alignment_Id)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",ki.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",ki.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",ki.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",ki.vcFinance_Agrreement_Number),
                new SqlParameter("@iKeymanInsurance_Asset_Type_Id",ki.iKeymanInsurance_Asset_Type_Id),
                new SqlParameter("@vcKeyman_Name",ki.vcKeyman_Name),
                new SqlParameter("@vcKeyman_Surname",ki.vcKeyman_Surname),
                new SqlParameter("@iKeyman_Identification_type_Id",ki.iKeyman_Identification_type_Id),
                new SqlParameter("@vcKeyman_Identity_Number",ki.vcKeyman_Identity_Number),
                new SqlParameter("@dtFinance_Start_Date",ki.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",ki.dtFinance_End_Date),
                 new SqlParameter("@iAsset_Policy_Alignment_Id",iAsset_Policy_Alignment_Id),
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_KeymanInsurance_Asset_Without_Policy", parameters);

        }
        public DataSet Get_FormFields_Policy_Update_ChangeCover_KeymanInsurance(int ipolicy_Id, int iKeymanInsurance_Asset_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFields_Policy_Update_ChangeCover_KeymanInsurance", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPolicy_Id", SqlDbType.Int).Value = ipolicy_Id;
            cmd.Parameters.Add("@iKeymanInsurance_Asset_Id", SqlDbType.Int).Value = iKeymanInsurance_Asset_Id;
            cmd.Parameters.Add("@iAsset_Type_Id", SqlDbType.Int).Value = 2;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;

        }
        public bool Save_ChangeCover_KeymanInsurance_Asset(int iPolicy_Id, int iVehicle_Asset_Id, int iPolicy_Cover_Type_Id_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {
            bool updated = false;
            SqlParameter[] parameters = new SqlParameter[]
                 {

                new SqlParameter("@iPolicy_Id",iPolicy_Id),
                new SqlParameter("@iKeymanInsurance_Asset_Id",iVehicle_Asset_Id),
                new SqlParameter("@iAsset_Cover_Type_Id_New",iPolicy_Cover_Type_Id_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
                 };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Policy_ChangeCover_KeymanInsurance_Asset", parameters);
            updated = true;

            return updated;

        }
        public bool Save_ChangeInsuranceValue_KeymanInsurance_Asset(int iKeymanInsurance_Asset_Id, decimal mAsset_Insurance_Value_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {

            bool updated = false;
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@iKeymanInsurance_Asset_Id",iKeymanInsurance_Asset_Id),
                new SqlParameter("@mAsset_Insurance_Value_New",mAsset_Insurance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
              };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Asset_Insurance_Value_KeymanInsurance_Asset", parameters);
            updated = true;

            return updated;

        }
        public bool Save_ChangeFinanceValue_KeymanInsurance_Asset(int iKeymanInsurance_Asset_Id, decimal mAsset_Finance_Value_New, string dtDateOfChange)
        {

            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iKeymanInsurance_Asset_Id",iKeymanInsurance_Asset_Id),
                new SqlParameter("@mAsset_Finance_Value_New",mAsset_Finance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
 "spUpd_Asset_ChangeFianceValue_KeymanInsurance_Asset", parameters);
            updated = true;

            return updated;

        }

    }
}








