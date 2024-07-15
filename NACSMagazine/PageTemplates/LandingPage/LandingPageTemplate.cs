using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using NACSMagazine;
using Microsoft.AspNetCore.Mvc;
using NACSMagazine.PageTemplates.LandingPage;
using MediatR;
using Kentico.Content.Web.Mvc;

[assembly: RegisterPageTemplate(
    identifier: LandingPage.CONTENT_TYPE_NAME,
    name: "Landing Page",
    propertiesType: null,
    customViewName: "~/PageTemplates/LandingPage/_LandingPage.cshtml",
    ContentTypeNames = [LandingPage.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: LandingPage.CONTENT_TYPE_NAME,
    controllerType: typeof(LandingPageTemplateController))]

namespace NACSMagazine.PageTemplates.LandingPage
{
    public class LandingPageTemplateController : Controller
    {
        private readonly IMediator mediator;
        private readonly IWebPageDataContextRetriever contextRetriever;

        public LandingPageTemplateController(IMediator _mediator, IWebPageDataContextRetriever _contextRetriever)
        {
            mediator = _mediator;
            contextRetriever = _contextRetriever;
        }

        public async Task<IActionResult> Index()
        {
            if (!contextRetriever.TryRetrieve(out var data))
            {
                return NotFound();
            }

            var page = await mediator.Send(new LandingPageQuery(data.WebPage));



            return new TemplateResult(page);
        }
    }
}
