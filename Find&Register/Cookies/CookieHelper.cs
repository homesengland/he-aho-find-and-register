namespace Find_Register.Cookies;

/// <summary>
/// Helper class to set and retrieve cookie data.
/// Usage: Keep this scoped to ensure cookie data does not leak between requests.
/// </summary>
public class CookieHelper : ICookieHelper
{
    private ApplicationCookieDataModel? _applicationCookieData;

    /// <summary>
    /// Generates the application cookie data model from request cookies or
    /// returns the current request's cookie data if already generated.
    /// </summary>
    public ApplicationCookieDataModel GetApplicationCookieData(IRequestCookieCollection? requestCookies, IResponseCookies? responseCookies)
    {
        if (requestCookies == null || responseCookies == null) return _applicationCookieData
                ?? throw new NullReferenceException("Cookie collections cannot be null in cookie helper");
        return _applicationCookieData ??= new ApplicationCookieDataModel(requestCookies, responseCookies);
    }
}