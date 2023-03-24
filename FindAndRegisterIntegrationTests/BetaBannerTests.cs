using System;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FindAndRegisterIntegrationTests
{
	public class BetaBannerTests : SeleniumTestsBase
    {
        //fact and trait are needed for selenium test functions
        [Fact]
        [Trait("Selenium", "Smoke")]
        public void EligibilityBetaBanner()
        {
            //we are using selenium web driver for web manipulations
            using IWebDriver driver = new ChromeDriver();
            //we begin with navigating to the route url of a journey.
            driver.Navigate().GoToUrl(Host + "check-eligibility-to-buy-a-shared-ownership-home/");

            //assert is used to set test pass/fail criteria. in the below case we are cheking if the current url that the driver is on,
            //check beta banner exist
            Assert.NotNull(driver.FindElement(By.ClassName("govuk-phase-banner")));

            //we then set step by step instructions. this is done by looking for a specific html id or class name.
            //we the give the driver an a function to perform on the id/class i.e. .Click()
            driver.FindElement(By.ClassName("govuk-phase-banner")).FindElement(By.ClassName("govuk-link")).Click();
            Assert.Equal("https://www.smartsurvey.co.uk/s/Eligible/", driver.Url);

            //after the last line of code is executed the driver will close as well as the web application instance that was used to perform the automated test.
            //this is the pattern we are using solution wide.
            //everything is either a click or a content check. content check will use the contains() function.
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void SearchBetaBanner()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "find-organisations-selling-shared-ownership-homes/");
            //check beta banner exist
            Assert.NotNull(driver.FindElement(By.ClassName("govuk-phase-banner")));
            driver.FindElement(By.ClassName("govuk-phase-banner")).FindElement(By.ClassName("govuk-link")).Click();
            Assert.Equal("https://www.smartsurvey.co.uk/s/FindOrganisation/", driver.Url);
        }
    }
}

