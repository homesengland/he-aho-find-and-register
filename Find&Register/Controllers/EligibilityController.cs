using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Find_Register.Cookies;
using Find_Register.Filters;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Find_Register.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Find_Register.Controllers
{
    [TypeFilter(typeof(UnhandledExceptionFilter))]
    public class EligibilityController : Controller
    {
        private ICookieHelper _cookieHelper;
        ILogger<EligibilityController> _logger { get; set; }
        public EligibilityController(ILogger<EligibilityController> logger, ICookieHelper cookieHelper)
        {
            _logger = logger;
            _cookieHelper = cookieHelper;
        }

        //page 1
        [HttpGet]
        public IActionResult WhereDoYouWantToBuyAHome()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());
            var _EligibilityWhereDoYouWantToBuyAHomePage = new EligibilityWhereDoYouWantToBuyAHomePage();
            return View(_EligibilityWhereDoYouWantToBuyAHomePage);
        }

        [HttpPost]
        public IActionResult WhereDoYouWantToBuyAHome(EligibilityWhereDoYouWantToBuyAHomePage _EligibilityWhereDoYouWantToBuyAHomePage)
        {
            if (_EligibilityWhereDoYouWantToBuyAHomePage.LivingLocation == true)
            {
                return RedirectToAction(nameof(EligibilityOutcome));
            }

            if (_EligibilityWhereDoYouWantToBuyAHomePage.LivingLocation == false)
            {
                return RedirectToAction(nameof(BuyingWithAnotherPerson));
            }

            if (!ModelState.IsValid)
            {
                return View(_EligibilityWhereDoYouWantToBuyAHomePage);
            }

            var applicationCookie = _cookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
            var cookie = applicationCookie.EligibilityResponses.Value;
            cookie.EligibilityWhereDoYouWantToBuyAHomePage = _EligibilityWhereDoYouWantToBuyAHomePage;
            applicationCookie.EligibilityResponses.Value = cookie;

            return View(_EligibilityWhereDoYouWantToBuyAHomePage);
        }

        public IActionResult BuyingWithAnotherPerson()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());
            return View();
        }

        public IActionResult EligibilityOutcome()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());
            return View();
        }

    }
}

