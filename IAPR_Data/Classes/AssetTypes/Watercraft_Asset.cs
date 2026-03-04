using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.AssetTypes
{
    public class Watercraft_Asset
    {
        public int iWatercraft_Asset_Id { get; set; }
        public int iPolicy_Id { get; set; }
        public int? TenantId { get; set; }
        public int iAsset_Cover_Type_Id { get; set; }
        public string vcAsset_Cover_Type_Description { get; set; }
        public int iFinancer_Id { get; set; }
        public string vcFinancer_Name { get; set; }
        public string dtFinance_Start_Date { get; set; }
        public string dtFinance_End_Date { get; set; }
        public string vcFinance_Agrreement_Number { get; set; }
        public decimal mAsset_Finance_Value { get; set; }
        public decimal mAsset_Insurance_Value { get; set; }
        public int iWatercraft_Asset_Type_Id { get; set; }
        public string vcWatercraft_Asset_Decsription { get; set; }
        public string vcName_Emblem { get; set; }
        public string vcRegistration_Number { get; set; }
        public string vcClass { get; set; }


    }
}
