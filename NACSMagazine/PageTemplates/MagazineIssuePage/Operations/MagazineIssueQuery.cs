using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc;

using NACS.Portal.Core.Operations;

namespace NACSMagazine.PageTemplates.MagazineIssuePage
{
    public record MagazineIssueQuery(RoutedWebPage Page) : WebPageRoutedQuery<IssuePage>(Page);

    public class IssuePageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<MagazineIssueQuery, IssuePage>(tools)
    {
        public override async Task<IssuePage> Handle(MagazineIssueQuery request, CancellationToken cancellationToken = default)
        {
            var b = new ContentItemQueryBuilder().ForWebPage(request.Page.WebsiteChannelName, request.Page);

            var r = await Executor.GetWebPageResult(b, WebPageMapper.Map<IssuePage>, DefaultQueryOptions, cancellationToken);

            return r.First();
        }
    }
}



