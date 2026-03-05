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
using U = IAPR_Data.Utils;
namespace IAPR_Data.Providers
{
    public class Report_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());

        #region Finance
        public DataSet Get_Policy_NonPayment_By_Financier_By_Period(int iFinancier_Id, int iPeriod_Id, int iYear_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Policy_NonPayment_By_Financier_By_Period", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancier_Id", SqlDbType.Int).Value = iFinancier_Id;
            cmd.Parameters.Add("@iPeriod_Id", SqlDbType.Int).Value = iPeriod_Id;
            cmd.Parameters.Add("@iYear_Id", SqlDbType.Int).Value = iYear_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance Agrreement Number", "Insurer Company", "Insurance Policy Number" });
            return ds;


        }
        public DataSet Get_Asset_ChangeOFCover_By_Financier_By_Period(int iFinancier_Id, int iPeriod_Id, int iYear_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();




            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Asset_ChangeOfCover_By_Financier_By_Period", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancier_Id", SqlDbType.Int).Value = iFinancier_Id;
            cmd.Parameters.Add("@iPeriod_Id", SqlDbType.Int).Value = iPeriod_Id;
            cmd.Parameters.Add("@iYear_Id", SqlDbType.Int).Value = iYear_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer Name", "Finance Agrreement Number", "Insurer Company", "Insurance Policy Numbe" });
            return ds;


        }
        public DataSet Get_Asset_ChangeOFCover_History(int iAsset_Id, int iAsset_Type_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();




            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Asset_ChangeOfCover_History", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iAsset_Id", SqlDbType.Int).Value = iAsset_Id;
            cmd.Parameters.Add("@iAsset_Type_Id", SqlDbType.Int).Value = iAsset_Type_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }

        public DataSet Get_Asset_ChangeOFInsuranceValue_By_Financier_By_Period(int iFinancier_Id, int iPeriod_Id, int iYear_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Asset_ChangeOfInsuranceValue_By_Financier_By_Period", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancier_Id", SqlDbType.Int).Value = iFinancier_Id;
            cmd.Parameters.Add("@iPeriod_Id", SqlDbType.Int).Value = iPeriod_Id;
            cmd.Parameters.Add("@iYear_Id", SqlDbType.Int).Value = iYear_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer Name", "Finance Agrreement Number", "Insurer Company", "Insurance Policy Number" });
            return ds;


        }
        public DataSet Get_Asset_ChangeOfInsuranceValue_History(int iAsset_Id, int iAsset_Type_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Asset_ChangeOfInsuranceValue_History", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iAsset_Id", SqlDbType.Int).Value = iAsset_Id;
            cmd.Parameters.Add("@iAsset_Type_Id", SqlDbType.Int).Value = iAsset_Type_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }

        public DataSet Get_Asset_Removal_By_Financier_By_Period(int iFinancier_Id, int iPeriod_Id, int iYear_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Asset_Removal_From_Policy", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancier_Id", SqlDbType.Int).Value = iFinancier_Id;
            cmd.Parameters.Add("@iPeriod_Id", SqlDbType.Int).Value = iPeriod_Id;
            cmd.Parameters.Add("@iYear_Id", SqlDbType.Int).Value = iYear_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer Name", "Finance Agrreement Number", "Insurer Company", "Insurance Policy Number" });
            return ds;


        }

        public List<C.AssetTypes.Uninsured_Assets> Get_Uninsured_Assets_Financer(int iFinancier_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Uninsured_Assets_By_Financier", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancier_Id", SqlDbType.Int).Value = iFinancier_Id;

            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            string dateSinceUninsured = string.Empty;
            List<C.AssetTypes.Uninsured_Assets> uninsured_AssetsL = new List<C.AssetTypes.Uninsured_Assets>();
            uninsured_AssetsL = (from DataRow dr in ds.Tables[0].Rows
                                 select new C.AssetTypes.Uninsured_Assets()
                                 {
                                     iPolicy_Id = dr["iPolicy_Id"] != DBNull.Value ? Convert.ToInt32(dr["iPolicy_Id"].ToString()) : 0,
                                     iAsset_Id = dr["iAsset_Id"] != DBNull.Value ? Convert.ToInt32(dr["iAsset_Id"].ToString()) : 0,

                                     iAsset_Type_Id = dr["iAsset_Type_Id"] != DBNull.Value ? Convert.ToInt32(dr["iAsset_Type_Id"].ToString()) : 0,
                                     iFinancer_Id = dr["iFinancer_Id"] != DBNull.Value ? Convert.ToInt32(dr["iFinancer_Id"].ToString()) : 0,
                                     vcFinancer_Name = dr["Financer"] != DBNull.Value ? U.CryptorEngine.GenericDecrypt(dr["Financer"].ToString(), true) : "",
                                     vcFinance_Agrreement_Number = dr["Finance Agreement Number"] != DBNull.Value ? U.CryptorEngine.GenericDecrypt(dr["Finance Agreement Number"].ToString(), true) : "",
                                     vcInsurance_Company_Name = dr["Insurer Company"] != DBNull.Value && dr["Insurer Company"].ToString() != "None" ? U.CryptorEngine.GenericDecrypt(dr["Insurer Company"].ToString(), true) : "None",
                                     vcPolicy_Number = dr["Insurance Policy Number"] != DBNull.Value && dr["Insurance Policy Number"].ToString() != "Unconfirmed" ? U.CryptorEngine.GenericDecrypt(dr["Insurance Policy Number"].ToString(), true) : "Unconfirmed",
                                     vcAsset_Type_Description = dr["Asset Type"] != DBNull.Value ? dr["Asset Type"].ToString() : "",
                                     vcAsset_SubType_Description = dr["Asset Description"] != DBNull.Value ? dr["Asset Description"].ToString() : "",
                                     mAsset_Finance_Value = dr["Finance Value"] != DBNull.Value ? string.Format("{0:c}", dr["Finance Value"]) : string.Format("{0:C}", 0),
                                     dtDate_since_Unisured = dr["Date since Unisured"] != DBNull.Value ? string.Format("{0:dd/MMM/yyyy}", dr["Date since Unisured"].ToString()) : ""
                                 }).ToList();


            return uninsured_AssetsL;


        }
        public DataSet Get_Uninsured_Assets_Financer_Report_Download(int iFinancier_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Uninsured_Assets_By_Financier_Report_Download", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancier_Id", SqlDbType.Int).Value = iFinancier_Id;

            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance Agreement Number", "Insurer Company", "Insurance Policy Number" });
            return ds;

        }

        public DataSet Get_ReistatedCover_Assets_Financer(int iFinancier_Id, int iPeriod_Id, int iAffected_Year)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_ReinstatedCover_Assets_By_Financier", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancier_Id", SqlDbType.Int).Value = iFinancier_Id;
            cmd.Parameters.Add("@iPeriod_Id", SqlDbType.Int).Value = iPeriod_Id;
            cmd.Parameters.Add("@iYear_Id", SqlDbType.Int).Value = iAffected_Year;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "vcFinance_Agrreement_Number", "vcInsurance_Company_Name", "vcPolicy_Number" });
            return ds;


        }
        public DataSet Get_ReistatedCover_Assets_Financer_Report_Download(int iFinancier_Id, int iPeriod_Id, int iAffected_Year)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_ReinstatedCover_Assets_By_Financier_Download", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancier_Id", SqlDbType.Int).Value = iFinancier_Id;
            cmd.Parameters.Add("@iPeriod_Id", SqlDbType.Int).Value = iPeriod_Id;
            cmd.Parameters.Add("@iYear_Id", SqlDbType.Int).Value = iAffected_Year;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance Agreement Number", "Insurance Company Name", "Policy Number" });
            return ds;


        }
        public List<C.AssetTypes.AllFinancerAssets> Get_All_Assets_Financer(int iFinancier_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_All_Assets_By_Financier", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancier_Id", SqlDbType.Int).Value = iFinancier_Id;

            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            string dateSinceUninsured = string.Empty;
            List<C.AssetTypes.AllFinancerAssets> AllFinancerAssetsL = new List<C.AssetTypes.AllFinancerAssets>();
            AllFinancerAssetsL = (from DataRow dr in ds.Tables[0].Rows
                                  select new C.AssetTypes.AllFinancerAssets()
                                  {
                                      iPolicy_Id = dr["iPolicy_Id"] != DBNull.Value ? Convert.ToInt32(dr["iPolicy_Id"].ToString()) : 0,
                                      iAsset_Id = dr["iAsset_Id"] != DBNull.Value ? Convert.ToInt32(dr["iAsset_Id"].ToString()) : 0,

                                      iAsset_Type_Id = dr["iAsset_Type_Id"] != DBNull.Value ? Convert.ToInt32(dr["iAsset_Type_Id"].ToString()) : 0,
                                      iFinancer_Id = dr["iFinancer_Id"] != DBNull.Value ? Convert.ToInt32(dr["iFinancer_Id"].ToString()) : 0,
                                      vcFinancer_Name = dr["Financer"] != DBNull.Value ? U.CryptorEngine.GenericDecrypt(dr["Financer"].ToString(), true) : "",
                                      vcFinance_Agrreement_Number = dr["Finance Agrreement Number"] != DBNull.Value ? U.CryptorEngine.GenericDecrypt(dr["Finance Agrreement Number"].ToString(), true) : "",
                                      vcInsurance_Company_Name = dr["Insurer Company"] != DBNull.Value && dr["Insurer Company"].ToString() != "None" ? U.CryptorEngine.GenericDecrypt(dr["Insurer Company"].ToString(), true) : "Unconfirmed",
                                      vcPolicy_Number = dr["Insurance Policy Number"] != DBNull.Value && dr["Insurance Policy Number"].ToString() != "Unconfirmed" ? U.CryptorEngine.GenericDecrypt(dr["Insurance Policy Number"].ToString(), true) : "Unconfirmed",
                                      vcAsset_Type_Description = dr["Asset Type"] != DBNull.Value ? dr["Asset Type"].ToString() : "",
                                      vcAsset_SubType_Description = dr["Asset Description"] != DBNull.Value ? dr["Asset Description"].ToString() : "",
                                      mAsset_Finance_Value = dr["Finance Value"] != DBNull.Value ? string.Format("{0:c}", dr["Finance Value"]) : string.Format("{0:C}", 0),
                                  }).ToList();


            return AllFinancerAssetsL;


        }
        public DataSet Get_All_Assets_Financer_Report_Download(int iFinancier_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_All_Assets_By_Financier_Download", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancier_Id", SqlDbType.Int).Value = iFinancier_Id;

            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance Agreement Number", "Insurance Company Name", "Policy Number" });
            return ds;


        }
        public DataSet Get_Asset_Comminications_Financer(int iFinancer_Id, int iPeriod_Id, int iYear_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Asset_Comminications_Financer", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iFinancer_Id", SqlDbType.Int).Value = iFinancer_Id;
            cmd.Parameters.Add("@iAffectedMonth", SqlDbType.Int).Value = iPeriod_Id;
            cmd.Parameters.Add("@iAffectedYear", SqlDbType.Int).Value = iYear_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Partner", "Finance Number", "Insurer", "Policy Number" });
            return ds;


        }

        public DataSet Get_Asset_Communications(int iAsset_Id, int iAsset_Type_Id, string dtComms_Date)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Asset_Communications", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iAsset_Id", SqlDbType.Int).Value = iAsset_Id;
            cmd.Parameters.Add("@iAsset_Type_Id", SqlDbType.Int).Value =iAsset_Type_Id;
            cmd.Parameters.Add("@dtComms_Date", SqlDbType.Date).Value = dtComms_Date;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            return ds;


        }

        #endregion

        #region  Insurer

        public DataSet Get_Policy_NonPayment_By_Insurer_By_Period(int iInsurer_Id, int iPeriod_Id, int iYear_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Policy_NonPayment_By_Insurer_By_Period", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iInsurer_Id", SqlDbType.Int).Value = iInsurer_Id;
            cmd.Parameters.Add("@iPeriod_Id", SqlDbType.Int).Value = iPeriod_Id;
            cmd.Parameters.Add("@iYear_Id", SqlDbType.Int).Value = iYear_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance Agrreement Number", "Insurer Company", "Insurance Policy Number" });
            return ds;


        }
        public DataSet Get_Asset_ChangeOFInsuranceValue_By_Insurer_By_Period(int iInsurer_Id, int iPeriod_Id, int iYear_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();




            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Asset_ChangeOfInsuranceValue_By_Insurer_By_Period", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iInsurer_Id", SqlDbType.Int).Value = iInsurer_Id;
            cmd.Parameters.Add("@iPeriod_Id", SqlDbType.Int).Value = iPeriod_Id;
            cmd.Parameters.Add("@iYear_Id", SqlDbType.Int).Value = iYear_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer Name", "Finance Agrreement Number", "Insurer Company", "Insurance Policy Number" });
            return ds;


        }
        public DataSet Get_Asset_ChangeOFCover_By_Insurer_By_Period(int iInsurer_Id, int iPeriod_Id, int iYear_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Asset_ChangeOfCover_By_Insurer_By_Period", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iInsurer_Id", SqlDbType.Int).Value = iInsurer_Id;
            cmd.Parameters.Add("@iPeriod_Id", SqlDbType.Int).Value = iPeriod_Id;
            cmd.Parameters.Add("@iYear_Id", SqlDbType.Int).Value = iYear_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer Name", "Finance Agrreement Number", "Insurer Company", "Insurance Policy Numbe" });
            return ds;


        }
        public DataSet Get_Asset_Removal_By_Insurer_By_Period(int iInsurer_Id, int iPeriod_Id, int iYear_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Asset_Removal_From_Policy_By_Insurer", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iInsurer_Id", SqlDbType.Int).Value = iInsurer_Id;
            cmd.Parameters.Add("@iPeriod_Id", SqlDbType.Int).Value = iPeriod_Id;
            cmd.Parameters.Add("@iYear_Id", SqlDbType.Int).Value = iYear_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer Name", "Finance Agrreement Number", "Insurer Company", "Insurance Policy Number" });
            return ds;


        }
        public List<C.AssetTypes.Uninsured_Assets> Get_Uninsured_Assets_Insurer(int iInsurer_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();




            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Uninsured_Assets_By_Insurer", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iInsurer_Id", SqlDbType.Int).Value = iInsurer_Id;

            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            string dateSinceUninsured = string.Empty;
            List<C.AssetTypes.Uninsured_Assets> uninsured_AssetsL = new List<C.AssetTypes.Uninsured_Assets>();
            uninsured_AssetsL = (from DataRow dr in ds.Tables[0].Rows
                                 select new C.AssetTypes.Uninsured_Assets()
                                 {
                                     iPolicy_Id = dr["iPolicy_Id"] != DBNull.Value ? Convert.ToInt32(dr["iPolicy_Id"].ToString()) : 0,
                                     iAsset_Id = dr["iAsset_Id"] != DBNull.Value ? Convert.ToInt32(dr["iAsset_Id"].ToString()) : 0,

                                     iAsset_Type_Id = dr["iAsset_Type_Id"] != DBNull.Value ? Convert.ToInt32(dr["iAsset_Type_Id"].ToString()) : 0,
                                     iFinancer_Id = dr["iFinancer_Id"] != DBNull.Value ? Convert.ToInt32(dr["iFinancer_Id"].ToString()) : 0,
                                     vcFinancer_Name = dr["Financer"] != DBNull.Value ? U.CryptorEngine.GenericDecrypt(dr["Financer"].ToString(), true) : "",
                                     vcFinance_Agrreement_Number = dr["Finance Agrreement Number"] != DBNull.Value ? U.CryptorEngine.GenericDecrypt(dr["Finance Agrreement Number"].ToString(), true) : "",
                                     vcInsurance_Company_Name = dr["Insurer Company"] != DBNull.Value && dr["Insurer Company"].ToString() != "None" ? U.CryptorEngine.GenericDecrypt(dr["Insurer Company"].ToString(), true) : "None",
                                     vcPolicy_Number = dr["Insurance Policy Number"] != DBNull.Value && dr["Insurance Policy Number"].ToString() != "Unconfirmed" ? U.CryptorEngine.GenericDecrypt(dr["Insurance Policy Number"].ToString(), true) : "Unconfirmed",
                                     vcAsset_Type_Description = dr["Asset Type"] != DBNull.Value ? dr["Asset Type"].ToString() : "",
                                     vcAsset_SubType_Description = dr["Asset Description"] != DBNull.Value ? dr["Asset Description"].ToString() : "",
                                     mAsset_Finance_Value = dr["Finance Value"] != DBNull.Value ? string.Format("{0:c}", dr["Finance Value"]) : string.Format("{0:C}", 0),
                                     dtDate_since_Unisured = dr["Date since Unisured"] != DBNull.Value ? string.Format("{0:dd/MMM/yyyy}", dr["Date since Unisured"].ToString()) : ""
                                 }).ToList();


            return uninsured_AssetsL;


        }
        public DataSet Get_Asset_Comminications_Insurer(int iInsurer_Id, int iPeriod_Id, int iYear_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Report_Asset_Comminications_Insurer", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iInsurer_Id", SqlDbType.Int).Value = iInsurer_Id;
            cmd.Parameters.Add("@iAffectedMonth", SqlDbType.Int).Value = iPeriod_Id;
            cmd.Parameters.Add("@iAffectedYear", SqlDbType.Int).Value = iYear_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Policy Number" });
            return ds;


        }
        #endregion
    }
}








