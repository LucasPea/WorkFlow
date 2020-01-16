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
    public class PriceDetailsController : ApiController
    {
        #region Sharepoint Connection
        string webUrl = WebConfigurationManager.AppSettings["webUrl"];
        string userName = WebConfigurationManager.AppSettings["userName"];
        string password = WebConfigurationManager.AppSettings["password"];
        #endregion

        [HttpGet]
        [Route("result/GetPriceDetailsPerCurrency")]
        public List<Product> GetPriceDetails(string currencyType)
        {          
            try
            {
                List<Product> productData = new List<Product>();
                SecureString sec_pass = new SecureString();
                Array.ForEach(password.ToArray(), sec_pass.AppendChar);
                sec_pass.MakeReadOnly();
                { 
                    using (var context = new ClientContext(webUrl))
                    {
                        Web web = context.Web;
                        context.Credentials = new SharePointOnlineCredentials(userName, sec_pass);

                        if(currencyType == "USD")
                        {
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
                                Product product = new Product();
                                foreach (var myfieldv in oListItem.FieldValues)
                                {

                                    if (myfieldv.Key.Equals("ProductId"))
                                    {
                                        product.ProductId = oListItem["ProductId"].ToString();
                                    }
                                    if (myfieldv.Key.Equals("ProductName"))
                                    {
                                        product.ProductName = oListItem["ProductName"].ToString();
                                    }
                                    if (myfieldv.Key.Equals("ProductPrice"))
                                    {
                                        product.ProductPrice = oListItem["ProductPrice"].ToString();
                                    }
                                }
                                productData.Add(product);
                            }
                        }
                        if(currencyType == "GBP")
                        {
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
                                Product product = new Product();
                                foreach (var myfieldv in oListItem.FieldValues)
                                {

                                    if (myfieldv.Key.Equals("ProductId"))
                                    {
                                        product.ProductId = oListItem["ProductId"].ToString();
                                    }
                                    if (myfieldv.Key.Equals("ProductName"))
                                    {
                                        product.ProductName = oListItem["ProductName"].ToString();
                                    }
                                    if (myfieldv.Key.Equals("ProductPrice"))
                                    {
                                        product.ProductPrice = oListItem["ProductPrice"].ToString();
                                    }
                                }
                                productData.Add(product);
                            }
                        }
                    }                               
                }
                return productData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }       

    }
}
