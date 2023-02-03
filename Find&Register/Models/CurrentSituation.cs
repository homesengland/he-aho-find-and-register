using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace Find_Register.Models
{
    public class CurrentSituation
    {
        [Required(ErrorMessage = "Please select if any of the below is true for your current circumstances.")]
        public bool? AnyTrueForCurrentCircumstances { get; set; }
    }
}