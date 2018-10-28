using System.Linq;
using System.Web.Mvc;
using MindTheGap.Helpers;
using MindTheGap.Models;
using MindTheGap.Repositories;

namespace MindTheGap.Controllers
{
    public class UserTrainHistoryController : Controller
    {
        // GET: UserTrainHistory
        public ActionResult Index()
        {
            if (!new UserHelper().IsLoggedIn(this))
                return RedirectToAction("Index", "Login");
            var user = Session["User"] as User;
            var repo = new GameHistoryRepository();
            var history = repo.GetUserGameHistory(user.UserName);
            if (history == null || !history.Any())
            {
                ViewBag.ErrorMessage = "You have not yet played any games.";
                return View();
            }

            return View(history);
        }
    }
}