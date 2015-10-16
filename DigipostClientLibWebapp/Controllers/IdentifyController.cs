using System.Web.Mvc;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;

namespace DigipostClientLibWebapp.Controllers
{
    public class IdentifyController : ControllerBase
    {
        // GET: Identify
        public ActionResult Index()
        {

            return View();
        }

        // GET: Identify/Identify/
        public ActionResult Identify(Identification identification)
        {
            var result = new IdentificationResult(IdentificationResultType.DigipostAddress, "test");

            return View("IdentificationResult",result);
        } 
    }
}
