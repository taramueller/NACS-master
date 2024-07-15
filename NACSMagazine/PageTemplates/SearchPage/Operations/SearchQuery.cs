using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc;

using NACS.Portal.Core.Operations;

namespace NACSMagazine.PageTemplates.SearchPage
{
    public record SearchQuery(RoutedWebPage Page) : WebPageRoutedQuery<NACSMagazine.Search>(Page);

    public class CategoryPageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<SearchQuery, NACSMagazine.Search>(tools)
    {
        public override async Task<NACSMagazine.Search> Handle(SearchQuery request, CancellationToken cancellationToken = default)
        {
            var b = new ContentItemQueryBuilder().ForWebPage(request.Page.WebsiteChannelName, request.Page);

            var r = await Executor.GetWebPageResult(b, WebPageMapper.Map<NACSMagazine.Search>, DefaultQueryOptions, cancellationToken);

            return r.First();
        }
    }
}
