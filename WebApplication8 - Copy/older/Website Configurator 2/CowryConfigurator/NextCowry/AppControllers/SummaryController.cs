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
    public class SummaryController : ApiController
    {
        #region Sharepoint Connection
        string webUrl = WebConfigurationManager.AppSettings["webUrl"];
        string userName = WebConfigurationManager.AppSettings["userName"];
        string password = WebConfigurationManager.AppSettings["password"];
        #endregion

        string spid,currency, premiumPrice, essentialPrice, teamPrice, totalPremium, totalEssential, totalTeam, stanSupport, enhanceSupport, totalSupport, finalTotal;

        [HttpPost]
        [Route("result/GetSummaryDetails")]
        public SummaryData GetSummaryDetails(Sum sum)
        {
            try
            {
                
                List<Product> productDataUSD = getProductDataUSD();
                List<Product> productDataGBP = getProductDataGBP();
                SummaryData sumData = new SummaryData();
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

                    spid = sum.SpId;
                    currency = sum.CurrencyType;

                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='SpId'/><Value Type='Text'>" + spid + "</Value></Eq></Where></Query><RowLimit>100</RowLimit></View>";
                    ListItemCollection collListItem = list.GetItems(camlQuery);
                    context.Load(collListItem);
                    context.ExecuteQuery();

                    foreach (ListItem oListItem in collListItem)
                    {                        

                        if (oListItem["PremiumUsers"] != null)
                        {
                            sumData.PremiumUsers = oListItem["PremiumUsers"].ToString();
                        }
                        if (oListItem["EssentialUsers"] != null)
                        {
                            sumData.EssentialUsers = oListItem["EssentialUsers"].ToString();
                        }
                        if (oListItem["TeamUsers"] != null)
                        {
                            sumData.TeamUsers = oListItem["TeamUsers"].ToString();
                        }
                        if (oListItem["TotalUsers"] != null)
                        {
                            sumData.TotalUsers = oListItem["TotalUsers"].ToString();
                        }

                        if(currency == "USD")
                        {
                            premiumPrice = productDataUSD[0].ProductPrice;
                            essentialPrice = productDataUSD[1].ProductPrice;
                            teamPrice = productDataUSD[2].ProductPrice;

                            totalPremium = (int.Parse(premiumPrice) * int.Parse(sumData.PremiumUsers)).ToString();
                            totalEssential = (int.Parse(essentialPrice) * int.Parse(sumData.EssentialUsers)).ToString();
                            totalTeam = (int.Parse(teamPrice) * int.Parse(sumData.TeamUsers)).ToString();

                            if (oListItem["StandardSupportPrice"] != null)
                            {
                                stanSupport = oListItem["StandardSupportPrice"].ToString();
                            }
                            if (oListItem["EnhancedSupportPrice"] != null)
                            {
                                enhanceSupport = oListItem["EnhancedSupportPrice"].ToString();
                            }

                            var stan = stanSupport.Split();
                            var enhan = enhanceSupport.Split();

                            string standard = stan[1].ToString();
                            string enhanced = enhan[1].ToString();

                            totalSupport = "$ " + (int.Parse(standard) + int.Parse(enhanced)).ToString();

                            if (oListItem["TotalPrice"] != null)
                            {
                                finalTotal = oListItem["TotalPrice"].ToString();
                            }
                            sumData.PremiumPrice = "$ " + premiumPrice;
                            sumData.EssentialPrice = "$ " + essentialPrice;
                            sumData.TeamPrice = "$ " + teamPrice;

                            sumData.TotalPremiumPrice = "$ " + totalPremium;
                            sumData.TotalEssentialPrice = "$ " + totalEssential;
                            sumData.TotalTeamPrice = "$ " + totalTeam;

                            sumData.TotalSupportPrice = totalSupport;
                            sumData.TotalPrice = finalTotal;
                        }
                        if (currency == "GBP")
                        {
                            premiumPrice = productDataGBP[0].ProductPrice;
                            essentialPrice = productDataGBP[1].ProductPrice;
                            teamPrice = productDataGBP[2].ProductPrice;

                            totalPremium = (decimal.Parse(premiumPrice) * int.Parse(sumData.PremiumUsers)).ToString();
                            totalEssential = (decimal.Parse(essentialPrice) * int.Parse(sumData.EssentialUsers)).ToString();
                            totalTeam = (decimal.Parse(teamPrice) * int.Parse(sumData.TeamUsers)).ToString();

                            if (oListItem["StandardSupportPrice"] != null)
                            {
                                stanSupport = oListItem["StandardSupportPrice"].ToString();
                            }
                            if (oListItem["EnhancedSupportPrice"] != null)
                            {
                                enhanceSupport = oListItem["EnhancedSupportPrice"].ToString();
                            }

                            var stan = stanSupport.Split();
                            var enhan = enhanceSupport.Split();

                            string standard = stan[1].ToString();
                            string enhanced = enhan[1].ToString();

                            totalSupport = "£ " + (int.Parse(standard) + int.Parse(enhanced)).ToString();

                            if (oListItem["TotalPrice"] != null)
                            {
                                finalTotal = oListItem["TotalPrice"].ToString();
                            }
                            sumData.PremiumPrice = "£ " + premiumPrice;
                            sumData.EssentialPrice = "£ " + essentialPrice;
                            sumData.TeamPrice = "£ " + teamPrice;

                            sumData.TotalPremiumPrice = "£ " + totalPremium;
                            sumData.TotalEssentialPrice = "£ " + totalEssential;
                            sumData.TotalTeamPrice = "£ " + totalTeam;

                            sumData.TotalSupportPrice = totalSupport;
                            sumData.TotalPrice = finalTotal;
                        }

                    }
                    return sumData;
                }               
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Getting data from USD Pricing table.
        public List<Product> getProductDataUSD()
        {
            try
            {
                List<Product> productDataUSD = new List<Product>();
                SecureString sec_pass = new SecureString();
                Array.ForEach(password.ToArray(), sec_pass.AppendChar);
                sec_pass.MakeReadOnly();
                using (var context = new ClientContext(webUrl))
                {
                    Web web = context.Web;
                    context.Credentials = new SharePointOnlineCredentials(userName, sec_pass);
                    List list = context.Web.Lists.GetByTitle("USDPricing");
                    context.Load(list);
                    context.ExecuteQuery();

                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query>< OrderBy >< FieldRef Name = 'ID' /></ OrderBy ></ Query >< RowLimit>1000</RowLimit></View>";

                    ListItemCollection collListItem = list.GetItems(camlQuery);
                    context.Load(collListItem);
                    context.ExecuteQuery();
                    int count = collListItem.Count;

                    foreach (ListItem oListItem in collListItem)
                    {
                        Product productUSD = new Product();
                        foreach (var myfieldv in oListItem.FieldValues)
                        {
                            if (myfieldv.Key.Equals("ProductId"))
                            {
                                productUSD.ProductId = oListItem["ProductId"].ToString();
                            }
                            if (myfieldv.Key.Equals("ProductName"))
                            {
                                productUSD.ProductName = oListItem["ProductName"].ToString();
                            }
                            if (myfieldv.Key.Equals("ProductPrice"))
                            {
                                productUSD.ProductPrice = oListItem["ProductPrice"].ToString();
                            }
                        }
                        productDataUSD.Add(productUSD);
                    }
                    return productDataUSD;
                }
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        //Getting data from USD Pricing table.
        public List<Product> getProductDataGBP()
        {
            try
            {
                List<Product> productDataGBP = new List<Product>();
                SecureString sec_pass = new SecureString();
                Array.ForEach(password.ToArray(), sec_pass.AppendChar);
                sec_pass.MakeReadOnly();
                using (var context = new ClientContext(webUrl))
                {
                    Web web = context.Web;
                    context.Credentials = new SharePointOnlineCredentials(userName, sec_pass);
                    List list = context.Web.Lists.GetByTitle("GBPPricing");
                    context.Load(list);
                    context.ExecuteQuery();

                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query>< OrderBy >< FieldRef Name = 'ID' /></ OrderBy ></ Query >< RowLimit>1000</RowLimit></View>";

                    ListItemCollection collListItem = list.GetItems(camlQuery);
                    context.Load(collListItem);
                    context.ExecuteQuery();
                    int count = collListItem.Count;

                    foreach (ListItem oListItem in collListItem)
                    {
                        Product productGBP = new Product();
                        foreach (var myfieldv in oListItem.FieldValues)
                        {
                            if (myfieldv.Key.Equals("ProductId"))
                            {
                                productGBP.ProductId = oListItem["ProductId"].ToString();
                            }
                            if (myfieldv.Key.Equals("ProductName"))
                            {
                                productGBP.ProductName = oListItem["ProductName"].ToString();
                            }
                            if (myfieldv.Key.Equals("ProductPrice"))
                            {
                                productGBP.ProductPrice = oListItem["ProductPrice"].ToString();
                            }
                        }
                        productDataGBP.Add(productGBP);
                    }
                    return productDataGBP;
                }
            }
            catch (Exception ex)
            {
                return null;
            }


        }

    }
    public class SummaryData
    {
        public string PremiumUsers { get; set; }
        public string EssentialUsers { get; set; }
        public string TeamUsers { get; set; }
        public string TotalUsers { get; set; }

        public string PremiumPrice { get; set; }
        public string EssentialPrice { get; set; }
        public string TeamPrice { get; set; }

        public string TotalPremiumPrice { get; set; }
        public string TotalEssentialPrice { get; set; }
        public string TotalTeamPrice { get; set; }

        public string TotalSupportPrice { get; set; }
        public string TotalPrice { get; set; }
    }

    public class Sum
    {
        public string SpId { get; set; }
        public string CurrencyType { get; set; }

    }


    
}
