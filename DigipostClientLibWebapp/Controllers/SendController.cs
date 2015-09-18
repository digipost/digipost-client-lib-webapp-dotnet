using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Search;
using Digipost.Api.Client.Domain.SendMessage;
using DigipostClientLibWebapp.Models;
using DigipostClientLibWebapp.Services.Digipost;

namespace DigipostClientLibWebapp.Controllers
{
    public class SendController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {

            SendModel sendModel =(SendModel) Session["sendModel"];

            return View("Send", sendModel);
        }

       
        [HttpPost]
        public async Task<ActionResult> Send(SendModel sendModel)//(string subject, string digipostAddress, SensitivityLevel sensitivityOption, AuthenticationLevel authenticationOption, bool smsAfterHour, string smsAfterHours)
        {
            var digipostService = new DigipostService();

            byte[] fileContent = null;
            var httpPostedFileBase = sendModel.FileCollection;// Request.Files[0];

            if (httpPostedFileBase == null || httpPostedFileBase.ContentLength == 0)
                return View();

            using (var binaryReader = new BinaryReader(httpPostedFileBase.InputStream))
            {
                fileContent = binaryReader.ReadBytes(httpPostedFileBase.ContentLength);
            }
            var fileType = mapToDigipostFileType(httpPostedFileBase.ContentType);

            //var result = await digipostService.Send(fileContent,fileType,sendModel.Subject,sendModel.SearchDetails.DigipostAddress) //(fileContent, fileType, subject, digipostAddress, sensitivityOption, authenticationOption, smsAfterHour, smsAfterHours);
            string result = null;
            return View("SendStatus", result);
        }

        private string mapToDigipostFileType(string mimeType)
        {
            if (mimeType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                return "pdf";
            if (mimeType.Equals("text/plain", StringComparison.OrdinalIgnoreCase))
                return "txt";

            return "";
        }
    }
}