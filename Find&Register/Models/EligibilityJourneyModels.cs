using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Xunit.Sdk;

namespace Find_Register.Models
{
    public class EligibilityJourneyModels
    {
    }

    public class EligibilityJourneyWhereDoYouWantToBuyAHome
    {
        [Required(ErrorMessage = "Choose where you want to buy a home")]
        public bool? LiveInLondon { get; set; }
    }

    public class EligibilityJourneyBuyingWithAnotherPerson
    {
        [Required(ErrorMessage = "Choose if you are buying with another person")]
        public bool? SingleBuyer { get; set; }
    }

    public class EligibilityJourneyHowMuchDoYouEarn
    {
        [Required(ErrorMessage = "Choose how much you earn")]
        public bool? SingleIncomeOver80 { get; set; }
    }

    public class EligibilityJourneyHowMuchDoYouEarn_MultiplePeople
    {
        [Required(ErrorMessage = "Choose how much you both earn")]
        public bool? JointIncomeOver80 { get; set; }
    }

    public class EligibilityJourneyFirstTimeBuyer
    {
        [Required(ErrorMessage = "Select at least one option")]
        public bool? firstTimeBuyer { get; set; }
        
        public bool? ownAHomeButNeedToMove { get; set; }
        public bool? cannotAffordAHome { get; set; }
        public bool? theseDoNotApply { get; set; }

        [Required(ErrorMessage = "Select either one of the first 3 options, or ‘These do not apply to me’")]
        public bool? Conflicting4Choices { get; set; }

        [Required(ErrorMessage = "Select either ‘I do not own a home’, or ‘I own a home but need to move’")]
        public bool? conflictingChoicesChosen { get; set; }
    }
}


