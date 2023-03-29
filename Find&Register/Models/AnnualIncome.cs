using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace Find_Register.Models
{
    public class AnnualIncome
    {
        [Required(ErrorMessage = "Please select the option that best matches your household income.")]
        public bool? IsYourTotalIncomeAboveThreshold { get; set; }
    }
}