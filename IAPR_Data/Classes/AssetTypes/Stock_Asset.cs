using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.AssetTypes
{
    public class Stock_Asset
    {
        public int iStock_Asset_Id { get; set; }
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
        public int iStock_Asset_Type_Id { get; set; }
        public string vcStock_Asset_Type_Decsription { get; set; }
        public string vcStock_Description { get; set; }
        public string vcStock_Value { get; set; }
    }
}







