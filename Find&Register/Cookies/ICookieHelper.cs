using Microsoft.AspNetCore.Http;

namespace Find_Register.Cookies;

public interface ICookieHelper
{
    public ApplicationCookieDataModel GetApplicationCookieData(IRequestCookieCollection? requestCookies, IResponseCookies? responseCookies);
}

