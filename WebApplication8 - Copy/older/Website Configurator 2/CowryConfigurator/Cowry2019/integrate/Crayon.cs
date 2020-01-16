using Crayon.Api.Sdk;
using Crayon.Api.Sdk.Filtering;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Net;

using Crayon.Api.Sdk.Domain;
using Crayon.Api.Sdk.Domain.Csp;
using Crayon.Api.Sdk.Domain.MasterData;
using System.Net.Http;
using System.IO;
using Cowry2019.integrate;

namespace Cowry2019.integrate
{
    public class CrayonAccess
    {
        // PROD
        //        public const string ClientId = "c4322857-ad4f-4845-9277-0141794d2c7f";
        //      public const string ClientSecret = "2bf66b33-672f-470b-8ca4-37c8f1d9f9c7";  


        // Sandbox
        private string Username;
        private string Password;

        private string ClientId;
        private string ClientSecret;

        private static readonly CrayonApiClient ApiClient = new CrayonApiClient(CrayonApiClient.ApiUrls.Production);
        private static Token _myToken;

        public CrayonAccess(string region)
        {
            switch (region) { 
                case "United Kingdom":
                    Username = ConfigurationManager.AppSettings["CrayonUsername"];
                    Password = ConfigurationManager.AppSettings["CrayonPassword"];
                    ClientId = ConfigurationManager.AppSettings["CrayonClientId"];
                    ClientSecret = ConfigurationManager.AppSettings["CrayonClientSecret"];
                    break;
                case "United States":
                    Username = ConfigurationManager.AppSettings["USCrayonUsername"];
                    Password = ConfigurationManager.AppSettings["USCrayonPassword"];
                    ClientId = ConfigurationManager.AppSettings["USCrayonClientId"];
                    ClientSecret = ConfigurationManager.AppSettings["USCrayonClientSecret"];
                    break;
            }
        }

