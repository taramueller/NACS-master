using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using NACS.Portal.Core.Rendering;

using NACSMagazine;
using NACSMagazine.PageTemplates.CategoryPage;

using Tag = CMS.ContentEngine.Tag;

[assembly: RegisterPageTemplate(
    identifier: CategoryPage.CONTENT_TYPE_NAME,
    name: "Category Page",
    propertiesType: null,
    customViewName: "~/PageTemplates/CategoryPage/_CategoryPage.cshtml",
    ContentTypeNames = [CategoryPage.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: CategoryPage.CONTENT_TYPE_NAME,
    controllerType: typeof(CategoryPageTemplateController))]


namespace NACSMagazine.PageTemplates.CategoryPage
{
    public class CategoryPageTemplateController : Controller
    {
        private readonly IMediator mediator;
        private readonly IContentQueryExecutor executor;
        private readonly ITaxonomyRetriever taxonomyRetriever;
        private readonly IWebPageDataContextRetriever contextRetriever;

        public CategoryPageTemplateController(IMediator _mediator, IContentQueryExecutor _executor, ITaxonomyRetriever _taxonomyRetriever, IWebPageDataContextRetriever _contextRetriever)
        {
            mediator = _mediator;
            executor = _executor;
            taxonomyRetriever = _taxonomyRetriever;
            contextRetriever = _contextRetriever;
        }

        public async Task<ActionResult> Index()
        {
            if(!contextRetriever.TryRetrieve(out var data))
            {
                return NotFound();
            }
            
            var page = await mediator.Send(new CategoryPageQuery(data.WebPage));
            
            var success = int.TryParse(Request.Query["page"], out int value);

            PagedList<Article> articles;
            if (success)
            {
                articles = await GetCategoryArticlesAsync(page, string.Empty, value, 9);
            }
            else
            {
                articles = await GetCategoryArticlesAsync(page, string.Empty, 1, 9);
            }

            page.ArticleList = await SetTaggedArticlesListAsync(page, articles);

            page.Years = Years;
            if(success)
            {
                page.PageNumber = value;
            }
            else
            {
                page.PageNumber = 1;
            }
            page.PageSize = 9;
            page.TotalPages = page.ArticleList.Count / 9;

            return new TemplateResult(page);
        }

        public async Task<PagedList<Article>> GetCategoryArticlesAsync(NACSMagazine.CategoryPage page, string year, int pageNumber, int pageSize)
        {
            var query = new ContentItemQueryBuilder()
                            .ForContentType(
                            Article.CONTENT_TYPE_NAME,
                            config => config
                            //.ForWebsite("NACSMagazine")
                            .WithLinkedItems(1)
                            .OrderBy("ContentCategory, IssueDate DESC")
                            //.Columns("ContentCategory")
                            .Where(where => where.WhereContains("IssueDate", year + "-"))
                            ).InLanguage("en");

            IEnumerable<Article> articleList = await executor.GetMappedResult<Article>(query);

            return PagedList<Article>.ToPagedList(articleList, pageNumber, pageSize);
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(NACSMagazine.CategoryPage page)
        {
            if (!contextRetriever.TryRetrieve(out var data))
            {
                return NotFound();
            }

            var success = int.TryParse(Request.Query["page"], out int value);
            if (success)
            {
                page.PageNumber = value;
            }
            else
            {
                page.PageNumber = 1;
            }
            page.PageSize = 9;

            var articles = await GetCategoryArticlesAsync(page, page.SelectedYear, page.PageNumber, page.PageSize);

            page = await mediator.Send(new CategoryPageQuery(data.WebPage));

            page.Years = Years;
            page.ArticleList = await SetTaggedArticlesListAsync(page, articles);
            page.TotalPages = articles.TotalPages;

            return new TemplateResult(page);
        }

        public async Task<PagedList<Article>> SetTaggedArticlesListAsync(NACSMagazine.CategoryPage page, List<Article> articles)
        {
            page.ArticleList = new PagedList<Article>();
            foreach (var article in articles)
            {
                IEnumerable<Guid> tagIdentifiers = article.ContentCategory.Select(item => item.Identifier);
                IEnumerable<Tag> tags = await taxonomyRetriever.RetrieveTags(tagIdentifiers, "en");

                foreach (Tag tag in tags)
                {
                    article.CategoryTags = tag.Title;
                }

                if (article.CategoryTags.ToLower().Replace('-', ' ').Equals(page.Title.ToLower()))
                {
                    page.ArticleList.Add(article);
                }
            }
            return page.ArticleList;
        }

        public static IEnumerable<SelectListItem> Years
        {
            get
            {
                yield return new SelectListItem(text: "2019", value: "2019");
                yield return new SelectListItem(text: "2020", value: "2020");
                yield return new SelectListItem(text: "2021", value: "2021");
                yield return new SelectListItem(text: "2022", value: "2022");
                yield return new SelectListItem(text: "2023", value: "2023");
                yield return new SelectListItem(text: "2024", value: "2024");
            }
        }
    }
}
