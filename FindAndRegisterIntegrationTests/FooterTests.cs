using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FindAndRegisterIntegrationTests
{
	public class FooterTests : SeleniumTestsBase
    {
        [Fact]
        [Trait("Selenium", "Smoke")]
        public void FooterLinks()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);

            driver.FindElement(By.Id("Accessibility_link")).Click();
            string AccessibilityNavingationLink = Host + "FooterLinks/Accessibility";
            Assert.Equal(AccessibilityNavingationLink, driver.Url);
            driver.Navigate().GoToUrl(Host);

            driver.FindElement(By.Id("Cookies_link")).Click();
            Assert.Equal(Host + "cookie-settings", driver.Url);
            driver.Navigate().GoToUrl(Host);

            driver.FindElement(By.Id("Contact_link")).Click();
            string contactUsNavingationLink = Host + "FooterLinks/ContactUs";
            Assert.Equal(contactUsNavingationLink, driver.Url);
            driver.Navigate().GoToUrl(Host);
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
    }
}

