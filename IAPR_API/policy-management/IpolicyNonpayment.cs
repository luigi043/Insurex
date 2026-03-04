using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using C = IAPR_Data.Classes;
using P = IAPR_Data.Providers;
using I = IAPR_Data.Interfaces;

namespace IAPR_API.policy_management
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Ipolicy_transactions" in both code and config file together.
    [ServiceContract]
    public interface IpolicyNonpayment
    {
      
        [OperationContract]
        string Policy_Nonpayment(string _policyNonPaymentRequest);
    }
}
