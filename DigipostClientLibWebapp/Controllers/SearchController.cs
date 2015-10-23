using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Digipost.Api.Client.Domain.Search;
using DigipostClientLibWebapp.Constants;
using DigipostClientLibWebapp.Services.Digipost;
using DigipostClientLibWebapp.Utilities;

namespace DigipostClientLibWebapp.Controllers
{
    public class SearchController : ControllerBase
    {
        public SearchController() : base()
        {
            
        }

        public SearchController(DigipostService digipostService) : base(digipostService)
        {
            
        }

        public ActionResult Index(IEnumerable<SearchDetails> search)
        {
            return View(search);
        }

        [HttpPost]
        public async Task<ActionResult> Search(string search)
        {
            var searchResult = await GetDigipostService().Search(search);

            SessionManager.AddToSession(searchResult);

            return View("Index",searchResult.PersonDetails); 
        }

        public ActionResult GoToSend(string digipostAddress)
        {
            var personDetails = SessionManager.GetFromSession<ISearchDetailsResult>();
            var person = personDetails.PersonDetails.Find(details => details.DigipostAddress.Equals(digipostAddress));
            
            Session[SessionConstants.PersonModel] = person;
            return RedirectToAction("Index", "Send");
        }
    }
}