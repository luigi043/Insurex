using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.AssetTypes
{
    public class UpdateAssetInsuredValueRequest
    {
        public int trasactionId { get; set; }
        public string sourceIdentifier { get; set; }
        public List<VehicleAssets> vehicleAssets { get; set; }
        public List<PropertyAssets> propertyAssets { get; set; }
        public List<WatercraftAssets> watercraftAssets { get; set; }
        public List<AviationtAssets> aviationtAssets { get; set; }
        public List<MachineryAssets> machineryAssets { get; set; }
        
        public List<PlantEquipmentAssets> plantEquipmentAssets { get; set; }
        public List<ElectronicEquipmentAssets> electronicEquipmentAssets { get; set; }

        public class VehicleAssets
        {
            public string policyNumber { get; set; }
            public string vinNumber { get; set; }
            public decimal newInsuredValue { get; set; }
        }
        public class PropertyAssets
        {
            public string policyNumber { get; set; }
            public string standNumber_ERFPortion { get; set; }
            public string sectionalTitleNumber { get; set; }
            public string sectionalTitleName { get; set; }
            public decimal newInsuredValue { get; set; }
        }
        public class WatercraftAssets
        {
            public string policyNumber { get; set; }
            public string identificationNumber { get; set; }
            public decimal newInsuredValue { get; set; }
        }
        public class AviationtAssets
        {
            public string policyNumber { get; set; }
            public string tailNumber { get; set; }
            public decimal newInsuredValue { get; set; }
        }
        public class MachineryAssets
        {
            public string policyNumber { get; set; }
            public string serialNumber { get; set; }
            public decimal newInsuredValue { get; set; }
        }
        public class PlantEquipmentAssets
        {
            public string policyNumber { get; set; }
           
            public string identificationNumber { get; set; }
            public string serialNumber { get; set; }
            public decimal newInsuredValue { get; set; }
        }

        public class ElectronicEquipmentAssets
        {
            public string policyNumber { get; set; }
            public string serialNumber { get; set; }
            public decimal newInsuredValue { get; set; }
        }
    }
}
