using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc;

using NACS.Portal.Core.Operations;

namespace NACSMagazine.PageTemplates.CategoryPage
{
    public record CategoryPageQuery(RoutedWebPage Page) : WebPageRoutedQuery<NACSMagazine.CategoryPage>(Page);

    public class CategoryPageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<CategoryPageQuery, NACSMagazine.CategoryPage>(tools)
    {
        public override async Task<NACSMagazine.CategoryPage> Handle(CategoryPageQuery request, CancellationToken cancellationToken = default)
        {
            var b = new ContentItemQueryBuilder().ForWebPage(request.Page.WebsiteChannelName, request.Page);

            var r = await Executor.GetWebPageResult(b, WebPageMapper.Map<NACSMagazine.CategoryPage>, DefaultQueryOptions, cancellationToken);

            return r.First();
        }
    }
}
