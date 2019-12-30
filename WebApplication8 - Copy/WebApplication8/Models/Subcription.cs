using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebApplication8.Models
{
    

    public partial class Subcription
    {
        [JsonProperty("value")]
        public Value[] Value { get; set; }

        [JsonProperty("count")]
        public Count Count { get; set; }
    }

    public partial class Count
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("authorizationSource")]
        public string AuthorizationSource { get; set; }

        [JsonProperty("managedByTenants")]
        public object[] ManagedByTenants { get; set; }

        [JsonProperty("subscriptionId")]
        public Guid SubscriptionId { get; set; }

        [JsonProperty("tenantId")]
        public Guid TenantId { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("subscriptionPolicies")]
        public SubscriptionPolicies SubscriptionPolicies { get; set; }
    }

    public partial class SubscriptionPolicies
    {
        [JsonProperty("locationPlacementId")]
        public string LocationPlacementId { get; set; }

        [JsonProperty("quotaId")]
        public string QuotaId { get; set; }

        [JsonProperty("spendingLimit")]
        public string SpendingLimit { get; set; }
    }

    public partial class Subcription
    {
        public static Subcription FromJson(string json) => JsonConvert.DeserializeObject<Subcription>(json, WebApplication8.Models.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Subcription self) => JsonConvert.SerializeObject(self, WebApplication8.Models.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}