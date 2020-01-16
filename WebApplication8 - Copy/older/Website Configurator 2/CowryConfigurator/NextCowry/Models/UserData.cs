using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextCowry.Models
{
    public class UserData
    {
        //First Data
        public string SpId { get; set; }
        public string Currency { get; set; }

        //Main Data
        public string PremiumUsers { get; set; }
        public string EssentialUsers { get; set; }
        public string TeamUsers { get; set; }
        public string TotalUsers { get; set; }
        public string SubTotalPrice { get; set; }
        public string StandardSupportPrice { get; set; }
        public string EnhancedSupportPrice { get; set; }
        public string EnhancedSupportChecked { get; set; }

        //Payment Option
        public string PaymentOptionMonthlyPrice { get; set; }
        public string PaymentOptionYearlyPrice { get; set; }
        public string PaymentOptionAns { get; set; }

        //PersonalInformation
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }

        //Company Information
        public string CompanyName { get; set; }
        public string CompanyNumber { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyCountry { get; set; }
        public string CompanyState { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyPostalCode { get; set; }

        //Microsoft Account Information
        public string MicrosoftOfficeAccChecked { get; set; }
        public string DomainName { get; set; }
        public string FindDomain { get; set; }

        //Authorization
        public string MicrosoftCloudAgreement { get; set; }
        public string SupportContract { get; set; }
        
    }

}