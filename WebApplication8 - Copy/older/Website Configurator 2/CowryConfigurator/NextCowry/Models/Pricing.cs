using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextCowry.Models
{
    public class Pricing
    {
        public string SpId { get; set; }
        public string CurrencyType { get; set; }
        public string PremiumUsersCount { get; set; }
        public string EssentialUsersCount { get; set; }
        public string TeamUsersCount { get; set; }
        public string SubTotal1 { get; set; }
        public string SubTotal2 { get; set; }
    }
}