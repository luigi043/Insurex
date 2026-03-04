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
    public class Admin_Provider
    {
        public Dictionary<string, string> Save_New_Financer(Classes.Partners.Financer fa, Classes.Common.CurrentUser u, int iUser_Id) //int iUser_Division_Id, List<Classes.Common.Common.Divisions> divisions
        {
            string pw = Security_Provider.GeneratePassword(10);
            Dictionary<string, string> Values = new Dictionary<string, string>();

            SqlDataAdapter da = new SqlDataAdapter();
            //var dtFinancedAssets = Convert_Financed_Assets_ToDatatable(financed_Assets);
            //var dtDivisions = Convert_Divisions_ToDatatable(divisions);
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@vcFinancer_Name",U.CryptorEngine.GenericEncrypt(fa.vcFinancer_Name,true)),
                new SqlParameter("@vcBusiness_registration_Number",U.CryptorEngine.GenericEncrypt(fa.vcBusiness_registration_Number,true)),
                new SqlParameter("@vcVat_Registration_Number",U.CryptorEngine.GenericEncrypt(fa.vcVat_Registration_Number,true)),
                new SqlParameter("@vcBuilding_Unit",U.CryptorEngine.GenericEncrypt(fa.vcBuilding_Unit,true)),
                new SqlParameter("@vcAddress_Line_1",U.CryptorEngine.GenericEncrypt(fa.vcAddress_Line_1,true)),
                new SqlParameter("@vcAddress_Line_2",U.CryptorEngine.GenericEncrypt(fa.vcAddress_Line_2,true)),
                new SqlParameter("@vcSuburb",U.CryptorEngine.GenericEncrypt(fa.vcSuburb,true)),
                new SqlParameter("@vcCity",U.CryptorEngine.GenericEncrypt(fa.vcCity,true)),
                new SqlParameter("@vcPostal_Code",U.CryptorEngine.GenericEncrypt(fa.vcPostal_Code,true)),
                new SqlParameter("@iProvince_Id",fa.iProvince_Id),
                new SqlParameter("@vcPOBox_Bag",U.CryptorEngine.GenericEncrypt(fa.vcPOBox_Bag,true)),
                new SqlParameter("@vcPost_Office_Name",U.CryptorEngine.GenericEncrypt(fa.vcPost_Office_Name,true)),
                new SqlParameter("@vcPost_Office_Postal_Code",U.CryptorEngine.GenericEncrypt(fa.vcPost_Office_Postal_Code,true)),
                new SqlParameter("@vcCompanyContact_Number",U.CryptorEngine.GenericEncrypt(fa.vcContact_Number,true)),
                new SqlParameter("@bPostalAddresSameAsPhysical",fa.bPostalAddresSameAsPhysical),

                new SqlParameter("@vcName",U.CryptorEngine.GenericEncrypt(u.vcName,true)),
                new SqlParameter("@vcSurname",U.CryptorEngine.GenericEncrypt(u.vcSurname,true)),
                new SqlParameter("@vcPosition_Title",U.CryptorEngine.GenericEncrypt(u.vcPosition_Title,true)),
                new SqlParameter("@vcUsername",U.CryptorEngine.ValidationEncrypt(u.vcUsername.ToLower(),true)),
                new SqlParameter("@vcContactNumber",U.CryptorEngine.GenericEncrypt(u.vcContactNumber,true)),
                new SqlParameter("@vcPassword", U.CryptorEngine.ValidationEncrypt(pw, true)),
                new SqlParameter("@bUserReceiveNotifications",u.bUserReceiveNotifications),
                new SqlParameter("@vcAPI_Source_Identifier",U.CryptorEngine.ValidationEncrypt(Guid.NewGuid().ToString(),true)),

                new SqlParameter("@iPackage_Id", fa.iPackage_Id),
                //new SqlParameter("@dtDivisions", dtDivisions,true), 
                new SqlParameter("@iUser_Id",iUser_Id),
                //new SqlParameter("@iUser_Division_Id",iUser_Division_Id)
            };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_Financer", parameters);
            while (dr.Read())
            {
                Values.Add("iPartnerId", dr["iPartnerId"].ToString());
            }
            Values.Add("PW", pw);
            return Values;
        }
        public Dictionary<string, string> Save_New_Insurer(Classes.Partners.Insurer fa, Classes.Common.CurrentUser u, int iUser_Id)
        {
            string pw = Security_Provider.GeneratePassword(10);
            Dictionary<string, string> Values = new Dictionary<string, string>();

            SqlDataAdapter da = new SqlDataAdapter();

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@vcInsurance_Company_Name",U.CryptorEngine.GenericEncrypt(fa.vcInsurance_Company_Name,true)),
                new SqlParameter("@vcBusiness_registration_Number",U.CryptorEngine.GenericEncrypt(fa.vcBusiness_registration_Number,true)),
                new SqlParameter("@vcVat_Registration_Number",U.CryptorEngine.GenericEncrypt(fa.vcVat_Registration_Number,true)),
                new SqlParameter("@vcBuilding_Unit",U.CryptorEngine.GenericEncrypt(fa.vcBuilding_Unit,true)),
                new SqlParameter("@vcAddress_Line_1",U.CryptorEngine.GenericEncrypt(fa.vcAddress_Line_1,true)),
                new SqlParameter("@vcAddress_Line_2",U.CryptorEngine.GenericEncrypt(fa.vcAddress_Line_2,true)),
                new SqlParameter("@vcSuburb",U.CryptorEngine.GenericEncrypt(fa.vcSuburb,true)),
                new SqlParameter("@vcCity",U.CryptorEngine.GenericEncrypt(fa.vcCity,true)),
                new SqlParameter("@vcPostal_Code",U.CryptorEngine.GenericEncrypt(fa.vcPostal_Code,true)),
                new SqlParameter("@iProvince_Id",fa.iProvince_Id),
                new SqlParameter("@vcPOBox_Bag",U.CryptorEngine.GenericEncrypt(fa.vcPOBox_Bag,true)),
                new SqlParameter("@vcPost_Office_Name",U.CryptorEngine.GenericEncrypt(fa.vcPost_Office_Name,true)),
                new SqlParameter("@vcPost_Office_Postal_Code",U.CryptorEngine.GenericEncrypt(fa.vcPost_Office_Postal_Code,true)),
                new SqlParameter("@vcCompanyContact_Number",U.CryptorEngine.GenericEncrypt(fa.vcContact_Number,true)),
                new SqlParameter("@bPostalAddresSameAsPhysical",fa.bPostalAddresSameAsPhysical),

                new SqlParameter("@vcName",U.CryptorEngine.GenericEncrypt(u.vcName,true)),
                new SqlParameter("@vcSurname",U.CryptorEngine.GenericEncrypt(u.vcSurname,true)),
                new SqlParameter("@vcPosition_Title",U.CryptorEngine.GenericEncrypt(u.vcPosition_Title,true)),
                new SqlParameter("@vcUsername",U.CryptorEngine.ValidationEncrypt(u.vcUsername.ToLower(),true)),
                new SqlParameter("@vcContactNumber",U.CryptorEngine.GenericEncrypt(u.vcContactNumber,true)),
                new SqlParameter("@vcPassword", U.CryptorEngine.ValidationEncrypt(pw, true)),
                new SqlParameter("@bUserReceiveNotifications",u.bUserReceiveNotifications),
                new SqlParameter("@vcAPI_Source_Identifier",U.CryptorEngine.ValidationEncrypt(Guid.NewGuid().ToString(),true)),
                new SqlParameter("@iUser_Id",iUser_Id),

            };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_Insurer", parameters);

            while (dr.Read())
            {
                Values.Add("iPartnerId", dr["iPartnerId"].ToString());
            }
            Values.Add("PW", pw);
            return Values;
        }
        public string Save_User(Classes.Common.CurrentUser u)
        {
            string pw = Security_Provider.GeneratePassword(10);


            SqlDataAdapter da = new SqlDataAdapter();

            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iUser_Type_Id", u.iUser_Type_Id),
                new SqlParameter("@vcName",U.CryptorEngine.GenericEncrypt(u.vcName,true)),
                new SqlParameter("@vcSurname",U.CryptorEngine.GenericEncrypt(u.vcSurname,true)),
                new SqlParameter("@iPartner_Type_Id",u.iPartner_Type_Id),
                new SqlParameter("@iPartner_Id",u.iPartner_Id),
                new SqlParameter("@vcPosition_Title",U.CryptorEngine.GenericEncrypt(u.vcPosition_Title,true)),
                new SqlParameter("@vcUsername",U.CryptorEngine.GenericEncrypt(u.vcUsername.ToLower(),true)),
                new SqlParameter("@vcContactNumber",U.CryptorEngine.GenericEncrypt(u.vcContactNumber,true)),
                new SqlParameter("@vcPassword", U.CryptorEngine.ValidationEncrypt(pw, true)),
                new SqlParameter("@bUserReceiveNotifications",u.bUserReceiveNotifications),
                 new SqlParameter("@iDivision_Id","0"),
                                   
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spIns_Save_User", parameters);

            return pw;
        }
        public bool Check_Username(string username)
        {
            bool exists = false;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@username",U.CryptorEngine.ValidationEncrypt(username.ToLower(),true)),


        };
            //sqlConn.Open();
            //da = new SqlDataAdapter(cmd);
            //da.Fill(ds);
            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spGet_Check_Username", parameters);

            while (dr.Read())
            {
                exists = true;
            }

            return exists;

        }
        private DataTable Convert_Financed_Assets_ToDatatable(IEnumerable<Classes.Partners.Financed_Assets> listItems)
        {
            var dataTable = new DataTable();

            foreach (var info in typeof(Classes.Partners.Financed_Assets).GetProperties())
            {
                dataTable.Columns.Add(info.Name, info.PropertyType);
            }
            dataTable.AcceptChanges();


            foreach (var item in listItems)
            {
                var datos = new Classes.Partners.Financed_Assets();
                datos = item;
                DataRow row = dataTable.NewRow();

                foreach (var property in datos.GetType().GetProperties())
                {
                    row[property.Name] = property.GetValue(datos, null);
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        private DataTable Convert_Divisions_ToDatatable(IEnumerable<Classes.Common.Common.Divisions> listItems)
        {
            var dataTable = new DataTable();

            foreach (var info in typeof(Classes.Common.Common.Divisions).GetProperties())
            {
                dataTable.Columns.Add(info.Name, info.PropertyType);
            }
            dataTable.AcceptChanges();


            foreach (var item in listItems)
            {
                var datos = new Classes.Common.Common.Divisions();
                datos = item;
                DataRow row = dataTable.NewRow();

                foreach (var property in datos.GetType().GetProperties())
                {
                    row[property.Name] = property.GetValue(datos, null);
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        public bool Update_Financer(int @iPartner_Id, Classes.Partners.Financer fa, List<Classes.Partners.Financed_Assets> financed_Assets)
        {
            bool updated = false;

            SqlDataAdapter da = new SqlDataAdapter();
            var dt = Convert_Financed_Assets_ToDatatable(financed_Assets);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iPartner_Id", iPartner_Id),
                new SqlParameter("@vcFinancer_Name", U.CryptorEngine.GenericEncrypt(fa.vcFinancer_Name,true)),
                new SqlParameter("@vcBusiness_registration_Number", U.CryptorEngine.GenericEncrypt(fa.vcBusiness_registration_Number,true)),
                new SqlParameter("@vcVat_Registration_Number", U.CryptorEngine.GenericEncrypt(fa.vcVat_Registration_Number, true)),
                new SqlParameter("@vcBuilding_Unit", U.CryptorEngine.GenericEncrypt(fa.vcBuilding_Unit,true)),
                new SqlParameter("@vcAddress_Line_1", U.CryptorEngine.GenericEncrypt(fa.vcAddress_Line_1,true)),
                new SqlParameter("@vcAddress_Line_2", U.CryptorEngine.GenericEncrypt(fa.vcAddress_Line_2,true)),
                new SqlParameter("@vcSuburb", U.CryptorEngine.GenericEncrypt(fa.vcSuburb,true)),
                new SqlParameter("@vcCity", U.CryptorEngine.GenericEncrypt(fa.vcCity,true)),
                new SqlParameter("@vcPostal_Code", U.CryptorEngine.GenericEncrypt(fa.vcPostal_Code,true)),
                new SqlParameter("@iProvince_Id", fa.iProvince_Id),
                new SqlParameter("@vcPOBox_Bag", U.CryptorEngine.GenericEncrypt(fa.vcPOBox_Bag,true)),
                new SqlParameter("@vcPost_Office_Name", U.CryptorEngine.GenericEncrypt(fa.vcPost_Office_Name,true)),
                new SqlParameter("@vcPost_Office_Postal_Code", U.CryptorEngine.GenericEncrypt(fa.vcPost_Office_Postal_Code,true)),
                new SqlParameter("@bPostalAddresSameAsPhysical", fa.bPostalAddresSameAsPhysical),
                new SqlParameter("@vcContact_Number", U.CryptorEngine.GenericEncrypt(fa.vcContact_Number,true)),
                new SqlParameter("@iPackage_Id", fa.iPackage_Id),
                new SqlParameter("@dtFinancedAssets", dt),
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spUpd_Financer", parameters);
            updated = true;

            return updated;
        }
        public bool Update_Insurer(int @iPartner_Id, Classes.Partners.Insurer fa)
        {
            bool updated = false;
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iPartner_Id", iPartner_Id),
                new SqlParameter("@vcInsurance_Company_Name", U.CryptorEngine.GenericEncrypt(fa.vcInsurance_Company_Name, true)),
                new SqlParameter("@vcBusiness_registration_Number", U.CryptorEngine.GenericEncrypt(fa.vcBusiness_registration_Number, true)),
                new SqlParameter("@vcVat_Registration_Number", U.CryptorEngine.GenericEncrypt(fa.vcVat_Registration_Number, true)),
                new SqlParameter("@vcBuilding_Unit", U.CryptorEngine.GenericEncrypt(fa.vcBuilding_Unit, true)),
                new SqlParameter("@vcAddress_Line_1", U.CryptorEngine.GenericEncrypt(fa.vcAddress_Line_1, true)),
                new SqlParameter("@vcAddress_Line_2", U.CryptorEngine.GenericEncrypt(fa.vcAddress_Line_2, true)),
                new SqlParameter("@vcSuburb", U.CryptorEngine.GenericEncrypt(fa.vcSuburb, true)),
                new SqlParameter("@vcCity", U.CryptorEngine.GenericEncrypt(fa.vcCity, true)),
                new SqlParameter("@vcPostal_Code", U.CryptorEngine.GenericEncrypt(fa.vcPostal_Code, true)),
                new SqlParameter("@iProvince_Id",fa.iProvince_Id),
                new SqlParameter("@vcPOBox_Bag", U.CryptorEngine.GenericEncrypt(fa.vcPOBox_Bag, true)),
                new SqlParameter("@vcPost_Office_Name", U.CryptorEngine.GenericEncrypt(fa.vcPost_Office_Name, true)),
                new SqlParameter("@vcPost_Office_Postal_Code", U.CryptorEngine.GenericEncrypt(fa.vcPost_Office_Postal_Code, true)),
                new SqlParameter("@bPostalAddresSameAsPhysical", fa.bPostalAddresSameAsPhysical),
                new SqlParameter("@vcContact_Number", U.CryptorEngine.GenericEncrypt(fa.vcContact_Number, true)),

        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spUpd_Insurer", parameters);
            updated = true;

            return updated;
        }

        public bool Update_Partner_Logo(int @iPartner_Id, int Ipartner_Type_Id, string Filename)
        {
            bool updated = false;
            SqlDataAdapter da = new SqlDataAdapter();


            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@iPartner_Id", iPartner_Id),
                new SqlParameter("@Ipartner_Type_Id", Ipartner_Type_Id),
                new SqlParameter("@Filename",Filename),


        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spUpd_PartnerLogo", parameters);
            updated = true;

            return updated;
        }

    }
}
