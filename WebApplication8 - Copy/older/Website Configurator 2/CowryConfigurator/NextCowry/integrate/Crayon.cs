using Crayon.Api.Sdk;
using Crayon.Api.Sdk.Filtering;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Crayon.Api.Sdk.Domain;
using Crayon.Api.Sdk.Domain.Csp;
using Crayon.Api.Sdk.Domain.MasterData;

namespace NextCowry.integrate
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

        public CrayonAccess()
        {
            Username = ConfigurationManager.AppSettings["CrayonUsername"];
            Password = ConfigurationManager.AppSettings["CrayonPassword"];
            ClientId = ConfigurationManager.AppSettings["CrayonClientId"];
            ClientSecret = ConfigurationManager.AppSettings["CrayonClientSecret"];
        }

        public void SetupNewCustomer( string firstname, string lastname, string email, string phone,   string companyName, string address1, string city, string zip, string state, string country)
        {
            //Organizations

            var microsoftRegions = ApiClient.Regions.Get(GetToken(), new RegionFilter { RegionList = RegionList.MicrosoftCsp }).GetData();


            var organizations = ApiClient.Organizations.Get(GetToken()).GetData();            
            Organization organization = ApiClient.Organizations.GetById(GetToken(), organizations.Items.First().Id).GetData();
            var publishers = ApiClient.Publishers.Get(GetToken()).GetData();
            Publisher microsoft = publishers.Items.First(p => p.Name.Equals("Microsoft"));

            var addresses = ApiClient.Addresses.Get(GetToken(), organization.Id, AddressType.Invoice).GetData().Items;
            
            var address = ApiClient.Addresses.GetById(GetToken(), organization.Id, addresses.First().Id).GetData();
            

            var newInvoiceProfile = new InvoiceProfile
            {
                Organization = new ObjectReference { Id = organization.Id },
                Name = "My console invoice profile",
                InvoiceAddressId = address.Id,
                DeliveryAddress = new AddressData
                {
                    Name = companyName,
                    City = city,
                    CountryCode = country,
                    ZipCode = zip,
                    Street = address1,
                    State = state,
                    County = ""
                }
            };

            var invoiceProfile = ApiClient.InvoiceProfiles.Create(GetToken(), newInvoiceProfile).GetData();



            //Customer tenants
            var newCustomerTenant = new CustomerTenantDetailed
            {
                Tenant = new CustomerTenant
                {
                    Name = "My console customer tenant",
                    CustomerTenantType = CustomerTenantType.T2,
                    DomainPrefix = "mydomainprefix_" + Guid.NewGuid().ToString().Substring(0, 8).Replace("-", ""),
                    Organization = new Organization { Id = organization.Id },
                    Publisher = new ObjectReference { Id = microsoft.Id },
                    InvoiceProfile = new ObjectReference { Id = invoiceProfile.Id }
                },
                Profile = new CustomerTenantProfile
                {
                    Address = new CustomerTenantAddress
                    {
                        FirstName = firstname,
                        LastName = lastname,
                        AddressLine1 = address1,
                        CountryCode = microsoftRegions.Items.First(r => r.Name == country).Code,
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
                    }
                }
            };

            newCustomerTenant = ApiClient.CustomerTenants.Create(GetToken(), newCustomerTenant).GetData();

            //Add existing customer tenant
            var existingCustomerTenant = new CustomerTenantDetailed
            {
                Tenant = new CustomerTenant
                {
                    ExternalPublisherCustomerId = newCustomerTenant.Tenant.ExternalPublisherCustomerId,
                    Organization = new Organization { Id = organization.Id }
                }
            };

            var cspProductsNoAddons = ApiClient.AgreementProducts.GetCspSeatProducts(GetToken(), new AgreementProductFilter { OrganizationId = organization.Id }, false).GetData();

            //Subscriptions
            var newSubscription = ApiClient.Subscriptions.Create(GetToken(), new SubscriptionDetailed
            {
                Name = "My console subscription",
                CustomerTenant = new CustomerTenantReference { Id = newCustomerTenant.Tenant.Id },
                Quantity = 5,
                Product = new ProductReference
                {
                    //Dynamics 365 Business Central Essential 1a90ee13-2cb4-4785-bb0f-542813f00a37
                    //Dynamics 365 Business Central Team Member   f72752c8-3e37-4c9b-a1a0-69e8442068dc

                    Id = cspProductsNoAddons.Items.First().ProductVariant.Product.Id,
                    PartNumber = cspProductsNoAddons.Items.First().ProductVariant.Product.PartNumber
                }
            }).GetData();

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