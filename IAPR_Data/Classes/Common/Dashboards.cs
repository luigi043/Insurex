using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.Common
{
    public class Dashboards
    {
        public class FinancerLandingTableTotals
        {
            #region UninsuredAssets
            public string vcFinancer { get; set; }
            public int PremiumUnpaidAssetCount { get; set; }
            public decimal PremiumUnpaidAssetTotal { get; set; }
            public int NoInsuranceAssetCount { get; set; }
            public decimal NoInsuranceAssetTotal { get; set; }
            public int PremiumUnpaidAssetCountPercent { get; set; }
            public int PremiumUnpaidAssetTotalPercent { get; set; }
            public int NoInsuranceAssetCountPercent { get; set; }
            public int NoInsuranceAssetTotalPercent { get; set; }
           

            #endregion


            #region AllAssets
            public int AllAssetCount { get; set; }
            public decimal AllAssetTotal { get; set; }
            #endregion


            #region InsuredAssets
            public decimal InsuredTotal { get; set; }
            public decimal InsuredAssetCount { get; set; }
            public decimal InsuredAdequatelyTotal { get; set; }
            public int InsuredAdequatelyAssetCount { get; set; }
            public decimal InsuredUnderInsuredTotal { get; set; }
            public int InsuredUnderInsuredAssetCount { get; set; }
            
            public int InsuredAdequatelyTotalPercent { get; set; }
            public int InsuredUnderInsuredTotalPercent { get; set; }

            #endregion


            //Calculated
            public int AllUninsuredCount { get; set; }
            public int AllUninsuredCountPercent { get; set; }
            public decimal AllUninsuredTotal { get; set; }

            public int AllUninSuredTotalPercent { get; set; }
            //public int iNumber_Of_Uninsued_Assets { get; set; }
            //public decimal dcUninsured_Finance_Value { get; set; }
            //public decimal dcUninsured_Insurance_Value { get; set; }

            //public int iNumber_Of_Assets { get; set; }
            //public decimal dcFinance_Value { get; set; }
            //public decimal dcInsurance_Value { get; set; }
        }

        public class InsurerLandingTableTotals
        {
            public string vcInsurer { get; set; }
            public int iNumber_Of_Uninsured_Assets { get; set; }
            public decimal dcInsurance_Uninsured_Value { get; set; }
            public int iTotal_Number_Of_Assets { get; set; }
            public decimal dcTotal_Insurance_Value { get; set; }
        }

        public class SimpleChart
        {
            string label { get; set; }
            string value { get; set; }
        }
    }
}







