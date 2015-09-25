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
        public ActionResult Index(SendModel sendModel)
        {
            var personmodel = "personModel";

            if (sendModel == null || string.IsNullOrEmpty(sendModel.DigipostAddress))
            {
                var searchDetails = (SearchDetails)Session[personmodel];
                if (searchDetails == null)
                    return View("Index", new SendModel());
                sendModel = ConvertToSendModel(searchDetails);
                Session.Remove(personmodel);
            }


            return View("Index", sendModel);
        }

        private static SendModel ConvertToSendModel(SearchDetails searchDetails)
        {
            var sendModel = new SendModel();
            if (searchDetails.SearchDetailsAddress != null)
            {
                sendModel.AdditionalAddressLine = searchDetails.SearchDetailsAddress.AdditionalAddressLine;
                sendModel.City = searchDetails.SearchDetailsAddress.City;
                sendModel.HouseLetter = searchDetails.SearchDetailsAddress.HouseLetter;
                sendModel.HouseNumber = searchDetails.SearchDetailsAddress.HouseNumber;
                sendModel.Street = searchDetails.SearchDetailsAddress.Street;
                sendModel.ZipCode = searchDetails.SearchDetailsAddress.ZipCode;
            }
            sendModel.DigipostAddress = searchDetails.DigipostAddress;
            sendModel.FirstName = searchDetails.FirstName;
            sendModel.MiddleName = searchDetails.MiddleName;
            sendModel.LastName = searchDetails.LastName;
            sendModel.MobileNumber = searchDetails.MobileNumber;
            sendModel.OrganizationName = searchDetails.OrganizationName;
            return sendModel;
        }


        [HttpPost]
        public async Task<ActionResult> Send(SendModel sendModel)
        {
            var digipostService = new DigipostService();
            bool hasError = false;
            byte[] fileContent = null;
            var fileType = "";

            if(string.IsNullOrEmpty(sendModel.DigipostAddress))
            {
                ModelState.AddModelError("ErrorMessage", "Please add the reciever.");
                hasError = true;
            }
            if (string.IsNullOrEmpty(sendModel.Subject))
            {
                ModelState.AddModelError("ErrorMessage", "Please set the subject.");
                hasError = true;
            }
            if (Request.Files == null || Request.Files.Count < 1)
            {
                ModelState.AddModelError("ErrorMessage", "Please select a file to send.");
                hasError = true;
            }
            else
            {
                HttpPostedFileBase httpPostedFileBase = sendModel.FileCollection = Request.Files[0];
                if (httpPostedFileBase == null || httpPostedFileBase.ContentLength == 0)
                {
                    ModelState.AddModelError("ErrorMessage", "Please select a file to send.");
                    hasError = true;
                }
                using (var binaryReader = new BinaryReader(httpPostedFileBase.InputStream))
                {
                    fileContent = binaryReader.ReadBytes(httpPostedFileBase.ContentLength);
                }
                fileType = mapToDigipostFileType(httpPostedFileBase.ContentType);
                if (string.IsNullOrEmpty(fileType))
                {
                    ModelState.AddModelError("ErrorMessage", "Unknown filetype, supported types is [.PDF, .TXT]");
                    hasError = true;
                }
            }

            if (hasError)
            {
                return View("Index", sendModel);
            }

            var result = await digipostService.Send(fileContent, fileType, sendModel.Subject, sendModel.DigipostAddress, sendModel.SensitivityOption, sendModel.AuthenticationOption, sendModel.SmsAfterHour, sendModel.SmsAfterHours);
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