        public void SetupNewCustomer(string firstname, string lastname, string email, string phone, string companyName,
            string address1, string city, string zip, string state, string country, int fullUsersCount, int readOnlyUsersConut, string domainName, bool hasAccount=false)
        {
            //Organizations
            CSP csp = new CSP();
            var microsoftRegions = ApiClient.Regions.Get(GetToken(), new RegionFilter { RegionList = RegionList.MicrosoftCsp }).GetData();

            var organizations = ApiClient.Organizations.Get(GetToken()).GetData();
            Organization organization = ApiClient.Organizations.GetById(GetToken(), organizations.Items.First().Id).GetData();
            var publishers = ApiClient.Publishers.Get(GetToken()).GetData();
            Publisher microsoft = publishers.Items.First(p => p.Name.Equals("Microsoft"));

            var addresses = ApiClient.Addresses.Get(GetToken(), organization.Id, AddressType.Invoice).GetData().Items;

            var address = ApiClient.Addresses.GetById(GetToken(), organization.Id, addresses.First().Id).GetData();
            InvoiceProfileFilter invoiceProfileFilter = new InvoiceProfileFilter
            {
                OrganizationId = organization.Id,
            };

            //invoice
            var invoiceProfile2 = ApiClient.InvoiceProfiles.Get(GetToken(), invoiceProfileFilter).GetData();
            int invId = 0;
            foreach (InvoiceProfile item in invoiceProfile2)
            {
                if (item.Name == "Default")
                {
                    invId = item.Id;
                    break;
                }
            }
            DateTime time = DateTime.UtcNow;
            string domainprefix = "";

            //customer detail
            CustomerTenantFilter customerTenantFilter = new CustomerTenantFilter
            {
                OrganizationId = organization.Id
            };
            domainName = domainName.Replace(" ", string.Empty);
            if (domainName != "")
            {
                if (domainName.Contains("@"))
                {
                    string[] domainP = domainName.Split('@');
                    domainprefix = domainP[1];
                    if (domainName.Contains(".com"))
                    {
                        string[] dom = domainP[1].Split(new string[] { ".com" }, StringSplitOptions.None);
                        domainprefix = dom[0];
                        if (domainName.Contains(".onmicrosoft"))
                        {
                            dom = dom[0].Split(new string[] { ".onmicrosoft" }, StringSplitOptions.None);
                            domainprefix = dom[0];
                        }
                    }
                }
                else
                {
                    domainprefix = domainName;
                }
            }
            else
            {
                int length = companyName.Length - 26;
                if (length > 1)
                {
                    domainprefix = companyName + Guid.NewGuid().ToString().Substring(0, length).Replace("-", "");
                }
                else
                {
                    if (companyName.Length < 26)
                    {
                        domainprefix = companyName;
                    }
                    else
                    {
                        domainprefix = companyName.Substring(0, 26);
                    }
                }
                if (!csp.VerifyCustomerDomain(domainprefix + ".onmicrosoft.com"))
                {
                    if (domainprefix.Length < 27)
                    {
                        domainprefix += Guid.NewGuid().ToString().Substring(0, 1).Replace("-", "");
                    }
                    else
                    {
                        domainprefix = domainprefix.Substring(0, 26);
                        domainprefix += Guid.NewGuid().ToString().Substring(0, 1).Replace("-", "");
                    }
                }
                
            }

            //Customer tenants
            var newCustomerTenant = new CustomerTenantDetailed
            {
                Tenant = new CustomerTenant
                {
                    Name = companyName,
                    CustomerTenantType = CustomerTenantType.T2,
                    DomainPrefix = domainprefix,
                    Organization = new Organization { Id = organization.Id },
                    Publisher = new ObjectReference { Id = microsoft.Id },
                    InvoiceProfile = new ObjectReference { Id = invId },
                    agreement = new Crayon.Api.Sdk.Domain.Csp.Agreement
                    {
                        FirstName = firstname,
                        LastName = lastname,
                        Email = email,
                        PhoneNumber = phone,
                        DateAgreed = time.ToString("yyyy-MM-dd"),
                        AgreementType = 1
                    }
                },
                Profile = new CustomerTenantProfile
                {
                    Address = new CustomerTenantAddress
                    {
                        FirstName = firstname,
                        LastName = lastname,
                        AddressLine1 = address1,
                        CountryCode = microsoftRegions.Items.First(r => r.Name.Contains(country)).Code,
                        City = city,
                        PostalCode = zip,
                        Region = state
                    },
                    Contact = new CustomerTenantContact
                    {
                        FirstName = firstname,
                        LastName = lastname,
                        Email = email,
                        PhoneNumber = phone
                    },
                    Agreement = new Crayon.Api.Sdk.Domain.Csp.Agreement
                    {
                        FirstName = firstname,
                        LastName = lastname,
                        Email = email,
                        PhoneNumber = phone,
                        DateAgreed = time.ToString("yyyy-MM-dd"),
                        AgreementType = 1
                    }
                }
            };

            //new customer
            if (!hasAccount)
            {
                var newCustomerTenant2 = ApiClient.CustomerTenants.Create(GetToken(), newCustomerTenant).GetData();
            }

            newCustomerTenant.Tenant.Id = ApiClient.CustomerTenants.Get(GetToken(), customerTenantFilter).GetData().Where(x => x.DomainPrefix == domainprefix).FirstOrDefault().Id;
            
            //agreement
            var existingCustomerTenant = new CustomerTenantDetailed
            {
                Tenant = new CustomerTenant
                {
                    ExternalPublisherCustomerId = newCustomerTenant.Tenant.ExternalPublisherCustomerId,
                    Organization = new Organization { Id = organization.Id }
                }
            };
            var cspProductsNoAddons = ApiClient.AgreementProducts.GetCspSeatProducts(GetToken(), new AgreementProductFilter { OrganizationId = organization.Id }, false).GetData();
            
            Crayon.Api.Sdk.Domain.Csp.Agreement agr = new Crayon.Api.Sdk.Domain.Csp.Agreement()
            {
                FirstName = firstname,
                LastName = lastname,
                Email = email,
                PhoneNumber = phone,
                DateAgreed = time.ToString("yyyy-MM-dd"),
                AgreementType = 1
            };

            Crayon.Api.Sdk.Domain.Csp.Agreement agrs = ApiClient.CustomerTenants.Consent(GetToken(), agr, newCustomerTenant.Tenant.Id).GetData();

            //subscriptions
            string skuID = "";
            if (fullUsersCount > 0)
            {
                skuID = "2880026b-2b0c-4251-8656-5d41ff11e3aa";
                var newSubscription = ApiClient.Subscriptions.Create(GetToken(), new SubscriptionDetailed
                {
                    Name = "Dynamics 365 Business Central Essential suscription",
                    CustomerTenant = new CustomerTenantReference { Id = newCustomerTenant.Tenant.Id },
                    Quantity = fullUsersCount,
                    Product = new ProductReference
                    {
                        Id = cspProductsNoAddons.Items.First(r => r.ProductVariant.PartNumber == "1A90EE13-2CB4-4785-BB0F-542813F00A37").ProductVariant.Product.Id,
                        PartNumber = cspProductsNoAddons.Items.First(r => r.ProductVariant.PartNumber == "1A90EE13-2CB4-4785-BB0F-542813F00A37").ProductVariant.Product.PartNumber
                    }
                }).GetData();
            }
            if (readOnlyUsersConut > 0)
            {
                if(skuID=="")skuID = "2e3c4023-80f6-4711-aa5d-29e0ecb46835";
                var newSubscription = ApiClient.Subscriptions.Create(GetToken(), new SubscriptionDetailed
                {
                    Name = "Dynamics 365 Business Central Team Member suscription",
                    CustomerTenant = new CustomerTenantReference { Id = newCustomerTenant.Tenant.Id },
                    Quantity = readOnlyUsersConut,
                    Product = new ProductReference
                    {
                        Id = cspProductsNoAddons.Items.First(r => r.ProductVariant.PartNumber == "F72752C8-3E37-4C9B-A1A0-69E8442068DC").ProductVariant.Product.Id,
                        PartNumber = cspProductsNoAddons.Items.First(r => r.ProductVariant.PartNumber == "F72752C8-3E37-4C9B-A1A0-69E8442068DC").ProductVariant.Product.PartNumber
                    },

                }).GetData();
            }

            //reset customer password, set customer usage, and asign license
            CustomerTenantDetailed detailed = ApiClient.CustomerTenants.GetDetailedById(GetToken(), newCustomerTenant.Tenant.Id).GetData();
            CustomerTenant exCustomerTenantT = ApiClient.CustomerTenants.Get(GetToken(), customerTenantFilter).GetData().Where(x => x.DomainPrefix == domainprefix).FirstOrDefault();
            csp.SetCustomerLicenses(exCustomerTenantT.PublisherCustomerId.ToString(), microsoftRegions.Items.First(r => r.Name.Contains(country)).Code,skuID);
        }

        public string GetToken()
        {
            //Reuse same token until it expires
            if (_myToken != null && _myToken.ExpiresIn > TimeSpan.FromMinutes(5).TotalSeconds)
            {
                return _myToken.AccessToken;
            }

            var result = ApiClient.Tokens.GetUserToken(ClientId, ClientSecret, Username, Password);
            _myToken = result.GetData();
            var token = _myToken.AccessToken;

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Unable to get Token." + _myToken?.Error);
            }
            return token;
        }
    }
}