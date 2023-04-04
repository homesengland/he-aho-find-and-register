using System;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium.Axe;

namespace FindAndRegisterIntegrationTests
{
	public class SearchJourneyTests : SeleniumTestsBase
    {
        public SearchJourneyTests()
        {
            Host = Host + "find-organisations-selling-shared-ownership-homes";
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void SearchJourneyStartPage()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);            
            // test accessibility

            var links = driver.FindElements(By.CssSelector("div.container a"));
            Assert.True(links.All(anchor => anchor.GetAttribute("class").Contains("govuk-link") || anchor.GetAttribute("class").Contains("govuk-back-link")));
            // all links have govuk-link class

            var texts = driver.FindElements(By.CssSelector("div.container :is(p, li, h1, h2, h3, h4, h5, h6)"));
            Assert.True(texts.All(paragraph => paragraph.GetCssValue("font-family").Contains("GDS Transport")));
            // all text are gds transport
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void SearchJourneyResultsPage()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
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
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void SearchJourneyResultsLondonPage()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);
            
            driver.FindElement(By.Id("Area")).SendKeys("Southwark\t");
            driver.FindElement(By.Id("submit-search")).Click();

            var pageName = driver.FindElement(By.CssSelector("div.container H1")).Text;
            Assert.Equal("You need to use a different service", pageName);

            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);
            // test accessibility

            var links = driver.FindElements(By.CssSelector("div.container a"));
            Assert.True(links.All(anchor => anchor.GetAttribute("class").Contains("govuk-link") || anchor.GetAttribute("class").Contains("govuk-back-link")));
            // all links have govuk-link class

            var texts = driver.FindElements(By.CssSelector("div.container :is(p, li, h1, h2, h3, h4, h5, h6)"));
            Assert.True(texts.All(paragraph => paragraph.GetCssValue("font-family").Contains("GDS Transport")));
            // all text are gds transport
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void SearchingReturnsResultsForProvidersWithoutAPhoneNumber()
        {
            var relativeUrl = "/organisations-that-sell-shared-ownership-homes?Area=Allerdale";

            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host + relativeUrl);
                
            Assert.Contains("Above Derwent Community Land Trust", driver.FindElement(By.Id("main-content")).Text);
        }
    }
}

