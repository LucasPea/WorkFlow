using Crayon.Api.Sdk.Domain;
using Crayon.Api.Sdk.Domain.Csp;
using Crayon.Api.Sdk.Filtering;
using Agreement = Crayon.Api.Sdk.Domain.Csp.Agreement;

namespace Crayon.Api.Sdk.Resources
{
    public class CustomerTenantResource
    {
        private readonly CrayonApiClient _client;

        public CustomerTenantResource(CrayonApiClient client)
        {
            _client = client;
        }

        public CrayonApiClientResult<ApiCollection<CustomerTenant>> Get(string token, CustomerTenantFilter filter = null)
        {
            var uri = "api/v1/customertenants/".Append(filter);
            return _client.Get<ApiCollection<CustomerTenant>>(token, uri);
        }

        public CrayonApiClientResult<CustomerTenant> GetById(string token, int id)
        {
            var uri = $"api/v1/customertenants/{id}/";
            return _client.Get<CustomerTenant>(token, uri);
        }

        public CrayonApiClientResult<CustomerTenantDetailed> GetDetailedById(string token, int id)
        {
            var uri = $"api/v1/customertenants/{id}/detailed/";
            return _client.Get<CustomerTenantDetailed>(token, uri);
        }

        public CrayonApiClientResult<CustomerTenantDetailed> Create(string token, CustomerTenantDetailed customerTenant)
        {
            var uri = "api/v1/customertenants/";
            return _client.Post<CustomerTenantDetailed>(token, uri, customerTenant);
        }

        public CrayonApiClientResult<Agreement> Consent(string token, Agreement consent,int id)
        {
            var uri = $"api/v1/customertenants/{id}/agreements";
            var Agg= _client.Post<Agreement>(token, uri, consent);
            return Agg;
        }

        public CrayonApiClientResult<CustomerTenantDetailed> CreateExisting(string token, CustomerTenantDetailed customerTenant)
        {
            var uri = "api/v1/customertenants/existing/";
            return _client.Post<CustomerTenantDetailed>(token, uri, customerTenant);
        }

        public CrayonApiClientResult<CustomerTenantDetailed> Update(string token, CustomerTenantDetailed customerTenant)
        {
            Guard.NotNull(customerTenant, nameof(customerTenant));

            var uri = $"api/v1/customertenants/{customerTenant.Tenant.Id}/";
            var test= _client.Put<CustomerTenantDetailed>(token, uri, customerTenant);
            return test;
        }

        public CrayonApiClientResult Delete(string token, int id, bool removeFromPublisher)
        {
            var uri = $"/api/v1/customertenants/{id}/".Append("removeFromPublisher", removeFromPublisher);
            return _client.Delete(token, uri);
        }
    }
}