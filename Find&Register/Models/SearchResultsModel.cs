using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Find_Register.Models
{
    public class SearchResultsModel
    {
        [Required]
        public string? Area { get; set; }

        public List<ProviderModel>? ProviderModels {get;set;}
        public List<LocationModel>? LocationModels { get; set; }

    }
}