using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium.Axe;
using Xunit;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;


namespace FindAndRegisterIntegrationTests
{
    public class EligibilityJourneyTests : SeleniumTestsBase
    {
        [Fact]
        [Trait("Selenium", "Smoke")]
        public void WhereDoYouWantToBuyAHomeNavigation()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility/whereDoYouWantToBuyAHome");
            driver.FindElement(By.Id("choice-For-Living-In-London")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.NotEqual(Host + "Eligibility/whereDoYouWantToBuyAHome", driver.Url);
            Assert.Equal(Host + "Eligibility/EligibilityOutcome", driver.Url);

            driver.Navigate().GoToUrl(Host + "Eligibility/whereDoYouWantToBuyAHome");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.NotEqual(Host + "Eligibility/whereDoYouWantToBuyAHome", driver.Url);
            Assert.Equal(Host + "Eligibility/BuyingWithAnotherPerson", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void WhereDoYouWantToBuyAHomeAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            AxeResult axeResult = new AxeBuilder(driver).Analyze();
            Assert.Null(axeResult.Error);
       }
    }
}

