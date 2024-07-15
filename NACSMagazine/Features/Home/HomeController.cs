using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc.Routing;
using NACSMagazine.Features.Home;
using NACSMagazine;
using static System.Net.Mime.MediaTypeNames;
using Tag = CMS.ContentEngine.Tag;
using System.Data;


[assembly: RegisterWebPageRoute(
    contentTypeName: Home.CONTENT_TYPE_NAME,
    controllerType: typeof(HomeController),
    ActionName = "Index",
    Path = "/Home",
    WebsiteChannelNames = ["NACSMagazine"])]

namespace NACSMagazine.Features.Home
{
    public class HomeController : Controller
    {
        // Service for executing content item queries
        private readonly IContentQueryExecutor executor;
        private readonly ITaxonomyRetriever taxonomyRetriever;

        public HomeController(IContentQueryExecutor executor, ITaxonomyRetriever taxonomyRetriever)
        {
            this.executor = executor;
            this.taxonomyRetriever = taxonomyRetriever;
        }

        public async Task<IActionResult> Index()
        {
            var homeHeroArticle = await GetHomePageHero();
            var heroFeaturedArticles = await GetHeroFeaturedArticles();
            var editorPicks = await GetEditorPicksArticles();
            var departmentArticles = await GetDepartmentArticles();
            //var NACSDailyArticles = await GetNACSDailyNewsArticles();
            
            var query = new ContentItemQueryBuilder()
                                // Scopes the query to pages of the MEDLAB.Home content type
                                .ForContentType(NACSMagazine.Home.CONTENT_TYPE_NAME,
                                config => config
                                    // Retrieves the page with the /Home tree path under the Refined Element website channel
                                    .ForWebsite("NACSMagazine", PathMatch.Single("/Home")));

            // Executes the query and stores the data in the generated 'Home' class
            NACSMagazine.Home page = (await executor.GetMappedWebPageResult<NACSMagazine.Home>(query)).FirstOrDefault()!;

            HomePageViewModel model = new HomePageViewModel(page) { 
                HomeHero = homeHeroArticle,
                HeroFeaturedArticles = heroFeaturedArticles, 
                EditorPicksArticles = editorPicks, 
                DepartmentArticles = departmentArticles,
                //NACSDailyArticles = NACSDailyArticles
                };
                

            // Passes the home page content to the view using HomePageViewModel
            return View("Features/Home/ContentTypes/Home.cshtml", model);
        }

        public async Task<Article> GetHomePageHero()
        {

            var contentQuery = new ContentItemQueryBuilder()
                                .ForContentType(
                                    "NACSMagazine.Article",
                                    config => config
                                    //.Columns("DocumentID, Title, IssueDate, Image, RollupImage, RollupImageURL, LedeText, IssueDate, MagazineSection, ContentCategory, CoverStory")
                                    .TopN(1)
                                    .WithLinkedItems(1)
                                    .OrderBy("IssueDate DESC")
                                    .Where(where => where.WhereEquals("CoverStory", 1))
                                    ).InLanguage("en");



            Article article = (await executor.GetMappedResult<Article>(contentQuery)).FirstOrDefault()!;

            IEnumerable<Guid> tagIdentifiers = article.ContentCategory.Select(item => item.Identifier);
			IEnumerable<Tag> tags = await taxonomyRetriever.RetrieveTags(tagIdentifiers, "en");

			foreach (var tag in tags)
			{
				article.CategoryTags = tag.Title;
			}

			return article;
        }

        public async Task<IEnumerable<Article>> GetHeroFeaturedArticles()
        {
            var contentQuery = new ContentItemQueryBuilder()
                                    .ForContentType(
                                    "NACSMagazine.Article",
                                    config => config
                                    .TopN(2)
                                    .WithLinkedItems(1)
                                    .OrderBy("IssueDate DESC") /*, NodeLevel, NodeOrder, NodeName")*/
                                    .Where(where => where.WhereEquals("CurrentIssue", 1))
                                    ).InLanguage("en");

            IEnumerable<Article> featuredArticles = await executor.GetMappedResult<Article>(contentQuery);

            foreach (var article in featuredArticles)
            {
				IEnumerable<Guid> tagIdentifiers = article.ContentCategory.Select(item => item.Identifier);
				IEnumerable<Tag> tags = await taxonomyRetriever.RetrieveTags(tagIdentifiers, "en");

                foreach (var tag in tags)
                {
                    article.CategoryTags = tag.Title;
                }
            }

            return featuredArticles;
        }

