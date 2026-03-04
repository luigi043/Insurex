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
using System.Net;
using System.Net.Mail;
using C = IAPR_Data.Classes;
using U = IAPR_Data.Utils;
namespace IAPR_Data.Providers
{
    public class Search_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());

        public DataSet Get_Search_Insurer_By_PolicyNumber(int iInsurance_Company_Id, string vcPolicy_Number)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_Search_Insurer_By_PolicyNumber", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iInsurance_Company_Id", SqlDbType.Int).Value = iInsurance_Company_Id;
            cmd.Parameters.Add("@vcPolicy_Number", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcPolicy_Number, true);
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Insurance Company Name", "Policy Number" });
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 1, 0);


            return ds;

        }
    }
}
