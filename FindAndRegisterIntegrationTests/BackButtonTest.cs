using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium.Axe;

namespace FindAndRegisterIntegrationTests
{
	public class BackButtonTest : SeleniumTestsBase
    {
        private string HostForSearch;
        //constructor to set journey url variables at runtime
        public BackButtonTest()
		{
            HostForSearch = Host + "find-organisations-selling-shared-ownership-homes/";
            Host = Host + "check-eligibility-to-buy-a-shared-ownership-home/";
        }

        //fact and trait are needed for selenium test functions
        [Fact]
        [Trait("Selenium", "Smoke")]
        public void WhereDoYouWantToBuyAHomeBackNavigation()
        {
            //we are using selenium web driver for web manipulations
            using IWebDriver driver = new ChromeDriver();
            //we begin with navigating to the route url of a journey.
            driver.Navigate().GoToUrl(Host);

            //we then set step by step instructions. this is done by looking for a specific html id or class name.
            //we the give the driver an a function to perform on the id/class i.e. .Click()

            driver.FindElement(By.Id("choice-For-Living-In-London")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            //assert is used to set test pass/fail criteria. in the below case we are cheking if the current url that the driver is on,
            //is equal to the host variable string
            Assert.NotEqual(Host, driver.Url);
            Assert.Equal(Host + "continue-on-the-homes-for-londoners-website", driver.Url);
            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.NotEqual(Host + "continue-on-the-homes-for-londoners-website", driver.Url);

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.NotEqual(Host, driver.Url);
            Assert.Equal(Host + "are-you-buying-with-another-person", driver.Url);
            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.NotEqual(Host + "are-you-buying-with-another-person", driver.Url);
            //after the last line of code is executed the driver will close as well as the web application instance that was used to perform the automated test.
            //this is the pattern we are using solution wide.
            //everything is either a click or a content check. content check will use the contains() function.
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void BuyingWithAnotherPersonBackNavigation()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            Assert.NotEqual(Host, driver.Url);
            Assert.Equal(Host + "are-you-buying-with-another-person", driver.Url);
            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.NotEqual(Host + "are-you-buying-with-another-person", driver.Url);

            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.NotEqual(Host + "are-you-buying-with-another-person", driver.Url);
            Assert.Equal(Host + "how-much-do-you-both-earn", driver.Url);
            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.NotEqual(Host + "how-much-do-you-both-earn", driver.Url);

            driver.Navigate().GoToUrl(Host);
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            Assert.NotEqual(Host + "are-you-buying-with-another-person", driver.Url);
            Assert.Equal(Host + "how-much-do-you-earn", driver.Url);
            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.NotEqual(Host + "how-much-do-you-earn", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HowMuchDoYouEarnBackNavigation()
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
            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.NotEqual(Host + "you-may-not-be-eligible-to-buy-a-shared-ownership-home", driver.Url);

            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-no")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-single-80")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Single-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "select-all-that-apply-to-you", driver.Url);
            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.NotEqual(Host + "select-all-that-apply-to-you", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void HowMuchDoYouEarn_MultiplePeopleBackNavigation()
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
            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.NotEqual(Host + "select-all-that-apply-to-you", driver.Url);

            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("choice-For-Living-In-Somewhere-Else")).Click();
            driver.FindElement(By.Id("eligibility-Page-1-Submit-Button")).Click();
            driver.FindElement(By.Id("buying-with-another-person-yes")).Click();
            driver.FindElement(By.Id("eligibility-Page-2-Submit-Button")).Click();
            driver.FindElement(By.Id("annual-income-multi-81")).Click();
            driver.FindElement(By.Id("eligibility-Page-3-Multi-Buyer-Submit-Button")).Click();
            Assert.Equal(Host + "you-may-not-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.NotEqual(Host + "you-may-not-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void FirstTimeBuyerBackNavigation()
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
            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.NotEqual(Host + "select-all-that-apply-to-you", driver.Url);

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
            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.NotEqual(Host + "you-may-not-be-eligible-to-buy-a-shared-ownership-home", driver.Url);

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
            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.NotEqual(Host + "you-may-be-eligible-to-buy-a-shared-ownership-home", driver.Url);
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void SearchJourneyResultsPage()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(HostForSearch);
            
            driver.FindElement(By.Id("Area")).SendKeys("Adur\t");
            driver.FindElement(By.Id("submit-search")).Click();

            var pageName = driver.FindElement(By.CssSelector("div.container H1")).Text;
            Assert.Equal("These organisations sell shared ownership homes in Adur", pageName);

            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
            // test accessibility

            var links = driver.FindElements(By.CssSelector("div.container a"));
            Assert.True(links.All(anchor => anchor.GetAttribute("class").Contains("govuk-link") || anchor.GetAttribute("class").Contains("govuk-back-link")));
            // all links have govuk-link class

            var texts = driver.FindElements(By.CssSelector("div.container :is(p, li, h1, h2, h3, h4, h5, h6)"));
            Assert.True(texts.All(paragraph => paragraph.GetCssValue("font-family").Contains("GDS Transport")));
            // all text are gds transport

            driver.FindElement(By.ClassName("govuk-back-link")).Click();
            Assert.NotEqual(HostForSearch + "organisations-that-sell-shared-ownership-homes?Area=Adur", driver.Url);
        }
    }
}

