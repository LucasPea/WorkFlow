using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cowry2019.Models;
using Newtonsoft.Json;
using QuickType;
using Licenses;
using RestSharp;

namespace Cowry2019.integrate
{
    public class CSP
    {
        public CSP(){ }


        public TokenClass RefreshTokenTest(string clientid, string refreshtoken)
        {
            var client = new RestClient("https://login.microsoftonline.com/common/oauth2/v2.0/token");
            var request = new RestRequest(RestSharp.Method.POST);
            string parameter = "client_id=" + clientid + "&scope=https%3A%2F%2Fapi.partnercenter.microsoft.com%2Fuser_impersonation&redirect_uri=https%3A%2F%2Flocalhost%2Fauth&grant_type=refresh_token&refresh_token=" + refreshtoken;
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Cookie", "buid=AQABAAEAAACQN9QBRU3jT6bcBQLZNUj71y8HjuyDv4AqRhik75O1EI7V2YFg9mNRy4JWpK3EI5cOLl0EHGeh38qV4qNS9VoyqYfGMNdiNyuXqROHtdZW1JDCSIGWr-4fw5r5YiQSN0EgAA; x-ms-gateway-slice=prod; stsservicecookie=ests; fpc=Ai0Txoy2aShHiY0BBb7Iq3Z-huEiAQAAAJ3FVNUOAAAA");
            request.AddHeader("Content-Length", parameter.Length.ToString());
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "login.microsoftonline.com");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", parameter, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //NO BORRAR, ES UN CARACTER ESPECIAL
            string res = response.Content.Replace("﻿", "");
            var content = JsonConvert.DeserializeObject<TokenClass>(res);
            return content;
        }

