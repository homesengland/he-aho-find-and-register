using Find_Register.Cookies;
using Find_Register.Filters;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Find_Register.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Find_Register.Controllers;

[TypeFilter(typeof(UnhandledExceptionFilter))]
[JourneyLayoutFilter(Journey.Eligibility)]
[Route("check-eligibility-to-buy-a-shared-ownership-home")]
public class EligibilityController : BaseControllerWithShareStaticPages
{
    ILogger<EligibilityController> _logger { get; set; }
    private IConfiguration _config;

    public EligibilityController(ILogger<EligibilityController> logger, ICookieHelper cookieHelper, IConfiguration configuration) : base(cookieHelper)
    {
        ViewBag.layoutModel = new LayoutDataModel(Journey.Eligibility);
        _logger = logger;
        _config = configuration;
    }

    //page 1 WhereDoYouWantToBuyAHome
    [HttpGet]
    [ServiceFilter(typeof(JourneyPageTrackerFilterAttribute))]
    public IActionResult Index()
    {
        ViewBag.previousPage = HttpUtility.HtmlEncode(_config["BaseUrl"]);
        var _EligibilityJourneyWhereDoYouWantToBuyAHome = new EligibilityJourneyWhereDoYouWantToBuyAHome();
        return View(_EligibilityJourneyWhereDoYouWantToBuyAHome);
    }

