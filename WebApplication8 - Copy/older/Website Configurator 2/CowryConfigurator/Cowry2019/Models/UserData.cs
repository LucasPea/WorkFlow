using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cowry2019.Models
{
    public class UserData
    {
        public string CustomerId { get; set; }

        //License Data       
        public string TotalEssentialUsers { get; set; }
        public string TotalEssentialPrice { get; set; }
        public string TotalTeamUsers { get; set; }
        public string TotalTeamPrice { get; set; }
        public string TotalUsers { get; set; }
        public string TotalStandardPrice { get; set; }
        public bool Selected_Enhanced { get; set; }
        public string TotalEnhancedPrice { get; set; }
        

        //Package Data
        public string CoreFinancialPrice { get; set; }
        public string MultipleBudgetPrice { get; set; }
        public string MultipleCurrenciesPrice { get; set; }
        public string BasicCommercialsPrice { get; set; }
        public string MultipleLocationsDSPrice { get; set; }
        public string CustomerSupplierPricingPrice { get; set; }
        public string StandardMigrationPrice { get; set; }
        public string HistoricalDataImportPrice { get; set; }
        public string PowerBIPrice { get; set; }
        public string AdditionalCompaniesPrice { get; set; }        

        //Payment Option
        public string TotalMonthlyPrice { get; set; }
        public string TotalAnnuityPrice { get; set; }
        public string TotalOneTimePrice { get; set; }
        public string Selected_PaymentOption { get; set; }

        //PersonalInformation
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerJobTitle { get; set; }
        public string CustomerContactNumber { get; set; }
        public string CustomerEmailAddress{ get; set; }

        //Company Information
        public string CustomerCompanyName { get; set; }
        public string CustomerCompanyContact { get; set; }
        public string CustomerCompanyAddress { get; set; }
        public string CustomerCompanyCountry { get; set; }
        public string CustomerCompanyState { get; set; }
        public string CustomerCompanyCity { get; set; }
        public string CustomerCompanyPostalCode { get; set; }

        //Microsoft Account Information
        public bool Has_MicrosoftOffice365Account { get; set; }
        public string DomainName { get; set; }
        public bool Find_DomainName { get; set; }

        //Authorization
        public bool Accept_MicrosoftCloudAgreement { get; set; }
        public bool Accept_SupportContract { get; set; }

    }
}