        public async Task<IEnumerable<Article>> GetEditorPicksArticles()
        {
            var contentQuery = new ContentItemQueryBuilder()
                                    .ForContentType(
                                    "NACSMagazine.Article",
                                    config => config
                                    .TopN(3)
                                    .WithLinkedItems(1)
                                    .OrderBy("IssueDate DESC")
                                    .Where(where => where.WhereEquals("EditorPick", 1))
                                    ).InLanguage("en");

            IEnumerable<Article> editorPicksArticles = await executor.GetMappedResult<Article> (contentQuery);

			foreach (var article in editorPicksArticles)
			{
				IEnumerable<Guid> tagIdentifiers = article.ContentCategory.Select(item => item.Identifier);
				IEnumerable<Tag> tags = await taxonomyRetriever.RetrieveTags(tagIdentifiers, "en");

				foreach (var tag in tags)
				{
					article.CategoryTags = tag.Title;
				}
			}

			return editorPicksArticles;
        }

        public async Task<IEnumerable<Article>> GetDepartmentArticles()
        {
            var articleList = new List<Article>();

            var sections = new List<string>();
            var sectionString = "From the Editor,The Big Question,NACS News,Convenience Cares,Inside Washington,Ideas 2 Go,Cool New Products,Gas Station Gourmet,Category Close-Up";

            foreach (string section in sectionString.Split(','))
            {
                sections.Add(section);
            }

            foreach (string section in sections)
            {
                await GetDepartmentArticle(section, articleList);
            }
            
            IEnumerable<Article> departmentArticles = articleList;

            foreach (Article article in departmentArticles)
            {
                IEnumerable<Guid> tagIdentifiers = article.ContentCategory.Select(item => item.Identifier);
                IEnumerable<Tag> tags = await taxonomyRetriever.RetrieveTags(tagIdentifiers, "en");

                foreach (Tag tag in tags)
                {
                    article.CategoryTags = tag.Title;
                }
            }

            return departmentArticles;
        }

        public async Task GetDepartmentArticle(string magSection, List<Article> articleList)
        {
			var contentQuery = new ContentItemQueryBuilder()
									.ForContentType(
									"NACSMagazine.Article",
									config => config
									.TopN(1)
									.WithLinkedItems(1)
									.OrderBy("IssueDate DESC")
									.Where(where => where.WhereEquals("MagazineSection", magSection))
									).InLanguage("en");

			var article = await executor.GetMappedResult<Article>(contentQuery);

            
			if (article != null && article.Count() > 0)
			{

                articleList.Add(article.FirstOrDefault()!);
			}
		}

        //This is getting articles from Convenience.org website and won't work until that site is built and the pagetype exists. For now I have it mapped to a NACSMagazine.Article page type, but this is wrong.
        //public async Task<IEnumerable<Article>> GetNACSDailyNewsArticles()
        //{
        //    var contentQuery = new ContentItemQueryBuilder()
        //                            .ForContentType(
        //                            "NACS.NewsArticle",
        //                            config => config
        //                            .TopN(6)
        //                            .WithLinkedItems(1)
        //                            .OrderBy("Date DESC, SortOrder ASC")
        //                            .ForWebsite("NACS", PathMatch.Single("/Media/Daily/%"))
        //                            ).InLanguage("en");

        //    var articles = await executor.GetMappedResult<Article>(contentQuery);

        //    return articles;
        //}

		//public async Task<IEnumerable<Article>> GetArticle(int topN, string orderBy, string whereEqualsColumn, string whereEqualsValue)
		//{
		//	var contentQuery = new ContentItemQueryBuilder()
		//							.ForContentType(
		//							"NACSMagazine.Article",
		//							config => config
		//							.TopN(1)
		//							.WithLinkedItems(topN)
		//							.OrderBy(orderBy)
		//							.Where(where => where.WhereEquals(whereEqualsColumn, whereEqualsValue))
		//							).InLanguage("en");

		//	var article = await executor.GetMappedResult<Article>(contentQuery);


  //          return article;
		//}
	}
}