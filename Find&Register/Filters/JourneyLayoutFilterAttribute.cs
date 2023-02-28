using Find_Register.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Find_Register.Filters;

public class JourneyLayoutFilterAttribute : ActionFilterAttribute
{
    private Journey _journey;
    public JourneyLayoutFilterAttribute(Journey journeySource) {
        _journey = journeySource;
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        context.HttpContext.Items.Add("LayoutModel", new LayoutDataModel(_journey));
    }
}
