﻿using System;
using System.ComponentModel.DataAnnotations;
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
}
