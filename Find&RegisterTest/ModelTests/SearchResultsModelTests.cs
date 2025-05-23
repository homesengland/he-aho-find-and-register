using Find_Register.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Find_RegisterTest.ModelTests;

public class SearchResultsModelTests
{
    [Fact]
    public void ValidationForSearchedForAreaThrowsExceptionIfLocationsNotPopulated()
    {
        var stateDictionary = new ModelStateDictionary();

        var model = new SearchResultsModel { Area1 = "" , Area2 = "", Area3 = "" };
        Assert.Throws<InvalidDataException>(() => model.ValidateLocalAuthorityAreaSearch(stateDictionary));
    }

    [Fact]
    public void SimplifiedRedirectionModelReturnsSameModelButOnlyWithArea()
    {
        var model = new SearchResultsModel {
            Area1 = "Somewhere",
            LocationModels = new List<LocationModel>()
        };
        var simplifiedModel = model.SimplifiedRedirectionModel();
        Assert.Null(simplifiedModel.LocationModels);
        Assert.Equal("Somewhere", simplifiedModel.Area1);
    }
}

