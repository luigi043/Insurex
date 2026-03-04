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
    public class Billing_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());

        public void Save_New_Partner_Charge(string vcPartner_Charge_Type_Description, string vcPartner_Charge_Type_Detailed_Description
            , decimal mPartner_Charge_Amount, bool bIs_Applicable_Monthly, int iPartner_Type_Applicable_To
            , int iPartner_Package_Applicable_To, string dtStart_Date, string dtEnd_Date)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@vcPartner_Charge_Type_Description",vcPartner_Charge_Type_Description),
                new SqlParameter("@vcPartner_Charge_Type_Detailed_Description",vcPartner_Charge_Type_Detailed_Description),
                new SqlParameter("@mPartner_Charge_Amount",mPartner_Charge_Amount),
                new SqlParameter("@bIs_Applicable_Monthly",bIs_Applicable_Monthly),
                new SqlParameter("@iPartner_Type_Applicable_To",iPartner_Type_Applicable_To),
                new SqlParameter("@iPartner_Package_Applicable_To",iPartner_Package_Applicable_To),
                new SqlParameter("@dtStart_Date",dtStart_Date),
                new SqlParameter("@dtEnd_Date",dtEnd_Date)
            };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_Partner_Charge_Type", parameters);
        }
        public DataSet GetPartnerChargeTypes()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_PartnerChargeTypes", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            return ds;
        }

        public DataSet GetPartnerChargeDetails(int iPartner_Charge_Type_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_PartnerChargeTypeDetails", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPartner_Charge_Type_Id", SqlDbType.Int).Value = iPartner_Charge_Type_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            //if (ds.Tables[0].Rows[0]["PolicyType"].ToString() == "Individual")
            //{
            //    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new int[] { 11 });



            //}
            //else
            //{
            //    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new int[] { 9 });
            //}
            //ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 1, new int[] { 2, 3, 4, 5, 6, 7 });
            //if (ds.Tables[2].Rows.Count > 0)
            //{
            //    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 2, new int[] { 2, 3, 4 });
            //}
            return ds;



        }

        public void Update_Partner_Charge(int iPartner_Charge_Type_Id, decimal mPartner_Charge_Amount, string dtStart_Date, string dtEnd_Date)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iPartner_Charge_Type_Id",iPartner_Charge_Type_Id),
                new SqlParameter("@mPartner_Charge_Amount",mPartner_Charge_Amount),
                new SqlParameter("@dtStart_Date",dtStart_Date),
                new SqlParameter("@dtEnd_Date",dtEnd_Date)
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spUpd_PartnerChargeTypeActiveSession", parameters);
        }

        public DataSet GetInvoiceTotalsForPartnerForPeriod(int iPartner_Id, int iPartner_Type_Id, int iInvoicing_Month, int iInvoicing_Year)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_Partner_Invoice_Totals_By_Period", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPartner_Id", SqlDbType.Int).Value = iPartner_Id;
            cmd.Parameters.Add("@iPartner_Type_Id", SqlDbType.Int).Value = iPartner_Type_Id;
            cmd.Parameters.Add("@iInvoicing_Month", SqlDbType.Int).Value = iInvoicing_Month;
            cmd.Parameters.Add("@iInvoicing_Year", SqlDbType.Int).Value = iInvoicing_Year;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            //if (ds.Tables[0].Rows[0]["PolicyType"].ToString() == "Individual")
            //{
            //    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new int[] { 11 });



            //}
            //else
            //{
            //    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new int[] { 9 });
            //}
            //ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 1, new int[] { 2, 3, 4, 5, 6, 7 });
            //if (ds.Tables[2].Rows.Count > 0)
            //{
            //    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 2, new int[] { 2, 3, 4 });
            //}
            return ds;



        }

    }
}
