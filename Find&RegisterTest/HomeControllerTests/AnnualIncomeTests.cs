using Find_Register.Controllers;
using Find_Register.Models;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Find_RegisterTest;
using Microsoft.Extensions.Logging;
using Moq;

public class AnnualIncomeTests
{
    private ILogger<HomeController> Logger => new Mock<ILogger<HomeController>>().Object;

    [Fact]
    public void AnnualIncome_Should_Redirect_To_View()
    {
        // Given
        var cookieHelper = new MockCookieHelper();
        HomeController homeController = new(Logger, cookieHelper);
        AnnualIncome annualIncome = new()
        {
            IsYourTotalIncomeAboveThreshold = true
        };

        // When
        var result = homeController.AnnualIncome(annualIncome) as RedirectToActionResult;
        // Then
        result?.ActionName.Should().Be(nameof(homeController.CurrentSituation));

    }

    [Fact]
    public void AnnualIncome_Stores_Cookie_Value()
    {
        // Given
        var cookieHelper = new MockCookieHelper();
        HomeController homeController = new(Logger, cookieHelper);
        AnnualIncome annualIncome = new()
        {
            IsYourTotalIncomeAboveThreshold = true
        };
        // When
        var result = homeController.AnnualIncome(annualIncome) as RedirectToActionResult;
        // Then
        cookieHelper.GetApplicationCookieData(null,null).EligibilityResponses.Value.AnnualIncome.IsYourTotalIncomeAboveThreshold.Should().Be(true);
    }
}
