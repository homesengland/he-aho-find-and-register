using System;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FindAndRegisterIntegrationTests
{
	public class BetaBannerTests : SeleniumTestsBase
    {
        [Fact]
        [Trait("Selenium", "Smoke")]
        public void EligibilityBetaBanner()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "check-eligibility-to-buy-a-shared-ownership-home/");
            //check beta banner exist
            Assert.NotNull(driver.FindElement(By.ClassName("govuk-phase-banner")));
            driver.FindElement(By.ClassName("govuk-phase-banner")).FindElement(By.ClassName("govuk-link")).Click();
            Assert.Equal("https://www.smartsurvey.co.uk/s/Eligible/", driver.Url);
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

