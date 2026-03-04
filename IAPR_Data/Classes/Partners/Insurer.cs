using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.Partners
{
    public class Insurer
    {
        public int iInsurance_Company_Id { get; set; }
        public string vcInsurance_Company_Name { get; set; }
        public string vcBusiness_registration_Number { get; set; }
        public string vcVat_Registration_Number { get; set; }
        public string vcBuilding_Unit { get; set; }
        public string vcAddress_Line_1 { get; set; }
        public string vcAddress_Line_2 { get; set; }
        public string vcSuburb { get; set; }
        public string vcCity { get; set; }
        public string vcPostal_Code { get; set; }
        public int iProvince_Id { get; set; }
        public string vcPOBox_Bag { get; set; }
        public string vcPost_Office_Name { get; set; }
        public string vcPost_Office_Postal_Code { get; set; }
        public int iAdmin_User_Id { get; set; }
        public string vcAPI_Source_Identifier { get; set; }
        public bool bPostalAddresSameAsPhysical { get; set; }
        public string vcContact_Number { get; set; }
    }
}
