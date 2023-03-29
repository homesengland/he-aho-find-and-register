using Find_Register.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Find_Register.Filters;

public class CustomAsyncResultFilterAttribute : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {

        var errorList = new List<ErrorSummary>();
        var errorData = context.ModelState.Where(x => x.Value != null && x.Value.Errors.Any())
                            .Select((x => new { x.Key, x.Value!.Errors }))
                            .ToList();

        foreach (var error in errorData)
        {
            var errorSummary = new ErrorSummary();
            errorSummary.ErrorMessage = error.Errors[0].ErrorMessage;
            errorSummary.ErrorPropertyHTMLRef = error.Key; 
            
            errorList.Add(errorSummary);
        }
        context.HttpContext.Items["errors"] = errorList;

        if (errorList.Any())
        {
            context.HttpContext.Items["errorsBool"] = true;
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Items.Add("errors", "");
        context.HttpContext.Items.Add("errorsBool", false);
    }
}
