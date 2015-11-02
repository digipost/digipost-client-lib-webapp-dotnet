using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Digipost.Api.Client.Domain.Search;
using DigipostClientLibWebapp.Constants;
using DigipostClientLibWebapp.Services.Digipost;
using DigipostClientLibWebapp.Utilities;

namespace DigipostClientLibWebapp.Controllers
{
    public class SearchController : Controller
    {
        private readonly IDigipostService _digipostService;
        
        public SearchController(IDigipostService digipostService)
        {
            _digipostService = digipostService;
        }

        public ActionResult Index(IEnumerable<SearchDetails> search)
        {
            return View(search);
        }

        [HttpPost]
        public async Task<ActionResult> Search(string search)
        {   
            var searchResult = await _digipostService.Search(search);
            
            SessionManager.AddToSession(HttpContext,SessionConstants.PersonDetails, searchResult);

            return View("Index", searchResult.PersonDetails);
        }

        public ActionResult GoToSend(string digipostAddress)
        {
            var personDetails = SessionManager.GetFromSession<ISearchDetailsResult>(HttpContext, SessionConstants.PersonDetails);
            var person = personDetails.PersonDetails.Find(details => details.DigipostAddress.Equals(digipostAddress));

            SessionManager.AddToSession(HttpContext, SessionConstants.PersonModel, person);
            
            return RedirectToAction("Index", "Send");
        }
        
    }
}