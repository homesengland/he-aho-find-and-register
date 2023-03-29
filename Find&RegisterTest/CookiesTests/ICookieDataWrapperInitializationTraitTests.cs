using Find_Register.Cookies;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Find_RegisterTest.CookiesTests;

public class ICookieDataWrapperInitializationTraitTests
{
    [Fact]
    public void CookieDataWrappersAreInitializedAfterCallingInitializationTraitMethod()
    {
        var mockRequestCookies = new RequestCookiesTestCollection();
        var mockResponseCookies = new Mock<IResponseCookies>();

        var mockDataModel = new MockDataModelForUnitTest();

        Assert.Null(mockDataModel.TestExpiryCookie);
        Assert.Null(mockDataModel.TestWithoutExpiryCookie);

        mockDataModel.Initialize(mockRequestCookies, mockResponseCookies.Object);

        Assert.NotNull(mockDataModel.TestExpiryCookie);
        Assert.NotNull(mockDataModel.TestWithoutExpiryCookie);
    }

    [Fact]
    public void ExpiryIsSetCorrectlyInCookieOptions()
    {
        var mockRequestCookies = new RequestCookiesTestCollection();
        var mockResponseCookies = new Mock<IResponseCookies>();
        CookieOptions ?lastCookieOptions = null;
        mockResponseCookies.Setup(m => m.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()))
            .Callback((string a, string b, CookieOptions c) => lastCookieOptions = c);

        var mockDataModel = new MockDataModelForUnitTest();
        mockDataModel.Initialize(mockRequestCookies, mockResponseCookies.Object);
        mockDataModel.TestExpiryCookie!.Value = "aaabbbccc";

        Assert.NotNull(lastCookieOptions!.Expires);
        Assert.InRange((DateTimeOffset)lastCookieOptions.Expires, DateTimeOffset.Now.AddDays(1).AddSeconds(-1), DateTimeOffset.Now.AddDays(1).AddSeconds(1));
        // TestExpiryCookie has an expires attribute, this is set on the cookie to the right time, allowing for small difference for test runtime

        mockDataModel.TestWithoutExpiryCookie!.Value = true;

        Assert.Null(lastCookieOptions.Expires);
        // TestWithoutExpiryCookie does not have this attribute - in which case the cookie should expire on session end (depending on browser).
    }

    private class MockDataModelForUnitTest : ICookieDataWrapperInitializationTrait
    {
        public void Initialize(IRequestCookieCollection requestCookies, IResponseCookies responseCookies) {
            (this as ICookieDataWrapperInitializationTrait).InitializeCookieDataWrappers(requestCookies, responseCookies);
        }

        [CookieSettings("test-expiry-cookie", ExpiryDays = 1)]
        public CookieDataWrapper<string> ?TestExpiryCookie { get; private set; }

        [CookieSettings("test-without-expiry-cookie")]
        public CookieDataWrapper<bool> ?TestWithoutExpiryCookie { get; private set; }
    }
}