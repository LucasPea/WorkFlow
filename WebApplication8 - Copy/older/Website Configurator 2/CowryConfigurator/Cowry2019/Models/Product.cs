using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cowry2019.Models
{
    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        public string ProductType { get; set; }
        public string LicenseType { get; set; }

        public string Subject { get; set; }

        public bool Required { get; set; }
        public string RequiredProduct { get; set; }
        public string ProductDescription { get; set; }
        public int MinimumSale { get; set; }
        public int MaximumSale { get; set; }
        public int OrderPosition { get; set; }
        public List<string> Features { get; set; }
    }

}