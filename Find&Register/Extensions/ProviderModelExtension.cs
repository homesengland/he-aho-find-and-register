using Find_Register.Models;

namespace Find_Register.Extensions
{
    public static class ProviderModelExtension
    {
        public static void GetListOfLaNamesFromLocations(this IEnumerable<ProviderModel> model, List<LocationModel>? matchedLocations)
        {
            foreach (var item in model.AsEnumerable())
            {
                item.Locations = matchedLocations?.Where(x => x.LocationCode != null && item.Locations != null && item.Locations.Contains(x.LocationCode)).Select(x => x.LocalAuthority!).ToList() ?? new List<string>();
            };
        }
    }
}
