using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Deque.AxeCore.Selenium;

namespace FindAndRegisterIntegrationTests
{
	public class FooterLinksTests : SeleniumTestsBase
    {
        public FooterLinksTests()
        {
            Host = Host + "check-eligibility-to-buy-a-shared-ownership-home/";
        }

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
        public void ContactUsTitleSize()
        {
            using IWebDriver driver = new ChromeDriver();
            
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("Contact_link")).Click();
            var title = driver.FindElement(By.Id("page-title")).GetAttribute("innerHTML");
            Assert.Equal("Contact us", title);

            var classes = driver.FindElement(By.Id("page-title")).GetAttribute("class");
            Assert.Equal("govuk-heading-l", classes);
        }


        [Fact]
        [Trait("Selenium", "Smoke")]
        public void ContactUsEmailLink()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("Contact_link")).Click();

            var emailLink = driver.FindElement(By.Id("contact-us-email")).GetAttribute("href");
            Assert.Contains("mailto:", emailLink);

            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void ContactUsEmailLinkColour()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("Contact_link")).Click();

            Assert.Contains(driver.FindElement(By.Id("contact-us-email")).GetAttribute("class"), "govuk-link");
        }

        /*[Fact]
        [Trait("Selenium", "Smoke")]
        public void ContactUsCallChargeLinkTargetsExpectedUrl()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("Contact_link")).Click();
            var emailLink = driver.FindElement(By.Id("contact-us-midlands-call-charge-link")).GetAttribute("href");
            Assert.Equal("https://www.gov.uk/call-charges", emailLink);
        }*/

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
        public void AccessibilityTitleSize()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("Accessibility_link")).Click();

            var classes = driver.FindElement(By.Id("page-title")).GetAttribute("class");
            Assert.Equal("govuk-heading-l", classes);
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

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void AccessibilityEmailLinkColour()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("Accessibility_link")).Click();

            Assert.Contains(driver.FindElement(By.Id("TechSupportEmail")).GetAttribute("class"), "govuk-link");
        }

    }
}