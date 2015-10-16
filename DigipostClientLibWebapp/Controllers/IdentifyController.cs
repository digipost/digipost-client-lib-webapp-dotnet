using System.Web.Mvc;
using Digipost.Api.Client.Domain.Identify;
using DigipostClientLibWebapp.Services.Digipost;
using System.Threading.Tasks;
using Digipost.Api.Client.Domain.SendMessage;
using DigipostClientLibWebapp.Models;

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
        public async Task<ActionResult> Identify(IdentifyModel identifyModel)
        {
            Identification identification = null;
            identification = new Identification(new RecipientById(identifyModel.IdentificationType, identifyModel.IdentificationValue));
            
            var result = await GetDigipostService().Identify(identification);

            return View("IdentificationResult",result);
        }

        public async Task<ActionResult> IdentifyByNameAndAddress(IdentifyModel identifyModel)
        {
            Identification identification = null;
            identification = new Identification(new RecipientByNameAndAddress(identifyModel.FullName, identifyModel.AddressLine1, identifyModel.PostalCode, identifyModel.City));
            
            var result = await GetDigipostService().Identify(identification);

            return View("IdentificationResult", result);
        }
    }
}
