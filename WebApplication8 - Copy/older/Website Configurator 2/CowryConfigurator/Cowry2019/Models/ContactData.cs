using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cowry2019.Models
{
    public class ContactData
    {
        //PersonalInformation
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerJobTitle { get; set; }
        public string CustomerContactNumber { get; set; }
        public string CustomerEmailAddress { get; set; }

        //Company Information
        public string CustomerCompanyName { get; set; }
        public string CustomerCompanyContact { get; set; }
        public string CustomerCompanyAddress { get; set; }
        public string CustomerCompanyCountry { get; set; }
        public string CustomerCompanyState { get; set; }
        public string CustomerCompanyCity { get; set; }
        public string CustomerCompanyPostalCode { get; set; }

    }
}