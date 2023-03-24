using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Find_Register.Models
{
    public class SearchResultsModel
    {
        [Required(ErrorMessage = "Enter a local authority")]
        public string? Area { get; set; }

        public ProviderModel? LocalAuthority { get; set; }
        public IEnumerable<ProviderModel>? ProviderModels {get;set;}
        public List<LocationModel>? LocationModels { get; set; }

        public int GetOpsoCount() => ProviderModels?.Count(p => p.Opso) ?? 0;
        public int GetHoldCount() => ProviderModels?.Count(p => p.Hold) ?? 0;
        public int GetSharedOwnershipCount() => ProviderModels?.Count(p => p.SharedOwnership) ?? 0;
        public int GetRentToBuyCount() => ProviderModels?.Count(p => p.RentToBuy) ?? 0;

        public void ValidateLocalAuthorityAreaSearch(ModelStateDictionary modelState)
        {
            if (LocationModels == null) throw new InvalidDataException("Cannot validate area without locations loaded first");
            if (LocationModels.All(l => !(l.LocalAuthority == Area)))
            {
                modelState.AddModelError(nameof(Area), "Enter a valid local authority");
            }
        }

        /// <summary>
        /// Remove data lists from the model before passing on in redirects
        /// </summary>
        /// <returns>A reduced SearchResultsModel with minimum data required for displaying errors</returns>
        public SearchResultsModel SimplifiedRedirectionModel() {
            return new SearchResultsModel
            {
                Area = Area
            };
        }
    }
}