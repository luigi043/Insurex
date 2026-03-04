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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "assetFinanceDetails" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select assetFinanceDetails.svc or assetFinanceDetails.svc.cs at the Solution Explorer and start debugging.
    public class assetFinanceDetails : IassetFinanceDetails
    {
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "vehicle/?trasactionId={trasactionId}&sourceIdentifier={sourceIdentifier}&policyNumber={policyNumber}&vinNumber={vinNumber}")]
        public string Vehicle_Details(string trasactionId, string sourceIdentifier, string policyNumber, string vinNumber)
        {
            C.vehicleDetailsResponse res = new C.vehicleDetailsResponse();

            List<string> sM = new List<string>();
            try
            {
                int iPartner_Id = 0;

                P.Partner_Provider pP = new P.Partner_Provider();
                iPartner_Id = pP.Get_Check_Insurer_Partner_By_API_Identifier(sourceIdentifier);

                if (iPartner_Id != 0)
                {
                    P.Vehicle_Asset_Provider p = new P.Vehicle_Asset_Provider();
                    res = p.GetVehicle_Finance_Details(sourceIdentifier, policyNumber, vinNumber);
                    if (res.vehicleFinanceDetails != null)
                    {
                        res.statusCode = 0;
                        res.statusMessage = "Success";
                        sM.Add("Processed successfully");
                        res.supportMessages = sM;
                    }
                    else
                    {
                        res.statusCode = 201;
                        res.statusMessage = "Fail";
                        sM.Add("Could not find asset ");
                        res.supportMessages = sM;
                    }
                }
                else
                {
                    res.statusCode = 200;
                    res.statusMessage = "Error";
                    sM.Add("Source identifier not found");
                    res.supportMessages = sM;
                }



            }
            catch (Exception ex)
            {

                res.statusCode = 101;
                res.statusMessage = "Error";
                sM.Add(ex.Message);
                res.supportMessages = sM;
            }

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            var res_json = JSS.Serialize(res);
            return res_json;
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "property/?trasactionId={trasactionId}&sourceIdentifier={sourceIdentifier}&policyNumber={policyNumber}&standNumber_ERFPortion={standNumber_ERFPortion}&sectionalTitleNumber={sectionalTitleNumber}&sectionalTitleName={sectionalTitleName}")]
        public string Property_Details(string trasactionId, string sourceIdentifier, string policyNumber, string standNumber_ERFPortion, string sectionalTitleNumber, string sectionalTitleName)
        {
            C.propertyDetailsResponse res = new C.propertyDetailsResponse();

            List<string> sM = new List<string>();
            try
            {
                int iPartner_Id = 0;

                P.Partner_Provider pP = new P.Partner_Provider();
                iPartner_Id = pP.Get_Check_Insurer_Partner_By_API_Identifier(sourceIdentifier);

                if (iPartner_Id != 0)
                {
                    P.Property_Asset_Provider p = new P.Property_Asset_Provider();
                    res = p.GetProperty_Finance_Details(sourceIdentifier, policyNumber, standNumber_ERFPortion, sectionalTitleNumber, sectionalTitleName);
                    if (res.propertyFinanceDetails != null)
                    {
                        res.statusCode = 0;
                        res.statusMessage = "Success";
                        sM.Add("Processed successfully");
                        res.supportMessages = sM;
                    }
                    else
                    {
                        res.statusCode = 201;
                        res.statusMessage = "Fail";
                        sM.Add("Could not find asset ");
                        res.supportMessages = sM;
                    }
                }
                else
                {
                    res.statusCode = 200;
                    res.statusMessage = "Error";
                    sM.Add("Source identifier not found");
                    res.supportMessages = sM;
                }



            }
            catch (Exception ex)
            {

                res.statusCode = 101;
                res.statusMessage = "Error";
                sM.Add(ex.Message);
                res.supportMessages = sM;
            }

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            var res_json = JSS.Serialize(res);
            return res_json;
        }


    }
}