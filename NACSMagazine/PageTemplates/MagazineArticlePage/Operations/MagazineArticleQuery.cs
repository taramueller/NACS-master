using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc;

using NACS.Portal.Core.Operations;

namespace NACSMagazine.PageTemplates.MagazineArticlePage
{
    public record MagazineArticleQuery(RoutedWebPage Page) : WebPageRoutedQuery<ArticlePage>(Page);

    public class CategoryPageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<MagazineArticleQuery, ArticlePage>(tools)
    {
        public override async Task<ArticlePage> Handle(MagazineArticleQuery request, CancellationToken cancellationToken = default)
        {
            var b = new ContentItemQueryBuilder().ForWebPage(request.Page.WebsiteChannelName, request.Page);

            var r = await Executor.GetWebPageResult(b, WebPageMapper.Map<ArticlePage>, DefaultQueryOptions, cancellationToken);

            return r.First();
        }
    }
}
