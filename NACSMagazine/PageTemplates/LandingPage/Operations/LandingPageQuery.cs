using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc;

using NACS.Portal.Core.Operations;

namespace NACSMagazine.PageTemplates.LandingPage
{
    public record LandingPageQuery(RoutedWebPage Page) : WebPageRoutedQuery<NACSMagazine.LandingPage>(Page);

    public class CategoryPageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<LandingPageQuery, NACSMagazine.LandingPage>(tools)
    {
        public override async Task<NACSMagazine.LandingPage> Handle(LandingPageQuery request, CancellationToken cancellationToken = default)
        {
            var b = new ContentItemQueryBuilder().ForWebPage(request.Page.WebsiteChannelName, request.Page);

            var r = await Executor.GetWebPageResult(b, WebPageMapper.Map<NACSMagazine.LandingPage>, DefaultQueryOptions, cancellationToken);

            return r.First();
        }
    }
}
