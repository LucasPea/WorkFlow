using Microsoft.SharePoint.Client;
using NextCowry.integrate;
using NextCowry.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Configuration;
using System.Web.Http;

namespace NextCowry.AppControllers
{
    [RoutePrefix("cowry")]
    public class PaymentInfoController : ApiController
    {
        #region Sharepoint Connection
        string webUrl = WebConfigurationManager.AppSettings["webUrl"];
        string userName = WebConfigurationManager.AppSettings["userName"];
        string password = WebConfigurationManager.AppSettings["password"];
        #endregion

        string SpId;

        [HttpPost]
        [Route("result/UpdatePaymentInfo")]
        public HttpResponseMessage UpdatePaymentInfo(Payment payment)
        {
            try
            {
                bool validCardNumber = LuhnTest(payment.CardNumber);
                if (validCardNumber == true)
                {
                    PayPalAccess myPayPalAccess = new PayPalAccess();
                    string pnref = "";
                    string sekureToken = "";
                    string sekureTokenId = "";

                    string totalAmt = payment.TotalAmount;

                    string trxId = string.Empty;

                    //trxId = myPayPalAccess.ProcessSalesTransaction2(firstName, lastName, totalAmt, dateCard, cardNumber, cvvNumber, "2", email, phone, address, address2, city, zip, state, country);
                    DateTime dateCard = new DateTime(int.Parse(payment.CardExpiryYear), int.Parse(payment.CardExpiryMonth), 1);

                    trxId = myPayPalAccess.ProcessSalesTransaction(payment.NameOnCard, "", totalAmt, dateCard, payment.CardNumber, payment.CardCVV, payment.Email, 
                                                        payment.phone, payment.address, "", payment.city, payment.zip, payment.state, payment.country, "");
                    //trxId = myPayPalAccess.ProcessSalesTransaction(payment.NameOnCard, "", totalAmt, dateCard, payment.CardNumber, payment.CardCVV, payment.Email, "", "", "", "", "", "", "", "");

                    string[] trxResponse = trxId.Split('&');

                    if (trxResponse[0].ToString() == "RESULT=0")
                    {
                        for (int i = 0; i < trxResponse.Length; i++)
                        {
                            string[] responseProperty = trxResponse[i].Split('=');
                            if (responseProperty[0].ToString().ToLower() == "PNREF".ToLower())
                            {
                                pnref = responseProperty[1].ToString();
                            }
                            else if (responseProperty[0].ToString().ToLower() == "SECURETOKEN".ToLower())
                            {
                                sekureToken = responseProperty[1].ToString();
                            }
                            else if (responseProperty[0].ToString().ToLower() == "SECURETOKENID".ToLower())
                            {
                                sekureTokenId = responseProperty[1].ToString();
                            }
                        }
                        CrayonAccess myCrayonAccess = new CrayonAccess();

                        myCrayonAccess.SetupNewCustomer(payment.NameOnCard, payment.NameOnCard, payment.Email, payment.phone, payment.NameOnCard, payment.address, 
                            payment.city, payment.zip, payment.state, payment.country);

                        //string accountNo = cardNumber.Substring(0, 4) + "XXXXXXXX" + cardNumber.Substring(12);
                        //transactionLog = myPayPalAccess.BuildTransactionLog(firstName, lastName, totalAmt, dateCard, accountNo, "XXX", email, phone, address, address2, city, zip, state, country, origID, true);
                    }
                    else
                    {
                        string errorMessage = "";
                        for (int ii = 0; ii < trxResponse.Length; ii++)
                        {
                            string[] responseProperty = trxResponse[ii].Split('=');
                            if (String.Compare(responseProperty[0], "RESPMSG", true) == 0)
                            {
                                errorMessage += "Error in payment gateway: " + responseProperty[1] + "\n";
                            }
                            else if (String.Compare(responseProperty[0], "PREFPSMSG", true) == 0)
                            {
                                errorMessage += " " + responseProperty[1] + "\n";
                            }
                            else if (responseProperty[0].ToString().ToLower() == "PNREF".ToLower())
                            {
                                pnref = responseProperty[1].ToString();
                            }                         
                        }
                        return Request.CreateErrorResponse(HttpStatusCode.Accepted, errorMessage);
                    }
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
                        SpId = payment.SpId;
                        CamlQuery camlQuery = new CamlQuery();
                        camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='SpId'/><Value Type='Text'>" + SpId + "</Value></Eq></Where></Query><RowLimit>100</RowLimit></View>";
                        ListItemCollection collListItem = list.GetItems(camlQuery);
                        context.Load(collListItem);
                        context.ExecuteQuery();
                        foreach (ListItem oListItem in collListItem)
                        {
                            oListItem["PaymentInfoCardType"] = payment.CardType;
                            oListItem["PaymentInfoNameOnCard"] = payment.NameOnCard;
                            oListItem["PaymentInfoCardNumber"] = payment.CardNumber.Substring(0, 4) + "XXXXXXXX" + payment.CardNumber.Substring(12);  ;
                            oListItem["PaymentInfoCardCVV"] = payment.CardCVV;
                            oListItem["PaymentInfoCardExpiryMonth"] = payment.CardExpiryMonth;
                            oListItem["PaymentInfoCardExpiryYear"] = payment.CardExpiryYear;
                            oListItem["PNREF"] = pnref;
                            ;

                            oListItem.Update();
                            context.ExecuteQuery();
                        }
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Accepted, "Invalid Card Number");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Request");
            }
        }

        public static bool LuhnTest(string cardNo)
        {
            int nDigits = cardNo.Length;
            int nSum = 0;
            bool isSecond = false;
            for (int i = nDigits - 1; i >= 0; i--)
            {

                int d = cardNo[i] - '0';

                if (isSecond == true)
                    d = d * 2;

                // We add two digits to handle 
                // cases that make two digits  
                // after doubling 
                nSum += d / 10;
                nSum += d % 10;

                isSecond = !isSecond;
            }
            return (nSum % 10 == 0);

        }
               
    }   
}
