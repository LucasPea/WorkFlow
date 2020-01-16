using PayPal.Payments.Common.Utility;
using PayPal.Payments.Communication;
using PayPal.Payments.DataObjects;
using PayPal.Payments.Transactions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cowry2019.integrate
{
    public class PayPalAccess
    {
        private static Semaphore _sem;
        private static Semaphore _semPaypal;

        private string payFlowUser;
        private string payFlowVendor;
        private string payFlowPartner;
        private string payFlowPassword;
        private string payFlowHost;
        private int LogLevel = 0;
        private string LogPath = null;
        private Guid DefaultOwnerId = Guid.Empty;

        public PayPalAccess()
        {
            _sem = new Semaphore(1, 1, "AppStoreLogSemaphore");
            _semPaypal = new Semaphore(1, 1, "AppStoreSemaphore");

            payFlowUser = ConfigurationManager.AppSettings["PayFlowUser"];
            payFlowVendor = ConfigurationManager.AppSettings["PayFlowVendor"];
            payFlowPartner = ConfigurationManager.AppSettings["PayFlowPartner"];
            payFlowPassword = ConfigurationManager.AppSettings["PayFlowPassword"];
            payFlowHost = ConfigurationManager.AppSettings["PayFlowHost"];
        }

        /// <summary>
        /// This method executes the Paypal Transaction and it will returns the PNR number, if transaction is succeeded otherwise, it will returns the error message
        /// </summary>
        /// <param name="totalAmt"></param>
        /// <param name="creditCardExpirationDate"></param>
        /// <param name="creditCardNumber"></param>
        /// <param name="cCAuthCode"></param>
        /// <param name="payFlowUser"></param>
        /// <param name="payFlowVendor"></param>
        /// <param name="payFlowPartner"></param>
        /// <param name="payFlowPassword"></param>
        /// <returns></returns>
        public string ProcessSalesTransaction(string firstName, string lastName, string totalAmt, DateTime creditCardExpirationDate,
                string creditCardNumber, string cCAuthCode, string email, string phone, string address, string address2, string city, string zip, string state, string country, string origID)
        {
            Logger myLogger = new Logger();
            _semPaypal.WaitOne();
            string result = "";
            int hostPort = 443;
            string trRequest = BuildTransactionLog(firstName, lastName, totalAmt, creditCardExpirationDate, creditCardNumber, cCAuthCode, email, phone, address, address2, city, zip, state, country, origID, false);
            PayflowNETAPI payflowNETAPI = new PayflowNETAPI(payFlowHost, hostPort);
            myLogger.Log("ProcessSalesTransaction of " + firstName + " " + lastName, 2);

            String trxResponse = payflowNETAPI.SubmitTransaction(trRequest, PayflowUtility.RequestId);

            String TrxErrors = payflowNETAPI.TransactionContext.ToString();

            if (TrxErrors != null && TrxErrors.Length > 0)
            {
                result = PayflowUtility.GetStatus(trxResponse);
                myLogger.Log("Transaction TrxErrors " + TrxErrors, 0);
                myLogger.Log("Transaction result " + result, 0);
            }
            else
            {
                result = trxResponse;
                myLogger.Log("Transaction ok", 2);
            }
            _semPaypal.Release();
            return result;
        }

        public string ProcessSalesTransaction2(string firstName, string lastName, string totalAmt, DateTime creditCardExpirationDate,
                string creditCardNumber, string cCAuthCode, string invoiceId, string email, string phone, string address, string address2, string city, string zip, string state, 
                string country, string currency)
        {

            _semPaypal.WaitOne();

            Logger myLogger = new Logger();
            myLogger.Log("ProcessSalesTransaction2 Start", 0);
            TransactionResponse TrxnResponse;
            // Create PayFlow the Data Objects.
            // Create the User data object with the required user details.

            UserInfo User = new UserInfo(payFlowUser,
                                                payFlowVendor,
                                                payFlowPartner,
                                                payFlowPassword);

            // Create the Payflow  Connection data object with the required connection details.
            // The PAYFLOW_HOST and CERT_PATH are properties defined in App config file.
            PayflowConnectionData Connection = new PayflowConnectionData(payFlowHost);
            decimal TotalAmount = decimal.Zero;
            // Create a new Invoice data object with the Amount, Billing Address etc. details.
            Invoice Inv = new Invoice();
            // Set Amount.
            if (!string.IsNullOrEmpty(totalAmt))
            {
                TotalAmount = decimal.Parse(totalAmt);
            }
            Currency Amt = new Currency(decimal.Parse(TotalAmount.ToString("00.00")), currency );
            Inv.Amt = Amt;

            // Set the Billing Address details.
            BillTo Bill = new BillTo();
            Bill.FirstName = firstName;
            Bill.LastName = lastName;

            Bill.Email = email;
            Bill.Street = address;
            Bill.BillToStreet2 = address2;
            Bill.City = city;
            Bill.Zip = zip;
            Bill.State = state;
            Bill.BillToCountry = country;
            Bill.PhoneNum = phone;

            Inv.Comment1 = "Invoice #" + invoiceId;
            Inv.Comment2 = "CRM Pluggin";
            Inv.BillTo = Bill;
            // Create a new Payment Device - Credit Card data object.
            // The input parameters are Credit Card No. and Expiry Date for the Credit Card.
            int smallyear = creditCardExpirationDate.Year - 2000;

            CreditCard CC = new CreditCard(creditCardNumber, creditCardExpirationDate.Month.ToString("00") + smallyear.ToString("00"));
            CC.Cvv2 = cCAuthCode;

            string accountNo = creditCardNumber.Substring(0, 4) + "XXXXXXXX" + creditCardNumber.Substring(12);
            myLogger.Log("accountNo = " + accountNo, 0);
            myLogger.Log("creditCardExpirationDate = " + creditCardExpirationDate.ToShortDateString(), 0);
            myLogger.Log("Cvv2 = " + cCAuthCode, 0);
            myLogger.Log("FirstName = " + firstName, 0);
            myLogger.Log("LastName = " + lastName, 0);
            myLogger.Log("invoiceId = " + invoiceId, 0);

            // Create a new Tender - Card Tender data object.
            CardTender Card = new CardTender(CC);
            ///////////////////////////////////////////////////////////////////

            // Create a new Sale Transaction.
            SaleTransaction Trans = new SaleTransaction(User, Connection, Inv, Card, PayflowUtility.RequestId);
            //Get EventName
            // Submit the Transaction
            Response Resp = Trans.SubmitTransaction();

            // Display the transaction response parameters.
            if (Resp != null)
            {
                // Get the Transaction Response parameters.
                TrxnResponse = Resp.TransactionResponse;

                if (TrxnResponse != null && TrxnResponse.Result == 0)
                {
                    myLogger.Log("ok Pnref = : " + TrxnResponse.Pnref, 0);
                    _semPaypal.Release();
                    return TrxnResponse.Pnref;
                }
                else
                {
                    myLogger.Log("Error Pnref = : " + TrxnResponse.Pnref, 0);
                    myLogger.Log("Error in payment gateway: " + TrxnResponse.RespMsg, 0);
                    _semPaypal.Release();
                    throw new ApplicationException("Error in payment gateway: " + TrxnResponse.RespMsg);
                }
            }
            else
            {
                TrxnResponse = null;
                myLogger.Log("Error in payment gateway: There was an error in your request", 0);
                _semPaypal.Release();
                throw new ApplicationException("Error in payment gateway: There was an error in your request");
            }
        }
        /// <summary>
        /// Method is used to build the Transaction log
        /// </summary>
        /// <param name="totalAmt"></param>
        /// <param name="creditCardExpirationDate"></param>
        /// <param name="creditCardNumber"></param>
        /// <param name="cCAuthCode"></param>
        /// <param name="payFlowUser"></param>
        /// <param name="payFlowVendor"></param>
        /// <param name="payFlowPartner"></param>
        /// <param name="payFlowPassword"></param>
        /// <returns></returns>
        public string BuildTransactionLog(string firstName, string lastName, string totalAmt, DateTime creditCardExpirationDate,
            string creditCardNumber, string cCAuthCode, string email, string phone, string address, string address2, string city, string zip, string state, string country, string origID, bool hidePaypalPasword)
        {
            try
            {
                int smallyear = creditCardExpirationDate.Year - 2000;
                StringBuilder trxRequest = new StringBuilder();
                if (!String.IsNullOrEmpty(origID)) // reuse PNREF for Tokenization
                {
                    trxRequest.Append("TRXTYPE=S");
                    trxRequest.Append("&TENDER=C&AMT=");
                    trxRequest.Append(totalAmt);
                    trxRequest.Append("&ORIGID=");
                    trxRequest.Append(origID);
                }
                else
                {
                    trxRequest.Append("TRXTYPE=S&ACCT=");
                    trxRequest.Append(creditCardNumber);
                    trxRequest.Append("&EXPDATE=");
                    trxRequest.Append(creditCardExpirationDate.Month.ToString("00") + smallyear.ToString("00"));
                    trxRequest.Append("&TENDER=C&AMT=");
                    trxRequest.Append(totalAmt);
                    trxRequest.Append("&CVV2=");
                    trxRequest.Append(cCAuthCode);

                    if (firstName.Trim() != string.Empty)
                    {
                        trxRequest.Append("&FIRSTNAME=");
                        trxRequest.Append(firstName.Trim());
                    }

                    if (lastName.Trim() != string.Empty)
                    {
                        trxRequest.Append("&LASTNAME=");
                        trxRequest.Append(lastName.Trim());
                    }

                    trxRequest.Append("&STREET=");
                    trxRequest.Append(address);
                    trxRequest.Append("&CITY=");
                    trxRequest.Append(city);
                    trxRequest.Append("&STATE=");
                    trxRequest.Append(state);
                    trxRequest.Append("&ZIP=");
                    trxRequest.Append(zip);
                    trxRequest.Append("&EMAIL=");
                    trxRequest.Append(email);
                }

                trxRequest.Append("&USER=");
                trxRequest.Append(payFlowUser);
                trxRequest.Append("&VENDOR=");
                trxRequest.Append(payFlowVendor);
                trxRequest.Append("&PARTNER=");
                trxRequest.Append(payFlowPartner);
                trxRequest.Append("&PWD=");
                if (hidePaypalPasword)
                {
                    trxRequest.Append("xxxxxx");
                }
                else
                {
                    trxRequest.Append(payFlowPassword);
                }

                //trxRequest.Append("&VERBOSITY=LOW");

                return trxRequest.ToString();
            }
            catch (ArgumentOutOfRangeException argOutEx)
            {
                ApplicationException appEx = new ApplicationException(argOutEx.Message);
                appEx.Source = argOutEx.Source;
                throw appEx;
            }
        }

        /// <summary>
        /// Method used for executes the transaction for the refund for the given PNR number
        /// </summary>
        /// <param name="total"></param>
        /// <param name="paymentOrgID"></param>
        /// <param name="payFlowUser"></param>
        /// <param name="payFlowVendor"></param>
        /// <param name="payFlowPartner"></param>
        /// <param name="payFlowPassword"></param>
        /// <returns></returns>
        public string ProcessCreditTransactionWithOrgId(string total, string paymentOrgID)
        {
            _semPaypal.WaitOne();
            try
            {
                StringBuilder trxRequest = new StringBuilder();
                trxRequest.Append("TRXTYPE=C&TENDER=C&AMT=");
                trxRequest.Append(total);
                trxRequest.Append("&USER=");
                trxRequest.Append(payFlowUser);
                trxRequest.Append("&VENDOR=");
                trxRequest.Append(payFlowVendor);
                trxRequest.Append("&PARTNER=");
                trxRequest.Append(payFlowPartner);
                trxRequest.Append("&PWD=");
                trxRequest.Append(payFlowPassword);
                trxRequest.Append("&ORIGID=");
                trxRequest.Append(paymentOrgID);

                PayflowNETAPI payflowNETAPI = new PayflowNETAPI(payFlowHost);

                String trxResponse = payflowNETAPI.SubmitTransaction(trxRequest.ToString(), PayflowUtility.RequestId);

                String TrxErrors = payflowNETAPI.TransactionContext.ToString();
                if (TrxErrors != null && TrxErrors.Length > 0)
                {
                    return PayflowUtility.GetStatus(trxResponse);
                }
                else
                {
                    return trxResponse;
                }
            }
            catch (ArgumentOutOfRangeException argOutEx)
            {
                ApplicationException appEx = new ApplicationException(argOutEx.Message);
                appEx.Source = argOutEx.Source;
                throw appEx;
            }
            finally
            {
                _semPaypal.Release();
            }
        }
    }
}
