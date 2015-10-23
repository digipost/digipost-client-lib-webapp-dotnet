using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Digipost.Api.Client.Domain.Search;
using DigipostClientLibWebapp.Constants;
using DigipostClientLibWebapp.Models;
using DigipostClientLibWebapp.Services.Digipost;

namespace DigipostClientLibWebapp.Controllers
{
    public class SendController : ControllerBase
    {
        public SendController() : base()
        {

        }
        public SendController(DigipostService digipostService) : base(digipostService)
        {

        }

        [HttpGet]
        public ActionResult Index(SendModel sendModel)
        {
            if (sendModel == null || string.IsNullOrEmpty(sendModel.DigipostAddress))
            {
                var searchDetails = (SearchDetails)Session[SessionConstants.PersonModel];
                if (searchDetails == null)
                    return View("Index", new SendModel());
                sendModel = Converter.SearchDetailsToSendModel(searchDetails);
                Session.Remove(SessionConstants.PersonModel);
            }
            return View("Index", sendModel);
        }


        [HttpPost]
        public async Task<ActionResult> Send(SendModel sendModel)
        {
            bool hasError = false;
            byte[] fileContent = null;
            var fileType = "";

            hasError = ValidateFileCollection(ref fileContent, ref fileType);

            if (hasError)
            {
                return View("Index", sendModel);
            }

            var result = await GetDigipostService().Send(fileContent, fileType, sendModel);
            return View("SendStatus", result);
        }

        private bool ValidateFileCollection( ref byte[] fileContent, ref string fileType)
        {
            bool hasError = false;
            if (Request.Files == null || Request.Files.Count < 1)
            {
                ModelState.AddModelError("ErrorMessage", "Please select a file to send.");
                hasError = true;
            }
            else
            {
                HttpPostedFileBase httpPostedFileBase  = Request.Files[0];
                if (httpPostedFileBase == null || httpPostedFileBase.ContentLength == 0)
                {
                    ModelState.AddModelError("ErrorMessage", "Please select a file to send.");
                    hasError = true;
                }
                using (var binaryReader = new BinaryReader(httpPostedFileBase.InputStream))
                {
                    fileContent = binaryReader.ReadBytes(httpPostedFileBase.ContentLength);
                }
                fileType = Converter.MimeTypeToDigipostFileType(httpPostedFileBase.ContentType);
                if (string.IsNullOrEmpty(fileType))
                {
                    ModelState.AddModelError("ErrorMessage", "Unknown filetype, supported types is [.PDF, .TXT]");
                    hasError = true;
                }
            }
            return hasError;
        }
    }
}