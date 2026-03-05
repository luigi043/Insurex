using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int? AssetId { get; set; }
        public string? Status { get; set; }
        public DateTime ExpiryDate { get; set; }

        // [NotMapped] prevents EF Core from following these navigation properties
        // and discovering Policy_Holder_Consumer / Policy_Holder_Business as entity
        // types. Those classes reference Phycisal_address which has no PK, causing
        // a model validation crash. They are DTO types used only with stored procs.
        [NotMapped]
        public Policy_Holder_Consumer policy_Holder_Individual { get; set; }

        [NotMapped]
        public Policy_Holder_Business policy_Holder_Business { get; set; }
    }
}