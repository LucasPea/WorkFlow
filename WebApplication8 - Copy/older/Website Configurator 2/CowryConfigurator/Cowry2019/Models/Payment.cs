using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cowry2019.Models
{
    public class Payment
    {
        public string CustomerId { get; set; }
        public string TotalPayableAmount { get; set; }
        public string PaymentCardType { get; set; }
        public string NameOnCreditCard { get; set; }
        public string CreditCardNumber { get; set; }
        public string CreditCardCVV { get; set; }
        public string CreditCardExpiryMonth { get; set; }
        public string CreditCardExpiryYear { get; set; }
        public string currencyType { get; set; }
        public string Detail { get; set; }
        public string PaymentOption { get; set; }
    }
}