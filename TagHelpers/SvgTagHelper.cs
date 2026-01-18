using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NCS.Prueba.TagHelpers;

[HtmlTargetElement("inline-svg")]
public class SvgTagHelper : TagHelper
{
    private readonly IWebHostEnvironment _env;

    public SvgTagHelper(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HtmlAttributeName("src")]
    public string Src { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrEmpty(Src)) return;

        var filePath = Path.Combine(_env.WebRootPath, "assets", "svg", Src);

        if (File.Exists(filePath))
        {
            var content = File.ReadAllText(filePath);
            output.TagName = null;
            output.Content.SetHtmlContent(content);
        }
    }
}
