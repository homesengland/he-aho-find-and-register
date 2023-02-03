using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Find_Register.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Find_Register.Controllers
{
    public class FooterLinksController : Controller
    {
        public IActionResult ContactUs()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());
            return View();
        }

        public IActionResult Accessibility()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());
            return View();
        }
    }
}

