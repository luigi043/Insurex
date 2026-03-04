using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.AssetTypes.API_NewAssets
{
    public class ConsumerDetails
    {
        public int policyHolderId { get; set; }
        public string identificationType { get; set; }
        public string identificationTypeDesccription { get; set; }
        public string consumerTitle { get; set; }

        public string firstNames { get; set; }
        public string surname { get; set; }
        public string identificationNumber { get; set; }
        public int policyPhysicalAddressId { get; set; }
        public int policyPostalAddressId { get; set; }
        public string CellPhoneNumber { get; set; }
        public string alternativeContactNumber { get; set; }
        public string emailAddress { get; set; }
        public bool postalAddresSameAsPhysical { get; set; }
        public Phycisaladdress physicalAddress { get; set; }

        public PostalAddress postalAddress { get; set; }
    }
}
