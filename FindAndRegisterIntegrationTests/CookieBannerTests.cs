using System.Web;
using Find_Register.Models;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace FindAndRegisterIntegrationTests;

public class CookieBannerTests : SeleniumTestsBase
{   
    [Fact]
    [Trait("Selenium", "Smoke")]
    public void WhenNoCookiesAreSet_DisplayCookieBanner()
    {
        using IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl(Host + "annual-income");
        Assert.NotNull(driver.FindElement(By.Id("cookie-banner")));
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void CookieBanner_HasLinkToPolicy()
    {
        using IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl(Host + "annual-income");
        driver.FindElement(By.Id("cookie-banner")).FindElement(By.Id("cookie-policy-link")).Click();
        Assert.Equal(Host + "cookie-policy", driver.Url);
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void ClickOnSetPreferenceButton_NavigatesToSettingsPage()
    {
        using IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl(Host + "annual-income");
        driver.FindElement(By.Id("cookie-banner")).FindElement(By.Id("cookie-preference-btn")).Click();
        Assert.Equal(Host + "cookie-settings", driver.Url);
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void ClickOnAcceptAllButton_SetsCookieAcceptanceCookieAndDisplaysConfirmation()
    {
        using IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl(Host + "annual-income");
        driver.FindElement(By.Id("cookie-banner")).FindElement(By.Id("accept-all-cookies-btn")).Click();
        Assert.Equal(Host + "annual-income", driver.Url);
        var acceptAnalyticsCookie = driver.Manage().Cookies.GetCookieNamed("analytic-settings");
        var deserialized = JsonConvert.DeserializeObject<AnalyticSettings>(HttpUtility.UrlDecode(acceptAnalyticsCookie.Value));
        Assert.True(deserialized.AcceptAnalytics);
        Assert.NotNull(driver.FindElement(By.Id("cookie-confirmation")));
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void HidingCookieConfirmation_ConfirmationButtonIsNoLongerDisplayed()
    {
        using IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl(Host + "annual-income");
        driver.FindElement(By.Id("cookie-banner")).FindElement(By.Id("accept-all-cookies-btn")).Click();
        driver.FindElement(By.Id("cookie-confirmation")).FindElement(By.Id("hide-confirmation-btn")).Click();
        Assert.Equal(Host + "annual-income", driver.Url);
        Assert.Throws<NoSuchElementException>(() => driver.FindElement(By.Id("cookie-confirmation")));
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void ClickOnSettingsFromConfirmation_NavigatesToSettingsPage()
    {
        using IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl(Host + "annual-income");
        driver.FindElement(By.Id("cookie-banner")).FindElement(By.Id("accept-all-cookies-btn")).Click();
        driver.FindElement(By.Id("cookie-confirmation")).FindElement(By.Id("cookie-settings-link")).Click();
        Assert.Equal(Host + "cookie-settings", driver.Url);
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void WhenCookiesAreSet_DoNotDisplayCookieBanner()
    {
        using IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl(Host);
        driver.Manage().Cookies.AddCookie(new Cookie("analytic-settings", HttpUtility.UrlEncode("{AcceptAnalytics:true, HideConfirmation: true}")));
        driver.Navigate().GoToUrl(Host + "annual-income");
        Assert.Throws<NoSuchElementException>(() => driver.FindElement(By.Id("cookie-banner")));
    }
}