    [HttpPost]
    public IActionResult Index(EligibilityJourneyWhereDoYouWantToBuyAHome _EligibilityJourneyWhereDoYouWantToBuyAHome)
    {
        var applicationCookie = CookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
        var cookie = applicationCookie.EligibilityResponses.Value;
        cookie.EligibilityJourneyWhereDoYouWantToBuyAHome = _EligibilityJourneyWhereDoYouWantToBuyAHome;

        applicationCookie.EligibilityResponses.Value = cookie;
        cookie.PreviousPageBeforeErrorOutcome = nameof(Index);

        ViewBag.previousPage = HttpUtility.HtmlEncode(_config["BaseUrl"]);

        if (_EligibilityJourneyWhereDoYouWantToBuyAHome.LiveInLondon == true)
        {
            cookie.EligibilityOutcome = "London";
            applicationCookie.EligibilityResponses.Value = cookie;
            return Redirect("/check-eligibility-to-buy-a-shared-ownership-home/continue-on-the-homes-for-londoners-website");
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
    [Route("are-you-buying-with-another-person")]
    [ServiceFilter(typeof(JourneyPageTrackerFilterAttribute))]
    public IActionResult BuyingWithAnotherPerson()
    {
        if(RequiresInitialization()) return RedirectToAction(nameof(Index));

        ViewBag.previousPage = HttpUtility.HtmlEncode(this.Url.Action(nameof(Index), "Eligibility"));
        return View();
    }

    [HttpPost]
    [Route("are-you-buying-with-another-person")]
    public IActionResult BuyingWithAnotherPerson(EligibilityJourneyBuyingWithAnotherPerson _EligibilityJourneyBuyingWithAnotherPerson)
    {
        if (RequiresInitialization()) return RedirectToAction(nameof(Index));

        var applicationCookie = CookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
        var cookie = applicationCookie.EligibilityResponses.Value;
        cookie.EligibilityJourneyBuyingWithAnotherPerson = _EligibilityJourneyBuyingWithAnotherPerson;
        applicationCookie.EligibilityResponses.Value = cookie;
        ViewBag.previousPage = HttpUtility.HtmlEncode(this.Url.Action(nameof(Index), "Eligibility"));

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
    [Route("how-much-do-you-earn")]
    [ServiceFilter(typeof(JourneyPageTrackerFilterAttribute))]
    public IActionResult HowMuchDoYouEarn()
    {
        if (RequiresInitialization()) return RedirectToAction(nameof(Index));

        ViewBag.previousPage = HttpUtility.HtmlEncode(this.Url.Action(nameof(BuyingWithAnotherPerson), "Eligibility"));
        return View();
    }

    [HttpPost]
    [Route("how-much-do-you-earn")]
    public IActionResult HowMuchDoYouEarn(EligibilityJourneyHowMuchDoYouEarn _EligibilityJourneyHowMuchDoYouEarn)
    {
        if (RequiresInitialization()) return RedirectToAction(nameof(Index));

        var applicationCookie = CookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
        var cookie = applicationCookie.EligibilityResponses.Value;
        cookie.PreviousPage = nameof(HowMuchDoYouEarn);
        cookie.PreviousPageBeforeErrorOutcome = nameof(HowMuchDoYouEarn);
        cookie.EligibilityJourneyHowMuchDoYouEarn = _EligibilityJourneyHowMuchDoYouEarn;        
        applicationCookie.EligibilityResponses.Value = cookie;
        ViewBag.previousPage = HttpUtility.HtmlEncode(this.Url.Action(nameof(BuyingWithAnotherPerson), "Eligibility"));

        if (_EligibilityJourneyHowMuchDoYouEarn.SingleIncomeOver80 == true)
        {
            cookie.EligibilityOutcome = "Over80K";
            applicationCookie.EligibilityResponses.Value = cookie;
            return Redirect("./you-may-not-be-eligible-to-buy-a-shared-ownership-home");
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
    [Route("how-much-do-you-both-earn")]
    [ServiceFilter(typeof(JourneyPageTrackerFilterAttribute))]
    public IActionResult HowMuchDoYouEarn_MultiplePeople()
    {
        if (RequiresInitialization()) return RedirectToAction(nameof(Index));

        ViewBag.previousPage = HttpUtility.HtmlEncode(this.Url.Action(nameof(BuyingWithAnotherPerson), "Eligibility"));
        return View();
    }

    [HttpPost]
    [Route("how-much-do-you-both-earn")]
    public IActionResult HowMuchDoYouEarn_MultiplePeople(EligibilityJourneyHowMuchDoYouEarn_MultiplePeople _EligibilityJourneyHowMuchDoYouEarn_MultiplePeople)
    {
        if (RequiresInitialization()) return RedirectToAction(nameof(Index));

        var applicationCookie = CookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
        var cookie = applicationCookie.EligibilityResponses.Value;
        cookie.PreviousPage = nameof(HowMuchDoYouEarn_MultiplePeople);
        cookie.PreviousPageBeforeErrorOutcome = nameof(HowMuchDoYouEarn_MultiplePeople);
        cookie.EligibilityJourneyHowMuchDoYouEarn_MultiplePeople = _EligibilityJourneyHowMuchDoYouEarn_MultiplePeople;
        applicationCookie.EligibilityResponses.Value = cookie;
        
        ViewBag.previousPage = HttpUtility.HtmlEncode(this.Url.Action(nameof(BuyingWithAnotherPerson), "Eligibility"));

        if (_EligibilityJourneyHowMuchDoYouEarn_MultiplePeople.JointIncomeOver80 == true)
        {
            cookie.EligibilityOutcome = "Over80K";
            applicationCookie.EligibilityResponses.Value = cookie;
            return Redirect("./you-may-not-be-eligible-to-buy-a-shared-ownership-home");
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
    [Route("select-all-that-apply-to-you")]
    [ServiceFilter(typeof(JourneyPageTrackerFilterAttribute))]
    public IActionResult FirstTimeBuyer()
    {
        if (RequiresInitialization()) return RedirectToAction(nameof(Index));

        var applicationCookie = CookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
        var cookie = applicationCookie.EligibilityResponses.Value;
        ViewBag.previousPage = HttpUtility.HtmlEncode(this.Url.Action(cookie.PreviousPage, "Eligibility"));

        return View();
    }


    [HttpPost]
    [Route("select-all-that-apply-to-you")]
    public IActionResult FirstTimeBuyer(EligibilityJourneyFirstTimeBuyer _EligibilityJourneyFirstTimeBuyer)
    {
        if (RequiresInitialization()) return RedirectToAction(nameof(Index));

        var applicationCookie = CookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
        var cookie = applicationCookie.EligibilityResponses.Value;
        cookie.EligibilityJourneyFirstTimeBuyer = _EligibilityJourneyFirstTimeBuyer;
        applicationCookie.EligibilityResponses.Value = cookie;
        cookie.PreviousPageBeforeErrorOutcome = nameof(FirstTimeBuyer);
        ViewBag.previousPage = HttpUtility.HtmlEncode(this.Url.Action(cookie.PreviousPage, "Eligibility"));

        ViewBag.FirstTimeBuyerCheck = _EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == true ? ViewBag.FirstTimeBuyerCheck = "checked" : ViewBag.FirstTimeBuyerCheck = "";
        ViewBag.OwnAHomeButNeedToMoveCheck = _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == true ? ViewBag.OwnAHomeButNeedToMoveCheck = "checked" : ViewBag.OwnAHomeButNeedToMoveCheck = "";
        ViewBag.CannotAffordAHomeCheck = _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == true ? ViewBag.CannotAffordAHomeCheck = "checked" : ViewBag.CannotAffordAHomeCheck = "";
        ViewBag.TheseDoNotApplyCheck = _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == true ? ViewBag.TheseDoNotApplyCheck = "checked" : ViewBag.TheseDoNotApplyCheck = "";

        //Nothing is selected
        if (
            _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == null &&
            _EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == null &&
            _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == null &&
            _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null
        )
        {
            ViewBag.nothingChosen = true;
            ViewBag.conflictingChoicesChosen = false;
            ViewBag.all4ChoicesChosen = false;
            return View(_EligibilityJourneyFirstTimeBuyer);
        }

        if (_EligibilityJourneyFirstTimeBuyer.theseDoNotApply == true
            && _EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == null
            && _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == null
            && _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == null)
        {
            ViewBag.nothingChosen = false;
            ViewBag.conflictingChoicesChosen = false;
            ViewBag.all4ChoicesChosen = false;
            cookie.EligibilityOutcome = "NotEligable";
            applicationCookie.EligibilityResponses.Value = cookie;
            return Redirect("./you-may-not-be-eligible-to-buy-a-shared-ownership-home");
        }

        if (_EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == true
            && _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == null
            && _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == null
            && _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null)
        {
            ViewBag.nothingChosen = false;
            ViewBag.conflictingChoicesChosen = false;
            ViewBag.all4ChoicesChosen = false;
            cookie.EligibilityOutcome = "NotEligable";
            applicationCookie.EligibilityResponses.Value = cookie;
            return Redirect("./you-may-not-be-eligible-to-buy-a-shared-ownership-home");
        }

        if (_EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == true
            && _EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == null
            && _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == null
            && _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null)
        {
            ViewBag.nothingChosen = false;
            ViewBag.conflictingChoicesChosen = false;
            ViewBag.all4ChoicesChosen = false;
            cookie.EligibilityOutcome = "NotEligable";
            applicationCookie.EligibilityResponses.Value = cookie;
            return Redirect("./you-may-not-be-eligible-to-buy-a-shared-ownership-home");
        }


        if (_EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == true
            && _EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == null
            && _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == null
            && _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null)
        {
            ViewBag.nothingChosen = false;
            ViewBag.conflictingChoicesChosen = false;
            ViewBag.all4ChoicesChosen = false;
            cookie.EligibilityOutcome = "NotEligable";
            applicationCookie.EligibilityResponses.Value = cookie;
            return Redirect("./you-may-not-be-eligible-to-buy-a-shared-ownership-home");
        }

        if (_EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == true
            && _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == true
            && _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == null
            && _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null)
        {
            ViewBag.nothingChosen = false;
            ViewBag.all4ChoicesChosen = false;
            ViewBag.conflictingChoicesChosen = true;
            return View(_EligibilityJourneyFirstTimeBuyer);
        }

        if (_EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null
            && _EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == null
            && _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == null
            && _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == null)
        {
            ViewBag.nothingChosen = false;
            ViewBag.all4ChoicesChosen = false;
            ViewBag.conflictingChoicesChosen = false;
            return View(_EligibilityJourneyFirstTimeBuyer);
        }

        if (_EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == true
            && _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == true
            && _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == true
            && _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null)
        {
            ViewBag.nothingChosen = false;
            ViewBag.all4ChoicesChosen = false;
            ViewBag.conflictingChoicesChosen = true;
            return View(_EligibilityJourneyFirstTimeBuyer);
        }

        if (_EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == true
            && _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == null
            && _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == true
            && _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null)
        {
            ViewBag.nothingChosen = false;
            ViewBag.all4ChoicesChosen = false;
            ViewBag.conflictingChoicesChosen = false;
            cookie.EligibilityOutcome = "Eligable";
            applicationCookie.EligibilityResponses.Value = cookie;
            return Redirect("./you-may-be-eligible-to-buy-a-shared-ownership-home");
        }

        if (_EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == null
            && _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == true
            && _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == true
            && _EligibilityJourneyFirstTimeBuyer.theseDoNotApply == null)
        {
            ViewBag.nothingChosen = false;
            ViewBag.all4ChoicesChosen = false;
            ViewBag.conflictingChoicesChosen = false;
            cookie.EligibilityOutcome = "Eligable";
            applicationCookie.EligibilityResponses.Value = cookie;
            return Redirect("./you-may-be-eligible-to-buy-a-shared-ownership-home");
        }

        //Users can only select from one group of options at a time
        if (_EligibilityJourneyFirstTimeBuyer.theseDoNotApply == true &

                 _EligibilityJourneyFirstTimeBuyer.cannotAffordAHome == true ||
                 _EligibilityJourneyFirstTimeBuyer.firstTimeBuyer == true ||
                 _EligibilityJourneyFirstTimeBuyer.ownAHomeButNeedToMove == true)
        {
            ViewBag.nothingChosen = false;
            ViewBag.conflictingChoicesChosen = false;
            ViewBag.all4ChoicesChosen = true;
            return View(_EligibilityJourneyFirstTimeBuyer);
        }
        return View(_EligibilityJourneyFirstTimeBuyer);
    }
    
    //page 5
    [HttpGet]
    [Route("you-may-be-eligible-to-buy-a-shared-ownership-home")]
    [Route("you-may-not-be-eligible-to-buy-a-shared-ownership-home")]
    [Route("continue-on-the-homes-for-londoners-website")]
    [ServiceFilter(typeof(JourneyPageTrackerFilterAttribute))]
    public IActionResult EligibilityOutcome()
    {
        if (RequiresInitialization()) return RedirectToAction(nameof(Index));

        var applicationCookie = CookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
        var cookie = applicationCookie.EligibilityResponses.Value;
        ViewBag.previousPage = HttpUtility.HtmlEncode(this.Url.Action(cookie.PreviousPageBeforeErrorOutcome, "Eligibility"));

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

    private bool RequiresInitialization()
    {
        var applicationCookie = CookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
        return !applicationCookie.EligibilityResponses.IsSet;
    }
}