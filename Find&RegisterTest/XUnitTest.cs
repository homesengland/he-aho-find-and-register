using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;

namespace FindAndRegisterUnitTest;

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

public class XUnitTest
{
    private ILogger<HomeController> Logger => new Mock<ILogger<HomeController>>().Object;
    
    
    private readonly HomeController _homeController;

    public XUnitTest()
    {
        var mockICookieHelper = new Mock<ICookieHelper>();
        _homeController = new HomeController(Logger,mockICookieHelper.Object);
        _homeController.ControllerContext = new ControllerContext();
    }

    [Fact]
    [Trait("XUnit", "Smoke")]
    public void CheckHomePage()
    {
        var result = _homeController.Index();
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(200, _homeController.Ok().StatusCode);
    }

    [Fact]
    [Trait("XUnit", "Smoke")]
    public void CheckGlobalFilter()
    {
        var result = _homeController.AnnualIncome();

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
}

