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
    public class UK_ContactDataController : ApiController
    {
        #region Sharepoint Connection
        string webUrl = WebConfigurationManager.AppSettings["webUrl"];
        string userName = WebConfigurationManager.AppSettings["userName"];
        string password = WebConfigurationManager.AppSettings["password"];
        #endregion

        [HttpPost]
        [Route("Common/AddContactData")]
        public HttpResponseMessage AddResult(UserData contactData)
        {
            long ticks1 = DateTime.Now.Ticks;
            byte[] bytes1 = BitConverter.GetBytes(ticks1);
            string Title = Convert.ToBase64String(bytes1).Replace('+', '_').Replace('/', '-').TrimEnd('=');

            SecureString sec_pass = new SecureString();
            Array.ForEach(password.ToArray(), sec_pass.AppendChar);
            sec_pass.MakeReadOnly();
            using (var context = new ClientContext(webUrl))
            {
                try
                {
                    Guid ret = Guid.Empty;
                    CrmAccess myAccess = new CrmAccess();
                    ret = myAccess.CreateNewLead(contactData);
                    return Request.CreateResponse(HttpStatusCode.OK, ret);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
                }
            }
        }


        [HttpPost]
        [Route("Common/GenerateCrayon")]
        public HttpResponseMessage CommonGenerateCrayon(UserData contactData)
        {
            try
            {/*
                CrayonAccess myCrayonAccess = new CrayonAccess();
                //myCrayonAccess.SetupNewCustomer("damian", "sinay", "dsinay@cowrysolutions.com", "+44 7968 809206",
                //  "BBcowrysoTest", "1st Floor The Blade Abbey Street", "Reading ", "RG1 3BE", "Berkshire", "United Kingdom", 3, 2);

                myCrayonAccess.SetupNewCustomer("damian", "sinay", "dsinay@cowrysolutions.com", "17074959909",
                    "BBcowrysoTest", "60 29th Street", "San Francisco ", "94110", "California", "United States", 3, 2);*/

                Guid ret = Guid.Empty;
                CrayonAccess myCrayonAccess = new CrayonAccess(contactData.CustomerCompanyCountry);
                myCrayonAccess.SetupNewCustomer(contactData.CustomerFirstName, contactData.CustomerLastName, contactData.CustomerEmailAddress, contactData.CustomerContactNumber,
                    contactData.CustomerCompanyName, contactData.CustomerCompanyAddress, contactData.CustomerCompanyCity, contactData.CustomerCompanyPostalCode, contactData.CustomerCompanyState,contactData.CustomerCompanyCountry, Int32.Parse(contactData.TotalEssentialUsers), Int32.Parse(contactData.TotalTeamUsers), "",false/*contactData.DomainName,contactData.Has_MicrosoftOffice365Account*/);
                return Request.CreateResponse(HttpStatusCode.OK, ret);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }
    }
}