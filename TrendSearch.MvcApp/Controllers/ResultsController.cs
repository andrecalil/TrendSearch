using System.Web.Mvc;
using TrendSearch.Domain;

namespace TrendSearch.MvcApp.Controllers
{
    public class ResultsController : Controller
    {
        public ActionResult Index()
        {
            if (this.Session["currentSearch"] != null)
            {
                Search mCurrentSearch = (Search)this.Session["currentSearch"];

                this.ViewBag.KeyWords = mCurrentSearch.KeyWords;

                return View(mCurrentSearch.SearchAndReate());
            }
            else
            {
                return Redirect("/Search");
            }
        }
    }
}