using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace FindAndRegisterIntegrationTests
{
    public class LoadTest: SeleniumTestsBase
    {
        public const int PageLoadTimeThreshold = 5;
        private const int NumUsers = 50;
        
        [Theory]
        [InlineData("Arun")]
        [Trait("Selenium", "LoadTest")]
        public async Task LoadTestForSearchForProviders(string area)
        {
            const int numRequestsPerUser = 1;
            string url = $"{Host}find-organisations-selling-shared-ownership-homes/organisations-that-sell-shared-ownership-homes?Area={area}";

            HttpClient client = new();

            var tasks = new Task[NumUsers];
            for (var i = 0; i < NumUsers; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    for (var j = 0; j < numRequestsPerUser; j++)
                    {
                        Stopwatch timePerRequest = new();
                        timePerRequest.Start();
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                        // request.Content = new StringContent($"Area={area}");
                        // request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        HttpResponseMessage response = await client.SendAsync(request);
                        string result = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"request time: minutes: {timePerRequest.Elapsed.Minutes} - seconds: {timePerRequest.Elapsed.Seconds}");
                        Assert.True(response.StatusCode == HttpStatusCode.OK);
                    }
                });
            }

            await Task.WhenAll(tasks);

        }

        public static long GetPageLoadTimeInSeconds(IWebDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var responseTime
                = Convert.ToInt32(js.ExecuteScript
                    ("return window.performance.timing.domContentLoadedEventEnd-window.performance.timing.navigationStart;"));
            return  responseTime/1000;
        }
    }
}