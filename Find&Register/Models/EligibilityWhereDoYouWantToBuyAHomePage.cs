using System;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace Find_Register.Models
{
    public class EligibilityWhereDoYouWantToBuyAHomePage
    {

        [Required(ErrorMessage = "Select if you want to live in London or Somewhere else.")]
        public bool? LivingLocation { get; set; }
    }
}

