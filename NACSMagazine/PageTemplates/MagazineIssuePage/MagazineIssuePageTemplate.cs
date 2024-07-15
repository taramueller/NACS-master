using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using NACSMagazine;
using Microsoft.AspNetCore.Mvc;
using NACSMagazine.PageTemplates.MagazineIssuePage;
using Kentico.Content.Web.Mvc;
using MediatR;
using NACSMagazine.PageTemplates.MagazineArchivePage;
using NACS.Portal.Core.Rendering;
using CMS.ContentEngine;

[assembly: RegisterPageTemplate(
    identifier: "NACSMagazine.IssuePage",
    name: "Magazine Issue Page",
    propertiesType: null,
    customViewName: "~/PageTemplates/MagazineIssuePage/_MagazineIssuePage.cshtml",
    ContentTypeNames = [IssuePage.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: IssuePage.CONTENT_TYPE_NAME,
    controllerType: typeof(MagazineIssuePageTemplateController))]

namespace NACSMagazine.PageTemplates.MagazineIssuePage
{
    public class MagazineIssuePageTemplateController : Controller
    {
        private readonly IMediator mediator;
        private readonly IWebPageDataContextRetriever contextRetriever;
        private readonly IContentQueryExecutor executor;

        public MagazineIssuePageTemplateController(IMediator _mediator, IWebPageDataContextRetriever _contextRetriever, IContentQueryExecutor _executor)
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
            
            var page = await mediator.Send(new MagazineIssueQuery(data.WebPage));

            var issue = await GetIssuesAsync(page);
            page.Issue = issue;

            var currentFeaturedArticleList = await GetCurrentFeaturedArticlesAsync(page);
            page.CurrentFeaturedArticleList = currentFeaturedArticleList;

            var articleList = await GetArticlesAsync(page);
            page.ArticleList = articleList.Where(where => where.MagazineSection != "Feature");

            var otherIssuesList = await GetOtherIssuesAsync(page);
            page.OtherIssuesList = otherIssuesList;

            return new TemplateResult(page);
        }

        public async Task<Issue> GetIssuesAsync(IssuePage page)
        {
            var query = new ContentItemQueryBuilder()
                            .ForContentType(
                                Issue.CONTENT_TYPE_NAME,
                                config => config
                                .TopN(1)
                                .WithLinkedItems(1)
                                .OrderBy("IssueDate DESC")
                                .Where(where => where.WhereEquals("Title", page.Title))
                                ).InLanguage("en");

            Issue issue = (await executor.GetMappedResult<Issue>(query)).FirstOrDefault()!;

            return issue;
        }

        public async Task<IEnumerable<Issue>> GetOtherIssuesAsync(IssuePage page)
        {
            var query = new ContentItemQueryBuilder()
                            .ForContentType(
                                Issue.CONTENT_TYPE_NAME,
                                config => config
                                .TopN(10)
                                .WithLinkedItems(1)
                                .OrderBy("IssueDate DESC")
                                .Where(where => where.WhereNotEquals("Title", page.Title))
                                ).InLanguage("en");

            IEnumerable<Issue> otherIssuesList = await executor.GetMappedResult<Issue>(query);

            return otherIssuesList;
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync(IssuePage page)
        {
            var query = new ContentItemQueryBuilder()
                        .ForContentType(
                                Article.CONTENT_TYPE_NAME,
                                config => config
                                .WithLinkedItems(1)
                                .Where(where => where.WhereEquals("IssueDate", page.Issue.IssueDate))
                                ).InLanguage("en");

            IEnumerable<Article> articleList = await executor.GetMappedResult<Article>(query);
            
            return articleList;
        }

        public async Task<IEnumerable<Article>> GetCurrentFeaturedArticlesAsync(IssuePage page)
        {
            var query = new ContentItemQueryBuilder()
                        .ForContentType(
                                Article.CONTENT_TYPE_NAME,
                                config => config
                                .WithLinkedItems(1)
                                .Where(where => where.WhereEquals("IssueDate", page.Issue.IssueDate)
                                .And()
                                .Where(where => where.WhereEquals("MagazineSection", "Feature")))
                                ).InLanguage("en");

            IEnumerable<Article> currentFeaturedArticleList = await executor.GetMappedResult<Article>(query);

            return currentFeaturedArticleList;
        }
    }
}
