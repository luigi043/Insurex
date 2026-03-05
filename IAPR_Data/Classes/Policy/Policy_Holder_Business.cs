using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.Policy
{
    public class Policy_Holder_Business
    {
        public int iPolicy_Holder_Id { get; set; }
        public string vcBusiness_Name { get; set; }
        public string vcBusiness_Registration_Number { get; set; }
        public int iPolicy_Physical_Address_Id { get; set; }
        public int iPolicy_Postal_Address_Id { get; set; }
        public string vcBusiness_Contact_Fullname { get; set; }
        public string vcBusiness_Contact_Number { get; set; }
        public string vcBusiness_Contact_Alternative_Number { get; set; }
        public string vcBusiness_Email_Address { get; set; }

        public bool bPostalAddresSameAsPhysical { get; set; }
        public Common.Addresses.Phycisal_address physical_Address { get; set; }

        public Common.Addresses.Postal_Address postal_Address { get; set; }
    }
}







