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
namespace IAPR_API.policy_management
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "cancelPolicy" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select cancelPolicy.svc or cancelPolicy.svc.cs at the Solution Explorer and start debugging.
    public class cancelPolicy : IpolicyStatus
    {
        [WebInvoke(UriTemplate = "/cancel-policy",
 RequestFormat = WebMessageFormat.Json,
 ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        public string Policy_Status(string _policyStatusRequest)
        {
            C.Response res = new C.Response();
            C.Policy.PolicyStatusRequest policyStatusRequest = new C.Policy.PolicyStatusRequest();
            int iPartner_Id = 0;
            // ((I.ijsonValidator)new P.jsonValidator_Provider()).Validate_Policy_NonPayment_Data(_policyNonPaymentRequest);
            ((I.ijsonValidator)new P.jsonValidator_Provider()).Validate_Policy_Status_Data(_policyStatusRequest, out res);

            if (res.statusCode == 0)
            {
                policyStatusRequest = JsonConvert.DeserializeObject<C.Policy.PolicyStatusRequest>(_policyStatusRequest);
                P.Partner_Provider pP = new P.Partner_Provider();
                iPartner_Id = pP.Get_Check_Insurer_Partner_By_API_Identifier(policyStatusRequest.sourceIdentifier);
            }

            if (res.statusCode == 0 && iPartner_Id != 0)
            {
                P.Policy_Provider p = new P.Policy_Provider();
                p.Save_Bulk_Policy_Status(policyStatusRequest, iPartner_Id);
            }


            JavaScriptSerializer JSS = new JavaScriptSerializer();
            var json = JSS.Serialize(res);
            return json;
        }
    }
}
