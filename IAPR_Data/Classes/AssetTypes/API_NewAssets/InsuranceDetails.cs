using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.AssetTypes.API_NewAssets
{
    public class InsuranceDetails
    {
        public string insurerName { get; set; }
        public string policyType { get; set; }
        public string policyNumber { get; set; }
        public string coverType { get; set; }
        public string premiumFrequency { get; set; }
        public double insuranceValue { get; set; }
    }
}
