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
using U = IAPR_Data.Utils;

namespace IAPR_Data.Providers
{
    public class Customer_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());
        public bool Check_Policy_To_Asset_Alignment_Exists(int @iAsset_Id, int iAsset_Type_Id, string vcLinkKey, int iPolicy_Holder_Id)
        {
            bool Policy_exists = false;


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iAsset_Id",iAsset_Id),
                new SqlParameter("@iAsset_Type_Id",iAsset_Type_Id),
                new SqlParameter("@vcLinkKey", vcLinkKey),
                new SqlParameter("@iPolicy_Holder_Id",iPolicy_Holder_Id)

      ,
         };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spGet_Check_Policy_To_Asset_Alignment_Exists", parameters);

            while (dr.Read())
            {
                Policy_exists = Convert.ToBoolean(dr["Exists"].ToString());
            }




            return Policy_exists;
        }
        public bool Check_Policy_To_Asset_Alignment_Confirmation_Exists(int @iAsset_Id, int iAsset_Type_Id, string vcLinkKey, int iPolicy_Holder_Id)
        {
            bool Policy_exists = false;


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iAsset_Id",iAsset_Id),
                new SqlParameter("@iAsset_Type_Id",iAsset_Type_Id),
                new SqlParameter("@vcLinkKey",vcLinkKey),
                new SqlParameter("@iPolicy_Holder_Id",iPolicy_Holder_Id)

      ,
         };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spGet_Check_Policy_To_Asset_Alignment_Confirmation_Exists", parameters);

            while (dr.Read())
            {
                Policy_exists = Convert.ToBoolean(dr["Exists"].ToString());
            }



            return Policy_exists;
        }
        public int Get_Policy_To_Asset_Alignment_Id(int @iAsset_Id, int iAsset_Type_Id, string vcLinkKey, int iPolicy_Holder_Id)
        {
            int iPolicy_To_Asset_Alignment_Id = 0;


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iAsset_Id",iAsset_Id),
                new SqlParameter("@iAsset_Type_Id",iAsset_Type_Id),
                new SqlParameter("@vcLinkKey",vcLinkKey),
                new SqlParameter("@iPolicy_Holder_Id",iPolicy_Holder_Id)

      ,
         };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spGet_Policy_To_Asset_Alignment_Id", parameters);

            while (dr.Read())
            {
                iPolicy_To_Asset_Alignment_Id = Convert.ToInt32(dr["iAsset_Policy_Alignment_Id"].ToString());
            }



            return iPolicy_To_Asset_Alignment_Id;
        }
        public int Get_Policy_To_Asset_Alignment_Id_For_Confirmation(int @iAsset_Id, int iAsset_Type_Id, string vcLinkKey, int iPolicy_Holder_Id)
        {
            int iPolicy_To_Asset_Alignment_Id = 0;


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iAsset_Id",iAsset_Id),
                new SqlParameter("@iAsset_Type_Id",iAsset_Type_Id),
                new SqlParameter("@vcLinkKey",vcLinkKey),
                new SqlParameter("@iPolicy_Holder_Id",iPolicy_Holder_Id)

      ,
         };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spGet_Policy_To_Asset_Alignment_Id_Confirmation", parameters);

            while (dr.Read())
            {
                iPolicy_To_Asset_Alignment_Id = Convert.ToInt32(dr["iAsset_Policy_Alignment_Id"].ToString());
            }



            return iPolicy_To_Asset_Alignment_Id;
        }
        public DataSet Get_Customer_Deatils_By_Policy(int iPolicy_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_Customer_Details_By_Policy", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPolicy_Id", SqlDbType.Int).Value = iPolicy_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();
            if (ds.Tables[0].Rows[0]["PolicyType"].ToString() == "Individual")
            {
                ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new int[] { 11 });



            }
            else
            {
                ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new int[] { 9 });
            }
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 1, new int[] { 2, 3, 4, 5, 6, 7 });
            if (ds.Tables[2].Rows.Count > 0)
            {
                ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 2, new int[] { 2, 3, 4 });
            }
            return ds;



        }
        public DataSet Get_Customer_Deatils_For_Alignment(int iAsset_Policy_Alignment_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_Customer_Details_For_Alignment", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iAsset_Policy_Alignment_Id", SqlDbType.Int).Value = iAsset_Policy_Alignment_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            if (ds.Tables[0].Rows[0]["iPolicy_Holder_Type_Id"].ToString() == "1")
            {
                ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 1, new string[] { "vcFirst_Names", "vcSurname", "vcIdentification_Number", "vcContact_Number", "vcAlternative_Contact_Number", "vcEmail_Address" });
            }
            else
            {
                ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 1, new string[] { "vcBusiness_Name", "vcBusiness_Registration_Number", "vcBusiness_Contact_Fullname", "vcBusiness_Contact_Number", "vcBusiness_Contact_Alternative_Number", "vcBusiness_Email_Address" });
            }
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 2, new string[] { "vcBuilding_Unit", "vcAddress_Line_1", "vcAddress_Line_2", "vcSuburb", "vcCity", "vcPostal_Code" });

            if (ds.Tables[0].Rows[0]["iPolicy_Postal_Address_Id"].ToString() != "")
            {
                ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 3, new string[] { "vcPOBox_Bag", "vcPost_Office_Name", "vcPostalCode", });
            }

            return ds;



        }

        public DataSet Get_Asset_Details_For_Alignment(int iAsset_Policy_Alignment_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            SqlCommand cmd = new SqlCommand("dbo.spGet_Asset_Details_For_Alignment", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iAsset_Policy_Alignment_Id", SqlDbType.Int).Value = iAsset_Policy_Alignment_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            switch (ds.Tables[1].Rows[0]["iAsset_Type_Id"].ToString())
            {
                case "1":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Vin Number" });
                    break;
                case "2":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "EFR Number", "Sectional Title Number", "Sectional Title Name" });
                    break;
                case "3":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Name/Emblem" });
                    break;
                case "4":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Tail number" });
                    break;
                case "5":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Stock description" });
                    break;
                case "6":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Description" });
                    break;
                case "7":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Serial number" });
                    break;
                case "8":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Serial number" });
                    break;
                case "9":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Description" });
                    break;
                case "10":
                    //ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Asset description", "Asset sub-type description", "Make", "Model", "Model Variant" });
                    break;
                case "11":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Serial number" });
                    break;

            }
            return ds;



        }
        public DataSet Get_Asset_Cover_Types_For_Alignment(int iAsset_Policy_Alignment_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Asset_Cover_Types_For_Alignment", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iAsset_Policy_Alignment_Id", SqlDbType.Int).Value = iAsset_Policy_Alignment_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;



        }
        public DataSet Get_Asset_Details_For_Alignment_Confirmation(int iAsset_Policy_Alignment_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlCommand cmd = new SqlCommand("dbo.spGet_Asset_Details_For_Alignment_Confirmation", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iAsset_Policy_Alignment_Id", SqlDbType.Int).Value = iAsset_Policy_Alignment_Id;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            switch (ds.Tables[1].Rows[0]["iAsset_Type_Id"].ToString())
            {
                case "1":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number" });
                    break;
                case "2":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "EFR Number", "Sectional Title Number", "Sectional Title Name" });
                    break;
                case "3":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Name/Emblem" });
                    break;
                case "4":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Tail number" });
                    break;
                case "5":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Stock description" });
                    break;
                case "6":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Description" });
                    break;
                case "7":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Serial number" });
                    break;
                case "8":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Serial number" });
                    break;
                case "9":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Description" });
                    break;
                case "10":
                    //ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Asset description", "Asset sub-type description", "Make", "Model", "Model Variant" });
                    break;
                case "11":
                    ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 0, new string[] { "Financer", "Finance/Account number", "Serial number" });
                    break;

            }
            ds = C.Common.ConvertToDataTable.DecrypDBtField(ds, 1, new string[] { "Insurer", "Policy number", "Serial number" });
            return ds;



        }
        public int Save_New_Policy_For_Alignment(Classes.Policy.Policy p, int iAsset_Policy_Alignment_Id, int iAsset_Cover_Type_Id)//, decimal @mAsset_Insurance_Value
        {
            int newPolicy_Id = 0;

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iInsurance_Company_Id",p.iInsurance_Company_Id),
                new SqlParameter("@vcPolicy_Number",U.CryptorEngine.GenericEncrypt(p.vcPolicy_Number,true)),
                new SqlParameter("@iPolicy_Type_Id",p.iPolicy_Type_Id),
                new SqlParameter("@iPolicy_Payment_Frequency_Type_Id", p.iPolicy_Payment_Frequency_Type_Id),
                new SqlParameter("@iAsset_Policy_Alignment_Id", iAsset_Policy_Alignment_Id),
               // new SqlParameter("@mAsset_Insurance_Value", mAsset_Insurance_Value),
                new SqlParameter("@iAsset_Cover_Type_Id", iAsset_Cover_Type_Id),
            };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spIns_Save_New_Policy_For_Alignment", parameters);

            while (dr.Read())
            {
                newPolicy_Id = Convert.ToInt32(dr["New_Policy_Id"].ToString());
            }

            return newPolicy_Id;
        }

        public int Save_Existing_Policy_For_Alignment(int iPolicy_Id, int iAsset_Policy_Alignment_Id, int iAsset_Cover_Type_Id) //, decimal @mAsset_Insurance_Value
        {
            int newPolicy_Id = 0;


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@iPolicy_Id",iPolicy_Id),
                    new SqlParameter("@iAsset_Policy_Alignment_Id", iAsset_Policy_Alignment_Id),
                   // new SqlParameter("@mAsset_Insurance_Value", mAsset_Insurance_Value),
                    new SqlParameter("@iAsset_Cover_Type_Id", iAsset_Cover_Type_Id),

            };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spIns_Save_Existing_Policy_For_Alignment", parameters);

            while (dr.Read())
            {
                newPolicy_Id = Convert.ToInt32(dr["New_Policy_Id"].ToString());
            }



            return newPolicy_Id;
        }

    }
}









