namespace Find_Register.Models;

/// <summary>
/// Cookie descriptions for the cookie policy table
/// </summary>
public class CookieDescriptions
{
    public List<CookieDescription> Descriptions { get; set; } = new List<CookieDescription>();
}

public class CookieDescription
{
    public CookieDescription(string name, string purpose, string expiry)
    {
        Name = name;
        Purpose = purpose;
        Expiry = expiry;
    }

    public string Name { get; set; }
    public string Purpose { get; set; }
    public string Expiry { get; set; }
}