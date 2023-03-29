namespace Find_Register.Models;

public class LayoutDataModel
{
    public LayoutDataModel(Journey journey)
    {
        switch (journey)
        {
            case Journey.Search:
                HeaderText = "Find an organisation that sells shared ownership homes";
                HeaderLink = "/find-organisations-selling-shared-ownership-homes";
                FeedbackLink = "https://www.smartsurvey.co.uk/s/FindOrganisation/";
                FooterLinkRoot = HeaderLink;
                break;
            case Journey.Eligibility:
                HeaderText = "Check if you are eligible to buy a shared ownership home";
                HeaderLink = "/check-eligibility-to-buy-a-shared-ownership-home";
                FeedbackLink = "https://www.smartsurvey.co.uk/s/Eligible/";
                FooterLinkRoot = HeaderLink;
                break;
            default:
                HeaderText = "Shared ownership scheme";
                HeaderLink = "https://www.gov.uk/shared-ownership-scheme";
                FeedbackLink = "https://www.smartsurvey.co.uk/s/Eligible/";
                FooterLinkRoot = "/check-eligibility-to-buy-a-shared-ownership-home";
                break;
        }
    }

    public string HeaderText { get; }
    public string HeaderLink { get; }
    public string FeedbackLink { get; }
    public string FooterLinkRoot { get; }
}

public enum Journey
{
    Eligibility,
    Search,
    Other
}