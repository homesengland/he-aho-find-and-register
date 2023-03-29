using Find_Register.Models;
using System.Text.RegularExpressions;

namespace Find_Register.Cookies;

public class ApplicationCookieDataModel: ICookieDataWrapperInitializationTrait
{

#pragma warning disable CS8618 // Properties decorated with CookieSettings are assigned through reflection
    public ApplicationCookieDataModel(IRequestCookieCollection requestCookies, IResponseCookies responseCookies)
#pragma warning restore CS8618
    {
        (this as ICookieDataWrapperInitializationTrait).InitializeCookieDataWrappers(requestCookies, responseCookies);
        AnalyticSettings?.SetAdditionalUpdateCallback((val) => SetAnalyticsCookiePreference(val, responseCookies, requestCookies));
    }

    [CookieSettings("analytic-settings", ExpiryDays = 365)]
    public CookieDataWrapper<AnalyticSettings> AnalyticSettings { get; private set; }


    [CookieSettings("eligibility-responses")]
    public CookieDataWrapper<EligibilityResponses> EligibilityResponses { get; private set; }


    // Static list of analytics cookies that should be deleted if user no longer accepts analytic cookies
    internal static readonly string[] AnalyticsCookieRegex = { "^ai_session$", "^ai_user$", "^_ga$", "^_ga_.+$", @"^__utm[a-z]$", };

    /// <summary>
    /// Callback to remove analytics cookies when user declines analytic cookies
    /// </summary>
    public static void SetAnalyticsCookiePreference(AnalyticSettings analyticSettings, IResponseCookies responseCookies, IRequestCookieCollection requestCookies)
    {
        if (analyticSettings.AcceptAnalytics) return;

        foreach (var regex in AnalyticsCookieRegex)
        {
            var cookieRegex = new Regex(regex);
            var matchedCookies = requestCookies
                .Select(cookie => cookie.Key)
                .Where(key => cookieRegex.IsMatch(key))
                .ToList();
            matchedCookies.ForEach(cookie => responseCookies.Delete(cookie));
        }
    }
}
