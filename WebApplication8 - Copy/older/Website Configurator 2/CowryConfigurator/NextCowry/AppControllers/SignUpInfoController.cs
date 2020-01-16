using Microsoft.SharePoint.Client;
using NextCowry.Models;
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
    public class SignUpInfoController : ApiController
    {
        #region Sharepoint Connection
        string webUrl = WebConfigurationManager.AppSettings["webUrl"];
        string userName = WebConfigurationManager.AppSettings["userName"];
        string password = WebConfigurationManager.AppSettings["password"];
        #endregion

        string spid;

        [HttpPost]
        [Route("result/UpdateSignUpInfo")]
        public HttpResponseMessage UpdatePersonalInfo(SignUpInfo signUpInfo)
        {
            try
            {
                SecureString sec_pass = new SecureString();
                Array.ForEach(password.ToArray(), sec_pass.AppendChar);
                sec_pass.MakeReadOnly();
                using (var context = new ClientContext(webUrl))
                {
                    Web web = context.Web;
                    context.Credentials = new SharePointOnlineCredentials(userName, sec_pass);
                    List list = context.Web.Lists.GetByTitle("CowryMainResult");
                    context.Load(list);
                    context.ExecuteQuery();
                    spid = signUpInfo.SpId;
                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='SpId'/><Value Type='Text'>" + spid + "</Value></Eq></Where></Query><RowLimit>100</RowLimit></View>";
                    ListItemCollection collListItem = list.GetItems(camlQuery);
                    context.Load(collListItem);
                    context.ExecuteQuery();
                    foreach (ListItem oListItem in collListItem)
                    {
                        oListItem["FirstName"] = signUpInfo.Firstname;

                        oListItem["LastName"] = signUpInfo.Lastname;

                        oListItem["JobTitle"] = signUpInfo.JobTitle;

                        oListItem["ContactNumber"] = signUpInfo.ContactNumber;

                        oListItem["Email"] = signUpInfo.Email;

                        oListItem["CompanyName"] = signUpInfo.CompanyName;

                        oListItem["CompanyContactNumber"] = signUpInfo.CompanyNumber;

                        oListItem["CompanyAddress"] = signUpInfo.CompanyAddress;

                        oListItem["CompanyCountry"] = signUpInfo.CompanyCountry;

                        oListItem["CompanyState"] = signUpInfo.CompanyState;

                        oListItem["CompanyCity"] = signUpInfo.CompanyCity;

                        oListItem["CompanyPostalCode"] = signUpInfo.CompanyPostalCode;

                        oListItem["MicrosoftOffice365Account"] = signUpInfo.MicrosoftOfficeAccChecked;

                        if(signUpInfo.DomainName != null) { 
                        oListItem["DomainName"] = signUpInfo.DomainName;
                        }
                        if(signUpInfo.FindDomain != null) { 
                        oListItem["HelpFindingDomain"] = signUpInfo.FindDomain;
                        }
                        oListItem["MicrosoftCloudAgreement"] = signUpInfo.MicrosoftCloudAgreement;

                        oListItem["SupportContract"] = signUpInfo.SupportContract;

                        oListItem.Update();
                        context.ExecuteQuery();
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data not found");
            }
        }
    }
}
