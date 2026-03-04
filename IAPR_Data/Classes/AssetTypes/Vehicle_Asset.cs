using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.AssetTypes
{
    public class Vehicle_Asset
    {
        public int iVehicle_Asset_Id { get; set; } //system Id
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
        public int iVehicle_Asset_Type_Id { get; set; }
        public string vcVehicle_Asset_Type_Description { get; set; }
        public int iVehicle_Asset_Licence_Type_Id { get; set; }
        public string vcVehicle_Asset_Licence_Type_Description { get; set; }
        public int iAsset_Usage_Type_Id { get; set; }
        public string vcAsset_Usage_Type_Description { get; set; }
        public int iAsset_Condition_Id { get; set; }
        public string vcAsset_Condition_Description { get; set; }
        public int iVehicle_Make_Id { get; set; }
        public string vcVehicle_Make_Description { get; set; }
        public int iVehicle_Model_Id { get; set; }
        public string vcVehicle_Model_Description { get; set; }
        public int iVehicle_Model_Variant_Id { get; set; }
        public string vcVehicle_Model_Variant_Description { get; set; }
        public string vcVin_Number { get; set; }
        public string vcRegistration_Number { get; set; }
        public int iModel_Year { get; set; }
        public string dtFinance_Start_Date { get; set; }
        public string dtFinance_End_Date { get; set; }
        public string vcVehicle_Color { get; set; }
        public Common.Addresses.Phycisal_address physical_Address { get; set; }
        public Common.Addresses.Postal_Address postal_Address { get; set; }


    }
    public class API_Vehicle_Asset
    {

        public string vcAsset_Cover_Type_Description { get; set; }

        public string vcFinancer_Name { get; set; }
        public string vcFinance_Agrreement_Number { get; set; }
        public decimal mAsset_Finance_Value { get; set; }
        public decimal mAsset_Insurance_Value { get; set; }

        public string vcAsset_Type_Description { get; set; }

        public string vcVehicle_Asset_Type_Description { get; set; }



        public string vcVehicle_Make_Description { get; set; }
        public string vcVehicle_Model_Description { get; set; }
        public string vcVehicle_Model_Variant_Description { get; set; }
        public string vcVin_Number { get; set; }
        public string vcRegistration_Number { get; set; }
        public int iModel_Year { get; set; }
        public string dtFinance_Start_Date { get; set; }
        public string dtFinance_End_Date { get; set; }

        public Common.Addresses.Phycisal_address physical_Address { get; set; }
        public Common.Addresses.Postal_Address postal_Address { get; set; }


    }
}
