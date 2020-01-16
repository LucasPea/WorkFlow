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
    public class PricingOptionController : ApiController
    {
        #region Sharepoint Connection
        string webUrl = WebConfigurationManager.AppSettings["webUrl"];
        string userName = WebConfigurationManager.AppSettings["userName"];
        string password = WebConfigurationManager.AppSettings["password"];
        #endregion

        string spid;
        string usd = "$ ";
        string gbp = "£ ";

        [HttpPost]
        [Route("result/UpdatePricingOptions")]
        public HttpResponseMessage UpdateResult(Pricing pricingData)
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
                    spid = pricingData.SpId;
                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='SpId'/><Value Type='Text'>" + spid + "</Value></Eq></Where></Query><RowLimit>100</RowLimit></View>";
                    ListItemCollection collListItem = list.GetItems(camlQuery);
                    context.Load(collListItem);
                    context.ExecuteQuery();
                    foreach (ListItem oListItem in collListItem)
                    {
                        //Premium Users
                        if (pricingData.PremiumUsersCount != null)
                        {
                            oListItem["PremiumUsers"] = pricingData.PremiumUsersCount;
                        }
                        else
                        {
                            oListItem["PremiumUsers"] = "0";
                        }

                        //Essential Users
                        if (pricingData.EssentialUsersCount != null)
                        {
                            oListItem["EssentialUsers"] = pricingData.EssentialUsersCount;
                        }
                        else
                        {
                            oListItem["EssentialUsers"] = "0";
                        }

                        //Team Users
                        if (pricingData.TeamUsersCount != null)
                        {
                            oListItem["TeamUsers"] = pricingData.TeamUsersCount;
                        }
                        else
                        {
                            oListItem["TeamUsers"] = "0";
                        }

                        oListItem["TotalUsers"] = (int.Parse(pricingData.PremiumUsersCount) + int.Parse(pricingData.EssentialUsersCount) + int.Parse(pricingData.TeamUsersCount)).ToString();

                        if(pricingData.CurrencyType == "USD")
                        { 
                        //SubTotal1 Price
                        if (pricingData.SubTotal1 != null)
                        {
                            oListItem["SubTotal1Price"] = "$ " + pricingData.SubTotal1;
                        }
                        else
                        {
                            oListItem["SubTotal1Price"] = "$ 0";
                        }

                        //SubTotal2 Price
                        if (pricingData.SubTotal2 != null)
                        {
                            oListItem["SubTotal2Price"] = "$ " + pricingData.SubTotal2;
                        }
                        else
                        {
                            oListItem["SubTotal2Price"] = "$ 0";
                        }

                        oListItem["TotalPrice"] = "$ " + (int.Parse(pricingData.SubTotal1) + int.Parse(pricingData.SubTotal2)).ToString();
                        }
                        if(pricingData.CurrencyType == "GBP")
                        {
                            //SubTotal1 Price
                            if (pricingData.SubTotal1 != null)
                            {
                                oListItem["SubTotal1Price"] = "£ " + pricingData.SubTotal1;
                            }
                            else
                            {
                                oListItem["SubTotal1Price"] = "£ 0";
                            }

                            //SubTotal2 Price
                            if (pricingData.SubTotal2 != null)
                            {
                                oListItem["SubTotal2Price"] = "£ " + pricingData.SubTotal2;
                            }
                            else
                            {
                                oListItem["SubTotal2Price"] = "£ 0";
                            }

                            oListItem["TotalPrice"] = "£ " + (decimal.Parse(pricingData.SubTotal1) + decimal.Parse(pricingData.SubTotal2)).ToString();
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
    }
}
