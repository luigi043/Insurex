using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace IAPR_API
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "REST_IAPR_Service" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select REST_IAPR_Service.svc or REST_IAPR_Service.svc.cs at the Solution Explorer and start debugging.
    public class REST_IAPR_Service : IREST_IAPR_Service
    {
        public int DoWork()
        {
            return 199;
        }
    }
}
