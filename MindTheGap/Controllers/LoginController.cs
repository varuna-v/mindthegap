using System.Web.Mvc;
using MindTheGap.Models;
using MindTheGap.Repositories;

namespace MindTheGap.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserRepository _userRepository;

        public LoginController()
        {
            _userRepository = new UserRepository();
        }

        public ActionResult Index()
        {
            var user = Session["User"] as User;
            if (user == null)
                return View();
            return RedirectToAction("Index", "LoggedInHome");
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                ViewBag.Error = "Please enter username and password.";
                return View();
            }
            var existingUser = _userRepository.GetUser(user.UserName);
            if (existingUser == null)
                _userRepository.AddUser(user);
            else if (existingUser.Password != user.Password)
            {
                ViewBag.Error = "Incorrect password. Please try again.";
                return View();
            }
            Login(user);
            return RedirectToAction("Index", "LoggedInHome");
        }

        private void Login(User user)
        {
            Session["User"] = user;
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}
