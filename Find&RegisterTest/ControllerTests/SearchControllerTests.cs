using System.Collections.Generic;
using Find_Register.DataSourceService;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using Find_Register.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Find_Register.Controllers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Find_RegisterTest.ControllerTests;

public class SearchControllerTests
{
    [Fact]
    public void WhenSearchResultHasOnlyLocalAuthorityReturnsNoSearchResultsFoundPage()
    {
        var mockLogger = new Mock<ILogger<SearchController>>();
        var mockDataSource = new Mock<IDataSources>();
        var mockLs = new Mock<ILocationDataSource>();
        var mockPs = new Mock<IProviderDataSource>();
        mockDataSource.Setup(s => s.GetLocationDataSource).Returns(mockLs.Object);
        mockDataSource.Setup(s => s.GetProviderDataSource).Returns(mockPs.Object);
        var returnResult = new ProviderModel { IsLocalAuthority = true, Locations = new List<string> { "Somewhere" } };
        mockLs.Setup(ls => ls.Locations).Returns(new List<LocationModel> { new LocationModel { LocalAuthority = "Somewhere", LocationCode = "Somewhere" } });
        mockPs.Setup(ls => ls.Providers).Returns(new List<ProviderModel> { returnResult });
        mockPs.Setup(ls => ls.ProvidersActiveInLocalAuthority(It.IsAny<string>())).Returns(new List<ProviderModel> { returnResult });

        var controller = new SearchController(mockLogger.Object, mockDataSource.Object, new MockCookieHelper(), new Mock<IAntiforgery>().Object);
        controller.ControllerContext.HttpContext = new DefaultHttpContext {Request = {Method = "post"}};

        var actionResult = controller.SearchResults(new SearchResultsModel { Area = "Somewhere" }) as ViewResult;
        Assert.Equal("NoSearchResults", actionResult?.ViewName);
    }

    [Fact]
    public void WhenSearchResultsIsEmptyReturnsNoSearchResultsFoundPage()
    {
        var mockLogger = new Mock<ILogger<SearchController>>();
        var mockDataSource = new Mock<IDataSources>();
        var mockLs = new Mock<ILocationDataSource>();
        var mockPs = new Mock<IProviderDataSource>();
        mockDataSource.Setup(s => s.GetLocationDataSource).Returns(mockLs.Object);
        mockDataSource.Setup(s => s.GetProviderDataSource).Returns(mockPs.Object);
        mockLs.Setup(ls => ls.Locations).Returns(new List<LocationModel> { new LocationModel { LocalAuthority = "Somewhere", LocationCode = "Somewhere" } });
        mockPs.Setup(ls => ls.Providers).Returns(new List<ProviderModel> ());
        mockPs.Setup(ls => ls.ProvidersActiveInLocalAuthority(It.IsAny<string>())).Returns(new List<ProviderModel>());

        var controller = new SearchController(mockLogger.Object, mockDataSource.Object, new MockCookieHelper(), new Mock<IAntiforgery>().Object);
        controller.ControllerContext.HttpContext = new DefaultHttpContext {Request = {Method = "post"}};

        var actionResult = controller.SearchResults(new SearchResultsModel { Area = "Somewhere" }) as ViewResult;
        Assert.Equal("NoSearchResults", actionResult?.ViewName);
    }

