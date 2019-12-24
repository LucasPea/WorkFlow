using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

public class TokenClass
{
    static string clientId= ConfigurationManager.AppSettings["clientId"];
    static string secret = ConfigurationManager.AppSettings["secret"];
    static string apiId = ConfigurationManager.AppSettings["apiId"];

    public TokenClass()
    {
    }

    public string GetToken()
    {
        var client = new RestClient("https://login.microsoftonline.com/" + apiId + "/oauth2/token");
        var request = new RestRequest(Method.POST);
        request.AddHeader("cache-control", "no-cache");
        request.AddHeader("Connection", "keep-alive");
        request.AddHeader("Cookie", "x-ms-gateway-slice=prod");
        request.AddHeader("Content-Length", "174");
        request.AddHeader("Accept-Encoding", "gzip, deflate");
        request.AddHeader("Host", "login.microsoftonline.com");
        request.AddHeader("Cache-Control", "no-cache");
        request.AddHeader("Accept", "*/*");
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        request.AddParameter("undefined", "grant_type=client_credentials&client_id=" + clientId + "&client_secret=" + secret + "&resource=https%3A%2F%2Fmanagement.azure.com%2F", ParameterType.RequestBody);
        IRestResponse response = client.Execute(request);
        string res = response.Content.Replace("﻿", "");
        dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(res);
        return json.access_token;
    }
}