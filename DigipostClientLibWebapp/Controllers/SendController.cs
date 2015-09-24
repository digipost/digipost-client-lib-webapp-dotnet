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
            
            var searchDetails = (SearchDetails)Session["personModel"];
            var sendModel = new SendModel();
            if(searchDetails.SearchDetailsAddress != null) { 
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
            


            return View("Index",sendModel);
        }
        

        [HttpPost]
        public async Task<ActionResult> Send(SendModel sendModel)
        {
            var digipostService = new DigipostService();

            byte[] fileContent = null;
            var httpPostedFileBase = Request.Files[0];

            if (httpPostedFileBase == null || httpPostedFileBase.ContentLength == 0)
                return View("Index",sendModel);

            using (var binaryReader = new BinaryReader(httpPostedFileBase.InputStream))
            {
                fileContent = binaryReader.ReadBytes(httpPostedFileBase.ContentLength);
            }
            var fileType = mapToDigipostFileType(httpPostedFileBase.ContentType);

            var result = await digipostService.Send(fileContent, fileType, sendModel.Subject , sendModel.DigipostAddress, sendModel.SensitivityOption, sendModel.AuthenticationOption, sendModel.SmsAfterHour, sendModel.SmsAfterHours);
            
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