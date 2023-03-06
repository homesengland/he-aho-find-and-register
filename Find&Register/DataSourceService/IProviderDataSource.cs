using Find_Register.Models;

namespace Find_Register.DataSourceService;

public interface IProviderDataSource
{
    public IEnumerable<ProviderModel>? Providers { get; }

    public IEnumerable<ProviderModel>? ProvidersActiveInLocalAuthority(string localAuthority);
}

public interface IProviderBlobDataSource : IProviderDataSource
{
}