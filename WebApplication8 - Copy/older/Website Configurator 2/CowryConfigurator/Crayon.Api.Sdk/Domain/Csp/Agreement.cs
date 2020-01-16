
using System;

namespace Crayon.Api.Sdk.Domain.Csp
{
    public class Agreement
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool SameAsPrimaryContact { get; set; }
        public string DateAgreed { get; set; }
        //public DateTime DateAgreed { get; set; }
        public bool Accepted { get; set; }        
        //public AgreementType AgreementType { get; set; }
        public int AgreementType { get; set; }
        public bool Disabled { get; set; }
    }
}
