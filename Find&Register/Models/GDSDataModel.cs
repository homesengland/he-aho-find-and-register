using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace Find_Register.Models
{
    
	public class GDSDataModel
	{
		public GDSDataModel()
		{

		}
    }

    public class PageOne
	{
        public List<ErrorSummary> errorList = new List<ErrorSummary>();

        [Required(ErrorMessage = "Select the country where you live")]
        public string? WhereDoYouLive{ get; set; }

        public string GetHTMLIdFromProperty(string property)
        {
            return nameof(property);
        }
    }


}

