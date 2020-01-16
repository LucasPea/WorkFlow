using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class License
    {
        [JsonProperty("Value")]
        public List<Value> Value { get; set; }

        [JsonProperty("@nextLink")]
        public object NextLink { get; set; }

        [JsonProperty("TotalCount")]
        public long TotalCount { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("processedDateTime")]
        public DateTimeOffset ProcessedDateTime { get; set; }

        [JsonProperty("serviceCode")]
        public string ServiceCode { get; set; }

        [JsonProperty("serviceName")]
        public string ServiceName { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("customerTenantId")]
        public string CustomerTenantId { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("licensesDeployed")]
        public long LicensesDeployed { get; set; }

        [JsonProperty("licensesSold")]
        public long LicensesSold { get; set; }
    }

    public partial class License
    {
        public static License FromJson(string json) => JsonConvert.DeserializeObject<License>(json, Licenses.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this License self) => JsonConvert.SerializeObject(self, Licenses.Converter.Settings);
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
