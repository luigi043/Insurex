using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.AssetTypes.API_NewAssets
{
    public class Phycisaladdress
    {

        public string buildingUnit { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string suburb { get; set; }
        public string city { get; set; }
        public int provinceId { get; set; }
        public string postalCode { get; set; }
    }
}
