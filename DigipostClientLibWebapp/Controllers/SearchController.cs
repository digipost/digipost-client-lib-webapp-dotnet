using System.Collections.Generic;
using System.Web.Mvc;

namespace DigipostClientLibWebapp.Controllers
{
    public class SearchController : Controller
    {
       
        public ActionResult Index(List<string> search)
        {
            return View(search); //return Search/Index
        }

        [HttpPost]
        public ActionResult Search(string search)
        {
            List<string> stringList = new List<string>();
            stringList.Add(search);

            return View("Index",stringList); //return Search/Search
        }
    }
}