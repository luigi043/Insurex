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
    public class Partner_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());

        public int Get_Check_Financer_Partner_By_API_Identifier(string vcAPI_Source_Identifier)
        {
            int iPartner_Id = 0;
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
       {

                new SqlParameter("@vcAPI_Source_Identifier",U.CryptorEngine.ValidationEncrypt(vcAPI_Source_Identifier,true)),
       };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
             "spGet_Check_Financer_Partner_By_API_Identifier", parameters);
            while (dr.Read())
            {
                iPartner_Id = Convert.ToInt32(dr["iFinancer_Id"].ToString());
            }


            return iPartner_Id;
        }
        public int Get_Check_Insurer_Partner_By_API_Identifier(string vcAPI_Source_Identifier)
        {
            int iPartner_Id = 0;
            SqlDataAdapter da = new SqlDataAdapter();

            SqlParameter[] parameters = new SqlParameter[]
          {

                new SqlParameter("@vcAPI_Source_Identifier",U.CryptorEngine.ValidationEncrypt(vcAPI_Source_Identifier,true)),
          };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
             "spGet_Check_Insurer_Partner_By_API_Identifier", parameters);
            while (dr.Read())
            {
                iPartner_Id = Convert.ToInt32(dr["iInsurance_Company_Id"].ToString());
            }

            return iPartner_Id;
        }

        public DataSet Get_Partner_Deatils(int iPartner_Type_Id, int iPartner_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_Partner_Details", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPartner_Id", SqlDbType.Int).Value = iPartner_Id;
            cmd.Parameters.Add("@iPartner_Type_Id", SqlDbType.Int).Value = iPartner_Type_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            //if (ds.Tables[0].Rows[0]["iPartner_Type_Id"].ToString() == C.Common.Common.Partner_types.Asset_financer.ToString())
            //{
            //    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 12, 13, 14, 15, 18 });

            //}
            //if (ds.Tables[0].Rows[0]["iPartner_Type_Id"].ToString() == C.Common.Common.Partner_types.Asset_financer.ToString())
            //{
            //    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 12, 13, 14, 15, 18 });

            //}
            //ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new int[] { 2, 3, 6, 7, 8 });
            return ds;
        }
    }
}








