using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
    }
}

