using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Find_Register.Models
{
    public class SearchResultsModel
    {
        [Required(ErrorMessage = "Enter a local authority")]
        public string? Area { get; set; }

        public IEnumerable<ProviderModel>? ProviderModels {get;set;}
        public List<LocationModel>? LocationModels { get; set; }

    }
}