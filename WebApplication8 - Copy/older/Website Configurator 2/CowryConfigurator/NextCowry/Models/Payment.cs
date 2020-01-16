using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextCowry.Models
{
    public class Payment
    {
        public string SpId { get; set; }
        public string CardType { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string CardCVV { get; set; }
        public string CardExpiryMonth { get; set; }
        public string CardExpiryYear { get; set; }

        public string Email { get; set; }
        public string TotalAmount { get; set; }

        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string state { get; set; }
        public string country { get; set; }

    }
}