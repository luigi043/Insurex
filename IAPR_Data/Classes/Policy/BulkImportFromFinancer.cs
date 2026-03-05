using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.Policy
{
    public class BulkImportFromFinancer
    {
        public int iCounterID { get; set; }
        public int iFinancier_Id { get; set; }
        public string vcFinance_Number { get; set; }
        public string vcID_Business_Number { get; set; }
        public string vcCustomer_Type_Description { get; set; }
        public string vcInsurance_Company { get; set; }
        public string vcPolicy_Number { get; set; }
        public string vcAsset_Type_Description { get; set; }
        public string vcAsset_Sub_Type_Description { get; set; }
        public string vcAsset_Unique_Identifier { get; set; }
        public string dtFinancing_StartDate { get; set; }
        public string dtFinancing_EndDate { get; set; }


        //public string vcCustomerName { get; set; }
        //public string vcSurname { get; set; }
        //public Common.Addresses.Phycisal_address physical_Address { get; set; }
        //public Common.Addresses.Postal_Address postal_Address { get; set; }



    }
}







