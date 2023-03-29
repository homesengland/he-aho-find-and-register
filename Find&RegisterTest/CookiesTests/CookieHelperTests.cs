using System.Web;
using Find_Register.Cookies;
using Find_Register.Models;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Find_RegisterTest.CookiesTests;

public class CookieHelperTests
{
    [Fact]
    public void ApplicationCookieDataIsNotNull()
    {
        var mockRequestCookies = new RequestCookiesTestCollection();
        var mockResponseCookies = new Mock<IResponseCookies>();

        var helper = new CookieHelper();
        var cookieData = helper.GetApplicationCookieData(mockRequestCookies, mockResponseCookies.Object);
        Assert.NotNull(cookieData);
        Assert.NotNull(cookieData.AnalyticSettings);
        // Ensure properties are initialized on construction

        var cookieData2 = helper.GetApplicationCookieData(null, null);
        Assert.Equal(cookieData, cookieData2);
        // subsequent calls to get cookie data returns already generated cookie data
    }

    [Fact]
    public void AcceptAnalyticsCookiesReturnsCorrectValue()
    {
        var mockRequestCookies = new RequestCookiesTestCollection();
        var mockResponseCookies = new Mock<IResponseCookies>();

        var helper = new CookieHelper();
        var cookieData = helper.GetApplicationCookieData(mockRequestCookies, mockResponseCookies.Object);
        Assert.False(cookieData.AnalyticSettings.Value.AcceptAnalytics);
        // False when not set

        helper = new CookieHelper();
        mockRequestCookies["analytic-settings"] = Base64Encode("{AcceptAnalytics:true, HideConfirmation: true}");

        cookieData = helper.GetApplicationCookieData(mockRequestCookies, mockResponseCookies.Object);
        Assert.True(cookieData.AnalyticSettings.Value.AcceptAnalytics);
        // Explicitly set to true

        helper = new CookieHelper();
        mockRequestCookies["analytic-settings"] = Base64Encode("{AcceptAnalytics:false, HideConfirmation: true}");

        cookieData = helper.GetApplicationCookieData(mockRequestCookies, mockResponseCookies.Object);
        Assert.False(cookieData.AnalyticSettings.Value.AcceptAnalytics);
        // Explicitly set to false
    }


    [Fact]
    public void SetAnalyticsCookiePreferenceToFalseDeletesAnnalyticsCookies()
    {
        var mockRequestCookies = new RequestCookiesTestCollection();
        var mockResponseCookies = new Mock<IResponseCookies>();

        mockRequestCookies.Add("_ga_A643FC42", Base64Encode("aaa"));
        mockRequestCookies.Add("_ga", Base64Encode("bbb"));
        mockRequestCookies.Add("__utmc", Base64Encode("ccc"));
        mockRequestCookies.Add("ai_user", Base64Encode("sam"));
        mockRequestCookies.Add("ai_session", Base64Encode("frodo"));

        var helper = new CookieHelper();
        var cookieData = helper.GetApplicationCookieData(mockRequestCookies, mockResponseCookies.Object);
        cookieData.AnalyticSettings.Value = new AnalyticSettings { AcceptAnalytics = false};

        mockResponseCookies.Verify(m => m.Delete(It.IsAny<string>()), Times.Exactly(5));
    }

    [Fact]
    public void SetAnalyticsCookiePreferenceToTrueDoesNotDeleteAnnalyticsCookies()
    {
        var mockRequestCookies = new RequestCookiesTestCollection();
        var mockResponseCookies = new Mock<IResponseCookies>();

        var helper = new CookieHelper();
        var cookieData = helper.GetApplicationCookieData(mockRequestCookies, mockResponseCookies.Object);
        cookieData.AnalyticSettings.Value = new AnalyticSettings { AcceptAnalytics = true };

        mockResponseCookies.Verify(m => m.Delete(It.IsAny<string>()), Times.Never);
    }

    private static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

}