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
        public void BetaBanner()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            //check beta banner exist
            Assert.NotNull(driver.FindElement(By.ClassName("govuk-phase-banner")));
            driver.FindElement(By.ClassName("govuk-phase-banner")).FindElement(By.ClassName("govuk-link")).Click();
            Assert.Equal(Host + "#", driver.Url);
        }
    }
}

