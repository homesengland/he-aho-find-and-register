using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Deque.AxeCore.Selenium;
using Microsoft.Graph;


namespace FindAndRegisterIntegrationTests
{
    public class EligibilityJourneyTests : SeleniumTestsBase
    {
        private string HostForSearch;
        public EligibilityJourneyTests()
        {
            HostForSearch = Host + "find-organisations-selling-shared-ownership-homes";
            Host = Host + "check-eligibility-to-buy-a-shared-ownership-home/"; 
        }

        private void AccessibilityTest(IWebDriver driver)
        {
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void WhereDoYouWantToBuyAHomeNavigation()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);

            AccessibilityTest(driver);
            driver.FindElement(By.Id("choice-For-Living-In-London")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.NotEqual(Host, driver.Url);
            Assert.Equal(Host + "continue-on-the-homes-for-londoners-website", driver.Url);
            AccessibilityTest(driver);

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.NotEqual(Host, driver.Url);
            Assert.Equal(Host + "are-you-buying-with-another-person", driver.Url);
            AccessibilityTest(driver);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void BuyingWithAnotherPersonNavigation()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.NotEqual(Host, driver.Url);
            Assert.Equal(Host + "are-you-buying-with-another-person", driver.Url);
            AccessibilityTest(driver);

            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            AccessibilityTest(driver);
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.NotEqual(Host + "are-you-buying-with-another-person", driver.Url);
            Assert.Equal(Host + "how-much-do-you-both-earn", driver.Url);
            AccessibilityTest(driver);

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.NotEqual(Host + "are-you-buying-with-another-person", driver.Url);
            Assert.Equal(Host + "how-much-do-you-earn", driver.Url);
            AccessibilityTest(driver);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HowMuchDoYouEarnChoosingBelow80k()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            AccessibilityTest(driver);
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Submit-Button")).Click();
            Assert.Equal(Host + "select-one-that-apply-to-you", driver.Url);
            AccessibilityTest(driver);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HowMuchDoYouEarnChoosingAbove80k()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-81")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Submit-Button")).Click();
            Assert.Equal(Host + "you-may-not-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
            AccessibilityTest(driver);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToHowMuchDoYouEarn_MultiplePeople()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            // below 80k
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            AccessibilityTest(driver);
            driver.FindElement(By.Id("annual-income-multi-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Submit-Button")).Click();
            Assert.Equal(Host + "select-one-that-apply-to-you", driver.Url);
            AccessibilityTest(driver);

            // over 80k
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-multi-81")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Submit-Button")).Click();
            Assert.Equal(Host + "you-may-not-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
            AccessibilityTest(driver);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToFirstTimeBuyerSinglesJourney()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Submit-Button")).Click();
            driver.FindElement(By.Id("first-time-buyer")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Equal(Host + "select-another-option-that-apply-to-you", driver.Url);
            AccessibilityTest(driver);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToFirstTimeBuyerMultiJourney()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-multi-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Submit-Button")).Click();
            driver.FindElement(By.Id("first-time-buyer")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Equal(Host + "select-another-option-that-apply-to-you", driver.Url);
            AccessibilityTest(driver);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToAffordabilityJourney()
        {
            using IWebDriver driver = new ChromeDriver();

            // single - first time buyer, cant' afford without shared ownership
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Submit-Button")).Click();
            driver.FindElement(By.Id("first-time-buyer")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            driver.FindElement(By.Id("cant-afford-without-sp")).Click();
            driver.FindElement(By.Id("eligibility-Page-5-Submit-Button")).Click();
            Assert.Equal(Host + "you-may-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
            AccessibilityTest(driver);

            // muti - first time buyer, cant' afford without shared ownership
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-multi-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Submit-Button")).Click();
            driver.FindElement(By.Id("first-time-buyer")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            driver.FindElement(By.Id("cant-afford-without-sp")).Click();
            driver.FindElement(By.Id("eligibility-Page-5-Submit-Button")).Click();
            Assert.Equal(Host + "you-may-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
            AccessibilityTest(driver);

            // single - first time buyer, can afford without shared ownership
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Submit-Button")).Click();
            driver.FindElement(By.Id("first-time-buyer")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            driver.FindElement(By.Id("can-afford-without-sp")).Click();
            driver.FindElement(By.Id("eligibility-Page-5-Submit-Button")).Click();
            Assert.Equal(Host + "you-may-not-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
            AccessibilityTest(driver);

            // muti - first time buyer, can afford without shared ownership
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-multi-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Submit-Button")).Click();
            driver.FindElement(By.Id("first-time-buyer")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            driver.FindElement(By.Id("can-afford-without-sp")).Click();
            driver.FindElement(By.Id("eligibility-Page-5-Submit-Button")).Click();
            Assert.Equal(Host + "you-may-not-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
            AccessibilityTest(driver);
        }


        ///eligability pages
        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToElibilityOutcomeForLondon()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-London")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.Equal(Host + "continue-on-the-homes-for-londoners-website", driver.Url);
            AccessibilityTest(driver);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToElibilityOutcomeForLondonLinkColour()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-London")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.Contains(driver.FindElement(By.Id("londonURL")).GetAttribute("class"), "govuk-link");
            AccessibilityTest(driver);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void ElibilityOutcomeForLondonLink()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-London")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("londonURL")).Click();
            Assert.Equal("https://www.london.gov.uk/programmes-strategies/housing-and-land/homes-londoners/search", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToEligableOutcomeLink()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Submit-Button")).Click();
            driver.FindElement(By.Id("first-time-buyer")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            driver.FindElement(By.Id("cant-afford-without-sp")).Click();
            driver.FindElement(By.Id("eligibility-Page-5-Submit-Button")).Click();
            Assert.Contains(driver.FindElement(By.Id("eligible-result-find-a-provider-link")).GetAttribute("class"), "govuk-link");
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void EligableOutcomeLinksToSearchForProvider()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Submit-Button")).Click();
            driver.FindElement(By.Id("first-time-buyer")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            driver.FindElement(By.Id("cant-afford-without-sp")).Click();
            driver.FindElement(By.Id("eligibility-Page-5-Submit-Button")).Click();
            driver.FindElement(By.Id("eligible-result-find-a-provider-link")).Click();
            Assert.True(driver.Url.Equals(Host + "Search") || driver.Url.Equals(HostForSearch));
        }
    }
}

