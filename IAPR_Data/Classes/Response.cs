using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes
{
    public class Response
    {
        public string trasactionId { get; set; }

        //public string oranisation { get; set; }
        public int statusCode { get; set; } //0,101,102,201,202 
        public string statusMessage { get; set; } //submitted successfully, Invalid json
        public List<string> supportMessages { get; set; } //

    }
    public class vehicleDetailsResponse
    {
        public string trasactionId { get; set; }

        //public string oranisation { get; set; }
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public List<string> supportMessages { get; set; }
        public VehicleFinanceDeatils vehicleFinanceDetails { get; set; }

    }
    public class VehicleFinanceDeatils
    {
        public string financer { get; set; }
        public string financeagrreementnumber { get; set; }

        public string assetTypeDescription { get; set; }

        public string assetSubTypeDescription { get; set; }

        public string make { get; set; }

        public string model { get; set; }

        public string modelVariant { get; set; }
        public string assetFinanceValue { get; set; }
        public string financeStartDate { get; set; }

        public string financeEndDate { get; set; }
    }


    public class propertyDetailsResponse
    {
        public string trasactionId { get; set; }

        //public string oranisation { get; set; }
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public List<string> supportMessages { get; set; }
        public PropertyFinanceDeatils propertyFinanceDetails { get; set; }

    }
    public class PropertyFinanceDeatils
    {
        public string financer { get; set; }
        public string financeagrreementnumber { get; set; }

        public string assetTypeDescription { get; set; }

        public string assetSubTypeDescription { get; set; }

        public string assetFinanceValue { get; set; }
        public string financeStartDate { get; set; }

        public string financeEndDate { get; set; }
    }
}
