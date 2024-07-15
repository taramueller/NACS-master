namespace NACSMagazine.Features.Home
{
    public class HomePageViewModel
    {
        public string Title { get; init; }
        public Article HomeHero { get; init; }
        public IEnumerable<Article> HeroFeaturedArticles { get; init; }
        public IEnumerable<Article> EditorPicksArticles { get; init; }
        public IEnumerable<Article> DepartmentArticles { get; init; }
		//This should be set to Convenience.NewsArticle page type which only exists on Convenience.org site (but that site hasn't been built as of writing this). We will need to change this for the functionality to work as expected.
		public IEnumerable<Article> NACSDailyArticles { get; init; }
        //public string HomeHeader { get; init; }
        //public string HomeTextHeading { get; init; }
        //public string HomeText { get; init; }

        public HomePageViewModel(NACSMagazine.Home home)
        {
            Title = home.Title;
            //HomeHero = (Article)home.HomeHero;
            //HeroFeaturedArticles = home.HeroFeaturedArticles;
            //HomeTextHeading = home.HomeTextHeading;
            //HomeText = home.HomeText;
        }
    }
}
