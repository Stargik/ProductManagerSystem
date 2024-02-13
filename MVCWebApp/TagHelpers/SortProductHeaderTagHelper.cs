using System;
using Azure;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MVCWebApp.TagHelpers
{
	public class SortProductHeaderTagHelper : TagHelper
	{
        public ProductSortState Property { get; set; } 
        public ProductSortState Current { get; set; }  
        public string? Action { get; set; }
        public string? SearchTitle { get; set; }
        public string? SearchCategoryId { get; set; }
        public string? SearchManufacturerId { get; set; }
        public bool Up { get; set; } 

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = null!;

        private readonly IUrlHelperFactory urlHelperFactory;
        public SortProductHeaderTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "a";
            string? url = urlHelper.Action(Action, new { sortOrder = Property,  searchTitle = SearchTitle, searchCategoryId = SearchCategoryId, searchManufacturerId = SearchManufacturerId });
            output.Attributes.SetAttribute("href", url);

            //if (Current == Property)
            {
                TagBuilder tag = new TagBuilder("i");
                tag.AddCssClass("bi");

                if (Up == true)
                {
                    tag.AddCssClass("bi-chevron-up");
                }
                else
                {
                    tag.AddCssClass("bi-chevron-down");
                }

                output.PostContent.AppendHtml(tag);


                if (Current != Property)
                {
                    tag.AddCssClass("chevron-hidden");
                }
            }

        }
    }
}

