using System.Text;
using Azure.Core;
using Newtonsoft.Json;

namespace Find_Register.Cookies;

/// <summary>
/// Cookie Data Wrapper is a helper to allow data models to quickly
/// configure new cookies and ensures updates are passed to the underlaying
/// Asp.net httpResponse mechanisms.
///
/// The data wrappers contain the cookie values and references to update
/// callback against an httpResponse object and are thus specific to a
/// particular cookie for a particular request.
/// </summary>
/// <typeparam name="T">Underlaying data type</typeparam>
public class CookieDataWrapper<T>
{

    private Action<T> _updateCookieCallback;
    private Action<T>? _addtionalUpdateCallback;
    private readonly Action _unSetCallback;
    /// <summary>
    /// Initializes a Cookie datawrapper for a particular cookie for a particular request.
    /// </summary>
    /// <param name="updateCookieCallback">callback to add or modify this cookie on the HttpResponse object</param>
    /// <param name="unSetCallback">callback to expire this cookie on the HttpResponse object</param>
    private CookieDataWrapper(T value, bool isSet, Action<T> updateCookieCallback, Action unSetCallback)
    {
        _value = value;
        IsSet = isSet;
        _updateCookieCallback = updateCookieCallback;
        _unSetCallback = unSetCallback;
    }

    public void SetAdditionalUpdateCallback(Action<T> updateCallBack)
    {
        _addtionalUpdateCallback = updateCallBack;
    }

    public bool IsSet { get; private set; }

    private T _value;

    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            IsSet = true;
            _updateCookieCallback(value);
            _addtionalUpdateCallback?.Invoke(value);
        }
    }

    public void UnSet()
    {
        IsSet = false;
        _unSetCallback();
    }

    /// <summary>
    /// Static constructor for a boolean cookie data.
    /// 
    /// We differentiate these constructors by underlaying generic type to allow for
    /// different serialization / deserialization methods.
    /// When extending this class with additional types, we also need to extend the
    /// handling within the <see cref="ICookieDataWrapperInitializationTrait"/>
    /// </summary>
    /// <param name="isSet">If this cookie has already been set on the HttpRequest object</param>
    /// <param name="cookieValue">Current value of this cookie on the HttpRequest object</param>
    /// <param name="cookieName">Current value of this cookie on the HttpRequest object</param>
    /// <param name="cookieOptions">Current value of this cookie on the HttpRequest object</param>
    /// <param name="responseCookies">Cookie collection on the HttpResponse object that we append to when there are modifications</param>
    /// <returns>new cookie data wrapper object</returns>
    public static CookieDataWrapper<bool> GenerateBoolCookieDataWrapper(
                        bool isSet,
                        string? cookieValue,
                        string cookieName,
                        CookieOptions cookieOptions,
                        IResponseCookies responseCookies)
    {
        
        var currentValue = !isSet ? default : cookieValue != null && Base64Decode(cookieValue).Equals(true.ToString(), StringComparison.OrdinalIgnoreCase);

        var updateCookieCallback = (bool newValue) => {
            responseCookies.Append(cookieName, Base64Encode(newValue.ToString()), cookieOptions);
        };

        var unSetCallback = () => responseCookies.Delete(cookieName);

        return new CookieDataWrapper<bool>(currentValue, isSet, updateCookieCallback, unSetCallback);
    }

    public static CookieDataWrapper<string> GenerateStringCookieDataWrapper(
                    bool isSet,
                    string? cookieValue,
                    string cookieName,
                    CookieOptions cookieOptions,
                    IResponseCookies responseCookies)
    {
        var currentValue = !isSet ? "" : Base64Decode(cookieValue ?? "");

        var updateCookieCallback = (string newValue) => {
            responseCookies.Append(cookieName, Base64Encode(newValue), cookieOptions);
        };

        var unSetCallback = () => responseCookies.Delete(cookieName);

        return new CookieDataWrapper<string>(currentValue, isSet, updateCookieCallback, unSetCallback);
    }

    public static CookieDataWrapper<TU> GenerateJsonResponsesCookieDataWrapper<TU>(
                    bool isSet,
                    string? cookieValue,
                    string cookieName,
                    CookieOptions cookieOptions,
                    IResponseCookies responseCookies) where TU : struct
    {        
        var currentValue = (!isSet || cookieValue == null) ?
            default : JsonConvert.DeserializeObject<TU>(Base64Decode(cookieValue));

        var updateCookieCallback = (TU newValue) => {
            responseCookies.Append(cookieName, Base64Encode(JsonConvert.SerializeObject(newValue)), cookieOptions);
        };

        var unSetCallback = () => responseCookies.Delete(cookieName);

        return new CookieDataWrapper<TU>(currentValue, isSet, updateCookieCallback, unSetCallback);
    }

    private static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    private static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}
