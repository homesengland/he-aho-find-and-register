using Find_Register.Cookies;
using Find_Register.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.WebUtilities;
using System.Reflection;

namespace Find_Register.Controllers
{
    public class CookieController : Controller
    {
        private ICookieHelper _cookieHelper;

        public CookieController(ICookieHelper cookieHelper)
        {
            _cookieHelper = cookieHelper;
        }

        [Route("cookie-settings")]
        [HttpGet]
        public IActionResult CookieSettings()
        {
            var applicationCookie = _cookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);

            var model = new CookieSettings();            
            model.AcceptAnalyticsCookies = applicationCookie.AnalyticSettings.Value.AcceptAnalytics;
            model.BackUrl = HttpUtility.HtmlEncode(Request?.Headers.Referer);

            return View(model);
        }

        [Route("cookie-settings")]
        [HttpPost]
        public IActionResult CookieSettings(CookieSettings model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var applicationCookie = _cookieHelper.GetApplicationCookieData(Request?.Cookies, Response?.Cookies);
            var analyticSettings = applicationCookie.AnalyticSettings.Value;
            analyticSettings.AcceptAnalytics = model.AcceptAnalyticsCookies ?? false;
            if (model.AcceptAnalyticsCookies == true && analyticSettings.GoogleAnalyticsNoJsClientId == null)
            {
                analyticSettings.GoogleAnalyticsNoJsClientId = Guid.NewGuid();
            }
            applicationCookie.AnalyticSettings.Value = analyticSettings;

            return Redirect(model.BackUrl ?? "annual-income");
        }

        [Route("cookie-policy")]
        [HttpGet]
        public IActionResult CookiePolicy()
        {
            ViewBag.previousPage = HttpUtility.HtmlEncode(Request.Headers.Referer.ToString());
            return View();
        }

        [Route("accept-all-cookies")]
        [HttpPost]
        public IActionResult AcceptCookies()
        {
            var applicationCookie = _cookieHelper.GetApplicationCookieData(Request.Cookies, Response.Cookies);
            applicationCookie.AnalyticSettings.Value = new AnalyticSettings { AcceptAnalytics = true };

            return Redirect(HttpUtility.HtmlEncode(Request.Headers.Referer.ToString()));
        }

        [Route("cookie-settings/hide-confirmation")]
        [HttpPost]
        public IActionResult HideConfirmation()
        {
            var applicationCookie = _cookieHelper.GetApplicationCookieData(Request.Cookies, Response.Cookies);
            var analyticSettings = applicationCookie.AnalyticSettings.Value;
            analyticSettings.HideConfirmation = true;
            applicationCookie.AnalyticSettings.Value = analyticSettings;

            return Redirect(HttpUtility.HtmlEncode(Request.Headers.Referer.ToString()));
        }

    }
}

