using Microsoft.AspNetCore.Http;

namespace Find_RegisterTest.CookiesTests;

// Create a fake class for this instead of mocking through Moq to avoid having to mock the linq methods
public class RequestCookiesTestCollection : Dictionary<string, string>, IRequestCookieCollection
{
    ICollection<string> IRequestCookieCollection.Keys => Keys;
}