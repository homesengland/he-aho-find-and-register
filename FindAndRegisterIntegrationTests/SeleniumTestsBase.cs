using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using OpenQA.Selenium;
using System.Web;

namespace FindAndRegisterIntegrationTests;

public abstract class SeleniumTestsBase
{
    protected string Host;
    public SeleniumTestsBase()
    {
        var builder = WebApplication.CreateBuilder();
        ConfigurationManager configuration = builder.Configuration;
        Host = configuration.GetValue<string>("BaseUrl");
        
    }

    protected void ClickSubmit(IWebDriver driver)
    {
        driver.FindElement(By.XPath("//main//button[@type='submit']")).Click();
    }

    protected static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return HttpUtility.UrlEncode(System.Convert.ToBase64String(plainTextBytes));
    }

    protected static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(HttpUtility.UrlDecode(base64EncodedData));
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}
