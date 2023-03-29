using Find_Register.Cookies;
using Find_Register.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Find_Register.Filters;

public class JourneyPageTrackerFilterAttribute : ActionFilterAttribute
{
    private ICookieHelper _cookieHelper;
    public JourneyPageTrackerFilterAttribute(ICookieHelper cookieHelper) {
        _cookieHelper = cookieHelper;
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var applicationCookies = _cookieHelper.GetApplicationCookieData(context.HttpContext.Request.Cookies, context.HttpContext.Response.Cookies);
        var cookieValue = applicationCookies.EligibilityResponses.Value;
        cookieValue.LastJourneyPage = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
        applicationCookies.EligibilityResponses.Value = cookieValue;
    }
}
