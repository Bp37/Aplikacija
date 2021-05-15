using SigurnostAplikacija.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SigurnostAplikacija.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //[Authorize]
        //[Authorize(Users = "jura@klafura.com")]
        //[Authorize(Roles = "Administrator")]
        public ActionResult Contact()
        {
            if (User.IsInRole(RoleHelper.Administrator))
            {
                ViewBag.Message = "Algebra";
            }
            else
            {
                ViewBag.Message = "E nećeš razbojniče!";
            }
            

            return View();
        }
    }
}