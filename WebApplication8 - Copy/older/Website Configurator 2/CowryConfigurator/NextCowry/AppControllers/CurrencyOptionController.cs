using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Configuration;
using System.Web.Http;

namespace NextCowry.AppControllers
{
    [RoutePrefix("cowry")]
    public class CurrencyOptionController : ApiController
    {
        #region Sharepoint Connection
        string webUrl = WebConfigurationManager.AppSettings["webUrl"];
        string userName = WebConfigurationManager.AppSettings["userName"];
        string password = WebConfigurationManager.AppSettings["password"];
        #endregion

        string SpId;

        [HttpPost]
        [Route("result/AddCurrencyDetails")]
        public HttpResponseMessage AddResult(string CurrencyType)
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
                    Web web = context.Web;
                    context.Credentials = new SharePointOnlineCredentials(userName, sec_pass);
                    List list = context.Web.Lists.GetByTitle("CowryMainResult");
                    context.Load(list);
                    context.ExecuteQuery();

                    ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();

                    long ticks2 = DateTime.Now.Ticks;
                    byte[] bytes2 = BitConverter.GetBytes(ticks2);
                    SpId = Convert.ToBase64String(bytes2).Replace('+', '_').Replace('/', '-').TrimEnd('=');

                    ListItem oListItem = list.AddItem(itemCreateInfo);
                    oListItem["Title"] = Title;
                    oListItem["SpId"] = SpId;
                    oListItem["CurrencyType"] = CurrencyType;

                    oListItem.Update();
                    context.ExecuteQuery();
                    return Request.CreateResponse(HttpStatusCode.OK, SpId);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data Not found!");
                }
            }
        }
    }
}
