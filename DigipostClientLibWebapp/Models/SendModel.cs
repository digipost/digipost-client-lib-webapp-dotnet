using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.UI.WebControls;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Search;
using Microsoft.AspNet.Identity;

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
        public bool SmsAfterHour { get; internal set; }
        public SensitivityLevel SensitivityOption { get; internal set; }
        public AuthenticationLevel AuthenticationOption { get; internal set; }
        public string SmsAfterHours { get; internal set; }

    }
}