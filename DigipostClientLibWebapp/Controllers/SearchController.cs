using System.Web.Mvc;

namespace DigipostClientLibWebapp.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Index()
        {
            return View(); //return Search/Index
        }

        [HttpPost]
        public ActionResult Search(string search)
        {
            return View(); //return Search/Search
        }
    }
}