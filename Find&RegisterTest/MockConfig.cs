using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Find_RegisterTest;

/// <summary>
/// A simple dictionary to act as IConfiguration for unit testing
/// </summary>
public class MockConfig : Dictionary<string, string>, IConfiguration
{
    public IEnumerable<IConfigurationSection> GetChildren() => throw new NotImplementedException();
    public IChangeToken GetReloadToken() => throw new NotImplementedException();
    public IConfigurationSection GetSection(string key) => throw new NotImplementedException();
}
