using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NCS.Prueba.TagHelpers;

[HtmlTargetElement("a", Attributes = "active-when")]
public class ActiveLinkTagHelper : TagHelper
{
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    [HtmlAttributeName("asp-controller")]
    public string Controller { get; set; }

    [HtmlAttributeName("asp-action")]
    public string Action { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var currentController = ViewContext.RouteData.Values["Controller"]?.ToString();
        var currentAction = ViewContext.RouteData.Values["Action"]?.ToString();

        var existingClasses = output.Attributes["class"]?.Value?.ToString() ?? "";
        var newClasses = string.Empty;
        if (string.Equals(currentController, Controller, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(currentAction, Action, StringComparison.OrdinalIgnoreCase))
        {
            newClasses = "active";
            output.Attributes.SetAttribute("aria-current", "page");
        }
        else
        {
            newClasses = "link-body-emphasis";
        }

        output.Attributes.SetAttribute("class", $"{existingClasses} {newClasses}");
    }
}