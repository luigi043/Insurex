using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.Policy
{
    public class PolicyNonPaymentRequest
    {

        // public Request_Header header { get; set; }
        public int trasactionId { get; set; }
        // public int companyId { get; set; }
        public string sourceIdentifier { get; set; }
        public IEnumerable<PolicyDetails> policyDetails { get; set; }

        public class PolicyDetails
        {
            public string policyNumber { get; set; }
            public string affectedPeriod { get; set; }
            public int affectedYear { get; set; }
            public string dateOfNonPayment { get; set; }
        }
    }
}







