using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Find_Register.Controllers;
using Find_Register.Filters;
using Microsoft.AspNetCore.Http;
using Find_Register.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Find_Register.Models;
using Moq;
using Microsoft.Extensions.Configuration;
using Find_RegisterTest;
using FluentAssertions;

namespace FindAndRegisterUnitTest;

public class XUnitTest
{
    private ILogger<EligibilityController> Logger => new Mock<ILogger<EligibilityController>>().Object;
 

    private readonly EligibilityController _eligibilityController;

    public XUnitTest()
    {
        var mockICookieHelper = new Mock<ICookieHelper>();

        var mockIConfig = new MockConfig();
        mockIConfig.Add("BaseUrl", "https://somewhere.somedomain.uk");
        _eligibilityController = new EligibilityController(Logger, mockICookieHelper.Object, mockIConfig);

        _eligibilityController.ControllerContext = new ControllerContext();
    }

    [Fact]
    [Trait("XUnit", "Smoke")]
    public void CheckHomePage()
    {
        var result = _eligibilityController.Index();
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(200, _eligibilityController.Ok().StatusCode);
    }

    [Fact]
    [Trait("XUnit", "Smoke")]
    public void CheckGlobalFilter()
    {
        var result = _eligibilityController.Index();

        // Create a default ActionContext (depending on our case-scenario)
        var actionContext = new ActionContext()
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };
        // Create the filter input parameters (depending on our case-scenario)
        var resourceExecutingContext = new ActionExecutedContext(
            actionContext,
            new List<IFilterMetadata>(),
            new List<IValueProviderFactory>()
            );

        // Act (Call the method under test with the arranged parameters)
        CustomAsyncResultFilterAttribute multipleActionsFilter = new CustomAsyncResultFilterAttribute();
        multipleActionsFilter.OnActionExecuted(resourceExecutingContext);

        Assert.Equal("", resourceExecutingContext.HttpContext.Items["errors"]);
    }

    [Fact]
    public void CommonErrorsComponentTest()
    {
        var attribute = new CustomAsyncResultFilterAttribute();
        // Create a default ActionContext (depending on our case-scenario)
        var actionContext = new ActionContext()
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };
        var resourceExecutingContext = new ActionExecutedContext(
            actionContext,
            new List<IFilterMetadata>(),
            new List<IValueProviderFactory>()
            );

        resourceExecutingContext.ModelState.AddModelError("testkey", "testerror");
        attribute.OnActionExecuted(resourceExecutingContext);

        Assert.IsType<List<ErrorSummary>>(resourceExecutingContext.HttpContext.Items["errors"]);
        var errors = resourceExecutingContext.HttpContext.Items["errors"] as List<ErrorSummary>;

        Assert.Single(errors!);
    }

    [Fact]
    public void GeneralStatusErrorsTest()
    {
        ////Arrange
        //var webPage = _eligibilityController;
        //webPage.StatusCode(500);

        //var actionContext = new ActionContext()
        //{
        //    HttpContext = new DefaultHttpContext(),
        //    RouteData = new RouteData(),
        //    ActionDescriptor = new ActionDescriptor()
        //};
        //var resourceExecutingContext = new ActionExecutedContext(
        //    actionContext,
        //    new List<IFilterMetadata>(),
        //    new List<IValueProviderFactory>()
        //);

        //// Act (Call the method under test with the arranged parameters)
        //resourceExecutingContext.ModelState.AddModelError("testkey", "testerror");
        ////attribute.OnActionExecuted(resourceExecutingContext);
        //webPage.Index();

        //var url = webPage.Url;

        ////Assert
        //Assert.IsType<List<ErrorSummary>>(resourceExecutingContext.HttpContext.Items["errors"]);
        //var errors = resourceExecutingContext.HttpContext.Items["errors"] as List<ErrorSummary>;

        //Assert.Single(errors!);




        
    }
}

