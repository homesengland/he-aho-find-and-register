namespace Find_Register.DataSourceService;

public interface IDataSources
{
    public ILocationDataSource GetLocationDataSource { get; }
    public IProviderDataSource GetProviderDataSource { get; }
}
