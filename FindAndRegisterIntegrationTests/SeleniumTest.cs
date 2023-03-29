using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FindAndRegisterIntegrationTests;

public class SeleniumTest : SeleniumTestsBase
{
    public SeleniumTest()
    {
        Host = Host + "check-eligibility-to-buy-a-shared-ownership-home/";
    }

    [Fact]
    [Trait("Selenium", "Smoke")]
    public void SkeletonTest()
    {
        //creating new instance of chrome driver
        using (IWebDriver driver = new ChromeDriver())
        {
            //does not work as we cant run the web instance with the test.
            driver.Navigate().GoToUrl(Host);
            Assert.Contains("Where do you want to buy a home", driver.Title);
        }
    }
}