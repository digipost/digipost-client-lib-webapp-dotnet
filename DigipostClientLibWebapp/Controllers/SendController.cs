using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Digipost.Api.Client.Domain.SendMessage;
using DigipostClientLibWebapp.Services.Digipost;

namespace DigipostClientLibWebapp.Controllers
{
    public class SendController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Send(string subject,string digipostAddress)
        {
            var digipostService = new DigipostService();
            
            
            byte[] fileContent = null;
            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
                using(var binaryReader = new BinaryReader(httpPostedFileBase.InputStream))
                {
                    fileContent = binaryReader.ReadBytes(httpPostedFileBase.ContentLength);
                }

            var result = await digipostService.Send(fileContent, "pdf", subject, digipostAddress);

            return View("SendStatus", result);
        }
        
    }
}