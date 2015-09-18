using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Search;
using Digipost.Api.Client.Domain.SendMessage;
using DigipostClientLibWebapp.Services.Digipost;

namespace DigipostClientLibWebapp.Controllers
{
    public class SendController : Controller
    {

        public ActionResult Send(SearchDetails person)
        {
            if (person == null || person.DigipostAddress == null)
                return View("../Search/Index");

            return View("Send", person);
        }

        [HttpPost]
        public async Task<ActionResult> Send(string subject,string digipostAddress, SensitivityLevel sensitivityOption,AuthenticationLevel authenticationOption, bool smsAfterHour, string smsAfterHours)
        {
            var digipostService = new DigipostService();
            
            byte[] fileContent = null;
            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
                using(var binaryReader = new BinaryReader(httpPostedFileBase.InputStream))
                {
                    fileContent = binaryReader.ReadBytes(httpPostedFileBase.ContentLength);
                }
            var fileType = httpPostedFileBase.ContentType;
            var result = await digipostService.Send(fileContent, "pdf", subject, digipostAddress,sensitivityOption, authenticationOption, smsAfterHour,smsAfterHours);

            return View("SendStatus", result);
        }
        
    }
}