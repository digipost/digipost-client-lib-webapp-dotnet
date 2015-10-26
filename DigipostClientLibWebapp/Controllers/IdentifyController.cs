using System.Web.Mvc;
using Digipost.Api.Client.Domain.Identify;
using DigipostClientLibWebapp.Services.Digipost;
using System.Threading.Tasks;
using System.Web;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.SendMessage;
using DigipostClientLibWebapp.Models;

namespace DigipostClientLibWebapp.Controllers
{
    public class IdentifyController : Controller
    {
        private readonly IDigipostService _digipostService;

        public IdentifyController(IDigipostService digipostService)
        {
            _digipostService = digipostService;
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
            
            var result = await _digipostService.Identify(identification);

            return PartialView("IdentificationResult", result);
        }

        public async Task<ActionResult> IdentifyByNameAndAddress(IdentifyModel identifyModel)
        {
            Identification identification = null;
            identification = new Identification(new RecipientByNameAndAddress(identifyModel.FullName, identifyModel.AddressLine1, identifyModel.PostalCode, identifyModel.City));
            
            var result = await _digipostService.Identify(identification);
            
            return PartialView("IdentificationResult", result);
            
        }
        
    }
}
