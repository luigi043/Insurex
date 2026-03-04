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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "policy_transactions" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select policy-transactions.svc or policy-transactions.svc.cs at the Solution Explorer and start debugging.
    public class policy_transactions : IpolicyNonpayment
    {



        [WebInvoke(UriTemplate = "/policy-nonpayment",
    RequestFormat = WebMessageFormat.Json,
    ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        public string Policy_Nonpayment(string _policyNonPaymentRequest)
        {
            C.Response res = new C.Response();
            C.Policy.PolicyNonPaymentRequest policyNonPaymentRequest = new C.Policy.PolicyNonPaymentRequest();
            int iPartner_Id = 0;
            // ((I.ijsonValidator)new P.jsonValidator_Provider()).Validate_Policy_NonPayment_Data(_policyNonPaymentRequest);
            ((I.ijsonValidator)new P.jsonValidator_Provider()).Validate_Policy_NonPayment_Data(_policyNonPaymentRequest, out res);

            if (res.statusCode == 0)
            {
                policyNonPaymentRequest = JsonConvert.DeserializeObject<C.Policy.PolicyNonPaymentRequest>(_policyNonPaymentRequest);
                P.Partner_Provider pP = new P.Partner_Provider();
                iPartner_Id = pP.Get_Check_Insurer_Partner_By_API_Identifier(policyNonPaymentRequest.sourceIdentifier);
            }

            if (res.statusCode == 0 && iPartner_Id != 0)
            {
                P.Policy_Provider p = new P.Policy_Provider();
                res = p.Save_Bulk_Policy_NonPayment(policyNonPaymentRequest, iPartner_Id);
            }


            JavaScriptSerializer JSS = new JavaScriptSerializer();
            var json = JSS.Serialize(res);
            return json;
        }

    }
}
