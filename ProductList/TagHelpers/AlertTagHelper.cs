using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ProductList.TagHelpers
{
    public class AlertTagHelper : TagHelper
    {
        public string? Texto { get; set; }

        public string? Attribute { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!string.IsNullOrEmpty(Texto))
            {
                output.TagName = "div";
                output.Attributes.SetAttribute("class", "alert alert-dismissible alert-secondary");
                output.Content.SetContent(Texto);
            }
        }
    }
}
