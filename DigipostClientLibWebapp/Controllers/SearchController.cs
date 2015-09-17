using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Digipost.Api.Client.Domain.Search;
using DigipostClientLibWebapp.Services.Digipost;

namespace DigipostClientLibWebapp.Controllers
{
    public class SearchController : Controller
    {
       
        public ActionResult Index(List<SearchDetails> search)
        {
            return View(search); //return Search/Index
        }

        [HttpPost]
        public async Task<ActionResult> Search(string search)
        {
            var digipostService = new DigipostService();
            var searchResult = await digipostService.Search(search);
            
            return View("Index",searchResult.PersonDetails); //return Search/Search
        }

        public ActionResult Send(SearchDetails person)
        {
            return View("Send",person);
        }
    }
}