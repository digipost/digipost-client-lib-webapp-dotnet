using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Digipost.Api.Client.Domain.Search;
using DigipostClientLibWebapp.Models;
using DigipostClientLibWebapp.Services.Digipost;

namespace DigipostClientLibWebapp.Controllers
{
    public class SearchController : Controller
    {
       
        public ActionResult Index(List<SearchDetails> search)
        {
            if (search == null)
                return View();

            return View(search); //return Search/Index
        }

        [HttpPost]
        public async Task<ActionResult> Search(string search)
        {
            var digipostService = new DigipostService();
            ISearchDetailsResult searchResult = await digipostService.Search(search);

            Session["PersonDetails"] = searchResult;

            return View("Index",searchResult.PersonDetails); //return Search/Search
        }

        public ActionResult GoToSend(string digipostAddress)
        {

            ISearchDetailsResult personDetails = (ISearchDetailsResult)Session["PersonDetails"];

            var person = personDetails.PersonDetails.Find(details => details.DigipostAddress.Equals(digipostAddress));

            SendModel model = new SendModel { SearchDetails = person };
            Session["sendModel"] = model;
            return RedirectToAction("Index", "Send");
        }

    }
}