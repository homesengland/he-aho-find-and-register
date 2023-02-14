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
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-London")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.NotEqual(Host + "Eligibility", driver.Url);
            Assert.Equal(Host + "Eligibility/EligibilityOutcome", driver.Url);

            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.NotEqual(Host + "Eligibility", driver.Url);
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

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void BuyingWithAnotherPersonNavigation()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.NotEqual(Host + "Eligibility", driver.Url);
            Assert.Equal(Host + "Eligibility/BuyingWithAnotherPerson", driver.Url);

            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.NotEqual(Host + "Eligibility/BuyingWithAnotherPerson", driver.Url);
            Assert.Equal(Host + "Eligibility/HowMuchDoYouEarn_MultiplePeople", driver.Url);


            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.NotEqual(Host + "Eligibility/BuyingWithAnotherPerson", driver.Url);
            Assert.Equal(Host + "Eligibility/HowMuchDoYouEarn", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void BuyingWithAnotherPersonAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            AxeResult axeResult = new AxeBuilder(driver).Analyze();
            Assert.Null(axeResult.Error);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToHowMuchDoYouEarn()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/HowMuchDoYouEarn", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HowMuchDoYouEarnChoosingBelow80k()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/FirstTimeBuyer", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HowMuchDoYouEarnChoosingAbove80k()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-81")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/EligibilityOutcome", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HowMuchDoYouEarnAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/HowMuchDoYouEarn", driver.Url);
            AxeResult axeResult = new AxeBuilder(driver).Analyze();
            Assert.Null(axeResult.Error);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToHowMuchDoYouEarn_MultiplePeople()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/HowMuchDoYouEarn_MultiplePeople", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HowMuchDoYouEarn_MultiplePeopleChoosingBelow80k()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-multi-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Multi-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/FirstTimeBuyer", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HowMuchDoYouEarn_MultiplePeopleChoosingAbove80k()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-multi-81")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Multi-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/EligibilityOutcome", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HowMuchDoYouEarn_MultiplePeopleAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/HowMuchDoYouEarn_MultiplePeople", driver.Url);
            AxeResult axeResult = new AxeBuilder(driver).Analyze();
            Assert.Null(axeResult.Error);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToFirstTimeBuyerSinglesJourney()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-multi-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Multi-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/FirstTimeBuyer", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToFirstTimeBuyerMultiJourney()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/FirstTimeBuyer", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void FirstTimeBuyerAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/FirstTimeBuyer", driver.Url);
            AxeResult axeResult = new AxeBuilder(driver).Analyze();
            Assert.Null(axeResult.Error);
        }

        ///eligability pages
        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToElibilityOutcomeForLondon()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-London")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/EligibilityOutcome", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void ElibilityOutcomeForLondonLink()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-London")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("londonURL")).Click();
            Assert.Equal("https://www.london.gov.uk/programmes-strategies/housing-and-land/homes-londoners/search", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void ElibilityOutcomeForLondonAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-London")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/EligibilityOutcome", driver.Url);
            AxeResult axeResult = new AxeBuilder(driver).Analyze();
            Assert.Null(axeResult.Error);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void ElibilityOutcomeForEarningsOver80k()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-81")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/EligibilityOutcome", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void ElibilityOutcomeForEarningsOver80kAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-81")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/EligibilityOutcome", driver.Url);
            AxeResult axeResult = new AxeBuilder(driver).Analyze();
            Assert.Null(axeResult.Error);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToFirstTimeBuyerNotEligableOutcome()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("none")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/EligibilityOutcome", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NotEligableOutcomeAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("none")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/EligibilityOutcome", driver.Url);
            AxeResult axeResult = new AxeBuilder(driver).Analyze();
            Assert.Null(axeResult.Error);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToEligableOutcome()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("own-a-home")).Click();
            driver.FindElement(By.Id("cannot-afford-home")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/EligibilityOutcome", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void EligableOutcomeAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + "Eligibility");
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("own-a-home")).Click();
            driver.FindElement(By.Id("cannot-afford-home")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Equal(Host + "Eligibility/EligibilityOutcome", driver.Url);
            AxeResult axeResult = new AxeBuilder(driver).Analyze();
            Assert.Null(axeResult.Error);
        }

    }
}

