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
    public class SupportOptionController : ApiController
    {
        #region Sharepoint Connection
        string webUrl = WebConfigurationManager.AppSettings["webUrl"];
        string userName = WebConfigurationManager.AppSettings["userName"];
        string password = WebConfigurationManager.AppSettings["password"];
        #endregion

        string spid,currency, totalUser, enhanceBase, enhanceUser, enhanceSupport, standardPrice, enhancedPrice;
        string usd = "$ ";
        string gbp = "£ ";

        [HttpPost]
        [Route("result/GetSupportDetails")]
        public string GetSupportDetails(EUser eUser)
        {
            try
            {
                List<Product> finalProductDataUSD = getProductDataUSD();
                List<Product> finalProductDataGBP = getProductDataGBP();
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

                    spid = eUser.SpId;
                    currency = eUser.CurrencyType;

                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='SpId'/><Value Type='Text'>" + spid + "</Value></Eq></Where></Query><RowLimit>100</RowLimit></View>";
                    ListItemCollection collListItem = list.GetItems(camlQuery);
                    context.Load(collListItem);
                    context.ExecuteQuery();

                    foreach (ListItem oListItem in collListItem)
                    {
                        if(currency == "USD")
                        {
                            totalUser = oListItem["TotalUsers"].ToString();
                            enhanceBase = finalProductDataUSD[4].ProductPrice;
                            enhanceUser = finalProductDataUSD[5].ProductPrice;

                            enhanceSupport = usd + (int.Parse(enhanceBase) + (int.Parse(totalUser) * int.Parse(enhanceUser))).ToString();
                        }
                        if(currency == "GBP")
                        {
                            totalUser = oListItem["TotalUsers"].ToString();
                            enhanceBase = finalProductDataGBP[4].ProductPrice;
                            enhanceUser = finalProductDataGBP[5].ProductPrice;

                            enhanceSupport = gbp + (int.Parse(enhanceBase) + (int.Parse(totalUser) * int.Parse(enhanceUser))).ToString();
                        }
                    }
                    return enhanceSupport;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost]
        [Route("result/UpdateSupportData")]
        public HttpResponseMessage UpdateResult(Support support)
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
                    spid = support.SpId;
                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='SpId'/><Value Type='Text'>" + spid + "</Value></Eq></Where></Query><RowLimit>100</RowLimit></View>";
                    ListItemCollection collListItem = list.GetItems(camlQuery);
                    context.Load(collListItem);
                    context.ExecuteQuery();
                    foreach (ListItem oListItem in collListItem)
                    {
                        if(support.CurrencyType == "USD")
                        {
                            if (support.StandardPrice != null)
                            {
                                oListItem["StandardSupportPrice"] = support.StandardPrice;
                                standardPrice = support.StandardPrice;
                            }
                            else
                            {
                                oListItem["StandardSupportPrice"] = "$ 0";
                                standardPrice = "0";
                            }

                            if (support.EnhancedAns != null)
                            {
                                oListItem["EnhancedSupportAns"] = support.EnhancedAns;
                            }
                            else
                            {
                                oListItem["EnhancedSupportAns"] = "False";
                            }

                            if (support.EnhancedAns != "False")
                            {
                                if (support.EnhancePrice != null)
                                {
                                    oListItem["EnhancedSupportPrice"] = support.EnhancePrice;
                                    enhancedPrice = support.EnhancePrice;
                                }
                                else
                                {
                                    oListItem["EnhancedSupportPrice"] = "$ 0";
                                    enhancedPrice = "$ 0";
                                }
                            }
                            else
                            {
                                oListItem["EnhancedSupportPrice"] = "$ 0";
                                enhancedPrice = "$ 0";
                            }

                            var stan = standardPrice.Split();
                            var enhan = enhancedPrice.Split();

                            string totPrice = oListItem["TotalPrice"].ToString();
                            var tot = totPrice.Split();

                            var finalPrice = (int.Parse(stan[1]) + int.Parse(enhan[1]) + int.Parse(tot[1])).ToString();

                            oListItem["TotalPrice"] = "$ " + finalPrice;
                        }
                        if(support.CurrencyType == "GBP")
                        {
                            if (support.StandardPrice != null)
                            {
                                oListItem["StandardSupportPrice"] = support.StandardPrice;
                                standardPrice = support.StandardPrice;
                            }
                            else
                            {
                                oListItem["StandardSupportPrice"] = "$ 0";
                                standardPrice = "0";
                            }

                            if (support.EnhancedAns != null)
                            {
                                oListItem["EnhancedSupportAns"] = support.EnhancedAns;
                            }
                            else
                            {
                                oListItem["EnhancedSupportAns"] = "False";
                            }

                            if (support.EnhancedAns != "False")
                            {
                                if (support.EnhancePrice != null)
                                {
                                    oListItem["EnhancedSupportPrice"] = support.EnhancePrice;
                                    enhancedPrice = support.EnhancePrice;
                                }
                                else
                                {
                                    oListItem["EnhancedSupportPrice"] = "$ 0";
                                    enhancedPrice = "$ 0";
                                }
                            }
                            else
                            {
                                oListItem["EnhancedSupportPrice"] = "$ 0";
                                enhancedPrice = "$ 0";
                            }

                            var stan = standardPrice.Split();
                            var enhan = enhancedPrice.Split();

                            string totPrice = oListItem["TotalPrice"].ToString();
                            var tot = totPrice.Split();

                            var finalPrice = (decimal.Parse(stan[1]) + decimal.Parse(enhan[1]) + decimal.Parse(tot[1])).ToString();

                            oListItem["TotalPrice"] = "£ " + finalPrice;
                        }
                        



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

        public class EUser
        {
            public string SpId { get; set; }
            public string CurrencyType { get; set; }
        }
    }
}
