using IAPR_Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

using C = IAPR_Data.Classes;
namespace IAPR_Data.Providers
{
    public class Insurer_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());
        public int Save_Confirm_Policy_For_Alignment(int iAsset_Policy_Alignment_Id, decimal @mAsset_Insurance_Value)
        {
            int newPolicy_Id = 0;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iAsset_Policy_Alignment_Id", iAsset_Policy_Alignment_Id),
                new SqlParameter("@mAsset_Insurance_Value", mAsset_Insurance_Value),


        };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spUpd_Policy_For_Alignment_Confirmation", parameters);

            while (dr.Read())
            {
                newPolicy_Id = Convert.ToInt32(dr["New_Policy_Id"].ToString());
            }



            return newPolicy_Id;
        }

        public int Reject_Policy_For_Alignment(int iAsset_Policy_Alignment_Id, string vcRejectionReason)
        {
            int newPolicy_Id = 0;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iAsset_Policy_Alignment_Id", iAsset_Policy_Alignment_Id),
                new SqlParameter("@vcRejectionReason", vcRejectionReason),

        };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spUpd_Reject_Policy_For_Alignment", parameters);

            while (dr.Read())
            {
                newPolicy_Id = Convert.ToInt32(dr["New_Policy_Id"].ToString());
            }
            return newPolicy_Id;
        }

        public DataSet Get_Policies_Awaiting_Confirmation(int iInsurance_Company_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("dbo.spGet_Insurer_Policies_Awaiting_Confirmation", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iInsurance_Company_Id", SqlDbType.Int).Value = iInsurance_Company_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 1);

            return ds;

        }
    }
}








