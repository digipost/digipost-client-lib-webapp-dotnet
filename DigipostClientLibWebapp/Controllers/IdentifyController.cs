using System.Web.Mvc;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using DigipostClientLibWebapp.Services.Digipost;
using System.Threading.Tasks;

namespace DigipostClientLibWebapp.Controllers
{
    public class IdentifyController : ControllerBase
    {
        public IdentifyController() : base()
        {

        }
        public IdentifyController(DigipostService digipostService) : base(digipostService)
        {

        }

        // GET: Identify
        public ActionResult Index()
        {

            return View();
        }

        // GET: Identify/Identify/
        public async Task<ActionResult> Identify(Identification identification)
        {
            var result = await GetDigipostService().Identify(identification);

            return View("IdentificationResult",result);
        } 
    }
}
