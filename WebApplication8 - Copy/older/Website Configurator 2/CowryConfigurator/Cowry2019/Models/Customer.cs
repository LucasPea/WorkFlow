using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Customers
    {
        [JsonProperty("totalCount")]
        public long TotalCount { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }

        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }
    }

    public partial class Attributes
    {
        [JsonProperty("objectType")]
        public string ObjectType { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("userPrincipalName")]
        public string UserPrincipalName { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("userDomainType")]
        public string UserDomainType { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }

        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }
    }

    public partial class Links
    {
        [JsonProperty("self")]
        public Self Self { get; set; }
    }

    public partial class Self
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("headers")]
        public List<object> Headers { get; set; }
    }

    public partial class Customers
    {
        public static Customers FromJson(string json) => JsonConvert.DeserializeObject<Customers>(json, QuickType.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Customers self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
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