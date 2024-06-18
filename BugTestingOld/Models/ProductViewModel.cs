using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace BugTestingOld.Models;

public class ProductViewModel : ProductPlaceholder
{
    public ProductViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
    {
    }

    public string? Sku { get; set; }
}