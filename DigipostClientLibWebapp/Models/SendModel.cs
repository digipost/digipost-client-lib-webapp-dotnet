using System.ComponentModel.DataAnnotations;
using System.Web;
using Digipost.Api.Client.Domain.Enums;

namespace DigipostClientLibWebapp.Models
{
    public class SendModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DigipostAddress { get; set; }
        public string MobileNumber { get; set; }
        public string OrganizationName { get; set; }

        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string HouseLetter { get; set; }
        public string AdditionalAddressLine { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }

        [Required]
        public HttpPostedFileBase FileCollection { get; set; }
        [Required]
        [MinLength(1)]
        public string Subject { get; set; }
        public bool SmsAfterHour { get;  set; }
        public SensitivityLevel SensitivityOption { get;  set; }
        public AuthenticationLevel AuthenticationOption { get;  set; }
        public string SmsAfterHours { get;  set; }

    }
}