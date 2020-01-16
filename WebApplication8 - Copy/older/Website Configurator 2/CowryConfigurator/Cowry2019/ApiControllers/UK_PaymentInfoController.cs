using Cowry2019.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cowry2019.integrate;


namespace Cowry2019.ApiControllers
{
    [RoutePrefix("Cowry")]
    public class UK_PaymentInfoController : ApiController
    {



        [HttpPost]
        [Route("Common/UpdatePaymentInfo")]
        public HttpResponseMessage CommonUpdateCredit(Payment payment)
        {
            try
            {
                bool validCardNumber = LuhnTest(payment.CreditCardNumber);
                if (validCardNumber == true)
                {
                    CrmAccess crmAccess = new CrmAccess();
                    string errorMessage = crmAccess.ProcessPayment(payment);
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Accepted, errorMessage);
                    }
                    else
                    {
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

        [HttpPost]
        [Route("Common/GenerateInvoice")]
        public string CommonGenerateInvoice(Payment payment)
        {
            string errorMessage = "";
            try
            {
                CrmAccess crmAccess = new CrmAccess();
                errorMessage = crmAccess.GenerateInvoice(payment,"");

                return errorMessage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return errorMessage = ex.ToString();
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
