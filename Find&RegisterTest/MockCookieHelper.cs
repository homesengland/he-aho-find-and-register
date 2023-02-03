using Find_Register.Cookies;
using Find_RegisterTest.CookiesTests;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Find_RegisterTest;

public class MockCookieHelper : ICookieHelper
{
    private ApplicationCookieDataModel? _cookie;

    public ApplicationCookieDataModel GetApplicationCookieData(IRequestCookieCollection? requestCookies, IResponseCookies? responseCookies)
    {
        var mockRequestCookies = new RequestCookiesTestCollection();
        var mockResponseCookies = new Mock<IResponseCookies>();

        return _cookie ??= new ApplicationCookieDataModel(mockRequestCookies, mockResponseCookies.Object);
    }
}
