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
    public class Financer_Provider
    {
        public int Save_Financer_Asset_Personal(Classes.Policy.Policy p, string vcLinkKey)
        {
            int alignmentId = 0;

            //var dtPackageEntries = GetPackageTable(cartItems.PackageEntries);
            //var dsLotterEntries = GetLotteryEntries(cartItems.LotteryEntries);
            //var dtLotteryMainInfo = dsLotterEntries.Tables[0];
            //var dtLotteryCombinations = dsLotterEntries.Tables[1];

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {



                new SqlParameter("@iIdentification_Type_Id",p.policy_Holder_Individual.iIdentification_Type_Id),
                new SqlParameter("@iPerson_Title_Id",p.policy_Holder_Individual.iPerson_Title_Id),
                new SqlParameter("@vcFirst_Names",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.vcFirst_Names,true)),
                new SqlParameter("@vcSurname",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.vcSurname,true)),
                new SqlParameter("@vcIdentification_Number",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.vcIdentification_Number,true)),

                new SqlParameter("@vcContact_Number",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.vcContact_Number,true)),
                new SqlParameter("@vcAlternative_Contact_Number",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.vcAlternative_Contact_Number,true)),
                new SqlParameter("@vcEmail_Address",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.vcEmail_Address,true)),


                new SqlParameter("@vcBuilding_Unit",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcBuilding_Unit,true)),
                new SqlParameter("@vcAddress_Line_1",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcAddress_Line_1,true)),
                new SqlParameter("@vcAddress_Line_2",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcAddress_Line_2,true)),
                new SqlParameter("@vcSuburb",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcSuburb,true)),
                new SqlParameter("@vcCity",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcCity,true)),
                new SqlParameter("@iProvince_Id",p.policy_Holder_Individual.physical_Address.iProvince_Id),
                new SqlParameter("@vcPostal_Code",U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.physical_Address.vcPostal_Code,true)),


                new SqlParameter( "@vcPOBox_Bag", (!p.policy_Holder_Individual.bPostalAddresSameAsPhysical) ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.postal_Address.vcPOBox_Bag,true):null),
                new SqlParameter("@vcPost_Office_Name",(!p.policy_Holder_Individual.bPostalAddresSameAsPhysical) ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.postal_Address.vcPost_Office_Name,true):null),
                new SqlParameter("@vcPost_Postal_Code",(!p.policy_Holder_Individual.bPostalAddresSameAsPhysical) ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Individual.postal_Address.vcPost_Postal_Code,true):null),
                new SqlParameter("@bPostalAddresSameAsPhysical",p.policy_Holder_Individual.bPostalAddresSameAsPhysical),
                new SqlParameter("@vcLinkKey",vcLinkKey),


                //Save asset firt
            };

            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spIns_Save_Financer_Asset_Holder_Personal", parameters);

            while (dr.Read())
            {
                alignmentId = Convert.ToInt32(dr["iAsset_Policy_Alignment_Id"].ToString());
            }


            return alignmentId;

        }

        public int Save_Financer_Asset_Business(Classes.Policy.Policy p, string vcLinkKey)
        {
            int alignmentId = 0;



            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();



            SqlParameter[] parameters = new SqlParameter[]
            {


                new SqlParameter("@vcBusiness_Name", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.vcBusiness_Name,true)),
                new SqlParameter("@vcBusiness_Registration_Number", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.vcBusiness_Registration_Number,true)),
                new SqlParameter("@Business_Contact_Fullname", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.vcBusiness_Contact_Fullname,true)),
                new SqlParameter("@vcBusiness_Contact_Number", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.vcBusiness_Contact_Number,true)),
                new SqlParameter("@vcBusiness_Contact_Alternative_Number", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.vcBusiness_Contact_Alternative_Number,true)),
                new SqlParameter("@vcBusiness_Email_Address", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.vcBusiness_Email_Address,true)),



                new SqlParameter("@vcBuilding_Unit", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.physical_Address.vcBuilding_Unit,true)),
                new SqlParameter("@vcAddress_Line_1", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.physical_Address.vcAddress_Line_1,true)),
                new SqlParameter("@vcAddress_Line_2", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.physical_Address.vcAddress_Line_2,true)),
                new SqlParameter("@vcSuburb", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.physical_Address.vcSuburb,true)),
                new SqlParameter("@vcCity", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.physical_Address.vcCity,true)),
                new SqlParameter("@iProvince_Id", p.policy_Holder_Business.physical_Address.iProvince_Id),
                new SqlParameter("@vcPostal_Code", U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.physical_Address.vcPostal_Code,true)),


                new SqlParameter( "@vcPOBox_Bag", (!p.policy_Holder_Business.bPostalAddresSameAsPhysical) ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.postal_Address.vcPOBox_Bag,true):null),
                new SqlParameter("@vcPost_Office_Name", (!p.policy_Holder_Business.bPostalAddresSameAsPhysical) ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.postal_Address.vcPost_Office_Name,true):null),
                new SqlParameter("@vcPost_Postal_Code", (!p.policy_Holder_Business.bPostalAddresSameAsPhysical) ? U.CryptorEngine.GenericEncrypt(p.policy_Holder_Business.postal_Address.vcPost_Postal_Code,true):null),
                new SqlParameter("@bPostalAddresSameAsPhysical", p.policy_Holder_Business.bPostalAddresSameAsPhysical),

                new SqlParameter("@vcLinkKey", vcLinkKey),
        };
            var dr = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
                "spIns_Save_Financer_Asset_Holder_Business", parameters);


            while (dr.Read())
            {
                alignmentId = Convert.ToInt32(dr["iAsset_Policy_Alignment_Id"].ToString());
            }




            return alignmentId;

        }
    }
}








