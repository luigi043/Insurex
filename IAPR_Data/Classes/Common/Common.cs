using System.Data;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace IAPR_Data.Classes.Common
{
    public class Common
    {
        public static string Connection()
        {
            return SqlHelper.ExecuteDataset(new SqlConnection(""), CommandType.Text, "SELECT 1", null).ToString() ?? ""; // Dummy to keep signature but logic is in SqlHelper
        }
        public static decimal ConvertToMillion(decimal value)
        {
            return value / 1000000;
        }
        public enum Asset_Type
        {
            Vehicle = 1,
            Building_Property = 2,
            Watercraft = 3,
            Aviation = 4,
            Inventory = 5,
            Accounts_receivable = 6,
            Machinery = 7,
            Plant_and_Equipment = 8,
            Business_Interruption = 9,
            Keyman_Insurance = 10

        }
        public enum User_Status
        {
            Active = 1,
            Pending_Password_Change = 2,
            Suspended = 3
        }
        public enum Partner_types
        {
            Insurance_provider = 1,
            Lender = 2,
            Insurex = 3,

        }
        public enum Policy_Holder_Types
        {
            Personal = 1,
            Business = 2,

        }
        public enum Identification_Types
        {
            RSAID = 1,
            PASSPORT = 2,

        }
        public enum Consumer_Titles
        {
            //[Description("Mr Test")]
            Mr = 1,
            Mrs = 2,
            Miss = 3,
            Dr = 2,
            Prof = 2,
            Hon = 2,

        }
        public enum Cover_Types
        {
            Comprehensive = 1,
            [Description("3rd Party Only")]
            ThirdPartyOnly = 2,
            [Description("3rd Party - Fire and Damage")]
            ThirdPartyFireAndDamage = 3,
            Standard = 4,
            FireOnly = 5,
            FireAndAlliedPerils = 6,
            AssetsAllRisks = 7,
            TheftOnly = 8,
            AllRisk = 9,
            DeathAndDisability = 10,
            Hull = 11,
            InFlight = 12,
            GroundRiskHullNotInMotion = 13,
            GroundRiskHullInMotion = 14,
            Unconfirmed = 15
        }
        public enum Partner_Packages
        {
            Consumer = 1,
            Commercial = 2,
            Consumer_and_Commercial = 3,

        }
        public enum Policy_Status
        {
            Active = 1,
            Suspended = 2,
            Cancelled = 3,
            Uncomfirmed = 4,
            In_arrears = 5

        }
        private static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public class Divisions
        {
            public int iCounter { get; set; }
            public int iAsset_Type_Id { get; set; }
            public string vcDivision_Name { get; set; }
        }


        public static int GetEnumFromDescription(string description, Type enumType)
        {
            foreach (var field in enumType.GetFields())
            {
                DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute == null)
                    continue;
                if (attribute.Description == description)
                {
                    return (int)field.GetValue(null);
                }
            }
            return 0;
        }
    }
}







