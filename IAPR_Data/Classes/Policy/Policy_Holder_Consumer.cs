using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.Policy
{
    public class Policy_Holder_Consumer
    {
        public int iPolicy_Holder_Id { get; set; }
        public int iIdentification_Type_Id { get; set; }
        public string vcIdentification_Type_Desccription { get; set; }
        public int iPerson_Title_Id { get; set; }
        public string vcPerson_Title_Description { get; set; }
        public string vcFirst_Names { get; set; }
        public string vcSurname { get; set; }
        public string vcIdentification_Number { get; set; }
        public int iPolicy_Physical_Address_Id { get; set; }
        public int iPolicy_Postal_Address_Id { get; set; }
        public string vcContact_Number { get; set; }
        public string vcAlternative_Contact_Number { get; set; }
        public string vcEmail_Address { get; set; }
        public bool bPostalAddresSameAsPhysical { get; set; }
        public Common.Addresses.Phycisal_address physical_Address { get; set; }

        public Common.Addresses.Postal_Address postal_Address { get; set; }

        // public  Vehicle_Asset vehicle_Asset { get; set; }
    }
}