    [Fact]
    public void WhenSeachingForALondonAreaReturnLondonResultLink()
    {
        var mockLogger = new Mock<ILogger<SearchController>>();
        var mockDataSource = new Mock<IDataSources>();
        var mockLs = new Mock<ILocationDataSource>();
        var mockPs = new Mock<IProviderDataSource>();
        mockDataSource.Setup(s => s.GetLocationDataSource).Returns(mockLs.Object);
        mockDataSource.Setup(s => s.GetProviderDataSource).Returns(mockPs.Object);
        mockLs.Setup(ls => ls.Locations).Returns(new List<LocationModel> { new LocationModel {
            LocalAuthority = "Somewhere",
            LocationCode = "Somewhere",
            AreaCode = "London Borough"
        }});
        mockPs.Setup(ls => ls.Providers).Returns(new List<ProviderModel>());
        mockPs.Setup(ls => ls.ProvidersActiveInLocalAuthority(It.IsAny<string>())).Returns(new List<ProviderModel>());

        var controller = new SearchController(mockLogger.Object, mockDataSource.Object, new MockCookieHelper(), new Mock<IAntiforgery>().Object);
        controller.ControllerContext.HttpContext = new DefaultHttpContext {Request = {Method = "post"}};
        var actionResult = controller.SearchResults(new SearchResultsModel { Area = "Somewhere" }) as ViewResult;
        Assert.Equal("SearchResultsLondon", actionResult?.ViewName);
    }

    [Fact]
    public void WhenSeachingForANonExistantAreaModelStateBecomesInvalidAndReturnUserToIndex()
    {
        var mockLogger = new Mock<ILogger<SearchController>>();
        var mockDataSource = new Mock<IDataSources>();
        var mockLs = new Mock<ILocationDataSource>();
        var mockPs = new Mock<IProviderDataSource>();
        mockDataSource.Setup(s => s.GetLocationDataSource).Returns(mockLs.Object);
        mockDataSource.Setup(s => s.GetProviderDataSource).Returns(mockPs.Object);        
        mockLs.Setup(ls => ls.Locations).Returns(new List<LocationModel> { new LocationModel {
            LocalAuthority = "Somewhere",
            LocationCode = "Somewhere"
        }});
        mockPs.Setup(ls => ls.Providers).Returns(new List<ProviderModel>());
        mockPs.Setup(ls => ls.ProvidersActiveInLocalAuthority(It.IsAny<string>())).Returns(new List<ProviderModel>());

        var controller = new SearchController(mockLogger.Object, mockDataSource.Object, new MockCookieHelper(), new Mock<IAntiforgery>().Object);
        controller.ControllerContext.HttpContext = new DefaultHttpContext {Request = {Method = "post"}};

        var actionResult = controller.SearchResults(new SearchResultsModel { Area = "abcdefg" }) as ViewResult;
        Assert.Equal(ModelValidationState.Invalid, actionResult?.ViewData.ModelState.ValidationState);
        Assert.Equal(nameof(SearchController.Index), actionResult?.ViewName);        
    }

    [Fact]
    public void WhenSeachHasResultsModelStateIsValid()
    {
        var mockLogger = new Mock<ILogger<SearchController>>();
        var mockDataSource = new Mock<IDataSources>();
        var mockLs = new Mock<ILocationDataSource>();
        var mockPs = new Mock<IProviderDataSource>();
        mockDataSource.Setup(s => s.GetLocationDataSource).Returns(mockLs.Object);
        mockDataSource.Setup(s => s.GetProviderDataSource).Returns(mockPs.Object);
        var returnResult = new ProviderModel { Locations = new List<string> { "Somewhere" } };
        mockLs.Setup(ls => ls.Locations).Returns(new List<LocationModel> { new LocationModel {
            LocalAuthority = "Somewhere",
            LocationCode = "Somewhere"
        }});
        mockPs.Setup(ls => ls.Providers).Returns(new List<ProviderModel> { returnResult });
        mockPs.Setup(ls => ls.ProvidersActiveInLocalAuthority(It.IsAny<string>())).Returns(new List<ProviderModel> { returnResult });

        var controller = new SearchController(mockLogger.Object, mockDataSource.Object, new MockCookieHelper(), new Mock<IAntiforgery>().Object);
        controller.ControllerContext.HttpContext = new DefaultHttpContext {Request = {Method = "post"}};
        var actionResult = controller.SearchResults(new SearchResultsModel { Area = "Somewhere" }) as ViewResult;
        Assert.Equal(ModelValidationState.Valid, actionResult?.ViewData.ModelState.ValidationState);
        Assert.Null(actionResult?.ViewName); // view name null uses controller's view resolver using the action name
    }
}