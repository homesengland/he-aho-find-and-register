namespace Find_Register.Models
{
    public class SearchResultsByAreaModel
    {
        public string? LocalAuthority { get; set; } = string.Empty;

        public IEnumerable<ProviderModel> OpsoProviderModels { get; set; } = new List<ProviderModel>();
        public IEnumerable<ProviderModel> HoldProviderModels { get; set; } = new List<ProviderModel>();
        public IEnumerable<ProviderModel> SharedOwnershipProviderModels { get; set; } = new List<ProviderModel>();
        public IEnumerable<ProviderModel> RentToBuyProviderModels { get; set; } = new List<ProviderModel>();
        public IEnumerable<ProviderModel> LaModels { get; set; } = new List<ProviderModel>();
    }
}