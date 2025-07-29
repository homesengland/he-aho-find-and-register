using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Find_Register.Models
{
    public class SearchResultsModel
    {
        public string Area1 { get; set; } = string.Empty;
        public string Area2 { get; set; } = string.Empty;
        public string Area3 { get; set; } = string.Empty;

        public List<OrganisationResultModel> OrganisationsInAreas = new List<OrganisationResultModel>();

        public List<LocationModel>? LocationModels { get; set; }

        public int GetOpsoCount() => OpsoProviderModels.Distinct().Count();
        public int GetHoldCount() => HoldProviderModels.Distinct().Count();
        public int GetSharedOwnershipCount() => SharedOwnershipProviderModels.Distinct().Count();
        public int GetRentToBuyCount() => RentToBuyProviderModels.Distinct().Count();

        public IEnumerable<ProviderModel> OpsoProviderModels { get; set; } = new List<ProviderModel>();
        public IEnumerable<ProviderModel> HoldProviderModels { get; set; } = new List<ProviderModel>();
        public IEnumerable<ProviderModel> SharedOwnershipProviderModels { get; set; } = new List<ProviderModel>(); 
        public IEnumerable<ProviderModel> RentToBuyProviderModels { get; set; } = new List<ProviderModel>();
        public IEnumerable<ProviderModel> LaModels { get; set; } = new List<ProviderModel>();

        public void ValidateLocalAuthorityAreaSearch(ModelStateDictionary modelState)
        {
            if (LocationModels == null) throw new InvalidDataException("Cannot validate area without locations loaded first");
            var areasToCheck = new List<string> { Area1, Area2, Area3 }
                .Where(area => !string.IsNullOrEmpty(area))
                .ToList();
            if (!LocationModels.Any(l => areasToCheck.Contains(l.LocalAuthority!)))
            {
                modelState.AddModelError(nameof(Area1), "Enter at least one valid local authority");
            }
        }
        public SearchResultsModel SimplifiedRedirectionModel() {
            return new SearchResultsModel
            {
                Area1 = Area1,
                Area2 = Area2,
                Area3 = Area3,
            };
        }
    }
}