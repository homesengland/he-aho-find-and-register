using Find_Register.Controllers;
using Find_Register.Models;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Find_RegisterTest;
using Microsoft.Extensions.Logging;
using Moq;

public class CurrentSituationTests
{
    private ILogger<HomeController> Logger => new Mock<ILogger<HomeController>>().Object;
    
    [Fact]
    public void CurrentSituation_Should_Redirect_To_Own_Home_View()
    {
        // Given
        var cookieHelper = new MockCookieHelper();
        HomeController homeController = new(Logger, cookieHelper);
        CurrentSituation currentSituation = new()
        {
            AnyTrueForCurrentCircumstances = true
        };

        // When
        var result = homeController.CurrentSituation(currentSituation) as RedirectToActionResult;
        // Then
        result?.ActionName.Should().Be(nameof(homeController.OwnHome));

    }

    [Fact]
    public void CurrentSituation_Stores_Cookie_Value()
    {
        // Given
        var cookieHelper = new MockCookieHelper();
        HomeController homeController = new(Logger, cookieHelper);
        CurrentSituation currentSituation = new()
        {
            AnyTrueForCurrentCircumstances = true
        };
        // When
        var result = homeController.CurrentSituation(currentSituation) as RedirectToActionResult;
        // Then
        cookieHelper.GetApplicationCookieData(null,null).EligibilityResponses.Value.CurrentSituation.AnyTrueForCurrentCircumstances.Should().Be(true);
    }
}
