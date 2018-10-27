using System.Web.Mvc;
using MindTheGap.Models;

namespace MindTheGap.Helpers
{
    public class UserHelper
    {
        public bool IsLoggedIn(Controller controller)
        {
            var user = controller.Session ["User"] as User;
            return user != null;
        }
    }
}