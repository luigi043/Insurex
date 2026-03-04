using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Net;
namespace IAPR_API.Auth
{
    public class RestAuthorizationManager : ServiceAuthorizationManager
    {
        /// <summary>  
        /// Method source sample taken from here: http://bit.ly/1hUa1LR  
        /// </summary>  
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            //Extract the Authorization header, and parse out the credentials converting the Base64 string:  
            var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
            if ((authHeader != null) && (authHeader != string.Empty))
            {
                var svcCredentials = System.Text.ASCIIEncoding.ASCII
                    .GetString(Convert.FromBase64String(authHeader.Substring(6)))
                    .Split(':');
                var user = new
                {
                    Name = svcCredentials[0],
                    Password = svcCredentials[1]
                };
                if ((user.Name == "testuser" && user.Password == "testpassword"))
                {
                    //User is authrized and originating call will proceed  
                    return true;
                }
                else
                {
                    //not authorized  
                    return false;
                }
            }
            else
            {
                //No authorization header was provided, so challenge the client to provide before proceeding:  
                WebOperationContext.Current.OutgoingResponse.Headers.Add("WWW-Authenticate: Basic realm=\"MyWCFService\"");
                //Throw an exception with the associated HTTP status code equivalent to HTTP status 401  
                throw new WebFaultException(HttpStatusCode.Unauthorized);
            }
        }
    }
}