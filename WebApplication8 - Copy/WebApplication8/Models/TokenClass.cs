using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WebApplication8.Models;

public class TokenClass
{
    public static string ClientId { get; } = ConfigurationManager.AppSettings["clientId"];

    public static string ApiId { get; } = ConfigurationManager.AppSettings["apiId"];

    public static string Secret { get; } = ConfigurationManager.AppSettings["secret"];

    public TokenClass()
    {
    }

    public string GetToken()
    {
        var client = new RestClient("https://login.microsoftonline.com/" + ApiId + "/oauth2/token");
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
        request.AddParameter("undefined", "grant_type=client_credentials&client_id=" + ClientId + "&client_secret=" + Secret + "&resource=https%3A%2F%2Fmanagement.azure.com%2F", ParameterType.RequestBody);
        IRestResponse response = client.Execute(request);
        string res = response.Content.Replace("﻿", "");
        dynamic json = JsonConvert.DeserializeObject(res);
        return json.access_token;
    }

    public string GetSubscription(string token)
    {
        var client = new RestClient("https://management.azure.com/subscriptions?api-version=2019-06-01");
        var request = new RestRequest(Method.GET);
        request.AddHeader("Authorization", "Bearer " + token); 
        IRestResponse response = client.Execute(request);
        string res = response.Content.Replace("﻿", "");
        var json = JsonConvert.DeserializeObject<Subcription>(res);
        return json.Value[0].SubscriptionId.ToString();
    }
}