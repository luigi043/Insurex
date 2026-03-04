using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.AssetTypes
{
    public class Property_Asset
    {
        public int iPolicy_Id { get; set; }
        public int iAsset_Cover_Type_Id { get; set; }
        public string vcAsset_Cover_Type_Description { get; set; }
        public int iFinancer_Id { get; set; }
        public string vcFinancer_Name { get; set; }
        public string vcFinance_Agrreement_Number { get; set; }
        public decimal mAsset_Finance_Value { get; set; }
        public decimal mAsset_Insurance_Value { get; set; }
        public int iAsset_Type_Id { get; set; }
        public string vcAsset_Type_Description { get; set; }
        public int iProperty_Asset_Type_Id { get; set; }
        public string vcProperty_Asset_Type_Description { get; set; }
        public string dtFinance_Start_Date { get; set; }
        public string dtFinance_End_Date { get; set; }
        public string vcStand_ERF_Number { get; set; }
        public string vcSectionalTitleNumber { get; set; }
        public string vcSectionalTitleName { get; set; }
        public int iAsset_Usage_Type_Id { get; set; }
        public string vcAsset_Usage_Type_Description { get; set; }
        public Common.Addresses.Phycisal_address physical_Address { get; set; }
        public Common.Addresses.Postal_Address postal_Address { get; set; }

    }
}
