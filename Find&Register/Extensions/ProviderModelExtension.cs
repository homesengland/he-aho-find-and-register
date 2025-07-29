using Find_Register.Models;

namespace Find_Register.Extensions
{
    public static class ProviderModelExtension
    {
        public static void GetListOfLaNamesFromLocations(this IEnumerable<ProviderModel> model, List<LocationModel>? matchedLocations)
        {
            foreach (var item in model.AsEnumerable())
            {
                item.LaLocations = matchedLocations?.Where(x => x.LocationCode != null && item.Locations != null && item.Locations.Contains(x.LocationCode)).Select(x => x.LocalAuthority!).ToList() ?? new List<string>();
            };
        }

        public static void SetListOfOragnisationCountsInLocations(this List<OrganisationResultModel> model, List<LocationModel>? matchedLocations, List<ProviderModel> providers)
        {
            var temp = new List<OrganisationResultModel>();
            var indexedMatchedLocations = matchedLocations!.Select((value, index) => new { Index = index, Value = value });
            foreach (var la in indexedMatchedLocations!)
            {
                var no_of_organisations = providers.DistinctBy(x => x.Name).Where(x => x.SharedOwnership || x.Opso || x.Hold || x.RentToBuy).Count(x => x.Locations.Contains(la.Value.LocationCode!));
                temp.Add(new OrganisationResultModel
                {
                    Id = la.Index,
                    Name = la.Value.LocalAuthority!,
                    No = no_of_organisations
                });
            }
            model.AddRange(temp);
        }
    }
}
