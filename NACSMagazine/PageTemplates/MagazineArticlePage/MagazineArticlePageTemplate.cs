using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using NACSMagazine;
using Microsoft.AspNetCore.Mvc;
using NACSMagazine.PageTemplates.MagazineArticlePage;
using Kentico.Content.Web.Mvc;
using MediatR;
using CMS.ContentEngine;
using CMS.Websites;

[assembly: RegisterPageTemplate(
    identifier: ArticlePage.CONTENT_TYPE_NAME,
    name: "Magazine Article Page",
    propertiesType: null,
    customViewName: "~/PageTemplates/MagazineArticlePage/_MagazineArticlePage.cshtml",
    ContentTypeNames = [ArticlePage.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: ArticlePage.CONTENT_TYPE_NAME,
    controllerType: typeof(MagazineArticlePageTemplateController))]

namespace NACSMagazine.PageTemplates.MagazineArticlePage
{
    public class MagazineArticlePageTemplateController : Controller
    {
        private readonly IMediator mediator;
        private readonly IWebPageDataContextRetriever contextRetriever;
        private readonly IContentQueryExecutor executor;

        public MagazineArticlePageTemplateController(IMediator _mediator, IWebPageDataContextRetriever _contextRetriever, IContentQueryExecutor _executor)
        {
            mediator = _mediator;
            contextRetriever = _contextRetriever;
            executor = _executor;
        }

        public async Task<IActionResult> Index()
        {
            if (!contextRetriever.TryRetrieve(out var data))
            {
                return NotFound();
            }
            
            var page = await mediator.Send(new MagazineArticleQuery(data.WebPage));
            
            
            var articles = await GetCurrentArticleAsync(true);

            page.ArticleContent = articles.First();
            

            return new TemplateResult(page);
        }

        public async Task<IEnumerable<Article>> GetCurrentArticleAsync(bool includeSecuredItems)
        {
            var query = new ContentItemQueryBuilder()
                            .ForContentType(
                                Article.CONTENT_TYPE_NAME,
                                config => config
                                .TopN(1)
                                .WithLinkedItems(2)
                                //.Where(where => where.WhereEquals("ArticleID", page.ArticleContent)
                                ).InLanguage("en");

            var queryOptions = new ContentQueryExecutionOptions()
            {
                IncludeSecuredItems = includeSecuredItems
            };

            IEnumerable<Article> articles = await executor.GetMappedResult<Article>(query, queryOptions);

            
            return articles;                
        }

        
            
    }
}
