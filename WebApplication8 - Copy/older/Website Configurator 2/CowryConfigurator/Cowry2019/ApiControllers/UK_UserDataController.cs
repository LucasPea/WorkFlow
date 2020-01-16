using Cowry2019.integrate;
using Cowry2019.Models;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Configuration;
using System.Web.Http;

namespace Cowry2019.ApiControllers
{
    [RoutePrefix("Cowry")]
    public class UK_UserDataController : ApiController
    {
        
        
        [HttpPost]
        [Route("Common/AddUserData")] 
        public HttpResponseMessage AddLead(UserData userData)
        {
            try
            {
                Guid ret = Guid.Empty;
                CrmAccess myAccess = new CrmAccess();
                ret=myAccess.CreateNewLead(userData);
                return Request.CreateResponse(HttpStatusCode.OK, ret);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }
    }
}
