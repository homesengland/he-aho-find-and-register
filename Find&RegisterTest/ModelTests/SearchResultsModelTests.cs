using Find_Register.DataSourceService;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using Find_Register.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Find_RegisterTest.ModelTests;

public class SearchResultsModelTests
{
    [Fact]
    public void ValidationForSearchedForAreaThrowsExceptionIfLocationsNotPopulated()
    {
        var stateDictionary = new ModelStateDictionary();

        var model = new SearchResultsModel { Area = "" };
        Assert.Throws<InvalidDataException>(() => model.ValidateLocalAuthorityAreaSearch(stateDictionary));
    }

    [Fact]
    public void SimplifiedRedirectionModelReturnsSameModelButOnlyWithArea()
    {
        var model = new SearchResultsModel {
            Area = "Somewhere",
            LocationModels = new List<LocationModel>(),
            ProviderModels = new List<ProviderModel>()
        };
        var simplifiedModel = model.SimplifiedRedirectionModel();
        Assert.Null(simplifiedModel.LocationModels);
        Assert.Null(simplifiedModel.ProviderModels);
        Assert.Equal("Somewhere", simplifiedModel.Area);
    }

    [Fact]
    public void SearchResultsAggragationCountsWork()
    {
        var model = new SearchResultsModel
        {
            Area = "Somewhere",
            LocationModels = new List<LocationModel>(),
            ProviderModels = new List<ProviderModel> {
                new ProviderModel{ Hold = true, Opso = true },
                new ProviderModel{ Hold = true, SharedOwnership = true },
                new ProviderModel{ Hold = true, RentToBuy = true, SharedOwnership = true },
            }
        };
        Assert.Equal(3, model.GetHoldCount());
        Assert.Equal(1, model.GetOpsoCount());
        Assert.Equal(2, model.GetSharedOwnershipCount());
        Assert.Equal(1, model.GetRentToBuyCount());
    }
}

