using System.Reflection;
using Find_Register.Models;

namespace Find_Register.Cookies;

/// <summary>
/// This trait interface contains reusable functions for cookie Data Models using [CookieSettings] attribute.
/// </summary>
public interface ICookieDataWrapperInitializationTrait
{
    /// <summary>
    /// Call this from the inheriting class constructor to initialize cookie data wrappers.
    /// </summary>
    /// <param name="requestCookies"></param>
    /// <param name="responseCookies"></param>
    public void InitializeCookieDataWrappers(IRequestCookieCollection requestCookies, IResponseCookies responseCookies) 
    {
        var props = GetType().GetProperties().Where(prop => prop.IsDefined(typeof(CookieSettingsAttribute), false));
        foreach (var prop in props)
        {
            InitializeCookieDataWrapper(requestCookies, responseCookies, prop);
        }
    }

    private void InitializeCookieDataWrapper(IRequestCookieCollection requestCookies, IResponseCookies responseCookies, PropertyInfo prop)
    {
        var propCookieSettings = prop.GetCustomAttribute<CookieSettingsAttribute>();
        if (propCookieSettings == null) return;

        var isSet = requestCookies.Any(kv => kv.Key.Equals(propCookieSettings.GetCookieName));
        string? cookieValue = isSet ? requestCookies.FirstOrDefault(kv => kv.Key.Equals(propCookieSettings.GetCookieName)).Value : null;

        var cookieOptions = propCookieSettings.GenerateCookieOptions();

        if (prop.PropertyType == typeof(CookieDataWrapper<bool>))
        {
            prop.SetValue(this, CookieDataWrapper<bool>.GenerateBoolCookieDataWrapper(
                    isSet, cookieValue, propCookieSettings.GetCookieName, cookieOptions, responseCookies
                ));
        }
        else if (prop.PropertyType == typeof(CookieDataWrapper<string>))
        {
            prop.SetValue(this, CookieDataWrapper<string>.GenerateStringCookieDataWrapper(
                        isSet, cookieValue, propCookieSettings.GetCookieName, cookieOptions, responseCookies
                    ));
        }
        else if (prop.PropertyType.IsGenericType
            && prop.PropertyType.GetGenericTypeDefinition() == typeof(CookieDataWrapper<>)
            && prop.PropertyType.GenericTypeArguments.First() == typeof(EligibilityResponses)
            )
        {
            prop.SetValue(this, CookieDataWrapper<EligibilityResponses>.GenerateJsonResponsesCookieDataWrapper<EligibilityResponses>(
                        isSet, cookieValue, propCookieSettings.GetCookieName, cookieOptions, responseCookies
                    ));
        }
        else if (prop.PropertyType.IsGenericType
            && prop.PropertyType.GetGenericTypeDefinition() == typeof(CookieDataWrapper<>)
            && prop.PropertyType.GenericTypeArguments.First() == typeof(AnalyticSettings)
            )
        {
            prop.SetValue(this, CookieDataWrapper<AnalyticSettings>.GenerateJsonResponsesCookieDataWrapper<AnalyticSettings>(
                        isSet, cookieValue, propCookieSettings.GetCookieName, cookieOptions, responseCookies
                    ));
        }
        else
        {
            throw new NotImplementedException("Unhandled cookie data wrapper type");
        }
    }
}