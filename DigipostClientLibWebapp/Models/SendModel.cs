using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.UI.WebControls;
using Digipost.Api.Client.Domain.Search;
using Microsoft.AspNet.Identity;

namespace DigipostClientLibWebapp.Models
{
    public class SendModel
    {
        public SearchDetails SearchDetails { get; set; }
        [Required]
        public HttpPostedFileBase FileCollection { get; set; }
        [Required]
        [MinLength(1)]
        public string Subject { get; set; }
    }
}