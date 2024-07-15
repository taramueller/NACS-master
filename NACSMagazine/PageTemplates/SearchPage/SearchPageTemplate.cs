using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using NACSMagazine;
using Microsoft.AspNetCore.Mvc;
using NACSMagazine.PageTemplates.SearchPage;

[assembly: RegisterPageTemplate(
    identifier: "NACSMagazine.Search",
    name: "Search Page",
    propertiesType: null,
    customViewName: "~/PageTemplates/SearchPage/_SearchPage.cshtml",
    ContentTypeNames = [Search.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: Search.CONTENT_TYPE_NAME,
    controllerType: typeof(SearchPageTemplateController))]

namespace NACSMagazine.PageTemplates.SearchPage
{
    public class SearchPageTemplateController: Controller
    {
    }
}
