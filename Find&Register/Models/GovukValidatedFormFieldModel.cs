using Microsoft.AspNetCore.Html;

namespace Find_Register.Models;

public class GovukValidatedFormFieldModel
{
    public bool HasErrors => Errors != null && Errors.Any();

    public string? HtmlId { get; set; }
    public IHtmlContent? ValidationMessage { get; set; }
    public string? AriaDescribe { get; set; }
    public IList<ErrorSummary>? Errors { get; set; }
    public string? Title { get; set; }
    public Func<object, IHtmlContent>? Content { get; set; }
}