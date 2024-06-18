using BugTestingOld.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace BugTestingOld.Controllers;

public class VirtualProductController : UmbracoPageController, IVirtualPageController
{
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly IPublishedValueFallback _publishedValueFallback;

    public VirtualProductController(
        ILogger<UmbracoPageController> logger,
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        IPublishedValueFallback publishedValueFallback) : base(logger,
        compositeViewEngine)
    {
        _umbracoContextAccessor = umbracoContextAccessor;
        _publishedValueFallback = publishedValueFallback;
    }

    public IPublishedContent? FindContent(ActionExecutingContext actionExecutingContext)
    {
        if (!_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext))
        {
            return null;
        }

        var rootContent = umbracoContext.Content?.GetAtRoot();

        var productPlaceholderPage = rootContent?.FirstOrDefault()?.Children(x => x.ContentType.Alias == ProductPlaceholder.ModelTypeAlias)?.FirstOrDefault();

        if (productPlaceholderPage is null)
        {
            return null;
        }

        return new ProductViewModel(productPlaceholderPage, _publishedValueFallback);
    }

    [Route("p/{sku}")]
    [Route("{lang}/p/{sku}")]
    public async Task<IActionResult> Index([FromRoute] string sku)
    {
        if (CurrentPage is not ProductViewModel productViewModel)
        {
            return NotFound();
        }

        productViewModel.Sku = sku;

        return View("~/Views/ProductPlaceholder.cshtml", productViewModel);
    }
}