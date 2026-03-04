using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.AssetTypes.API_NewAssets
{
    public class BusinessDetails
    {
        public int policyHolderId { get; set; }
        public string businessName { get; set; }
        public string registrationNumber { get; set; }
        public string contactFullname { get; set; }
        public string contactCellPhoneNumber { get; set; }
        public string contactAlternativeNumber { get; set; }
        public string contactEmailAddress { get; set; }
        public bool postalAddresSameAsPhysical { get; set; }
        public Phycisaladdress physicalAddress { get; set; }
        public PostalAddress postalAddress { get; set; }
    }
}
