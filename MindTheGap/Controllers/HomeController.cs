using System.Web.Mvc;
using MindTheGap.Models;

namespace MindTheGap.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var user = Session["User"] as User;
            if (user == null)
                return RedirectToAction("Index", "Login");
            return RedirectToAction("Index", "LoggedInHome");
        }
    }
}
