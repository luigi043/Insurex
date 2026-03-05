using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.Policy
{
    public class Policy
    {

        public int iPolicy_Id { get; set; }
        public int? TenantId { get; set; }
        public int iInsurance_Company_Id { get; set; }
        public string vcPolicy_Number { get; set; }
        
        public int iPolicy_Type_Id { get; set; }
        public int iPolicy_Payment_Frequency_Type_Id { get; set; }
        public Policy_Holder_Consumer policy_Holder_Individual { get; set; }
        public Policy_Holder_Business policy_Holder_Business { get; set; }
    }
}







