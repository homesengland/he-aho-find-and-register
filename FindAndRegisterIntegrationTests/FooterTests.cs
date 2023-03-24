using System;
using Microsoft.Graph;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FindAndRegisterIntegrationTests
{
	public class FooterTests : SeleniumTestsBase
    {
        public FooterTests()
        {
            Host = Host + "check-eligibility-to-buy-a-shared-ownership-home/";
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void FooterLinks()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("Accessibility_link")).Click();
            string AccessibilityNavingationLink = Host + "accessibility";
            
            Assert.Equal(AccessibilityNavingationLink, driver.Url);

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("Cookies_link")).Click();
            Assert.Equal(Host + "cookie-settings", driver.Url);

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("Contact_link")).Click();
            string contactUsNavingationLink = Host + "contact-us";
            Assert.Equal(contactUsNavingationLink, driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void FooterCrownLogoNavigation()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);

            driver.FindElement(By.ClassName("govuk-footer__copyright-logo")).Click();
            string crownLogoNavingationLink = "https://www.nationalarchives.gov.uk/information-management/re-using-public-sector-information/uk-government-licensing-framework/crown-copyright/";
            Assert.Equal(crownLogoNavingationLink, driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void FooterOpenGovernmentLicenceNavigation()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);

            driver.FindElement(By.ClassName("govuk-footer__licence-description")).Click();
            string crownLogoNavingationLink = "https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/";
            Assert.Equal(crownLogoNavingationLink, driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void BackLinkGoesToLastJourneyPage() {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("Contact_link")).Click();
            string contactUsNavingationLink = Host + "contact-us";
            Assert.Equal(contactUsNavingationLink, driver.Url);

            driver.FindElement(By.Id("Accessibility_link")).Click();
            driver.FindElement(By.ClassName("govuk-back-link")).Click();

            Assert.Equal(Host, driver.Url);
        }
    }
}

