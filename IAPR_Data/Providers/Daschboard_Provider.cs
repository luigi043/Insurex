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

using System.Net;
using System.Net.Mail;
using C = IAPR_Data.Classes;

namespace IAPR_Data.Providers
{
    public class Daschboard_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());

        //Insurer
        public DataSet Get_Insurer_Landing_Uninsured_Table(int iInsurance_Company_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Insurer_Landing_Uninsured_Table", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iInsurance_Company_Id", SqlDbType.Int).Value = iInsurance_Company_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "vcInsurance_Company_Name", "vcPolicy_Number" });
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 1, new string[] { "vcPolicy_Number" });
            return ds;


        }
        public DataSet Get_Insurer_Landing_Asset_Totals(int iInsurer_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Insurer_Landing_Asset_Totals", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iInsurer_Id", SqlDbType.Int).Value = iInsurer_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }
        public DataSet Get_Insurer_Landing_Dashboard_NonPayment_History_Chart(int iInsurer_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Insurer_Landing_Non_Paymnet_History", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iInsurer_Id", SqlDbType.Int).Value = iInsurer_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }

        //Financer

        public DataSet Get_Financer_Landing_DashboardTable(int iFinancer_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_Financer_Landing_Dashboard_Totals", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancer_Id", SqlDbType.Int).Value = iFinancer_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }
        public DataSet Get_Financer_Landing_DashboardCharts(int iFinancer_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Financer_Landing_Dashboard_Charts", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancer_Id", SqlDbType.Int).Value = iFinancer_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }
        public DataSet Get_Financer_Landing_Dashboard_NonPayment_Annual_Chart(int iFinancer_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();




            SqlCommand cmd = new SqlCommand("dbo.spGet_Financer_Landing_Dashboard_NonPayment-Annual_Chart", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancer_Id", SqlDbType.Int).Value = iFinancer_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }
        public DataSet Get_Financer_Landing_Dashboard_NonPayment_History_Chart(int iFinancer_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();




            SqlCommand cmd = new SqlCommand("dbo.spGet_Financer_Landing_Dashboard_NonPayment_Annual_Chart", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancer_Id", SqlDbType.Int).Value = iFinancer_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }

        public DataSet Get_Financer_Landing_Dashboard_NonPayment_History(int iFinancer_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Financer_Landing_Non_Paymnet_History", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancer_Id", SqlDbType.Int).Value = iFinancer_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Vin Number" });

            return ds;


        }
        public DataSet Get_Financer_Landing_Dashboard_ArrearVsUnconfirmed(int iFinancer_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Financer_Landing_ArrearVsUnconfirmed", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancer_Id", SqlDbType.Int).Value = iFinancer_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();


            return ds;


        }
        public DataSet Get_Financer_Landing_Dashboard_Communications_Current_Month(int iFinancer_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Financer_Landing_Communications_Current_Month", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancer_Id", SqlDbType.Int).Value = iFinancer_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 0);// C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Vin Number" });

            return ds;


        }
        public DataSet Get_Financer_Landing_Dashboard_Communications_History(int iFinancer_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Financer_Landing_Communications_History", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancer_Id", SqlDbType.Int).Value = iFinancer_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            
            return ds;


        }
        public DataSet Get_Financer_Landing_Dashboard_Uninsured_Statistics(int iFinancer_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Financer_Landing_Dashboard_Uninsured_Statistics", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancer_Id", SqlDbType.Int).Value = iFinancer_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            return ds;


        }

        public DataSet Get_Financer_Landing_Dashboard_Uninsured_By_Insurer(int iFinancer_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Financer_Landing_Dashboard_Uninsured_By_Insurer", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancer_Id", SqlDbType.Int).Value = iFinancer_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 0);// C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Vin Number" });

            return ds;


        }
        //Admin
        public DataSet Get_Admin_Landing_DashboardTable()
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Admin_Landing_Dashboard_Totals", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;

            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }
        public DataSet Get_Admin_Landing_DashboardCharts()
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Admin_Landing_Dashboard_Charts", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 4, new string[] { "Financer" });
            return ds;


        }
        public DataSet Get_Admin_Landing_Dashboard_NonPayment_Annual_Chart()
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Admin_Landing_Dashboard_NonPayment_Annual_Chart", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;

            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }
        public DataSet Get_Admin_Landing_Dashboard_NonPayment_History()
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Admin_Landing_Non_Paymnet_History", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;

            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Vin Number" });

            return ds;


        }
        public DataSet Get_Admin_Landing_Dashboard_ArrearVsUnconfirmed()
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Admin_Landing_ArrearVsUnconfirmed", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;

            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();


            return ds;


        }

        public DataSet Get_Admin_Landing_Dashboard_UninsuredByFinancer()
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Admin_Landing_UninsuredByFinancer", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;

            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 1);// C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Vin Number" });

            return ds;


        }

        public DataSet Get_Admin_Landing_Dashboard_Communications_Current_Month()
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Admin_Landing_Communications_Current_Month", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;

            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 0);// C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Vin Number" });

            return ds;


        }
        public DataSet Get_Admin_Landing_Dashboard_Communications_History()
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Admin_Landing_Communications_History", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }
        public DataSet Get_Admin_Landing_Dashboard_Uninsured_Statistics()
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Admin_Landing_Dashboard_Uninsured_Statistics", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            return ds;


        }
        public DataSet Get_Admin_Landing_Dashboard_Uninsured_By_Insurer()
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Admin_Landing_Dashboard_Uninsured_By_Insurer", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, 0);// C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Vin Number" });

            return ds;


        }

    }
}








