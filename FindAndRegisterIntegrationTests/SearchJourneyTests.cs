using System;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Deque.AxeCore.Selenium;
using System.Text;
using Microsoft.AspNetCore.Mvc;

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
        public void SearchJourneyAccessibilityTest()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);

            var axeResult = new AxeBuilder(driver)
                .WithTags("wcag21aa", "best-practice").Analyze();
            Assert.True(axeResult.Violations.Length == 1); // I would expect one error for two invisible labels on the search form. We can't change the requirements so either we should make the test case redundant or validate it with number of errors as I've done.
        }

        [Fact]
        [Trait("Selenium", "Smoke")]
        public void SearchJourneyStartPage()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);

            DoGDSTestsOnSearchPage(driver);
        }

        [Theory]
        [Trait("Selenium", "Smoke")]
        [InlineData("Adur", null, null)]
        [InlineData(null, "Brighton and Hove", null)]
        [InlineData(null, null, "Southampton")]
        [InlineData("Peterborough", null, "Adur")]
        public void SearchJourneyResultsPageForValidLAs(string area1, string area2, string area3)
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);

            var Areas = new List<string> { area1, area2, area3 };
            for (int j = 0; j < Areas.Count; j++)
            {
                if (Areas[j] != null)
                {
                    driver.FindElement(By.Id($"Area{j + 1}")).SendKeys(Areas[j]+"\t");
                }
            }
            Areas = Areas.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();

            driver.FindElement(By.Id("submit-search")).Click();

            // test header title
            var pageTitle = driver.FindElement(By.CssSelector("div.container H1")).Text;
            Assert.Equal($"Organisations offering homes in your local authority area{(Areas.Count() > 1 ? "s": "")}", pageTitle);

            // test header counsil container
            for (int j = 0; j < Areas.Count; j++)
            {
                var headerCounsilList = driver.FindElement(By.ClassName("counsil-container"))
                    .FindElement(By.TagName("ul"))
                    .FindElements(By.TagName("li")).ElementAt(j);
                Assert.Equal(Areas[j], headerCounsilList.Text);
            }

            // test no of categories (at least 4 categories are listed)
            var categories = driver.FindElements(By.ClassName("govuk-accordion__section-heading-text-focus"));
            Assert.True(categories.Count >= 4);

            // test the "Search again button and its attributes"
            var searchAgainButton = driver.FindElement(By.CssSelector("input.govuk-button"));
            Assert.Equal("Search again", searchAgainButton.GetAttribute("Value"));
            Assert.Equal(Host, searchAgainButton.FindElement(By.XPath("..")).GetAttribute("href"));

            DoAccessibilityAndGDSTestsOnResultPage(driver);
        }

        [Theory]
        [Trait("Selenium", "Smoke")]
        [InlineData("London", null, null)]
        [InlineData(null, "Somewhere", null)]
        [InlineData(null, null, "Somewhere")]
        [InlineData("Somewhere", null, "Somewhere")]
        public void SearchJourneyResultsPageForNotValidLAs(string area1, string area2, string area3)
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Host);

            var Areas = new List<string> { area1, area2, area3 };
            for (int j = 0; j < Areas.Count; j++)
            {
                if (Areas[j] != null)
                {
                    driver.FindElement(By.Id($"Area{j + 1}")).SendKeys(Areas[j]);
                }
            }

            driver.FindElement(By.Id("submit-search")).Click();
            driver.FindElement(By.Id("submit-search")).Click();

            // test error title
            var errorTitle = driver.FindElement(By.ClassName("govuk-error-summary__title")).Text;
            Assert.Equal("There is a problem", errorTitle);

            // test error body test
            var ErrorPointer = string.Empty;
            var errorIndex = 0;
            for (int j = 0; j < Areas.Count; j++)
            {
                if (Areas[j] != null)
                {
                    var errorBody = driver.FindElement(By.ClassName("govuk-error-summary__body"))
                        .FindElement(By.TagName("ul"))
                        .FindElements(By.TagName("li")).ElementAt(errorIndex)
                        .FindElement(By.CssSelector("a"));
                    Assert.True(errorBody.Text.Contains($"Enter a valid local authority as choice {(j + 1)}",StringComparison.OrdinalIgnoreCase));
                    ErrorPointer += $"Area{(j + 1)}={Areas[j]}&";
                    errorIndex++;
                } else
                {
                    ErrorPointer += $"Area{(j + 1)}=&";
                }
            }

            ErrorPointer = new string(ErrorPointer.Take(ErrorPointer.Length - 1).ToArray());

            // test error body link
            errorIndex = 0;
            for (int j = 0; j < Areas.Count; j++)
            {
                if (Areas[j] != null)
                {
                    var errorBody = driver.FindElement(By.ClassName("govuk-error-summary__body"))
                        .FindElement(By.TagName("ul"))
                        .FindElements(By.TagName("li")).ElementAt(errorIndex)
                        .FindElement(By.CssSelector("a"));
                    Assert.Equal(Host + $"/organisations-that-sell-shared-ownership-homes?{ErrorPointer}#Area{(j + 1)}", errorBody.GetAttribute("href"));
                    errorIndex++;
                }
            }
        }

        private void DoGDSTestsOnSearchPage(IWebDriver driver)
        {
            // all links have govuk-link class
            var links = driver.FindElements(By.CssSelector("div.container a"));
            Assert.True(links.All(anchor => anchor.GetAttribute("class").Contains("govuk-link") || anchor.GetAttribute("class").Contains("govuk-back-link")));

            // all text are gds transport
            var texts = driver.FindElements(By.CssSelector("div.container :is(p, li, h1, h2, h3, h4, h5, h6)"));
            Assert.True(texts.All(paragraph => paragraph.GetCssValue("font-family").Contains("GDS Transport")));
        }

        private void DoAccessibilityAndGDSTestsOnResultPage(IWebDriver driver)
        {
            // test accessibility
            var axeResult = new AxeBuilder(driver).WithTags("wcag21aa", "best-practice").Analyze();
            Assert.Empty(axeResult.Violations);

            // all links have govuk-link class
            var links = driver.FindElements(By.CssSelector("div.container a"));
            Assert.True(links.All(anchor => anchor.GetAttribute("class").Contains("govuk-link") || anchor.GetAttribute("class").Contains("govuk-back-link")));

            // all text are gds transport
            var texts = driver.FindElements(By.CssSelector("div.container :is(p, li, h1, h2, h3, h4, h5, h6)"));
            Assert.True(texts.All(paragraph => paragraph.GetCssValue("font-family").Contains("GDS Transport")));
        }
    }
}

