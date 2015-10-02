using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Digipost.Api.Client.Domain.Search;
using DigipostClientLibWebapp.Constants;
using DigipostClientLibWebapp.Services.Digipost;

namespace DigipostClientLibWebapp.Controllers
{
    public class SearchController : Controller
    {
        private readonly DigipostService _digipostService;

        public SearchController(DigipostService digipostService)
        {
            _digipostService = digipostService;
        }

        public SearchController()
        {
            _digipostService =  new DigipostService();
        }

        private DigipostService GetDigipostService()
        {
            return _digipostService ?? new DigipostService();
        }

        public ActionResult Index(List<SearchDetails> search)
        {
            if (search == null)
                return View();

            return View(search);
        }
        

        [HttpPost]
        public async Task<ActionResult> Search(string search)
        {
            var searchResult = await GetDigipostService().Search(search);
            
            Session[SessionConstants.PersonDetails] = searchResult;

            return View("Index",searchResult.PersonDetails); 
        }

        public ActionResult GoToSend(string digipostAddress)
        {
            var personDetails = (ISearchDetailsResult)Session[SessionConstants.PersonDetails];
            var person = personDetails.PersonDetails.Find(details => details.DigipostAddress.Equals(digipostAddress));
            
            Session[SessionConstants.PersonModel] = person;
            return RedirectToAction("Index", "Send");
        }
    }
}