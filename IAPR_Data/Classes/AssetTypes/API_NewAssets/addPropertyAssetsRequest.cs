using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.AssetTypes.API_NewAssets
{
    class addPropertyAssetsRequest
    {
        public int trasactionId { get; set; }
        public string sourceIdentifier { get; set; }
        public VehicleAssets vehicleAssets { get; set; }
        public class VehicleAssets
        {
            public List<ConsumerVehicles> consumerVehicles { get; set; }
            public List<BusinessVehicles> businessVehicles { get; set; }
        }

        public class ConsumerVehicles
        {
            public ConsumerDetails consumerDetails { get; set; }
            public PropertyDetails propertyDetails { get; set; }
            public bool insuranceDetailsAvalable { get; set; }
            public InsuranceDetails insuranceDetails { get; set; }
        }
        public class BusinessVehicles
        {
            public BusinessDetails businessDetails { get; set; }
            public PropertyDetails propertyDetails { get; set; }
            public bool insuranceDetailsAvalable { get; set; }
            public InsuranceDetails insuranceDetails { get; set; }
        }

        public class PropertyDetails
        {
            public string vcFinanceAgrreementNumber { get; set; }
            public decimal assetFinanceValue { get; set; }
            public string vcAssetType { get; set; }
            public string propertyType { get; set; }
            public string financeStartDate { get; set; }
            public string financeEndDate { get; set; }
            public string standERFNumber { get; set; }
            public string sectionalTitleNumber { get; set; }
            public string sectionalTitleName { get; set; }
            public string assetUsage { get; set; }
                                 
        }
    }
}
