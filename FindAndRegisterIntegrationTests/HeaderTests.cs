using System;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FindAndRegisterIntegrationTests
{
	public class HeaderTests : SeleniumTestsBase
    {
        [Fact]
        [Trait("Selenium", "Smoke")]
        public void EligibilityServiceName()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host + "check-eligibility-to-buy-a-shared-ownership-home/");
            var serviceName = driver.FindElement(By.ClassName("govuk-header__content")).Text;
            Assert.Equal("Check if you are eligible to buy a shared ownership home", serviceName);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void SearchServiceName()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host + "find-organisations-selling-shared-ownership-homes/");
            var serviceName = driver.FindElement(By.ClassName("govuk-header__content")).Text;
            Assert.Equal("Find an organisation that sells shared ownership homes", serviceName);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void EligibilityHeaderServiceNameNavigation()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host + "check-eligibility-to-buy-a-shared-ownership-home/");
            driver.FindElement(By.ClassName("govuk-header__content")).Click();
            Assert.Equal(Host + "check-eligibility-to-buy-a-shared-ownership-home", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void SearchHeaderServiceNameNavigation()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host + "find-organisations-selling-shared-ownership-homes/");
            driver.FindElement(By.ClassName("govuk-header__content")).Click();
            Assert.Equal(Host + "find-organisations-selling-shared-ownership-homes", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HeaderLogoNavigation()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host + "check-eligibility-to-buy-a-shared-ownership-home/");
            driver.FindElement(By.ClassName("govuk-header__link")).Click();
            string landingSite = "https://www.gov.uk/";
            Assert.Equal(landingSite, driver.Url);
        }

    }
}

