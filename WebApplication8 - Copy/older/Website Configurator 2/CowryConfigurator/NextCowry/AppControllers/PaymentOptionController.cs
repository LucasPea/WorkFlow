using Microsoft.SharePoint.Client;
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
    public class PaymentOptionController : ApiController
    {
        #region Sharepoint Connection
        string webUrl = WebConfigurationManager.AppSettings["webUrl"];
        string userName = WebConfigurationManager.AppSettings["userName"];
        string password = WebConfigurationManager.AppSettings["password"];
        #endregion

        string SpId,currency, mont, month, year;
        decimal monthlyTotal, yearlyTotal;

        [HttpPost]
        [Route("result/GetPaymentOptions")]
        public PaymentOptions GetSupportDetails(Pay pay)
        {
            try
            {
                PaymentOptions paymentOptions = new PaymentOptions();
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

                    SpId = pay.SpId;
                    currency = pay.CurrencyType;

                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='SpId'/><Value Type='Text'>" + SpId + "</Value></Eq></Where></Query><RowLimit>100</RowLimit></View>";
                    ListItemCollection collListItem = list.GetItems(camlQuery);
                    context.Load(collListItem);
                    context.ExecuteQuery();

                    foreach (ListItem oListItem in collListItem)
                    {
                        if (oListItem["TotalPrice"] != null)
                        {
                            mont = oListItem["TotalPrice"].ToString();
                        }
                        var gst = "0.9";
                        var mon = mont.Split();
                        monthlyTotal = Math.Ceiling(decimal.Parse(mon[1]));
                        yearlyTotal = Math.Ceiling((decimal.Parse(gst) * (monthlyTotal * 12)));

                        month = monthlyTotal.ToString();
                        year = yearlyTotal.ToString();

                        if (currency == "USD")
                        {
                            paymentOptions.MonthlyPrice = "$ " + month;
                            paymentOptions.YearlyPrice = "$ " + year;
                        }
                        if (currency == "GBP")
                        {
                            paymentOptions.MonthlyPrice = "£ " + month;
                            paymentOptions.YearlyPrice = "£ " + year;
                        }

                    }
                    return paymentOptions;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("result/UpdatePaymentOptions")]
        public HttpResponseMessage UpdatePaymentOptions(PaymentOptionAns paymentOption)
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
                    SpId = paymentOption.SpId;
                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='SpId'/><Value Type='Text'>" + SpId + "</Value></Eq></Where></Query><RowLimit>100</RowLimit></View>";
                    ListItemCollection collListItem = list.GetItems(camlQuery);
                    context.Load(collListItem);
                    context.ExecuteQuery();
                    foreach (ListItem oListItem in collListItem)
                    {
                        oListItem["PaymentOption"] = paymentOption.PaymentOption;

                        oListItem["PaymentOptionPrice"] = paymentOption.PaymentOptionPrice;                       

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

    public class PaymentOptions
    {
        public string MonthlyPrice { get; set; }
        public string YearlyPrice { get; set; }
    }

    public class PaymentOptionAns
    {
        public string SpId { get; set; }
        public string Currency { get; set; }
        public string PaymentOption { get; set; }
        public string PaymentOptionPrice { get; set; }

    }

    public class Pay
    {
        public string SpId { get; set; }
        public string CurrencyType { get; set; }
    }


}
