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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IassetFinanceDetails" in both code and config file together.
    [ServiceContract]
    public interface IassetFinanceDetails
    {
        [OperationContract]
        string Vehicle_Details(string trasactionId, string sourceIdentifier, string policyNumber, string vinNumber);
    }
}
