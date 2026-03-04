using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.AssetTypes
{
    public class ReinstatedCover_Assets
    {
        public int iPolicy_Id { get; set; }
        public int iAsset_Id { get; set; }

        public int iAsset_Type_Id { get; set; }
        public bool bPolicy_Holder_Confirmed { get; set; }
        public int iFinancer_Id { get; set; }
        public string vcFinancer_Name { get; set; }

        public string vcFinance_Agrreement_Number { get; set; }
        public string vcInsurance_Company_Name { get; set; }

        public string vcPolicy_Number { get; set; }

        public string vcAsset_Type_Description { get; set; }
        public string vcAsset_SubType_Description { get; set; }
        public string mAsset_Finance_Value { get; set; }

        public string dtDate_since_Unisured { get; set; }
        public string dtDate_Reinstated { get; set; }
        public int iDays_Difference { get; set; }
        public int iAlighnment_Id { get; set; }
    }
}
