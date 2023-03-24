namespace Find_Register.Cookies;

[AttributeUsage(AttributeTargets.Property)]
public class CookieSettingsAttribute : Attribute
{
    private string _cookieName;

    /// <summary>
    /// CookieSettings attribute marks a property as cookie data. This should be used in
    /// conjecture with <see cref="ICookieDataWrapperInitializationTrait"/> to manage a cookie
    /// data model.
    /// </summary>
    /// <param name="cookieName">Name used to store this cookie. Each cookie must have a unique cookie name</param>
    public CookieSettingsAttribute(string cookieName)
    {
        _cookieName = cookieName;
    }

    public string GetCookieName => _cookieName;

    /// <summary>
    /// Optional - number of days before this cookie expires.
    /// When this is not set, the cookie will expire at the end of the session. (depending on browser)
    /// </summary>
    public int ExpiryDays;


    /// <summary>
    /// Generate cookie options using the settings in this cookie.
    /// 
    /// Cookies declared through this attribute are expected to be functional application cookies
    /// and are thus marked essential. Analytics cookies are expected be managed through their
    /// own packages with the exception of the generic analytics acceptance cookie.
    /// <see cref="CookieHelper.AcceptAnalyticCookies" />
    /// </summary>
    /// <returns>CookieOption object that should be used when setting this cookie</returns>
    public CookieOptions GenerateCookieOptions()
    {
        var cookieOptions = new CookieOptions { IsEssential = true, Secure = true, SameSite = SameSiteMode.Strict, HttpOnly = true};
        if (ExpiryDays > 0) cookieOptions.Expires = DateTimeOffset.Now.AddDays(ExpiryDays);
        return cookieOptions;
    }
}