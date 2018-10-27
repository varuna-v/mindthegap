using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using MindTheGap.Repositories;

namespace MindTheGap.Controllers
{
    public class LoggedInHomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}
