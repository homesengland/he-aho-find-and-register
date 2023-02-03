using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FindAndRegisterIntegrationTests
{
	public class HeaderTests : SeleniumTestsBase
    {
        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HeaderServiceName()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host);
            var serviceName = driver.FindElement(By.ClassName("govuk-header__content")).Text;
            Assert.Equal("Find a shared ownership provider", serviceName);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HeaderServiceNameNavigation()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.ClassName("govuk-header__content")).Click();
            Assert.Equal(Host + "annual-income", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HeaderLogoNavigation()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.ClassName("govuk-header__link")).Click();
            string landingSite = "https://www.gov.uk/";
            Assert.Equal(landingSite, driver.Url);
        }

    }
}

