using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.Policy
{
    public class PolicyStatusRequest
    {       // public Request_Header header { get; set; }
        public int trasactionId { get; set; }
        // public int companyId { get; set; }
        public string sourceIdentifier { get; set; }
        public IEnumerable<PolicyDetails> policyDetails { get; set; }

        public class PolicyDetails
        {
            public string policyNumber { get; set; }
            public string newStatus { get; set; }
            public string dateStatusChanged { get; set; }
        }
    }
}
