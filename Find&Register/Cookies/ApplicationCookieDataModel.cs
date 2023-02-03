using Find_Register.Models;

namespace Find_Register.Cookies;

public class ApplicationCookieDataModel: ICookieDataWrapperInitializationTrait
{

#pragma warning disable CS8618 // Properties decorated with CookieSettings are assigned through reflection
    public ApplicationCookieDataModel(IRequestCookieCollection requestCookies, IResponseCookies responseCookies)
#pragma warning restore CS8618
    {
        (this as ICookieDataWrapperInitializationTrait).InitializeCookieDataWrappers(requestCookies, responseCookies);
        AnalyticSettings?.SetAdditionalUpdateCallback((val) => SetAnalyticsCookiePreference(val, responseCookies));
    }

    [CookieSettings("analytic-settings", ExpiryDays = 365)]
    public CookieDataWrapper<AnalyticSettings> AnalyticSettings { get; private set; }


    [CookieSettings("eligibility-responses")]
    public CookieDataWrapper<EligibilityResponses> EligibilityResponses { get; private set; }


    // Static list of analytics cookies that should be deleted if user no longer accepts analytic cookies
    internal static readonly string[] AnalyticsCookieNames = { "ai_session", "ai_user", "__utma", "__utmb", "__utmc" };

    /// <summary>
    /// Callback to remove analytics cookies when user declines analytic cookies
    /// </summary>
    public static void SetAnalyticsCookiePreference(AnalyticSettings analyticSettings, IResponseCookies responseCookies)
    {
        if (analyticSettings.AcceptAnalytics) return;
        foreach (var analyticCookie in AnalyticsCookieNames)
        {
            responseCookies.Delete(analyticCookie);
        }
    }
}