        public Customers GetCustomerUsersTest(string token, string tenantID)
        {
            var client = new RestClient("https://api.partnercenter.microsoft.com/v1/customers/" + tenantID + "/users");
            var request = new RestRequest(RestSharp.Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "api.partnercenter.microsoft.com");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);
            //NO BORRAR, ES UN CARACTER ESPECIAL
            string res = response.Content.Replace("﻿", "");
            var customers = Customers.FromJson(res);
            return customers;
        }

        public bool VerifyCustomerDomain(string domain)
        {
            bool available = true;
            string clientID = "b16aa27a-684c-44e6-bae1-df2fdda5e981";
            string refreshToken = "OAQABAAAAAACQN9QBRU3jT6bcBQLZNUj7U8ee1ZJf9TNaBhrshbx4tCW9O_RbWrECn6CLIAVZKov0V-iHoKUvlzPG9UH-joDR1vWPSwG3UB16tnQZzvdM1JVEniSvyLIJxYRV_ZwBeFxGiq1VQBXewecdJLgka1HQYOOBzZcL-o1NDVFJhrF6CqN5cKZjsBygDZXPpR50uwU7Kicmy-QnfZV_0FOXrZoVEdTtvLSLg0ofmhT8_AfH40NoLB1w6GfDS5RNyXjXws6lUPPGPLki_NJ4kep0_xFxBTUdNqnFu3tLZXUhfhqRul5P976JnwbQiubHK2UWyUt9zwZh8LAUxQpAM-7AKixU1eamJNNLKCJASp4FySO7KSulouKI8C4ajWGHB1C1-5tskBPE_4Calinm9K_mU4P0xqCPQD_itmDekyRB-jDeHyM6xHANQt6UvXOvgemuW-O1aRXdHLd25hnVKA--dPpTIsc5EdSUEtGjn4_0H6TC15zw1m85K_WKdV-6MPBaNuurWDbhdMlXYiafFMyuzHVAc5K0JNmkyIQfrfgd8ZaiTK1NbGF-yAL3buPqfMbH8EdXBr3uWbYtx1wLnYqxQzNbbaFNonZELxCGYr89qtrHYFZWZtVpMBlBNgaKyYAV9OSrHzY4t6Ex3tNP-vHs_RNfksKmJc7kr5JB3C9qbdABogwigQD9hw_ShKye3HUBHU990B_pMUWwx4krUqiLDvndjzb8HoJ6JFL0pKA4ahSdj3Y6mWfnZKj3jFKuQHB2zTPtcB1AkPCn0WmYBjM8523QItuOm6eYOeJyyeZCAQb-tw2eWLt-qmbBtEtJ49HmXM4Tlk1YtND0Uio7_PiyooZufXQozVLXFPXt35aRp4u3a3-X0R5NUjNMBW_LHIaCB0kJYpao4BzOdOJorGrKjDv850HUlnMdK6W6l3ULtGpGlsu0EJdLWgIkIvYdBGnqo0v-oBHi_GNw2Hl55VcRpWToeWlq2860wpEs7hsIFnyWUA5ZZ_iL8xptoe7OXx6Al-MgAA";
            TokenClass token = RefreshTokenTest(clientID, refreshToken);

            var client = new RestClient("https://api.partnercenter.microsoft.com/v1/domains/" + domain);
            var request = new RestRequest(RestSharp.Method.HEAD);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "api.partnercenter.microsoft.com");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Bearer " + token.access_token);
            IRestResponse response = client.Execute(request);
            //NO BORRAR, ES UN CARACTER ESPECIAL
            string res = response.Content.Replace("﻿", "");
            if (response.StatusCode == System.Net.HttpStatusCode.OK) available = false;
            return available;
        }


        public License GetLicenseTest(string token)
        {
            var client = new RestClient("https://api.partnercenter.microsoft.com/partner/v1/analytics/commercial/deployment/license/");
            var request = new RestRequest(RestSharp.Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "api.partnercenter.microsoft.com");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);
            //NO BORRAR, ES UN CARACTER ESPECIAL
            string res = response.Content.Replace("﻿", "");
            var license = License.FromJson(res);
            return license;
        }

        public Customers ResetCustomerPasswordTest(string tenantID,string token,string userID)
        {
            var client = new RestClient("https://api.partnercenter.microsoft.com/v1/customers/" + tenantID + "/users/" + userID);
            var request = new RestRequest(RestSharp.Method.PATCH);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "api.partnercenter.microsoft.com");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddJsonBody(new { passwordProfile = new { password = "RemotingCoders123", forceChangePassword = false }, attributes = new { objectType = "CustomerUser" } });
            IRestResponse response = client.Execute(request);
            //NO BORRAR, ES UN CARACTER ESPECIAL
            string res = response.Content.Replace("﻿", "");
            var customer = Customers.FromJson(res);
            return customer;
        }

        public dynamic SetUsageLocation(string token, string tenantID, string userID, string usage)
        {
            var client = new RestClient("https://api.partnercenter.microsoft.com/v1/customers/" + tenantID + "/users/" + userID);
            var request = new RestRequest(RestSharp.Method.PATCH);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "api.partnercenter.microsoft.com");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddJsonBody(new { usageLocation = usage, attributes = new { objectType = "CustomerUser" } });
            IRestResponse response = client.Execute(request);
            //NO BORRAR, ES UN CARACTER ESPECIAL
            string res = response.Content.Replace("﻿", "");
            var customer = JsonConvert.DeserializeObject(res);
            return customer;
        }

        public dynamic SetLicenseCustomer(string token, string tenantID, string userID, string skuID)
        {
            var client = new RestClient("https://api.partnercenter.microsoft.com/v1/customers/" + tenantID + "/users/" + userID + "/licenseupdates");
            var request = new RestRequest(RestSharp.Method.POST);
            request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "api.partnercenter.microsoft.com");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddJsonBody("{ \"licensesToAssign\":[{ \"excludedPlans\": \"\", \"skuId\": \""+skuID+"\" }], \"licensesToRemove\": \"\", \"licenseWarnings\": \"\", \"attributes\": { \"objectType\": \"LicenseUpdate\" } }");
            IRestResponse response = client.Execute(request);
            //NO BORRAR, ES UN CARACTER ESPECIAL
            string res = response.Content.Replace("﻿", "");
            dynamic dynamic = JsonConvert.DeserializeObject(res);
            return dynamic;
        }

        public void SetCustomerLicenses(string tenantID, string usage,string skuID)
        {
            string clientID = "b16aa27a-684c-44e6-bae1-df2fdda5e981";
            string refreshToken = "OAQABAAAAAACQN9QBRU3jT6bcBQLZNUj7U8ee1ZJf9TNaBhrshbx4tCW9O_RbWrECn6CLIAVZKov0V-iHoKUvlzPG9UH-joDR1vWPSwG3UB16tnQZzvdM1JVEniSvyLIJxYRV_ZwBeFxGiq1VQBXewecdJLgka1HQYOOBzZcL-o1NDVFJhrF6CqN5cKZjsBygDZXPpR50uwU7Kicmy-QnfZV_0FOXrZoVEdTtvLSLg0ofmhT8_AfH40NoLB1w6GfDS5RNyXjXws6lUPPGPLki_NJ4kep0_xFxBTUdNqnFu3tLZXUhfhqRul5P976JnwbQiubHK2UWyUt9zwZh8LAUxQpAM-7AKixU1eamJNNLKCJASp4FySO7KSulouKI8C4ajWGHB1C1-5tskBPE_4Calinm9K_mU4P0xqCPQD_itmDekyRB-jDeHyM6xHANQt6UvXOvgemuW-O1aRXdHLd25hnVKA--dPpTIsc5EdSUEtGjn4_0H6TC15zw1m85K_WKdV-6MPBaNuurWDbhdMlXYiafFMyuzHVAc5K0JNmkyIQfrfgd8ZaiTK1NbGF-yAL3buPqfMbH8EdXBr3uWbYtx1wLnYqxQzNbbaFNonZELxCGYr89qtrHYFZWZtVpMBlBNgaKyYAV9OSrHzY4t6Ex3tNP-vHs_RNfksKmJc7kr5JB3C9qbdABogwigQD9hw_ShKye3HUBHU990B_pMUWwx4krUqiLDvndjzb8HoJ6JFL0pKA4ahSdj3Y6mWfnZKj3jFKuQHB2zTPtcB1AkPCn0WmYBjM8523QItuOm6eYOeJyyeZCAQb-tw2eWLt-qmbBtEtJ49HmXM4Tlk1YtND0Uio7_PiyooZufXQozVLXFPXt35aRp4u3a3-X0R5NUjNMBW_LHIaCB0kJYpao4BzOdOJorGrKjDv850HUlnMdK6W6l3ULtGpGlsu0EJdLWgIkIvYdBGnqo0v-oBHi_GNw2Hl55VcRpWToeWlq2860wpEs7hsIFnyWUA5ZZ_iL8xptoe7OXx6Al-MgAA";

            TokenClass token= RefreshTokenTest(clientID, refreshToken);
            Customers userID = GetCustomerUsersTest(token.access_token, tenantID);
            //ResetCustomerPasswordTest(tenantID, token.access_token, userID.Items[0].Id.ToString());
            SetUsageLocation(token.access_token,tenantID, userID.Items[0].Id.ToString(),usage);
            SetLicenseCustomer(token.access_token, tenantID, userID.Items[0].Id.ToString(), skuID);
        }
    }
}