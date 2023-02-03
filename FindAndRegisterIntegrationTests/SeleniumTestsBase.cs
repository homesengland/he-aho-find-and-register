using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using OpenQA.Selenium;

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
}
