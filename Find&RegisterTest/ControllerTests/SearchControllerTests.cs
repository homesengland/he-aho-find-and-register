using Find_Register.DataSourceService;
using Moq;
using Microsoft.Extensions.Logging;
using Find_Register.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Find_Register.Controllers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Find_Register.Cookies;
using Microsoft.AspNetCore.Http;

namespace Find_RegisterTest.ControllerTests;

public class SearchControllerTests
{
    private readonly Mock<ISearchService> _searchServiceMock;
    private readonly Mock<ILogger<SearchController>> _loggerMock;
    private readonly Mock<IAntiforgery> _antiforgeryMock;
    private readonly Mock<ICookieHelper> _cookieHelperMock;
    private readonly Mock<IDataSources> _dataSourceMock;
    private readonly Mock<ILocationDataSource> _locationDataSourceMock;
    private readonly Mock<IProviderDataSource> _providerDataSourceMock;
    private readonly SearchController _controller;

    public SearchControllerTests()
    {
        _searchServiceMock = new Mock<ISearchService>();
        _loggerMock = new Mock<ILogger<SearchController>>();
        _antiforgeryMock = new Mock<IAntiforgery>();
        _cookieHelperMock = new Mock<ICookieHelper>();

        _dataSourceMock = new Mock<IDataSources>();
        _locationDataSourceMock = new Mock<ILocationDataSource>();
        _providerDataSourceMock = new Mock<IProviderDataSource>();

        _controller = new SearchController(
            _loggerMock.Object,
            _searchServiceMock.Object,
            _cookieHelperMock.Object,
            _antiforgeryMock.Object);
    }

    [Fact]
    public void Index_ReturnsViewWithInitializedModel()
    {
        // Arrange
        var expectedModel = new SearchResultsModel();
        _searchServiceMock.Setup(s => s.InitializeSearchModel()).Returns(expectedModel);

        // Act
        var result = _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(expectedModel, viewResult.Model);
    }

    [Fact]
    public void SearchResults_WhenModelStateIsInvalid_ReturnsIndexView()
    {
        // Arrange
        var inputModel = new SearchResultsModel { Area1 = "SomeArea" };

        _searchServiceMock.Setup(s => s.ProcessSearch(It.IsAny<SearchResultsModel>(), It.IsAny<ModelStateDictionary>()))
            .Callback<SearchResultsModel, ModelStateDictionary>((model, state) =>
            {
                state.AddModelError("TestError", "Error message");
            })
            .Returns((SearchResultsModel m, ModelStateDictionary state) => m);

        // Act
        var result = _controller.SearchResults(inputModel);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.Equal(inputModel, viewResult.Model);
        Assert.False(_controller.ModelState.IsValid);
    }

    [Fact]
    public void SearchResults_WhenNoProviderResults_ReturnsNoSearchResultsView()
    {
        // Arrange
        var inputModel = new SearchResultsModel { Area1 = "ValidArea" };

        _searchServiceMock.Setup(s => s.ProcessSearch(It.IsAny<SearchResultsModel>(), It.IsAny<ModelStateDictionary>()))
            .Returns((SearchResultsModel m, ModelStateDictionary state) => m);

        inputModel.OpsoProviderModels = Enumerable.Empty<ProviderModel>();
        inputModel.HoldProviderModels = Enumerable.Empty<ProviderModel>();
        inputModel.SharedOwnershipProviderModels = Enumerable.Empty<ProviderModel>();
        inputModel.RentToBuyProviderModels = Enumerable.Empty<ProviderModel>();

        // Act
        var result = _controller.SearchResults(inputModel);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("NoSearchResults", viewResult.ViewName);
        Assert.Equal(inputModel, viewResult.Model);
    }

    [Fact]
    public void SearchResults_WhenProviderResultsExist_ReturnsDefaultView()
    {
        // Arrange
        var inputModel = new SearchResultsModel { Area1 = "ValidArea" };

        _searchServiceMock.Setup(s => s.ProcessSearch(It.IsAny<SearchResultsModel>(), It.IsAny<ModelStateDictionary>()))
            .Returns((SearchResultsModel m, ModelStateDictionary state) => m);

        var provider = new ProviderModel { Name = "Provider1" };
        inputModel.OpsoProviderModels = new List<ProviderModel> { provider };
        inputModel.HoldProviderModels = Enumerable.Empty<ProviderModel>();
        inputModel.SharedOwnershipProviderModels = Enumerable.Empty<ProviderModel>();
        inputModel.RentToBuyProviderModels = Enumerable.Empty<ProviderModel>();

        // Act
        var result = _controller.SearchResults(inputModel);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.True(string.IsNullOrEmpty(viewResult.ViewName));
        Assert.Equal(inputModel, viewResult.Model);
    }

    [Fact]
    public void SearchResults_RemovesExistingAreaModelStateErrors_BeforeProcessing()
    {
        // Arrange
        var inputModel = new SearchResultsModel
        {
            Area1 = "Area1",
            Area2 = "Area2",
            Area3 = "Area3"
        };

        _controller.ModelState.AddModelError(nameof(inputModel.Area1), "Error1");
        _controller.ModelState.AddModelError(nameof(inputModel.Area2), "Error2");
        _controller.ModelState.AddModelError(nameof(inputModel.Area3), "Error3");

        _searchServiceMock.Setup(s => s.ProcessSearch(It.IsAny<SearchResultsModel>(), It.IsAny<ModelStateDictionary>()))
            .Returns((SearchResultsModel m, ModelStateDictionary state) => m);

        // Act
        var result = _controller.SearchResults(inputModel);

        // Assert
        Assert.False(_controller.ModelState.ContainsKey(nameof(inputModel.Area1)));
        Assert.False(_controller.ModelState.ContainsKey(nameof(inputModel.Area2)));
        Assert.False(_controller.ModelState.ContainsKey(nameof(inputModel.Area3)));
    }

