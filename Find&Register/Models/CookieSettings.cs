using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace Find_Register.Models;

public class CookieSettings
{
    [Required(ErrorMessage = "Please select if you accept analytics cookies.")]
    public bool? AcceptAnalyticsCookies { get; set; }

    public string? BackUrl { get; set; }
}