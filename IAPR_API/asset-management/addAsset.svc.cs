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

using System.Threading.Tasks;
using System.Web.Script.Serialization;

using C = IAPR_Data.Classes;
using P = IAPR_Data.Providers;
using I = IAPR_Data.Interfaces;
using U = IAPR_Data.Utils;
namespace IAPR_API.asset_management
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "addAsset" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select addAsset.svc or addAsset.svc.cs at the Solution Explorer and start debugging.
    public class addAsset : IaddAsset
    {
        [WebInvoke(UriTemplate = "add-vehicle-assets",
    RequestFormat = WebMessageFormat.Json,
    ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        public string AddVehicleAssets(string _NewAssets)
        {
            C.Response res = new C.Response();
            JavaScriptSerializer JSS = new JavaScriptSerializer();
            List<string> sM = new List<string>();
            res.statusCode = 0;
            try
            {
                C.AssetTypes.API_NewAssets.addVehicleAssetsRequest newVehicleAssetsRequest = new C.AssetTypes.API_NewAssets.addVehicleAssetsRequest();
                int iPartner_Id = 0;

                dynamic request = JsonConvert.DeserializeObject(_NewAssets);

                ((I.ijsonValidator)new P.jsonValidator_Provider()).Validate_New_Asset_Vehicle(_NewAssets, out res);

                res.trasactionId = request.ContainsKey("trasactionId") ? request.trasactionId.Value.ToString() : "";

                if (res.statusCode == 0)
                {
                    newVehicleAssetsRequest = JsonConvert.DeserializeObject<C.AssetTypes.API_NewAssets.addVehicleAssetsRequest>(_NewAssets);
                    P.Partner_Provider pP = new P.Partner_Provider();
                    iPartner_Id = pP.Get_Check_Financer_Partner_By_API_Identifier(newVehicleAssetsRequest.sourceIdentifier);
                }

                if (res.statusCode == 0 && iPartner_Id != 0)
                {
                    P.Vehicle_Asset_Provider p = new P.Vehicle_Asset_Provider();
                    p.Import_New_Vehicles(newVehicleAssetsRequest, iPartner_Id);
                    sM.Add("Processed successfully");
                    res.statusMessage = "Success";
                    res.supportMessages = sM;
                }
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "IAPR_API.asset_management", "AddVehicleAssets");
                sM.Add("An internal error has occured. The administrator has been notified. We will revert back soonest");
                res.statusMessage = "Error";
                res.supportMessages = sM;
            }
            var json = JSS.Serialize(res);
            return json;
        }



    }
}
