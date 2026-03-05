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
    public class Property_Asset_Provider
    {
        public DataSet Check_Property_Details_Exist(string vcFinance_Agrreement_Number, string vcStand_ERF_Number, string vcSectionalTitleNumber, string vcSectionalTitleName)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Check_Property_Details_Exists", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@vcFinance_Agrreement_Number", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcFinance_Agrreement_Number, true);
            cmd.Parameters.Add("@vcStand_ERF_Number", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcStand_ERF_Number, true);
            cmd.Parameters.Add("@vcSectionalTitleNumber", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcSectionalTitleNumber, true);
            cmd.Parameters.Add("@vcSectionalTitleName", SqlDbType.VarChar).Value = U.CryptorEngine.GenericEncrypt(vcSectionalTitleName, true);
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;


        }

        public DataSet Get_FormFields_Policy_Update_ChangeCover_Property(int ipolicy_Id, int iProperty_Asset_Id)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_FormFields_Policy_Update_ChangeCover_Property", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPolicy_Id", SqlDbType.Int).Value = ipolicy_Id;
            cmd.Parameters.Add("@iProperty_Asset_Id", SqlDbType.Int).Value = iProperty_Asset_Id;
            cmd.Parameters.Add("@iAsset_Type_Id", SqlDbType.Int).Value = 2;
            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();

            return ds;



        }

        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());

        public void Save_New_Property_Asset(Classes.AssetTypes.Property_Asset pa)
        {


            //var dtPackageEntries = GetPackageTable(cartItems.PackageEntries);
            //var dsLotterEntries = GetLotteryEntries(cartItems.LotteryEntries);
            //var dtLotteryMainInfo = dsLotterEntries.Tables[0];
            //var dtLotteryCombinations = dsLotterEntries.Tables[1];

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",pa.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",pa.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",pa.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(pa.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@mAsset_Finance_Value",pa.mAsset_Finance_Value),
                new SqlParameter("@mAsset_Insurance_Value",pa.mAsset_Insurance_Value),
                new SqlParameter("@iProperty_Asset_Type_Id",pa.iProperty_Asset_Type_Id),
                new SqlParameter("@vcStand_ERF_Number",U.CryptorEngine.GenericEncrypt(pa.vcStand_ERF_Number,true)),
                new SqlParameter("@vcSectionalTitleNumbe",U.CryptorEngine.GenericEncrypt(pa.vcSectionalTitleNumber,true)),
                new SqlParameter("@vcSectionalTitleName",U.CryptorEngine.GenericEncrypt(pa.vcSectionalTitleName,true)),
                new SqlParameter("@iAsset_Usage_Type_Id",pa.iAsset_Usage_Type_Id),
                new SqlParameter("@dtFinance_Start_Date",pa.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",pa.dtFinance_End_Date),
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_Property_Asset", parameters);

        }

        public void Save_New_Property_Asset_Without_Policy(Classes.AssetTypes.Property_Asset pa, int iAsset_Policy_Alignment_Id)
        {


            //var dtPackageEntries = GetPackageTable(cartItems.PackageEntries);
            //var dsLotterEntries = GetLotteryEntries(cartItems.LotteryEntries);
            //var dtLotteryMainInfo = dsLotterEntries.Tables[0];
            //var dtLotteryCombinations = dsLotterEntries.Tables[1];

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",pa.iPolicy_Id),
                new SqlParameter("@iAsset_Cover_Type_Id",pa.iAsset_Cover_Type_Id),
                new SqlParameter("@iFinancer_Id",pa.iFinancer_Id),
                new SqlParameter("@vcFinance_Agrreement_Number",U.CryptorEngine.GenericEncrypt(pa.vcFinance_Agrreement_Number,true)),
                new SqlParameter("@mAsset_Finance_Value",pa.mAsset_Finance_Value),
                new SqlParameter("@mAsset_Insurance_Value",pa.mAsset_Insurance_Value),
                new SqlParameter("@iProperty_Asset_Type_Id",pa.iProperty_Asset_Type_Id),
                new SqlParameter("@vcStand_ERF_Number",U.CryptorEngine.GenericEncrypt(pa.vcStand_ERF_Number,true)),
                new SqlParameter("@vcSectionalTitleNumber",U.CryptorEngine.GenericEncrypt(pa.vcSectionalTitleNumber,true)),
                new SqlParameter("@vcSectionalTitleName",U.CryptorEngine.GenericEncrypt(pa.vcSectionalTitleName,true)),
                new SqlParameter("@iAsset_Usage_Type_Id",pa.iAsset_Usage_Type_Id),
                new SqlParameter("@dtFinance_Start_Date",pa.dtFinance_Start_Date),
                new SqlParameter("@dtFinance_End_Date",pa.dtFinance_End_Date),
                new SqlParameter("@iAsset_Policy_Alignment_Id",iAsset_Policy_Alignment_Id),
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_New_Property_Asset_Without_Policy", parameters);

        }





        public bool Save_ChangeCover_Property_Asset(int iPolicy_Id, int iVehicle_Asset_Id, int iPolicy_Cover_Type_Id_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {
            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iPolicy_Id",iPolicy_Id),
                new SqlParameter("@iProperty_Asset_Id",iVehicle_Asset_Id),
                new SqlParameter("@iAsset_Cover_Type_Id_New",iPolicy_Cover_Type_Id_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
                "spUpd_Policy_ChangeCover_Property_Asset", parameters);
            updated = true;

            return updated;

        }
        public bool Save_ChangeInsuranceValue_Property_Asset(int iProperty_Asset_Id, decimal mAsset_Insurance_Value_New, string dtDateOfChange)//int ipolicy_Payment_Frequency_Type_Id, int iPolicy_Transaction_Type_Id,
        {

            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iProperty_Asset_Id",iProperty_Asset_Id),
                new SqlParameter("@mAsset_Insurance_Value_New",mAsset_Insurance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
        "spUpd_Asset_Insurance_Value_Property_Asset", parameters);
            updated = true;

            return updated;

        }
        public bool Save_ChangeFinanceValue_Property_Asset(int iProperty_Asset_Id, decimal mAsset_Finance_Value_New, string dtDateOfChange)
        {

            bool updated = false;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iProperty_Asset_Id",iProperty_Asset_Id),
                new SqlParameter("@mAsset_Finance_Value_New",mAsset_Finance_Value_New),
                new SqlParameter("@dtDateOfChange",dtDateOfChange),
            };
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
    "spUpd_Asset_ChangeFianceValue_Property_Asset", parameters);
            updated = true;

            return updated;

        }

        #region API_Related
        public C.propertyDetailsResponse GetProperty_Finance_Details(string vcAPI_Source_Identifier, string vcPolicy_Number, string standNumber_ERFPortion, string sectionalTitleNumber, string sectionalTitleName)
        {
            C.propertyDetailsResponse res = new propertyDetailsResponse();
            C.PropertyFinanceDeatils pd = new PropertyFinanceDeatils();
            SqlParameter[] arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@vcPolicy_Number", U.CryptorEngine.GenericEncrypt(vcPolicy_Number, true));
            arParams[1] = new SqlParameter("@vcAPI_Source_Identifier", U.CryptorEngine.ValidationEncrypt(vcAPI_Source_Identifier, true));
            arParams[2] = new SqlParameter("@standNumber_ERFPortion", U.CryptorEngine.GenericEncrypt(standNumber_ERFPortion, true));
            arParams[3] = new SqlParameter("@sectionalTitleNumber", U.CryptorEngine.GenericEncrypt(sectionalTitleNumber, true));
            arParams[4] = new SqlParameter("@sectionalTitleName", U.CryptorEngine.GenericEncrypt(sectionalTitleName, true));
            using (
     var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(),
     CommandType.StoredProcedure, "spGet_Asset_Finance_Details_Property_API", arParams))
                while (dr.Read())
                {
                    pd.financer = dr["Financer"] != null ? dr["Financer"].ToString() : null;
                    pd.financeagrreementnumber = dr["Finance agrreement number"] != null ? dr["Finance agrreement number"].ToString() : null;

                    pd.assetTypeDescription = dr["Asset type description"] != null ? dr["Asset type description"].ToString() : null;
                    pd.assetSubTypeDescription = dr["Asset sub-type description"] != null ? dr["Asset sub-type description"].ToString() : null;
                    //pd.make = dr["Make"] != null ? dr["Make"].ToString() : null;
                    //pd.model = dr["Model"] != null ? dr["Model"].ToString() : null;
                    //pd.modelVariant = dr["Model Variant"] != null ? dr["Model Variant"].ToString() : null;
                    //pd.financeStartDate = dr["Finance Start Date"] != null ? dr["Finance Start Date"].ToString() : null;
                    //pd.financeEndDate = dr["Finance End Date"] != null ? dr["Finance End Date"].ToString() : null;
                    //pd.assetFinanceValue = dr["Asset Finance Value"] != null ? dr["Asset Finance Value"].ToString() : null;
                }
            res.propertyFinanceDetails = pd;


            return res;

        }

        #endregion
    }
}








