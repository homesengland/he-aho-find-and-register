using Find_Register.Cookies;
using Find_Register.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Web;

namespace Find_Register.Controllers;

public abstract class BaseControllerWithShareStaticPages : Controller
{
    protected readonly ICookieHelper CookieHelper;

    protected BaseControllerWithShareStaticPages(ICookieHelper cookieHelper)
    {
        CookieHelper = cookieHelper;
    }

    [HttpGet]
    [Route("cookie-settings")]
    public IActionResult CookieSettings()
    {
        var applicationCookie = CookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);

        var model = new CookieSettings();            
        model.AcceptAnalyticsCookies = applicationCookie.AnalyticSettings.Value.AcceptAnalytics;
        model.BackUrl = applicationCookie.EligibilityResponses.Value.LastJourneyPage;

        return View(model);
    }

    [HttpPost]
    [Route("cookie-settings")]
    public IActionResult CookieSettings(CookieSettings model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var applicationCookie = CookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
        var analyticSettings = applicationCookie.AnalyticSettings.Value;
        analyticSettings.AcceptAnalytics = model.AcceptAnalyticsCookies ?? false;
        if (model.AcceptAnalyticsCookies == true && analyticSettings.GoogleAnalyticsNoJsClientId == null)
        {
            analyticSettings.GoogleAnalyticsNoJsClientId = Guid.NewGuid();
        }
        applicationCookie.AnalyticSettings.Value = analyticSettings;

        if (Url.IsLocalUrl(model.BackUrl))
        {
            return Redirect(model.BackUrl);
        }
        return Redirect(".");
    }

    [HttpGet]
    [Route("cookie-policy")]
    public IActionResult CookiePolicy()
    {
        var applicationCookie = CookieHelper.GetApplicationCookieData(Request.Cookies, Response.Cookies);
        ViewBag.previousPage = applicationCookie.EligibilityResponses.Value.LastJourneyPage;
        return View();
    }

    [Route("contact-us")]
    public IActionResult ContactUs()
    {
        var applicationCookie = CookieHelper.GetApplicationCookieData(Request.Cookies, Response.Cookies);
        ViewBag.previousPage = applicationCookie.EligibilityResponses.Value.LastJourneyPage;
        return View();
    }

    [Route("accessibility")]
    public IActionResult Accessibility()
    {
        var applicationCookie = CookieHelper.GetApplicationCookieData(Request.Cookies, Response.Cookies);
        ViewBag.previousPage = applicationCookie.EligibilityResponses.Value.LastJourneyPage;
        ViewBag.FindSharedOwnershipLink = HttpUtility.HtmlEncode("/check-eligibility-to-buy-a-shared-ownership-home");
        ViewBag.FindSearchProviderLink = HttpUtility.HtmlEncode("/find-organisations-selling-shared-ownership-homes");
        return View();
    }

    [Route("error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    [Route("accept-all-cookies")]
    public IActionResult AcceptCookies()
    {
        var applicationCookie = CookieHelper.GetApplicationCookieData(Request.Cookies, Response.Cookies);
        applicationCookie.AnalyticSettings.Value = new AnalyticSettings { AcceptAnalytics = true };

        return Redirect(applicationCookie.EligibilityResponses.Value.LastJourneyPage);
    }

    [HttpPost]
    [Route("hide-confirmation")]
    public IActionResult HideConfirmation()
    {
        var applicationCookie = CookieHelper.GetApplicationCookieData(Request.Cookies, Response.Cookies);
        var analyticSettings = applicationCookie.AnalyticSettings.Value;
        analyticSettings.HideConfirmation = true;
        applicationCookie.AnalyticSettings.Value = analyticSettings;

        return Redirect(applicationCookie.EligibilityResponses.Value.LastJourneyPage);
    }
}

