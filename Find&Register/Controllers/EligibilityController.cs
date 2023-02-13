
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
        ILogger<EligibilityController> _logger{ get; set; }

        public EligibilityController(ILogger<EligibilityController>
                                        logger, ICookieHelper cookieHelper)
        {
            _logger = logger;
            _cookieHelper = cookieHelper;
        }

        //page 1 WhereDoYouWantToBuyAHome
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());
            var _EligibilityJourneyWhereDoYouWantToBuyAHome = new EligibilityJourneyWhereDoYouWantToBuyAHome();
            return View(_EligibilityJourneyWhereDoYouWantToBuyAHome);
        }

        [HttpPost]
        public IActionResult Index(EligibilityJourneyWhereDoYouWantToBuyAHome _EligibilityJourneyWhereDoYouWantToBuyAHome)
        {
            var applicationCookie = _cookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
            var cookie = applicationCookie.EligibilityResponses.Value;
            cookie.EligibilityJourneyWhereDoYouWantToBuyAHome = _EligibilityJourneyWhereDoYouWantToBuyAHome;
            applicationCookie.EligibilityResponses.Value = cookie;

            if (_EligibilityJourneyWhereDoYouWantToBuyAHome.LiveInLondon == true)
            {
                return RedirectToAction(nameof(EligibilityOutcomeLondon));
            }

            if (_EligibilityJourneyWhereDoYouWantToBuyAHome.LiveInLondon == false)
            {
                return RedirectToAction(nameof(BuyingWithAnotherPerson));
            }

            if (!ModelState.IsValid)
            {
                return View(_EligibilityJourneyWhereDoYouWantToBuyAHome);
            }

            return View(_EligibilityJourneyWhereDoYouWantToBuyAHome);
        }

        //page 2
        [HttpGet]
        public IActionResult BuyingWithAnotherPerson()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());
            return View();
        }

        [HttpPost]
        public IActionResult BuyingWithAnotherPerson(EligibilityJourneyBuyingWithAnotherPerson _EligibilityJourneyBuyingWithAnotherPerson)
        {
            var applicationCookie = _cookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
            var cookie = applicationCookie.EligibilityResponses.Value;
            cookie.EligibilityJourneyBuyingWithAnotherPerson = _EligibilityJourneyBuyingWithAnotherPerson;
            applicationCookie.EligibilityResponses.Value = cookie;

            if (_EligibilityJourneyBuyingWithAnotherPerson.SingleBuyer == true)
            {
                return RedirectToAction(nameof(HowMuchDoYouEarn_MultiplePeople));
            }

            if (_EligibilityJourneyBuyingWithAnotherPerson.SingleBuyer == false)
            {
                return RedirectToAction(nameof(HowMuchDoYouEarn));
            }

            if (!ModelState.IsValid)
            {
                return View(_EligibilityJourneyBuyingWithAnotherPerson);
            }

            return View(_EligibilityJourneyBuyingWithAnotherPerson);
        }

        //page 3 - single
        [HttpGet]
        public IActionResult HowMuchDoYouEarn()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());
            return View();
        }

        [HttpPost]
        public IActionResult HowMuchDoYouEarn(EligibilityJourneyHowMuchDoYouEarn _EligibilityJourneyHowMuchDoYouEarn)
        {
            var applicationCookie = _cookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
            var cookie = applicationCookie.EligibilityResponses.Value;
            cookie.EligibilityJourneyHowMuchDoYouEarn = _EligibilityJourneyHowMuchDoYouEarn;
            applicationCookie.EligibilityResponses.Value = cookie;

            if (_EligibilityJourneyHowMuchDoYouEarn.SingleIncomeOver80 == true)
            {
                return RedirectToAction(nameof(EligibilityOutcomeForOver80KIncome));
            }

            if (_EligibilityJourneyHowMuchDoYouEarn.SingleIncomeOver80 == false)
            {
                return RedirectToAction(nameof(FirstTimeBuyer));
            }

            if (!ModelState.IsValid)
            {
                return View(_EligibilityJourneyHowMuchDoYouEarn);
            }

            return View(_EligibilityJourneyHowMuchDoYouEarn);
        }

        //page 3 - multi
        [HttpGet]
        public IActionResult HowMuchDoYouEarn_MultiplePeople()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());
            return View();
        }

        [HttpPost]
        public IActionResult HowMuchDoYouEarn_MultiplePeople(EligibilityJourneyHowMuchDoYouEarn_MultiplePeople _EligibilityJourneyHowMuchDoYouEarn_MultiplePeople)
        {
            var applicationCookie = _cookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
            var cookie = applicationCookie.EligibilityResponses.Value;
            cookie.EligibilityJourneyHowMuchDoYouEarn_MultiplePeople = _EligibilityJourneyHowMuchDoYouEarn_MultiplePeople;
            applicationCookie.EligibilityResponses.Value = cookie;

            if (_EligibilityJourneyHowMuchDoYouEarn_MultiplePeople.JointIncomeOver80 == true)
            {
                return RedirectToAction(nameof(EligibilityOutcomeForOver80KIncome));
            }

            if (_EligibilityJourneyHowMuchDoYouEarn_MultiplePeople.JointIncomeOver80 == false)
            {
                return RedirectToAction(nameof(FirstTimeBuyer));
            }

            if (!ModelState.IsValid)
            {
                return View(_EligibilityJourneyHowMuchDoYouEarn_MultiplePeople);
            }

            return View(_EligibilityJourneyHowMuchDoYouEarn_MultiplePeople);
        }

        //page 4
        [HttpGet]
        public IActionResult FirstTimeBuyer()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());
            return View();
        }

        [HttpPost]
        public IActionResult FirstTimeBuyer(EligibilityJourneyFirstTimeBuyer _EligibilityJourneyFirstTimeBuyer, IFormCollection u)
        {
            var applicationCookie = _cookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
            var cookie = applicationCookie.EligibilityResponses.Value;
            cookie.EligibilityJourneyFirstTimeBuyer = _EligibilityJourneyFirstTimeBuyer;
            applicationCookie.EligibilityResponses.Value = cookie;

            if (u.Keys.Contains("current-circumstances"))
            {
                return RedirectToAction(nameof(EligibilityOutcome));
            }

            //if (_EligibilityJourneyFirstTimeBuyer.JointIncomeOver80 == false)
            //{
            //    return RedirectToAction(nameof(EligibilityOutcome));
            //}

            if (!ModelState.IsValid)
            {
                return View(_EligibilityJourneyFirstTimeBuyer);
            }

            return View(_EligibilityJourneyFirstTimeBuyer);
        }

        //page 5
        [HttpGet]
        public IActionResult EligibilityOutcome()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());
            return View();
        }

        [HttpGet]
        public IActionResult EligibilityOutcomeForOver80KIncome()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());
            return View();
        }

        [HttpGet]
        public IActionResult Eligable()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());
            return View();
        }

        [HttpGet]
        public IActionResult EligibilityOutcomeLondon()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());
            return View("EligibilityOutcomeLondon");
        }
        
    }
}