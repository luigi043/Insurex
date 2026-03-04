using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using IAPR_Data.Interfaces;
using C = IAPR_Data.Classes;

namespace IAPR_Data.Interfaces
{
    public interface iPolicy_Provider
    {

        int Save_New_Policy_Personal(Classes.Policy.Policy p);
        int Save_New_Policy_Business(Classes.Policy.Policy p);
        int Get_Policy_Id(int iInsurance_Company_Id, string vcPolicy_Number);
        SqlDataReader Get_Individual_Policy_Details(int iInsurance_Company_Id, string vcPolicy_Number);

        int Save_Single_Policy_NonPayment(int iPolicy_Id, int iAffected_Period_Id, int iAffected_Year, string dtDateOfNonPayment);
        C.Response Save_Bulk_Policy_NonPayment(C.Policy.PolicyNonPaymentRequest policyNonPaymentRequest);
    }
}
