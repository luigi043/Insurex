using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Web.Script.Serialization;

using C = IAPR_Data.Classes;
using P = IAPR_Data.Providers;
using I = IAPR_Data.Interfaces;

namespace IAPR_API.asset_management
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "updateAssetInsuredValue" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select updateAssetInsuredValue.svc or updateAssetInsuredValue.svc.cs at the Solution Explorer and start debugging.
    public class updateAssetInsuredValue : IupdateAssetInsuredValue
    {
        [WebInvoke(UriTemplate = "/update-asset-insured-value",
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        public string Update_Asset_InsuredValue(string _updateAssetInsuredValueRequest)
        {
            C.Response res = new C.Response();
            List<string> sM = new List<string>();

            C.AssetTypes.UpdateAssetInsuredValueRequest updateAssetInsuredValueRequest = new C.AssetTypes.UpdateAssetInsuredValueRequest();
            int iPartner_Id = 0;
            ((I.ijsonValidator)new P.jsonValidator_Provider()).Validate_Update_Asset_Insured_Value_Data(_updateAssetInsuredValueRequest, out res);

            if (res.statusCode == 0)
            {
                updateAssetInsuredValueRequest = JsonConvert.DeserializeObject<C.AssetTypes.UpdateAssetInsuredValueRequest>(_updateAssetInsuredValueRequest);
                P.Partner_Provider pP = new P.Partner_Provider();
                iPartner_Id = pP.Get_Check_Insurer_Partner_By_API_Identifier(updateAssetInsuredValueRequest.sourceIdentifier);
            }

            if (res.statusCode == 0 && iPartner_Id != 0)
            {
                P.Generic_Asset_Provider p = new P.Generic_Asset_Provider();
                p.Save_Bulk_UpdateAssetInsuredValue(updateAssetInsuredValueRequest, iPartner_Id);
                sM.Add("Processed successfully");
                res.supportMessages = sM;
            }
            else
            {

                res.statusMessage = "Error";

                sM.Add("Source identifier not found");
                res.supportMessages = sM;
                res.supportMessages = sM;
            }

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            var res_json = JSS.Serialize(res);
            return res_json;
        }
    }
}
