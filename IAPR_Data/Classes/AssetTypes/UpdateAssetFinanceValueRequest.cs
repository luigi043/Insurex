using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IAPR_Data.Classes.AssetTypes
{
    public class UpdateAssetFinanceValueRequest
    {
        public int trasactionId { get; set; }
        public string sourceIdentifier { get; set; }
        public List<AssetDetails> assetDetails { get; set; }
        public class AssetDetails
        {
            public string assetType { get; set; }
            public string financeAgreementNumber { get; set; }
            public decimal financeValue { get; set; }
        }
    }
}
