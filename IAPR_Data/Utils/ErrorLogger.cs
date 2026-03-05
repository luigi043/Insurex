using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Data;
using Microsoft.Data.SqlClient;


namespace IAPR_Data.Utils
{
    public class ErrorLogger
    {
        public void LogErrorInDB(Exception exception, string application_Area, string method)
        {
            //this.IsSucsessfull = false;
            try
            {
             SqlHelper.ExecuteNonQuery(Classes.Common.Common.Connection(), CommandType.StoredProcedure, "spIns_InsertError",
             new SqlParameter("@vcApplication_Area", application_Area),
             new SqlParameter("@vcMethod", method),
             new SqlParameter("@vcError", exception.Message),
             new SqlParameter("@vcStackTrace", exception.StackTrace));
                               
            }
            catch (Exception ex)
            {

            }
        }
    }
}







