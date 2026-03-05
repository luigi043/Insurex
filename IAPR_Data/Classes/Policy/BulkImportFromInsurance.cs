using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.Policy
{
    public class BulkImportFromInsurance
    {
        public class GenericItems
        {
            public int iCounterID { get; set; }
            public int iInsurance_Company_Id { get; set; }
            public string vcFinancer_Name { get; set; }
            public string vcFinance_Agrreement_Number { get; set; }
            public string vcPolicy_Number { get; set; }
            public string vcAsset_Type_Description { get; set; }
            public string vcAsset_Sub_Type_Description { get; set; }
            public string vcAsset_Unique_Identifier { get; set; }

            public string vcAsset_Cover_Type_Description { get; set; }
            public string vcPolicy_Status { get; set; }

            public string vcPolicy_Payment_Frequency_Type_Description { get; set; }

            //public string vcPolicy_Type_Description { get; set; }
        }

        public class IndividualHolder
        {
            public int iCounterID { get; set; }
            public string vcIdentification_Type_Desccription { get; set; }
            public string vcIdentification_Number { get; set; }
            public string vcPerson_Title_Description { get; set; }
            public string vcFirst_Names { get; set; }
            public string vcSurname { get; set; }
            public bool bPostalAddresSameAsPhysical { get; set; }
            public string vcContact_Number { get; set; }
            public string vcAlternative_Contact_Number { get; set; }
            public string vcEmail_Address { get; set; }

        }
        public class BusinessHolder
        {
            public int iCounterID { get; set; }
            public string vcBusiness_Name { get; set; }
            public string vcBusiness_Registration_Number { get; set; }

            public string vcBusiness_Contact_Fullname { get; set; }
            public bool bPostalAddresSameAsPhysical { get; set; }
            public string vcBusiness_Contact_Number { get; set; }
            public string vcBusiness_Contact_Alternative_Number { get; set; }
            public string vcBusiness_Email_Address { get; set; }

        }

        public class Phycisal_address
        {
            public int iCounterID { get; set; }
            public string vcBuilding_Unit { get; set; }
            public string vcAddress_Line_1 { get; set; }
            public string vcAddress_Line_2 { get; set; }
            public string vcSuburb { get; set; }
            public string vcCity { get; set; }
            public string vcProvince { get; set; }
            public string vcPostal_Code { get; set; }


        }

        public class Postal_Address
        {
            public int iCounterID { get; set; }
            public string vcPOBox_Bag { get; set; }
            public string vcPost_Office_Name { get; set; }
            public string vcPost_Postal_Code { get; set; }


        }

    }
}







