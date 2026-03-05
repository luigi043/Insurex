using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.Common
{
    public class Addresses
    {
        public class Phycisal_address
        {
           
            public string vcBuilding_Unit { get; set; }
            public string vcAddress_Line_1 { get; set; }
            public string vcAddress_Line_2 { get; set; }
            public string vcSuburb { get; set; }
            public string vcCity { get; set; }
            public int iProvince_Id { get; set; }
            public string vcPostal_Code { get; set; }


        }

        public class Postal_Address
        {
           
            public string vcPOBox_Bag { get; set; }
            public string vcPost_Office_Name { get; set; }
            public string vcPost_Postal_Code { get; set; }


        }
    }
}







