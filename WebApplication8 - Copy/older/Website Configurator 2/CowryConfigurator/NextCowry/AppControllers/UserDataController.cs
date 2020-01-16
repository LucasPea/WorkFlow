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
    public class UserDataController : ApiController
    {
        #region Sharepoint Connection
        string webUrl = WebConfigurationManager.AppSettings["webUrl"];
        string userName = WebConfigurationManager.AppSettings["userName"];
        string password = WebConfigurationManager.AppSettings["password"];
        #endregion

        string spid, currency, totalUser, enhanceBase, enhanceUser, enhanceSupport, standardPrice, enhancedPrice;

        [HttpPost]
        [Route("result/UpdateUserData")]
        public HttpResponseMessage UpdateUserData(UserData userData)
        {
            try
            {
                var tot = 0;
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
                    spid = userData.SpId;
                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='SpId'/><Value Type='Text'>" + spid + "</Value></Eq></Where></Query><RowLimit>100</RowLimit></View>";
                    ListItemCollection collListItem = list.GetItems(camlQuery);
                    context.Load(collListItem);
                    context.ExecuteQuery();
                    foreach (ListItem oListItem in collListItem)
                    {

                        #region Pricing Options Data
                        //Premium Users
                        if (userData.PremiumUsers != null)
                        {
                            oListItem["PremiumUsers"] = userData.PremiumUsers;
                        }
                        else
                        {
                            oListItem["PremiumUsers"] = "0";
                        }

                        //Essential Users
                        if (userData.EssentialUsers != null)
                        {
                            oListItem["EssentialUsers"] = userData.EssentialUsers;
                        }
                        else
                        {
                            oListItem["EssentialUsers"] = "0";
                        }

                        //Team Users
                        if (userData.TeamUsers != null)
                        {
                            oListItem["TeamUsers"] = userData.TeamUsers;
                        }
                        else
                        {
                            oListItem["TeamUsers"] = "0";
                        }

                        oListItem["TotalUsers"] = (int.Parse(userData.PremiumUsers) + int.Parse(userData.EssentialUsers) + int.Parse(userData.TeamUsers)).ToString();
                        #endregion

                        #region Support Option Data
                        if (userData.Currency == "USD")
                        {
                            if (userData.StandardSupportPrice != null)
                            {
                                oListItem["StandardSupportPrice"] = userData.StandardSupportPrice;
                                standardPrice = userData.StandardSupportPrice;
                            }
                            else
                            {
                                oListItem["StandardSupportPrice"] = "$ 0";
                                standardPrice = "0";
                            }

                            if (userData.EnhancedSupportChecked != null)
                            {
                                oListItem["EnhancedSupportChecked"] = userData.EnhancedSupportChecked;
                            }
                            else
                            {
                                oListItem["EnhancedSupportChecked"] = "False";
                            }

                            if (userData.EnhancedSupportChecked != "False")
                            {
                                if (userData.EnhancedSupportPrice != null)
                                {
                                    oListItem["EnhancedSupportPrice"] = userData.EnhancedSupportPrice;
                                    enhancedPrice = userData.EnhancedSupportPrice;
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

                        }
                        if (userData.Currency == "GBP")
                        {
                            if (userData.StandardSupportPrice != null)
                            {
                                oListItem["StandardSupportPrice"] = userData.StandardSupportPrice;
                                standardPrice = userData.StandardSupportPrice;
                            }
                            else
                            {
                                oListItem["StandardSupportPrice"] = "£ 0";
                                standardPrice = "0";
                            }

                            if (userData.EnhancedSupportChecked != null)
                            {
                                oListItem["EnhancedSupportChecked"] = userData.EnhancedSupportChecked;
                            }
                            else
                            {
                                oListItem["EnhancedSupportChecked"] = "False";
                            }

                            if (userData.EnhancedSupportChecked != "False")
                            {
                                if (userData.EnhancedSupportPrice != null)
                                {
                                    oListItem["EnhancedSupportPrice"] = userData.EnhancedSupportPrice;
                                    enhancedPrice = userData.EnhancedSupportPrice;
                                }
                                else
                                {
                                    oListItem["EnhancedSupportPrice"] = "£ 0";
                                    enhancedPrice = "£ 0";
                                }
                            }
                            else
                            {
                                oListItem["EnhancedSupportPrice"] = "£ 0";
                                enhancedPrice = "£ 0";
                            }

                        }
                        #endregion

                        #region Payment Option Data
                        oListItem["PaymentOptionType"] = userData.PaymentOptionAns;

                        oListItem["PaymentOptionTotalMonthlyPrice"] = userData.PaymentOptionMonthlyPrice;

                        oListItem["PaymentOptionTotalYearlyPrice"] = userData.PaymentOptionYearlyPrice;
                        #endregion

                        #region Personal Info
                        oListItem["FirstName"] = userData.FirstName;

                        oListItem["LastName"] = userData.LastName;

                        oListItem["JobTitle"] = userData.JobTitle;

                        oListItem["ContactNumber"] = userData.ContactNumber;

                        oListItem["Email"] = userData.Email;
                        #endregion

                        #region Company Info
                        oListItem["CompanyName"] = userData.CompanyName;

                        oListItem["CompanyNumber"] = userData.CompanyNumber;

                        oListItem["CompanyAddress"] = userData.CompanyAddress;

                        oListItem["CompanyCountry"] = userData.CompanyCountry;

                        oListItem["CompanyState"] = userData.CompanyState;

                        oListItem["CompanyCity"] = userData.CompanyCity;

                        oListItem["CompanyPostalCode"] = userData.CompanyPostalCode;
                        #endregion

                        #region Microsoft & Office 365 Account Info
                        oListItem["MicrosoftOfficeAccChecked"] = userData.MicrosoftOfficeAccChecked;

                        if (userData.DomainName != null)
                        {
                            oListItem["DomainName"] = userData.DomainName;
                        }

                        if (userData.FindDomain != null)
                        {
                            oListItem["FindDomain"] = userData.FindDomain;
                        }
                        #endregion

                        #region Microsoft Cloud Agreement
                        oListItem["MicrosoftCloudAgreement"] = userData.MicrosoftCloudAgreement;

                        oListItem["SupportContract"] = userData.SupportContract;
                        #endregion

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
