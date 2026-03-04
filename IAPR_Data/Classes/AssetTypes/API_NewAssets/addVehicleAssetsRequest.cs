using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.AssetTypes.API_NewAssets
{
    public class addVehicleAssetsRequest
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
            public VehicleDetails vehicleDetails { get; set; }
            public bool insuranceDetailsAvalable { get; set; }
            public InsuranceDetails insuranceDetails { get; set; }
        }
        public class BusinessVehicles
        {
            public BusinessDetails businessDetails { get; set; }
            public VehicleDetails vehicleDetails { get; set; }
            public bool insuranceDetailsAvalable { get; set; }
            public InsuranceDetails insuranceDetails { get; set; }
        }

        public class VehicleDetails
        {
            public string financeAgreementNumber { get; set; }
            public decimal assetFinanceValue { get; set; }
            public int assetTypeId { get; set; }
            public string assetTypeDescription { get; set; }
            public string vehicleType { get; set; }
            public string vehicleAssetTypeDescription { get; set; }
            public int vehicleAssetLicenceTypeId { get; set; }

            public string assetUsage { get; set; }
            public string assetCondition { get; set; }
            public string vehicleMakeDescription { get; set; }
            public string vehicleModelDescription { get; set; }
            public string vehicleModelVariantDescription { get; set; }
            public string vinNumber { get; set; }
            public string registrationNumber { get; set; }
            public int modelYear { get; set; }
            public string financeStartDate { get; set; }
            public string financeEndDate { get; set; }
            //public string vehicleColor { get; set; }



        }
    }
}
