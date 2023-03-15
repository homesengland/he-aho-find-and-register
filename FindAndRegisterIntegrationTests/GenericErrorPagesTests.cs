using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium.Axe;

namespace FindAndRegisterIntegrationTests
{
	public class GenericErrorPagesTests : SeleniumTestsBase
    {
        [Fact]
        [Trait("Selenium", "Smoke")]
        public void GenericErrorPages()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "GenericErrors/serviceUnavailable");
            string serviceUnavailable = Host + "GenericErrors/serviceUnavailable";
            Assert.Equal(serviceUnavailable, driver.Url);

            driver.Navigate().GoToUrl(Host + "GenericErrors/PageNotFound");
            string PageNotFound = Host + "GenericErrors/PageNotFound";
            Assert.Equal(PageNotFound, driver.Url);

            driver.Navigate().GoToUrl(Host + "GenericErrors/InternalServerError");
            string InternalServerError = Host + "GenericErrors/InternalServerError";
            Assert.Equal(InternalServerError, driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void GenericErrorPagesAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "GenericErrors/serviceUnavailable");
            string serviceUnavailable = Host + "GenericErrors/serviceUnavailable";
            Assert.Equal(serviceUnavailable, driver.Url);
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);

            driver.Navigate().GoToUrl(Host + "GenericErrors/PageNotFound");
            string PageNotFound = Host + "GenericErrors/PageNotFound";
            Assert.Equal(PageNotFound, driver.Url);
            Assert.Null(axeResult.Error);

            driver.Navigate().GoToUrl(Host + "GenericErrors/InternalServerError");
            string InternalServerError = Host + "GenericErrors/InternalServerError";
            axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void PageNotFoundErrorTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            string pageUnavailable = Host + "GenericErrors/PageNotFound";
            Assert.Equal(pageUnavailable, driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void PageNotFoundErrorAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            string pageUnavailable = Host + "GenericErrors/PageNotFound";
            Assert.Equal(pageUnavailable, driver.Url);
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void InternalServerErrorPageTest()
        {
            using IWebDriver driver = new ChromeDriver();
            string pageUnavailable = Host + "GenericErrors/InternalServerError";
            driver.Navigate().GoToUrl(pageUnavailable);
            Assert.Equal(pageUnavailable, driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void InternalServerErrorPageAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            string pageUnavailable = Host + "GenericErrors/InternalServerError";
            driver.Navigate().GoToUrl(pageUnavailable);
            Assert.Equal(pageUnavailable, driver.Url);
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void ServiceUnavailablePageTest()
        {
            using IWebDriver driver = new ChromeDriver();
            string pageUnavailable = Host + "GenericErrors/ServiceUnavailable";
            driver.Navigate().GoToUrl(pageUnavailable);
            Assert.Equal(pageUnavailable, driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void ServiceUnavailableErrorAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            string pageUnavailable = Host + "GenericErrors/ServiceUnavailable";
            driver.Navigate().GoToUrl(pageUnavailable);
            Assert.Equal(pageUnavailable, driver.Url);
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
        }
    }
}

