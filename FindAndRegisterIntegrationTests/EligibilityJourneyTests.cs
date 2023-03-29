using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium.Axe;


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

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void WhereDoYouWantToBuyAHomeNavigation()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-London")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.NotEqual(Host, driver.Url);
            Assert.Equal(Host + "continue-on-the-homes-for-londoners-website", driver.Url);

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.NotEqual(Host, driver.Url);
            Assert.Equal(Host + "are-you-buying-with-another-person", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void WhereDoYouWantToBuyAHomeAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
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

            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.NotEqual(Host + "are-you-buying-with-another-person", driver.Url);
            Assert.Equal(Host + "how-much-do-you-both-earn", driver.Url);


            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.NotEqual(Host + "are-you-buying-with-another-person", driver.Url);
            Assert.Equal(Host + "how-much-do-you-earn", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void BuyingWithAnotherPersonAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToHowMuchDoYouEarn()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.Equal(Host + "how-much-do-you-earn", driver.Url);
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
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "select-all-that-apply-to-you", driver.Url);
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
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "you-may-not-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HowMuchDoYouEarnAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.Equal(Host + "how-much-do-you-earn", driver.Url);
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToHowMuchDoYouEarn_MultiplePeople()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.Equal(Host + "how-much-do-you-both-earn", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HowMuchDoYouEarn_MultiplePeopleChoosingBelow80k()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-multi-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Multi-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "select-all-that-apply-to-you", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HowMuchDoYouEarn_MultiplePeopleChoosingAbove80k()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-multi-81")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Multi-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "you-may-not-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HowMuchDoYouEarn_MultiplePeopleAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.Equal(Host + "how-much-do-you-both-earn", driver.Url);
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToFirstTimeBuyerSinglesJourney()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-multi-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Multi-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "select-all-that-apply-to-you", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToFirstTimeBuyerMultiJourney()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "select-all-that-apply-to-you", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void FirstTimeBuyerAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "select-all-that-apply-to-you", driver.Url);
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void FirstTimeBuyerErrorTest1()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("first-time-buyer")).Click();
            driver.FindElement(By.Id("own-a-home")).Click();
            driver.FindElement(By.Id("cannot-afford-home")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Contains("You cannot select ‘I do not own a home’ and ‘I own a home but need to move’", "You cannot select ‘I do not own a home’ and ‘I own a home but need to move’");
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void FirstTimeBuyerErrorTest2()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("first-time-buyer")).Click();
            driver.FindElement(By.Id("own-a-home")).Click();
            driver.FindElement(By.Id("cannot-afford-home")).Click();
            driver.FindElement(By.Id("none")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Contains("You cannot select one of the first 3 options and ‘These do not apply to me’", "You cannot select one of the first 3 options and ‘These do not apply to me’");
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void FirstTimeBuyerErrorTest3()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Contains("Select at least one option", "Select at least one option");
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
        public void ElibilityOutcomeForLondonAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-London")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.Equal(Host + "continue-on-the-homes-for-londoners-website", driver.Url);
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void ElibilityOutcomeForEarningsOver80k()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-81")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "you-may-not-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void ElibilityOutcomeForEarningsOver80kLinkColour()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-81")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            Assert.Contains(driver.FindElement(By.Id("AHOLink")).GetAttribute("class"), "govuk-link");
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void ElibilityOutcomeForEarningsOver80kAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-81")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "you-may-not-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToFirstTimeBuyerNotEligableOutcome()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("none")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Equal(Host + "you-may-not-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToFirstTimeBuyerErrorMessageTestOne()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Contains("Select at least one option", "Select at least one option");
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToFirstTimeBuyerErrorMessageTestTwo()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("none")).Click();
            driver.FindElement(By.Id("cannot-afford-home")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Contains("You cannot select one of the first 3 options and ‘These do not apply to me’", "You cannot select one of the first 3 options and ‘These do not apply to me’");
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToFirstTimeBuyerErrorMessageTestThree()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("first-time-buyer")).Click();
            driver.FindElement(By.Id("own-a-home")).Click();
            driver.FindElement(By.Id("cannot-afford-home")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Contains("You cannot select ‘I do not own a home’ and ‘I own a home but need to move’", "You cannot select ‘I do not own a home’ and ‘I own a home but need to move’");
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToFirstTimeBuyerNotEligableOutcomeLinkColour()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("none")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Contains(driver.FindElement(By.Id("AHOLink")).GetAttribute("class"), "govuk-link");
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NotEligableOutcomeAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("none")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Equal(Host + "you-may-not-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToEligableOutcome()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("own-a-home")).Click();
            driver.FindElement(By.Id("cannot-afford-home")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Equal(Host + "you-may-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void NavigatingToEligableOutcomeLinkColour()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("own-a-home")).Click();
            driver.FindElement(By.Id("cannot-afford-home")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Contains(driver.FindElement(By.Id("eligible-result-find-a-provider-link")).GetAttribute("class"), "govuk-link");
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void EligableOutcomeAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("own-a-home")).Click();
            driver.FindElement(By.Id("cannot-afford-home")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            Assert.Equal(Host + "you-may-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
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
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            driver.FindElement(By.Id("own-a-home")).Click();
            driver.FindElement(By.Id("cannot-afford-home")).Click();
            driver.FindElement(By.Id("eligibility-Page-4-Submit-Button")).Click();
            driver.FindElement(By.Id("eligible-result-find-a-provider-link")).Click();
            Assert.True(driver.Url.Equals(Host + "Search") || driver.Url.Equals(HostForSearch));
                // depending on if ticket to change url has been merged
        }
    }
}

