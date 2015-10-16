using Digipost.Api.Client.Domain.Enums;

namespace DigipostClientLibWebapp.Models
{
    public class IdentifyModel
    {
        public IdentificationType IdentificationType { get; set; }
        public string IdentificationValue { get; set; }

        public string PostalCode { get;  set; }
        public string AddressLine1 { get;  set; }
        public string City { get;  set; }
        public string FullName { get;  set; }
    }
}