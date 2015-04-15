using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;

namespace Viseo.WiiWars.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult WiiWarsGame()
        {
            ViewBag.Message = "Your WiiWars application description page.";

            return View();
        }

        public IActionResult UnderTheHood()
        {
            ViewBag.Message = "Your UnderTheHood page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}