using Microsoft.IdentityModel.Clients.ActiveDirectory;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace Cowry2019.integrate
{
    public class HydrationAPI
    {
        public string token { get; set; }
        public HydrationAPI()
        {
            string clientId = "2adf2543-7020-4af6-b7c7-6faaa5cb327c";
            string aadInstance = "https://login.microsoftonline.com/";            
            string resource = "https://api.businesscentral.dynamics.com";

            //string tenantID = "554a0ce1-6fe9-4425-baa3-5963dd66aa27"; // Remoting Coders
            string tenantID = "5fc4f8fd-0b3a-46b7-9249-ace042155b2c"; // cowrysoTest4c10895d


            //UserPasswordCredential userPasswordCredential = new UserPasswordCredential("mbancalari@remotingcoders.com", "Remoting2017!");

         //   UserPasswordCredential userPasswordCredential = new UserPasswordCredential("admin@BBcowrysoTest5d5d8e26.onmicrosoft.com", "Webfortis2019!");

            AuthenticationContext authenticationContext = new AuthenticationContext(aadInstance + tenantID);
            //AuthenticationResult authenticationResult = authenticationContext.AcquireTokenAsync(resource, clientId, userPasswordCredential).Result;

            //this.token = authenticationResult.AccessToken;
        }
        public Guid  CreateCompany(string companyName)
        {
            var request = new HttpRequestMessage( HttpMethod.Post, "https://api.businesscentral.dynamics.com/v1.0/api/microsoft/automation/v1.0/companies(ce5c46ea-843c-4171-8eae-30b369a78f8b)/automationCompanies");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://api.businesscentral.dynamics.com/v1.0/api/microsoft/automation") };

            var jsonContent = new StringContent("{ \"name\": \"" + companyName + "\", \"displayName\": \"" + companyName + "\", \"businessProfileId\": \"\"}", Encoding.UTF8, "application/json");
            request.Content = jsonContent;
            HttpResponseMessage response = _httpClient.SendAsync(request).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            int index = result.IndexOf(",\"id\":\"");
            if ( index > 0)
            {
                return new Guid(result.Substring(index + 7, 36));
            }
            else
            {
                return Guid.Empty;
            }

        }
        public  void uploadExtension(Guid companyID)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, "https://api.businesscentral.dynamics.com/v1.0/api/microsoft/automation/v1.0/companies(" + companyID.ToString().Replace("{", "").Replace("}", "") + ") /extensionUpload(0)/content");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://api.businesscentral.dynamics.com/v1.0/api/microsoft/automation") };

            
            //request.Content = jsonContent;
            HttpResponseMessage response = _httpClient.SendAsync(request).Result;
        }


    }
}