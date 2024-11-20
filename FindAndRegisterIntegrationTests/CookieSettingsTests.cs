using System;
using System.Web;
using Find_Register.Models;
using FindAndRegisterIntegrationTests;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.TermStore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace FindAndRegisterIntegrationTests;

public class CookieSettingsTests : SeleniumTestsBase
{
    private string SearchUrl;
    public CookieSettingsTests()
    {
        SearchUrl = Host + "find-organisations-selling-shared-ownership-homes/";
        Host = Host + "check-eligibility-to-buy-a-shared-ownership-home/";
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void CookieSettings_PageLoads()
    {
        using IWebDriver driver = new ChromeDriver() ;
        driver.Navigate().GoToUrl(Host + "cookie-settings");
        
        Assert.Contains("Cookie Settings", driver.Title);
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void DefaultCookieSettings_IsDoNotAccept()
    {
        using IWebDriver driver = new ChromeDriver();
        driver.Manage().Cookies.DeleteAllCookies();
        driver.Navigate().GoToUrl(Host + "cookie-settings");
        
        var doNotAccept = driver.FindElement(By.Id("accept-no"));
        Assert.True(doNotAccept.Selected);

        var doAccept = driver.FindElement(By.Id("accept-yes"));
        Assert.False(doAccept.Selected);
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void PreviousCookieSettings_AreKept()
    {
        using IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl(Host);
        
        var actions = new Actions(driver);
        // open front page first so that the driver can fetch the context for cookie domains

        driver.Manage().Cookies.AddCookie(new Cookie("analytic-settings", Base64Encode(
                JsonConvert.SerializeObject(new AnalyticSettings { AcceptAnalytics = true, HideConfirmation = true }))
            ));
        driver.Navigate().GoToUrl(Host + "cookie-settings");
        actions.ScrollByAmount(0, 500).Perform();
        var doNotAccept = driver.FindElement(By.Id("accept-no"));
        Assert.False(doNotAccept.Selected);

        var doAccept = driver.FindElement(By.Id("accept-yes"));
        Assert.True(doAccept.Selected);


        driver.Manage().Cookies.AddCookie(new Cookie("analytic-settings", Base64Encode(
                JsonConvert.SerializeObject(new AnalyticSettings { AcceptAnalytics = false, HideConfirmation = true }))
            ));
        driver.Navigate().GoToUrl(Host + "cookie-settings");
        actions.ScrollByAmount(0, 500).Perform();
        doNotAccept = driver.FindElement(By.Id("accept-no"));
        Assert.True(doNotAccept.Selected);

        doAccept = driver.FindElement(By.Id("accept-yes"));
        Assert.False(doAccept.Selected);
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void OnDecliningCookies_analyticsCookiesAreRemovedAndAreNotReadded()
    {
        using IWebDriver driver = new ChromeDriver();
        var actions = new Actions(driver);

        driver.Manage().Cookies.DeleteAllCookies();
        driver.Navigate().GoToUrl(Host);
        
        actions.ScrollByAmount(0, 500).Perform();

        driver.Manage().Cookies.AddCookie(new Cookie("ai_session", Base64Encode("1231243234")));
        driver.Manage().Cookies.AddCookie(new Cookie("ai_user", Base64Encode("testuser")));

        driver.Manage().Cookies.AddCookie(new Cookie("_ga", Base64Encode("aaa")));
        driver.Manage().Cookies.AddCookie(new Cookie("_ga_S65GS35J678", Base64Encode("bbb")));

        driver.FindElement(By.Id("Cookies_link")).Click();
        actions.ScrollByAmount(0, 500).Perform();
        driver.FindElement(By.Id("accept-no")).Click();

        ClickSubmit(driver);

        Assert.Equal(Host, driver.Url);
        // user is returned to the previous page they were on

        var acceptAnalyticsCookie = driver.Manage().Cookies.GetCookieNamed("analytic-settings");
        var deserialized = JsonConvert.DeserializeObject<AnalyticSettings>(Base64Decode(acceptAnalyticsCookie.Value));
        Assert.False(deserialized.AcceptAnalytics);
        Assert.InRange(acceptAnalyticsCookie.Expiry!.Value, DateTime.Now.AddDays(364), DateTime.Now.AddDays(366));
        // analytics acceptance cookie has been set and expires in 365 days
        
        Assert.Null(driver.Manage().Cookies.GetCookieNamed("ai_session"));
        Assert.Null(driver.Manage().Cookies.GetCookieNamed("ai_user"));
        // application insight cookies have been removed

        Assert.Null(driver.Manage().Cookies.GetCookieNamed("_ga"));
        Assert.Null(driver.Manage().Cookies.GetCookieNamed("_ga_S65GS35J678"));
        // google analytics cookies have been removed

        driver.Navigate().GoToUrl(Host);

        Assert.Null(driver.Manage().Cookies.GetCookieNamed("ai_session"));
        Assert.Null(driver.Manage().Cookies.GetCookieNamed("ai_user"));
        // application insight cookies have not been readded after moving to a new page

        Assert.Null(driver.Manage().Cookies.GetCookieNamed("_ga"));
        Assert.Null(driver.Manage().Cookies.GetCookieNamed("_ga_S65GS35J678"));
        // google analytics cookies have not been readded after moving to a new page
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void OnAcceptingCookies_analyticsCookiesAreNotChanged()
    {
        using IWebDriver driver = new ChromeDriver();
        var actions = new Actions(driver);

        driver.Manage().Cookies.DeleteAllCookies();
        driver.Navigate().GoToUrl(Host);
        
        actions.ScrollByAmount(0, 500).Perform();
        

        driver.Manage().Cookies.AddCookie(new Cookie("ai_session", Base64Encode("1231243234")));
        driver.Manage().Cookies.AddCookie(new Cookie("ai_user", Base64Encode("testuser")));

        driver.Manage().Cookies.AddCookie(new Cookie("__utma", Base64Encode("Lorem ipsum dolor sit amet,")));
        driver.Manage().Cookies.AddCookie(new Cookie("__utmb", Base64Encode("consectetur adipiscing elit,")));
        driver.Manage().Cookies.AddCookie(new Cookie("__utmc", Base64Encode("sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.")));

        driver.FindElement(By.Id("Cookies_link")).Click();
        actions.ScrollByAmount(0, 500).Perform();
        driver.FindElement(By.Id("accept-yes")).Click();
        ClickSubmit(driver);

        Assert.Equal(Host, driver.Url);
        // user is returned to the previous page they were on

        var acceptAnalyticsCookie = driver.Manage().Cookies.GetCookieNamed("analytic-settings");
        var deserialized = JsonConvert.DeserializeObject<AnalyticSettings>(Base64Decode(acceptAnalyticsCookie.Value));
        Assert.True(deserialized.AcceptAnalytics);
        Assert.InRange(acceptAnalyticsCookie.Expiry!.Value, DateTime.Now.AddDays(364), DateTime.Now.AddDays(366));
        // analytics acceptance cookie has been set and expires in 365 days

        Assert.Equal(Base64Encode("1231243234"), driver.Manage().Cookies.GetCookieNamed("ai_session").Value);
        Assert.Equal(Base64Encode("testuser"), driver.Manage().Cookies.GetCookieNamed("ai_user").Value);
        // application insight cookies have been removed

        Assert.Equal(Base64Encode("Lorem ipsum dolor sit amet,"), driver.Manage().Cookies.GetCookieNamed("__utma").Value);
        Assert.Equal(Base64Encode("consectetur adipiscing elit,"), driver.Manage().Cookies.GetCookieNamed("__utmb").Value);
        Assert.Equal(Base64Encode("sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."), driver.Manage().Cookies.GetCookieNamed("__utmc").Value);
        // google analytics cookies have been removed
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void CookieSettings_BackButtonRedirectsToPreviousPag()
    {
        using IWebDriver driver = new ChromeDriver();
        var actions = new Actions(driver);

        driver.Navigate().GoToUrl(SearchUrl);
        driver.Navigate().GoToUrl(Host + "cookie-policy");
        
        actions.ScrollByAmount(0, 1000);
        driver.FindElement(By.Id("setting-link")).Click();
        driver.FindElement(By.ClassName("govuk-back-link")).Click();
        Assert.Equal(SearchUrl, driver.Url);

        driver.Navigate().GoToUrl(Host);
        actions.ScrollByAmount(0, 1000);
        driver.FindElement(By.Id("Cookies_link")).Click();
        driver.FindElement(By.ClassName("govuk-back-link")).Click();
        Assert.Equal(Host, driver.Url);
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void CookiePolicy_PageLoads()
    {
        using IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl(Host + "cookie-policy");
        Assert.Contains("Cookie Policy", driver.Title);
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void CookiePolicy_LinkToSettingsAndBackWorks()
    {
        using IWebDriver driver = new ChromeDriver();
        var actions = new Actions(driver);

        driver.Navigate().GoToUrl(Host + "cookie-policy");

        actions.ScrollByAmount(0, 1000);
        actions.Perform();

        driver.FindElement(By.Id("setting-link")).Click();
        Assert.Equal(Host + "cookie-settings", driver.Url);

        actions.ScrollByAmount(0, 1000);
        actions.Perform();
        driver.FindElement(By.Id("policy-link")).Click();
        Assert.Equal(Host + "cookie-policy", driver.Url);
    }


    [Fact]
    [Trait("Selenium", "Smoke")]
    public void CookiePolicy_BackButtonRedirectsToPreviousJourneyPage()
    {
        using IWebDriver driver = new ChromeDriver();
        var actions = new Actions(driver);

        driver.Navigate().GoToUrl(Host);
        driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
        driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
        Assert.NotEqual(Host, driver.Url);
        var fromUrl = driver.Url;

        driver.Navigate().GoToUrl(Host + "cookie-settings");
        actions.ScrollByAmount(0, 1000);
        driver.FindElement(By.Id("policy-link")).Click();
        Assert.NotEqual(fromUrl, driver.Url);
        driver.FindElement(By.ClassName("govuk-back-link")).Click();
        Assert.Equal(fromUrl, driver.Url);
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void OnDecliningCookies_analyticsContainerIsNotRendered()
    {
        using IWebDriver driver = new ChromeDriver();
        var actions = new Actions(driver);

        driver.Manage().Cookies.DeleteAllCookies();
        driver.Navigate().GoToUrl(Host + "cookie-settings");
        
        actions.ScrollByAmount(0, 500).Perform();
        driver.FindElement(By.Id("accept-no")).Click();
        ClickSubmit(driver);

        driver.Navigate().GoToUrl(Host);
        Assert.Throws<NoSuchElementException>(() => driver.FindElement(By.Id("google-analytics-container")));
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void OnAcceptingCookies_analyticsContainerIsRendered()
    {
        using IWebDriver driver = new ChromeDriver();
        var actions = new Actions(driver);

        driver.Manage().Cookies.DeleteAllCookies();
        driver.Navigate().GoToUrl(Host + "cookie-settings");
        actions.ScrollByAmount(0, 500).Perform();
        driver.FindElement(By.Id("accept-yes")).Click();
        ClickSubmit(driver);

        driver.Navigate().GoToUrl(Host);
        Assert.NotNull(driver.FindElement(By.Id("google-analytics-container")));

        var localScript = driver.FindElement(By.CssSelector("#google-analytics-container > script:not([src])")).GetAttribute("nonce");
        Assert.True(localScript.Length > 10);
        // test that a nonce of succient length is set on the local script

        // note:
        // scripts should additionally be manually tested in online environments as the
        // infrastructure may cause changes to the headers affecting script performance
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public async Task CookieSettings_Post_RedirectsToLocalUrl_WhenBackUrlIsLocal()
    {
        // Arrange
        using IWebDriver driver = new ChromeDriver();
        var actions = new Actions(driver);
        var localBackUrl = SearchUrl;

        // Navigate to the Cookie Settings page
        driver.Navigate().GoToUrl($"{SearchUrl}cookie-settings");

        var backUrl = driver.FindElement(By.Name("BackUrl"));

        // Fill out the form
        actions.ScrollByAmount(0, 500).Perform();
        driver.FindElement(By.Id("accept-yes")).Click();
        ((IJavaScriptExecutor)driver).ExecuteScript($"arguments[0].setAttribute('value', '{localBackUrl}');", backUrl);
        driver.FindElement(By.CssSelector("button.govuk-button[data-module='govuk-button']")).Click();

        // Wait for the redirect
        await Task.Delay(1000); // Adjust as needed

        // Assert
        Assert.Equal(SearchUrl, driver.Url);
    }

    [Fact]
    public async Task CookieSettings_Post_RedirectsToHome_WhenBackUrlIsExternal()
    {
        // Arrange
        using IWebDriver driver = new ChromeDriver();
        var actions = new Actions(driver);
        var externalBackUrl = "http://malicious.com";

        // Navigate to the Cookie Settings page
        driver.Navigate().GoToUrl($"{SearchUrl}cookie-settings");

        var backUrl = driver.FindElement(By.Name("BackUrl"));
        // Fill out the form
        actions.ScrollByAmount(0, 500).Perform();
        driver.FindElement(By.Id("accept-yes")).Click();
        ((IJavaScriptExecutor)driver).ExecuteScript($"arguments[0].setAttribute('value', '{externalBackUrl}');", backUrl);
        driver.FindElement(By.CssSelector("button.govuk-button[data-module='govuk-button']")).Click();

        // Wait for the redirect
        await Task.Delay(1000); // Adjust as needed

        // Assert
        Assert.Equal(SearchUrl, driver.Url);
    }
}
