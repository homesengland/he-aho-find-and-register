using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Find_Register.Models;
using Find_Register.Filters;
using Microsoft.Extensions.Logging;
using Find_Register.Cookies;

namespace Find_Register.Controllers;

[TypeFilter(typeof(UnhandledExceptionFilter))]
public class HomeController : Controller
{ 
    private ICookieHelper _cookieHelper;
    ILogger<HomeController> _logger { get; set; }
    public HomeController(ILogger<HomeController> logger, ICookieHelper cookieHelper)
    {
        _logger = logger;
        _cookieHelper = cookieHelper;
    }

    public IActionResult Index()
    {
        var _gdsModel = new PageOne();
        return View(_gdsModel);
    }

    [HttpPost]
    public IActionResult Index(PageOne _gdsModel)
    {        
       return View(_gdsModel);
    }

    [Route("own-home")]
    [HttpGet]
    public IActionResult OwnHome()
    {
        // the view is not created but this boilerplate is created for redirection from current-situation page
        return View();
    }

    [HttpPost]
    [Route("annual-income")]
    public IActionResult AnnualIncome(AnnualIncome model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var applicationCookie = _cookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
        var cookie = applicationCookie.EligibilityResponses.Value;
        cookie.AnnualIncome = model;
        applicationCookie.EligibilityResponses.Value = cookie;

        return RedirectToAction(nameof(CurrentSituation));
    }

    [HttpGet]
    [Route("annual-income")]
    public IActionResult AnnualIncome()
    {
        return View();
    }

    [HttpGet]
    [Route("current-situation")]
    public IActionResult CurrentSituation()
    {
        return View();
    }

    [HttpPost]
    [Route("current-situation")]
    public IActionResult CurrentSituation(CurrentSituation model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var applicationCookie = _cookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
        var cookie = applicationCookie.EligibilityResponses.Value;
        cookie.CurrentSituation = model;
        applicationCookie.EligibilityResponses.Value = cookie;

        return RedirectToAction(nameof(OwnHome));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}