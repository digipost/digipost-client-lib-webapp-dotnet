using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Digipost.Api.Client.Domain.Search;
using DigipostClientLibWebapp.Constants;
using DigipostClientLibWebapp.Models;
using DigipostClientLibWebapp.Services.Digipost;
using DigipostClientLibWebapp.Utilities;

namespace DigipostClientLibWebapp.Controllers
{
    public class SendController : Controller
    {
        private readonly IDigipostService _digipostService;
        
        public SendController(IDigipostService digipostService)
        {
            _digipostService = digipostService;
        }

        [HttpGet]
        public ActionResult Index(SendModel sendModel)
        {
            var isEmptyModel = sendModel == null || string.IsNullOrEmpty(sendModel.DigipostAddress);

            if (isEmptyModel)
            {
                sendModel = GetSendModelFromSession();
            }

            return View("Index", sendModel);
        }

        private SendModel GetSendModelFromSession()
        {
            var searchDetails = SessionManager.GetFromSession<SearchDetails>(HttpContext, SessionConstants.PersonModel);

            SendModel sendModel = new SendModel();
            if (searchDetails != null)
            {
                sendModel = Converter.SearchDetailsToSendModel(searchDetails);
            }

            SessionManager.RemoveFromSession(HttpContext, SessionConstants.PersonModel);

            return sendModel;
        }

        [HttpPost]
        public async Task<ActionResult> Send(SendModel sendModel)
        {
            byte[] fileContent = null;
            var fileType = "";

            var hasError = IsValidFileContent(ref fileContent, ref fileType);

            if (hasError)
            {
                return View("Index", sendModel);
            }

            var result = await _digipostService.Send(fileContent, fileType, sendModel);
            return View("SendStatus", result);
        }

        private bool IsValidFileContent( ref byte[] fileContent, ref string fileType)
        {
            bool hasError = IsFileSelected();

            HttpPostedFileBase httpPostedFileBase = Request.Files[0];
            if (!hasError)
            {
                if (httpPostedFileBase == null || httpPostedFileBase.ContentLength == 0)
                {
                    ModelState.AddModelError("ErrorMessage", "Please select a file to send.");
                    hasError = true;
                }
            }

            if (!hasError)
            {
                GetFileContent(out fileContent, httpPostedFileBase);
                GetMimeType(out fileType, httpPostedFileBase);

                if (string.IsNullOrEmpty(fileType))
                {
                    ModelState.AddModelError("ErrorMessage", "Unknown filetype, supported types is [.PDF, .TXT]");
                    hasError = true;
                }
            }
            
            return hasError;
        }

        private static void GetMimeType(out string fileType, HttpPostedFileBase httpPostedFileBase)
        {
            fileType = Converter.MimeTypeToDigipostFileType(httpPostedFileBase.ContentType);
        }

        private static void GetFileContent(out byte[] fileContent, HttpPostedFileBase httpPostedFileBase)
        {
            using (var binaryReader = new BinaryReader(httpPostedFileBase.InputStream))
            {
                fileContent = binaryReader.ReadBytes(httpPostedFileBase.ContentLength);
            }
        }

        private bool IsFileSelected()
        {
            var hasError = false;
            if (Request.Files == null || Request.Files.Count < 1)
            {
                ModelState.AddModelError("ErrorMessage", "Please select a file to send.");
                hasError = true;
            }

            return hasError;
        }       
    }
}