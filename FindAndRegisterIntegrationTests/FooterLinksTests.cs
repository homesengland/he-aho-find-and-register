using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FindAndRegisterIntegrationTests
{
	public class FooterLinksTests : SeleniumTestsBase
    {
        [Fact]
        [Trait("Selenium", "Smoke")]
        public void ContactUsBackButton()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.ClassName("govuk-header__service-name")).Click();
            var previousURL = driver.Url.ToString();

            driver.FindElement(By.Id("Contact_link")).Click();
            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.Equal(previousURL, driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void ContactUsEmailLink()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("Contact_link")).Click();
            var previousURL = driver.FindElement(By.Id("TechSupportEmail")).GetAttribute("href");
            Assert.Contains("mailto:", previousURL);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void AccessibilityBackButton()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.ClassName("govuk-header__service-name")).Click();
            var previousURL = driver.Url.ToString();

            driver.FindElement(By.Id("Accessibility_link")).Click();
            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.Equal(previousURL, driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void AccessibilityEmailLinks()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("Accessibility_link")).Click();

            var emailLink = driver.FindElement(By.Id("TechSupportEmail")).GetAttribute("href");
            Assert.Contains("mailto:", emailLink);
        }

    }
}

