using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FindAndRegisterIntegrationTests;

public class SeleniumTest : SeleniumTestsBase
{
    [Fact]
    [Trait("Selenium", "Smoke")]
    public void SkeletonTest()
    {
        //creating new instance of chrome driver
        using (IWebDriver driver = new ChromeDriver())
        {
            //does not work as we cant run the web instance with the test.
            driver.Navigate().GoToUrl(Host);
            Assert.Contains("Find a shared ownership provider", driver.Title);
        }
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void Eligibility_HappyPath()
    {
        using IWebDriver driver = new ChromeDriver();

        driver.Navigate().GoToUrl(Host + "annual-income");
        Assert.Contains("Annual Income", driver.Title);
        driver.FindElement(By.Id("income-yes")).Click();

        ClickSubmit(driver);
        Assert.Equal(Host + "current-situation", driver.Url);
        Assert.Contains("Current Situation", driver.Title);
        // Check that we have moved to the next page

        driver.FindElement(By.Id("situation-yes")).Click();

        ClickSubmit(driver);
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void CurrentSituation_BackButton()
    {
        //creating new instance of chrome driver
        using (IWebDriver driver = new ChromeDriver())
        {
            driver.Navigate().GoToUrl(Host + "current-situation");
            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.Equal(Host + "annual-income", driver.Url);
        }
    }
}

