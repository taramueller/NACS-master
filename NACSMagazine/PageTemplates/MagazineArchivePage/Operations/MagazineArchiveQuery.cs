using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc;

using NACS.Portal.Core.Operations;

namespace NACSMagazine.PageTemplates.MagazineArchivePage
{
    public record MagazineArchivePageQuery(RoutedWebPage Page) : WebPageRoutedQuery<ArchivePage>(Page);

    public class ArchivePageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<MagazineArchivePageQuery, ArchivePage>(tools)
    {
        public override async Task<ArchivePage> Handle(MagazineArchivePageQuery request, CancellationToken cancellationToken = default)
        {
            var b = new ContentItemQueryBuilder().ForWebPage(request.Page.WebsiteChannelName, request.Page);

            var r = await Executor.GetWebPageResult(b, WebPageMapper.Map<ArchivePage>, DefaultQueryOptions, cancellationToken);

            return r.First();
        }
    }
}
