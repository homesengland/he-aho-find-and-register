using Find_Register.Models;

namespace Find_Register.Models;

/// <summary>
/// *** Sample type for Json serializable cookie ***
/// Note: If using a reference type (class) for underlaying cookie class, please ensure you trigger the value setter
/// to update the HttpResponse cookie.
/// </summary>
public struct EligibilityResponses
{
    public AnnualIncome AnnualIncome { get; set; }
    public CurrentSituation CurrentSituation { get; set; }
    public EligibilityWhereDoYouWantToBuyAHomePage EligibilityWhereDoYouWantToBuyAHomePage { get; set; }
}
