using Microsoft.Xrm.Sdk.WebServiceClient;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;

using System.Collections.Generic;

using System.Web;
using System.Configuration;
using Cowry2019.Models;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Net;
using System.IO;
using System.Net.Http;


namespace Cowry2019.integrate
{
    public class CrmAccess
    {
        private CrmServiceClient cli1;
        public CrmAccess()
        {
            string ConnectionStr = ConfigurationManager.ConnectionStrings["CRM"].ConnectionString;
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            cli1 = new CrmServiceClient(ConnectionStr);
            if (cli1.IsReady)
            {
                //GenerateReport();
                HttpContext.Current.Trace.Write("connected");
            }
            else
            {
                HttpContext.Current.Trace.Warn("not connected = " + cli1.LastCrmError);
            }

        }
        public Guid CreateNewCustomer()
        {
            Guid ret = Guid.Empty;
            Entity myCustomer = new Entity("account");
            myCustomer.Attributes.Add("name", "test customer from configurator");
            ret = cli1.Create(myCustomer);
            return ret;
        }
        public List<Product> GetProducts(string currencyType)
        {
            string priceLevelID = "{A4511096-07BA-E911-A82D-000D3A0B617E}";
            if (currencyType == "USD")
            {
                priceLevelID = "{83ee4ea2-07ba-e911-a82d-000d3a0b617e}";
            }
            List<Product> productData = new List<Product>();
            string FetchXml = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                  <entity name=""productpricelevel"">
                                    <attribute name=""productid"" />
                                    <attribute name=""uomid"" />
                                    
                                    <attribute name=""pricingmethodcode"" />
                                    <attribute name=""amount"" />
                                    <order attribute=""productid"" descending=""false"" />
                                    <filter type=""and"">
                                      <condition attribute=""pricelevelid"" operator=""eq"" uiname=""GP Prices"" uitype=""pricelevel"" value=""" + priceLevelID + @""" />
                                    </filter>
                                    <link-entity name=""product"" from=""productid"" to=""productid"" visible=""false"" link-type=""outer"" alias=""a_ad6bb04de39ae911a9b5002248073192"">
                                      <attribute name=""productnumber"" />
                                      <attribute name=""producttypecode"" />
                                      <attribute name=""description"" /> 
                                      <attribute name=""cowry_features"" /> 
                                      <attribute name=""cowry_licensetype"" />
                                      <attribute name=""subjectid"" />
                                      <attribute name=""cowry_minimumsale"" />
                                      <attribute name=""cowry_maximumsale"" />
                                      <attribute name=""cowry_required"" />
                                      <attribute name=""cowry_requiredproduct"" />
                                      <attribute name=""cowry_orderposition"" />
                                      <filter type=""and"">
                                        <condition attribute=""statecode"" operator=""eq"" value=""0"" />
                                      </filter>
                                    <link-entity name=""product"" from=""productid"" to=""cowry_requiredproduct"" visible=""false"" link-type=""outer"" alias=""a_187cffea23c4e911a82f000d3a0b5769"">
                                      <attribute name=""productnumber"" />
                                      <filter type=""and"">
                                        <condition attribute=""statecode"" operator=""eq"" value=""0"" />
                                      </filter>
                                    </link-entity>
                                    </link-entity>
                                    <link-entity name=""product"" from=""productid"" to=""productid"" link-type=""inner"" alias=""af"">
                                      <attribute name=""productnumber"" />
                                      <filter type=""and"">
                                        <condition attribute=""statecode"" operator=""eq"" value=""0"" />
                                      </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            FetchExpression myQueryExpression = new FetchExpression(FetchXml);
            EntityCollection myEntityCollection = cli1.RetrieveMultiple(myQueryExpression);

            foreach (Entity item in myEntityCollection.Entities)
            {
                Product product = new Product();
                if (item.Contains("a_ad6bb04de39ae911a9b5002248073192.productnumber"))
                {
                    product.ProductId = ((AliasedValue)item["a_ad6bb04de39ae911a9b5002248073192.productnumber"]).Value.ToString();
                }
                if (item.Contains("productid"))
                {
                    product.ProductName = ((EntityReference)item["productid"]).Name;
                }
                if (item.Contains("amount"))
                {
                    product.ProductPrice = ((Money)item["amount"]).Value.ToString("0.00");
                }
                if (item.Contains("a_ad6bb04de39ae911a9b5002248073192.producttypecode"))
                {
                    product.ProductType = ((OptionSetValue)((AliasedValue)item["a_ad6bb04de39ae911a9b5002248073192.producttypecode"]).Value).Value.ToString();
                }
                if (item.Contains("a_ad6bb04de39ae911a9b5002248073192.description"))
                {
                    product.ProductDescription = ((AliasedValue)item["a_ad6bb04de39ae911a9b5002248073192.description"]).Value.ToString();
                }
                if (item.Contains("a_ad6bb04de39ae911a9b5002248073192.cowry_licensetype"))
                {
                    product.LicenseType = ((AliasedValue)item["a_ad6bb04de39ae911a9b5002248073192.cowry_licensetype"]).Value.ToString();
                }
                if (item.Contains("a_ad6bb04de39ae911a9b5002248073192.cowry_required"))
                {
                    product.Required = (bool)((AliasedValue)item["a_ad6bb04de39ae911a9b5002248073192.cowry_required"]).Value;
                }
                if (item.Contains("a_187cffea23c4e911a82f000d3a0b5769.productnumber"))
                {
                    product.RequiredProduct = ((AliasedValue)item["a_187cffea23c4e911a82f000d3a0b5769.productnumber"]).Value.ToString();
                }
                if (item.Contains("a_ad6bb04de39ae911a9b5002248073192.subjectid"))
                {
                    product.Subject = ((Microsoft.Xrm.Sdk.EntityReference)((Microsoft.Xrm.Sdk.AliasedValue)item["a_ad6bb04de39ae911a9b5002248073192.subjectid"]).Value).Name;
                }
                if (item.Contains("a_ad6bb04de39ae911a9b5002248073192.cowry_minimumsale"))
                {
                    product.MinimumSale = (int)((Microsoft.Xrm.Sdk.AliasedValue)item["a_ad6bb04de39ae911a9b5002248073192.cowry_minimumsale"]).Value;
                }
                if (item.Contains("a_ad6bb04de39ae911a9b5002248073192.cowry_maximumsale"))
                {
                    product.MaximumSale = (int)((Microsoft.Xrm.Sdk.AliasedValue)item["a_ad6bb04de39ae911a9b5002248073192.cowry_maximumsale"]).Value;
                }
                if (item.Contains("a_ad6bb04de39ae911a9b5002248073192.cowry_orderposition"))
                {
                    product.OrderPosition = (int)((Microsoft.Xrm.Sdk.AliasedValue)item["a_ad6bb04de39ae911a9b5002248073192.cowry_orderposition"]).Value;
                }


                if (item.Contains("a_ad6bb04de39ae911a9b5002248073192.cowry_features"))
                {
                    product.Features = new List<string>();

                    string features = ((AliasedValue)item["a_ad6bb04de39ae911a9b5002248073192.cowry_features"]).Value.ToString();
                    foreach (var itemFeature in features.Split('\n'))
                    {
                        product.Features.Add(itemFeature);
                    }
                }

                productData.Add(product);
            }
            productData.Sort((u1, u2) => (int.Parse(u1.ProductId).CompareTo(int.Parse(u2.ProductId))));
            return productData;

        }
        public List<Product> GetProductInvoice(string priceLevelID, string[] productnumber, int[] minimum)
        {
            List<Product> products = new List<Product>();
            string FetchXml = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                  <entity name=""productpricelevel"">
                                    <attribute name=""productnumber"" />
                                    <attribute name=""productid"" />
                                    <attribute name=""uomid"" />
                                    <attribute name=""amount"" />
                                    <order attribute=""productid"" descending=""false"" />
                                    <filter type=""and"">
                                        <filter type=""or"">
                                            <condition attribute=""productnumber"" operator=""eq"" value=""";
            for (int i = 0; i < productnumber.Length; i++)
            {
                FetchXml += productnumber[i] + @""" />";
                if (i < productnumber.Length - 1)
                {
                    FetchXml += @"<condition attribute=""productnumber"" operator=""eq"" value=""";
                }
                else
                {
                    FetchXml += @"</filter>";
                }
            }
            FetchXml += @"<condition attribute=""transactioncurrencyid"" operator=""eq"" uiname=""Pound Sterling"" uitype=""transactioncurrency"" value=""" + priceLevelID + @""" />
                            </filter>
                            <link-entity name=""product"" from=""productid"" to=""productid"" visible=""false"" link-type=""outer"" alias=""a_ad6bb04de39ae911a9b5002248073192"">
                                <attribute name=""producttypecode"" />
                                <attribute name=""productnumber"" />
                            </link-entity>
                            </entity>
                        </fetch>";
            FetchExpression myQueryExpression = new FetchExpression(FetchXml);
            EntityCollection myEntityCollection = cli1.RetrieveMultiple(myQueryExpression);
            foreach (Entity item in myEntityCollection.Entities)
            {
                Product product = new Product();
                if (item.Contains("amount"))
                {
                    product.ProductPrice = ((Money)item["amount"]).Value.ToString("0.00");
                }
                if (item.Contains("a_ad6bb04de39ae911a9b5002248073192.producttypecode"))
                {
                    product.ProductType = ((OptionSetValue)((AliasedValue)item["a_ad6bb04de39ae911a9b5002248073192.producttypecode"]).Value).Value.ToString();
                }
                if (item.Contains("a_ad6bb04de39ae911a9b5002248073192.productnumber"))
                {
                    product.ProductId = ((AliasedValue)item["a_ad6bb04de39ae911a9b5002248073192.productnumber"]).Value.ToString();
                }
                for (int j = 0; j < minimum.Length; j++)
                {
                    if (productnumber[j] == product.ProductId) product.MinimumSale = minimum[j];
                }
                products.Add(product);
            }
            return products;
        }

        public Guid CreateNewLead(UserData userData)
        {
            Guid ret = Guid.Empty;
            Entity myLead = new Entity("lead");

            myLead.Attributes.Add("firstname", userData.CustomerFirstName);
            myLead.Attributes.Add("lastname", userData.CustomerLastName);
            myLead.Attributes.Add("companyname", userData.CustomerCompanyName);
            myLead.Attributes.Add("subject", userData.CustomerCompanyName);
            myLead.Attributes.Add("jobtitle", userData.CustomerJobTitle);
            myLead.Attributes.Add("emailaddress1", userData.CustomerEmailAddress);
            myLead.Attributes.Add("telephone1", userData.CustomerContactNumber);
            myLead.Attributes.Add("telephone2", userData.CustomerCompanyContact);
            myLead.Attributes.Add("address1_line1", userData.CustomerCompanyAddress);
            myLead.Attributes.Add("address1_country", userData.CustomerCompanyCountry);
            myLead.Attributes.Add("address1_city", userData.CustomerCompanyCity);
            myLead.Attributes.Add("address1_stateorprovince", userData.CustomerCompanyState);
            myLead.Attributes.Add("address1_postalcode", userData.CustomerCompanyPostalCode);
            /*if (userData.Has_MicrosoftOffice365Account)
            {
                if (userData.DomainName != "")
                {
                    myLead.Attributes.Add("cowry_microsoft365account", userData.DomainName);
                    if (userData.Find_DomainName)
                    {
                        OptionSetValue value = new OptionSetValue();
                        value.Value = 847200000;
                        myLead.Attributes.Add("cowry_contactmicrosoftaccount", value);
                    }
                }
            }*/


            ret = cli1.Create(myLead);
            return ret;
        }

        public QualifyLeadResponse QualifyLead(string leadId)
        {
            QualifyLeadResponse qualifyIntoAccountContactRes = new QualifyLeadResponse();
            try
            {
                Entity myLead = cli1.Retrieve("lead", new Guid(leadId), new ColumnSet(true));
                var qualifyIntoAccountContactReq = new QualifyLeadRequest
                {
                    CreateContact = true,
                    CreateAccount = true,
                    LeadId = new EntityReference(myLead.LogicalName, new Guid(leadId)),
                    Status = new OptionSetValue(3)
                };
                qualifyIntoAccountContactReq.Parameters.Add("SuppressDuplicateDetection", true);
                qualifyIntoAccountContactRes = (QualifyLeadResponse)cli1.Execute(qualifyIntoAccountContactReq);
            }
            catch (Exception e)
            {
                throw e;
            }


            return qualifyIntoAccountContactRes;
        }
        public List<Product> AddInvoiceProduct(Guid invoice, string detail, string currency)
        {
            List<Product> products = new List<Product>();
            string currencyType = "{225AF3B4-50AD-E911-A82A-000D3A0B6C55}";
            if (currency == "USD")
            {
                currencyType = "{5AAD5430-ED9A-E911-A9B5-002248073192}";
            }
            Guid ret = Guid.Empty;
            string[] tokens = detail.Split(',');
            string[] idProducts = new string[tokens.Length];
            int[] minimumSale = new int[tokens.Length];
            int index = 0;
            foreach (string prod in tokens)
            {
                Entity invoiceProduct = new Entity("invoicedetail");
                string[] test3 = prod.Split(':');
                QueryByAttribute aux = new QueryByAttribute("product");
                aux.ColumnSet = new ColumnSet(true);
                aux.Attributes.AddRange("productnumber");
                aux.Values.AddRange(test3[0]);
                EntityCollection productAux = cli1.RetrieveMultiple(aux);
                Guid productGuid = productAux[0].Id;
                //Product prodAux = new Product();
                idProducts[index] = test3[0];
                //prodAux = GetProductInvoice(currencyType, test3[0]);
                string unit = "{affa0cae-dff5-42bc-ae5a-1dd7c07e115c}";
                invoiceProduct.Attributes.Add("productid", new EntityReference("product", productGuid));
                invoiceProduct.Attributes.Add("quantity", Decimal.Parse(test3[1]));
                //prodAux.MinimumSale = int.Parse(test3[1]);
                minimumSale[index] = int.Parse(test3[1]);
                invoiceProduct.Attributes.Add("invoiceid", new EntityReference("invoice", invoice));
                invoiceProduct.Attributes.Add("uomid", new EntityReference("uomschedule", Guid.Parse(unit)));
                //products.Add(prodAux);
                ret = cli1.Create(invoiceProduct);
                index++;
            }
            products = GetProductInvoice(currencyType, idProducts, minimumSale);
            return products;
        }
        public void AddInvoicePayOption(Guid invoice, List<Product> products, string option)
        {
            Entity myInvoice = new Entity("invoice");
            OptionSetValue payment = new OptionSetValue();
            int totalMonth = 0;
            int totalOneTime = 0;
            int totalannual = 0;
            foreach (Product item in products)
            {
                if (item.ProductType == "847200002")
                {
                    totalOneTime += item.MinimumSale * (int)Math.Round(double.Parse(item.ProductPrice));
                }
                else
                {
                    totalMonth += item.MinimumSale * (int)Math.Round(double.Parse(item.ProductPrice));
                }
            }
            if (option == "UpFront")
            {
                payment.Value = 847200002;
                totalOneTime = ((int)Math.Round(totalOneTime * 0.9)) + (totalMonth * 12);
            }
            else
            {
                if (option == "Annuity")
                {
                    payment.Value = 847200001;
                    totalannual = (int)Math.Round((totalOneTime * 1.1) / 12) + totalMonth;
                    totalOneTime = 0;
                }
                else
                {
                    payment.Value = 847200000;
                }
            }
            myInvoice.Id = invoice;
            myInvoice.Attributes["cowry_paymentoption"] = payment;
            myInvoice.Attributes["cowry_paypermonth"] = totalMonth;
            myInvoice.Attributes["cowry_payonetime"] = totalOneTime;
            myInvoice.Attributes["cowry_annualpayment"] = totalannual;
            try
            {
                cli1.Update(myInvoice);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public string GetInvoiceID(Guid invoice)
        {
            string id = "";
            Entity invoiceId = cli1.Retrieve("invoice", invoice, new ColumnSet(true));
            id = invoiceId["invoicenumber"].ToString();
            return id;
        }
        public Guid CreateNewInvoice(QualifyLeadResponse qualify, string currencyType, string name, string pnref)
        {
            string priceLevelID = "{A4511096-07BA-E911-A82D-000D3A0B617E}";
            string currency = "{225AF3B4-50AD-E911-A82A-000D3A0B6C55}";
            if (currencyType == "USD")
            {
                priceLevelID = "{83ee4ea2-07ba-e911-a82d-000d3a0b617e}";
                currency = "{5AAD5430-ED9A-E911-A9B5-002248073192}";
            }


            Entity myInvoice = new Entity("invoice");
            foreach (EntityReference entity in qualify.CreatedEntities)
            {
                if (entity.LogicalName == "account")
                {
                    Entity customerAux = cli1.Retrieve("account", entity.Id, new ColumnSet(true));
                    myInvoice.Attributes.Add("customerid", new EntityReference("account", customerAux.Id));
                }
            }
            myInvoice.Attributes.Add("name", name);
            if (pnref != "")
            {
                myInvoice.Attributes.Add("new_pnref", pnref);
            }
            myInvoice.Attributes.Add("pricelevelid", new EntityReference("pricelevel", Guid.Parse(priceLevelID)));
            myInvoice.Attributes.Add("transactioncurrencyid", new EntityReference("transactioncurrency", Guid.Parse(currency)));

            Guid ret = cli1.Create(myInvoice);

            return ret;
        }
        public string GenerateInvoice(Payment payment, string pnref)
        {
            string errorMessage = "";
            QualifyLeadResponse qualify = QualifyLead(payment.CustomerId);
            try
            {
                string name = "";
                if (payment.NameOnCreditCard != "") name = payment.NameOnCreditCard;
                Guid invoice = CreateNewInvoice(qualify, payment.currencyType, name, pnref);
                List<Product> products = AddInvoiceProduct(invoice, payment.Detail, payment.currencyType);
                AddInvoicePayOption(invoice, products, payment.PaymentOption);
                errorMessage = GetInvoiceID(invoice);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return errorMessage;
        }
        public void GenerateReport()
        {
            var invoiceId = "3afefa12-dcd0-e911-a812-000d3a7ed2f2";
            var reportID = "867B6434-F4DE-E911-A812-000D3A7ED2F2";
            var reportName = "ReportInvoice.rdl";

            string strParameterXML = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'><entity name='invoice'><all-attributes /><filter type='and'><condition attribute='invoiceid' operator='eq' value='" + invoiceId + "' /></filter></entity></fetch>";
            string rptPathString = "id=%7B" + reportID +
        "%7D&uniquename=" + cli1.ConnectedOrgUniqueName
        + "&iscustomreport=true&reportnameonsrs=&reportName=" + reportName
        + "&isScheduledReport=false&p:CRM_FilteredInvoice=" + HttpUtility.UrlEncode(strParameterXML) + "&p:InvoiceId=" + invoiceId;


            
            var httpWebRequestPDF = (HttpWebRequest)WebRequest.Create("https://cowrysolutions.crm11.dynamics.com/CRMReports/rsviewer/reportviewer.aspx");
            
            httpWebRequestPDF.Method = "POST";
            httpWebRequestPDF.ContentType = "application/x-www-form-urlencoded";
            httpWebRequestPDF.Accept = "*/*";
            httpWebRequestPDF.Headers.Add("Authorization", String.Format("Bearer {0}", cli1.CurrentAccessToken));
            
            using (var streamWriter = new StreamWriter(httpWebRequestPDF.GetRequestStream()))
            {
                streamWriter.Write(rptPathString) ;
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponsePDF = (HttpWebResponse)httpWebRequestPDF.GetResponse();
            using (var streamReader = new StreamReader(httpResponsePDF.GetResponseStream()))
            {
                var result2 = streamReader.ReadToEnd();

                var x = result2.LastIndexOf("ReportSession=");
                var y = result2.LastIndexOf("ControlID=");

                var ret = new string[2];

                ret[0] = result2.Substring(x + 14, 24);
                ret[1] = result2.Substring(x + 10, 32);
            }
        }
        public string ProcessPayment(Payment payment)
        {
            try
            {
                PayPalAccess myPayPalAccess = new PayPalAccess();
                string pnref = "";
                string sekureToken = "";
                string sekureTokenId = "";

                string totalAmt = payment.TotalPayableAmount;

                string trxId = string.Empty;
                Entity myLead = cli1.Retrieve("lead", new Guid(payment.CustomerId), new ColumnSet(true));
                DateTime dateCard = new DateTime(int.Parse(payment.CreditCardExpiryYear), int.Parse(payment.CreditCardExpiryMonth), 1);

                trxId = myPayPalAccess.ProcessSalesTransaction2(payment.NameOnCreditCard, "", totalAmt, dateCard, payment.CreditCardNumber,
                    payment.CreditCardCVV, "", myLead.Attributes["emailaddress1"].ToString(), myLead.Attributes["telephone1"].ToString(),
                    myLead.Attributes["address1_line1"].ToString(), "", myLead.Attributes["address1_stateorprovince"].ToString(),
                    myLead.Attributes["address1_city"].ToString(), myLead.Attributes["address1_postalcode"].ToString(),
                    myLead.Attributes["address1_country"].ToString(), payment.currencyType);

                string[] trxResponse = trxId.Split('&');

                if (trxResponse.Length == 1 || trxResponse[0].ToString() == "RESULT=0")
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
                    if (pnref == "" && trxResponse.Length == 1)
                    {
                        pnref = trxResponse[0];
                    }
                    GenerateInvoice(payment, pnref);
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
                    return errorMessage;
                }
                return pnref;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}