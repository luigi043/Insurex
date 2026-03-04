using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C = IAPR_Data.Classes;
namespace IAPR_Data.Interfaces
{
   public interface ijsonValidator
    {
        void Validate_Policy_NonPayment_Data1(string nonPayment_Request);
        void Validate_Policy_NonPayment_Data(string nonPayment_Request, out C.Response res);
        void Validate_Update_Asset_Finance_Value_Data(string updateAssetFinanceValue_Request, out C.Response res);
        void Validate_Update_Asset_Insured_Value_Data(string updateAssetInsuredValue_Request, out C.Response res);
        void Validate_Update_Asset_Cover_Data(string updateAssetCover_Request, out C.Response res);
        void Validate_Update_Asset_Remove_Data(string updateAssetCover_Request, out C.Response res);

        void Validate_Policy_Status_Data(string policyStatus_Request, out C.Response res);

        #region AssetManagement
        void Validate_New_Asset_Vehicle(string nonPayment_Request, out C.Response res);
        #endregion
    }
}
