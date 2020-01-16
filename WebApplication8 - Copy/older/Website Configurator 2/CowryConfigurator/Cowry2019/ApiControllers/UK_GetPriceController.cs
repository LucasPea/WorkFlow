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
using Cowry2019.integrate;

namespace Cowry2019.ApiControllers
{
    [RoutePrefix("Cowry")]
    public class UK_GetPriceController : ApiController
    {
        [HttpGet]
        [Route("Common/GetCRMPrice")]
        public List<Product> GetCRMPrice(string currencyType)
        {
            try
            {
                 /*CrayonAccess myCrayonAccess = new CrayonAccess("United Kingdom");
                  myCrayonAccess.GetAgreements();
                myCrayonAccess.SetupNewCustomer("Lucas", "Pena", "lpena@remotingcoders.com", "17074959909",
                      "RemotingCodersTest", "1st Floor The Blade Abbey Street", "Reading ", "RG1 3BE", "Berkshire", "United Kingdom", 1, 0, "");*/
                CrmAccess myAccess = new CrmAccess();
                List<Product> productData = myAccess.GetProducts(currencyType);
                return productData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
