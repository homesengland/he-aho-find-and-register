using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using OpenQA.Selenium;
using System.Web;

namespace FindAndRegisterIntegrationTests;

//Base class for all integration test classes to inherit.
public abstract class SeleniumTestsBase
{
    protected string Host;

    //constructor to instantiate variable at run time
    public SeleniumTestsBase()
    {
        //these three lines of code allows us to use variable set within the appsettings.json file
        var builder = WebApplication.CreateBuilder();
        ConfigurationManager configuration = builder.Configuration;
        //BaseUrl is variable within the appsettings.json file. you can change the variable string to point to the dev/qa/uat URL
        //currently it has been set to run on local host which requires us to have an instance of the code running.
        Host = configuration.GetValue<string>("BaseUrl");
        
    }

    //the below functions are reusable functions to be used within inherited classes
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