    [Fact]
    public void SearchResults_EmptySearch_ReturnsIndexViewWithError()
    {
        // Arrange
        var inputModel = new SearchResultsModel { Area1 = "", Area2 = "", Area3 = "" };

        _searchServiceMock.Setup(s => s.ProcessSearch(It.IsAny<SearchResultsModel>(), It.IsAny<ModelStateDictionary>()))
            .Callback<SearchResultsModel, ModelStateDictionary>((model, state) =>
            {
                state.AddModelError(nameof(model.Area1), "Enter at least one local authority");
            })
            .Returns((SearchResultsModel m, ModelStateDictionary state) => m);

        // Act
        var result = _controller.SearchResults(inputModel);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(inputModel.Area1)));
    }

    [Fact]
    public void SearchResults_NonLondonArea_ReturnsDefaultView()
    {
        // Arrange
        var inputModel = new SearchResultsModel { Area1 = "NonLondonArea" };
        
        var provider = new ProviderModel { Name = "NonLondonProvider", Opso = true, IsLocalAuthority = false };

        _searchServiceMock.Setup(s => s.ProcessSearch(It.IsAny<SearchResultsModel>(), It.IsAny<ModelStateDictionary>()))
            .Returns((SearchResultsModel m, ModelStateDictionary state) =>
            {
                m.OpsoProviderModels = new List<ProviderModel> { provider };
                return m;
            });

        // Act
        var result = _controller.SearchResults(inputModel);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.True(string.IsNullOrEmpty(viewResult.ViewName));
        Assert.Equal(inputModel, viewResult.Model);
    }

    [Fact]
    public void SearchResults_LondonArea_ReturnsNoSearchResultsView()
    {
        // Arrange
        var inputModel = new SearchResultsModel { Area1 = "London" };

        _searchServiceMock.Setup(s => s.ProcessSearch(It.IsAny<SearchResultsModel>(), It.IsAny<ModelStateDictionary>()))
            .Returns((SearchResultsModel m, ModelStateDictionary state) =>
            {
                m.OpsoProviderModels = Enumerable.Empty<ProviderModel>();
                m.HoldProviderModels = Enumerable.Empty<ProviderModel>();
                m.SharedOwnershipProviderModels = Enumerable.Empty<ProviderModel>();
                m.RentToBuyProviderModels = Enumerable.Empty<ProviderModel>();
                return m;
            });

        // Act
        var result = _controller.SearchResults(inputModel);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("NoSearchResults", viewResult.ViewName);
    }

    [Theory]
    [InlineData("SomeArea1", null, null)]
    [InlineData(null, "SomeArea2", null)]
    [InlineData(null, null, "SomeArea3")]
    [InlineData("SomeArea1", null, "SomeArea3")]
    public void SearchResults_WhenNoValidLAModelStateIsInvalid_ReturnsIndexView(string area1, string area2, string area3)
    {
        // Arrange
        var inputModel = new SearchResultsModel { Area1 = area1, Area2 = area2, Area3 = area3 };

        _dataSourceMock.Setup(s => s.GetLocationDataSource).Returns(_locationDataSourceMock.Object);
        _dataSourceMock.Setup(s => s.GetProviderDataSource).Returns(_providerDataSourceMock.Object);
        _locationDataSourceMock.Setup(ls => ls.Locations).Returns(
            new List<LocationModel> { 
                new LocationModel {
                    LocalAuthority = "SomeArea",
                    LocationCode = "SomeArea"
                }
            }
        );

        _providerDataSourceMock.Setup(ls => ls.Providers).Returns(new List<ProviderModel>());
        _providerDataSourceMock.Setup(ls => ls.ProvidersActiveInLocalAuthority(It.IsAny<string>())).Returns(new List<ProviderModel>());

        var searchService = new SearchService(_dataSourceMock.Object, new Mock<ILogger<SearchService>>().Object);
        searchService.InitializeSearchModel();
        var controller = new SearchController(_loggerMock.Object, searchService, _cookieHelperMock.Object, _antiforgeryMock.Object);

        // Act 
        var result = controller.SearchResults(inputModel);
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName);
        Assert.False(controller.ModelState.IsValid);
        if (!string.IsNullOrEmpty(area1))
        {
            Assert.True(controller.ModelState.ContainsKey(nameof(inputModel.Area1)));
        }
        if (!string.IsNullOrEmpty(area2))
        {
            Assert.True(controller.ModelState.ContainsKey(nameof(inputModel.Area2)));
        }
        if (!string.IsNullOrEmpty(area3))
        {
            Assert.True(controller.ModelState.ContainsKey(nameof(inputModel.Area3)));
        }
        Assert.Contains(controller.ModelState.Values, v => v.ValidationState == ModelValidationState.Invalid);
    }

}