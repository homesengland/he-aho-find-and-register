using Find_Register.Models;

namespace Find_Register.Models;

public struct EligibilityResponses
{
    public AnnualIncome AnnualIncome { get; set; }
    public CurrentSituation CurrentSituation { get; set; }
    public EligibilityJourneyWhereDoYouWantToBuyAHome EligibilityJourneyWhereDoYouWantToBuyAHome { get; set; }
    public EligibilityJourneyBuyingWithAnotherPerson EligibilityJourneyBuyingWithAnotherPerson { get; set; }
    public EligibilityJourneyHowMuchDoYouEarn EligibilityJourneyHowMuchDoYouEarn { get; set; }
    public EligibilityJourneyHowMuchDoYouEarn_MultiplePeople EligibilityJourneyHowMuchDoYouEarn_MultiplePeople { get; set; }
    public EligibilityJourneyFirstTimeBuyer EligibilityJourneyFirstTimeBuyer { get; set; }
    public EligibilityJourneyAffordability EligibilityJourneyAffordability { get; set; }
    public string EligibilityOutcome { get; set; }
    public bool FirstTimeBuyer { get; set; }
    public bool AffordableWithoutSharedOwnership { get; set; }
    public string PreviousPage { get; set; }
    public string PreviousPageBeforeErrorOutcome { get; set; }
    public string LastJourneyPage { get; set; }
}
