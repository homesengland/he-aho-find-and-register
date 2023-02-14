
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
                cookie.EligibilityOutcome = "London";
                applicationCookie.EligibilityResponses.Value = cookie;
                return RedirectToAction(nameof(EligibilityOutcome));
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
                cookie.EligibilityOutcome = "Over80K";
                applicationCookie.EligibilityResponses.Value = cookie;
                return RedirectToAction(nameof(EligibilityOutcome));
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
                cookie.EligibilityOutcome = "Over80K";
                applicationCookie.EligibilityResponses.Value = cookie;
                return RedirectToAction(nameof(EligibilityOutcome));
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
        public IActionResult FirstTimeBuyer(EligibilityJourneyFirstTimeBuyer _EligibilityJourneyFirstTimeBuyer)
        {
            var applicationCookie = _cookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
            var cookie = applicationCookie.EligibilityResponses.Value;
            cookie.EligibilityJourneyFirstTimeBuyer = _EligibilityJourneyFirstTimeBuyer;
            applicationCookie.EligibilityResponses.Value = cookie;

            if (!ModelState.IsValid)
            {
                return View(_EligibilityJourneyFirstTimeBuyer);
            }
            
            //Nothing is selected
            if(
                _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == null &&
                _EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == null &&
                _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == null &&
                _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null
            )
            {
                ModelState.AddModelError("Invalid Selection", "Please choose at least one option");
                return View(_EligibilityJourneyFirstTimeBuyer);
            }

           // Users can only select from one group of options at a time
           if(
                _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == true && 
                (
                    _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == true ||
                    _EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == true ||
                    _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == true
                )
           ) 
           {
                ModelState.AddModelError("Invalid Answer", "Please check your answers and try again");
                return View(_EligibilityJourneyFirstTimeBuyer);
           }
       
            if (_EligibilityJourneyFirstTimeBuyer.theseDoNotApply == true & _EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == null
                & _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == null & _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == null)

            {
                cookie.EligibilityOutcome = "NotEligable";
                applicationCookie.EligibilityResponses.Value = cookie;
                return RedirectToAction(nameof(EligibilityOutcome));
            }

            if (_EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == true & _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == null
                & _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == null & _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null)
            {
                cookie.EligibilityOutcome = "NotEligable";
                applicationCookie.EligibilityResponses.Value = cookie;
                return RedirectToAction(nameof(EligibilityOutcome));
            }

            if (_EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == true & _EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == null
                &  _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == null & _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null)
            {
                cookie.EligibilityOutcome = "NotEligable";
                applicationCookie.EligibilityResponses.Value = cookie;
                return RedirectToAction(nameof(EligibilityOutcome));
            }


            if (_EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == true & _EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == null
                & _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == null & _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null)
            {
                cookie.EligibilityOutcome = "NotEligable";
                applicationCookie.EligibilityResponses.Value = cookie;
                return RedirectToAction(nameof(EligibilityOutcome));
            }

            if (_EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == true & _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == true
                 &  _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == null & _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null)
            {
                cookie.EligibilityOutcome = "NotEligable";
                applicationCookie.EligibilityResponses.Value = cookie;
                return RedirectToAction(nameof(EligibilityOutcome));
            }

            if (_EligibilityJourneyFirstTimeBuyer.theseDoNotApply == true & _EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == true
                    & _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == true & _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == true)
            {
                ViewBag.conflictingChoicesChosen = true;
                return View(_EligibilityJourneyFirstTimeBuyer);
            }

            if (_EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null & _EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == null
                    & _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == null & _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == null)
            { 
                ViewBag.conflictingChoicesChosen = false;
                return View(_EligibilityJourneyFirstTimeBuyer);
            }

            if (_EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == true & _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == true
                 & _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == true & _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null)
            {
                ViewBag.conflictingChoicesChosen = true;
                return View(_EligibilityJourneyFirstTimeBuyer);
            }

            if (_EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == true & _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == null
                & _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == true &_EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null)
            {
                cookie.EligibilityOutcome = "Eligable";
                applicationCookie.EligibilityResponses.Value = cookie;
                return RedirectToAction(nameof(EligibilityOutcome));
            }

            if (_EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == null & _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == true
                & _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == true & _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null)
            {
                cookie.EligibilityOutcome = "Eligable";
                applicationCookie.EligibilityResponses.Value = cookie;
                return RedirectToAction(nameof(EligibilityOutcome));
            }

            

            return View(_EligibilityJourneyFirstTimeBuyer);
        }

        //page 5
        [HttpGet]
        public IActionResult EligibilityOutcome()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());

            var applicationCookie = _cookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
            var cookie = applicationCookie.EligibilityResponses.Value;

            if (cookie.EligibilityOutcome == "London") { ViewBag.outcome = "EligibilityOutcomeLondon"; }
            if (cookie.EligibilityOutcome == "Over80K") { ViewBag.outcome = "EligibilityOutcomeForOver80KIncome"; }
            if (cookie.EligibilityOutcome == "NotEligable")
            {
                ViewBag.outcome = "NotEligable";
                ViewBag.firstTimeBuyer = cookie.EligibilityJourneyFirstTimeBuyer.firstTimeBuyer.GetValueOrDefault();
                ViewBag.ownAHomeButNeedToMove = cookie.EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove.GetValueOrDefault();
                ViewBag.cannotAffordAHome = cookie.EligibilityJourneyFirstTimeBuyer.cannotAffordAHome.GetValueOrDefault();
                ViewBag.theseDoNotApply = cookie.EligibilityJourneyFirstTimeBuyer.theseDoNotApply.GetValueOrDefault();
            }
            if (cookie.EligibilityOutcome == "Eligable")
            {
                ViewBag.outcome = "Eligable"; 
            }

            return View();
        }
    }
}