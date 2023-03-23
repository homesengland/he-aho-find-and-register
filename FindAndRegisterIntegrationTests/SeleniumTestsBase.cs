using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using System.Web;
using Microsoft.Graph;
using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;

namespace FindAndRegisterIntegrationTests;

//Base class for all integration test classes to inherit.
public abstract class SeleniumTestsBase
{
    protected string Host;
    protected string[] Pages;


    //constructor to instantiate variable at run time
    public SeleniumTestsBase()
    {
        //these three lines of code allows us to use variable set within the appsettings.json file
        var builder = WebApplication.CreateBuilder();
        var configuration = builder.Configuration;
        Pages = configuration.GetSection("Pages").Get<string[]>();
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
        return HttpUtility.UrlEncode(Convert.ToBase64String(plainTextBytes));
    }

    protected static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(HttpUtility.UrlDecode(base64EncodedData));
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}