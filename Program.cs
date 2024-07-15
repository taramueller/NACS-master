using Kentico.Web.Mvc;
using Kentico.Xperience.Cloud;
using Microsoft.Extensions.Hosting;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.OnlineMarketing.Web.Mvc;
using Kentico.Activities.Web.Mvc;
using NACS;
using CMS.Core;
using Microsoft.AspNetCore.Mvc.Razor;
using NACS.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.FileProviders;
using Org.BouncyCastle.Bcpg.Sig;
using NACSMagazine;
using Kentico.Membership;
using Microsoft.AspNetCore.Identity;
using NACSMagazine.PageTemplates.CategoryPage;
using MediatR;
using NACS.Portal.Core.Operations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddXperienceCloudApplicationInsights(builder.Configuration);

if (builder.Environment.IsQa() || builder.Environment.IsUat() || builder.Environment.IsProduction())
{
    builder.Services.AddKenticoCloud(builder.Configuration);
    builder.Services.AddXperienceCloudSendGrid(builder.Configuration);
}

// Enable desired Kentico Xperience features
builder.Services.AddKentico(features =>
{
     features.UsePageBuilder(new PageBuilderOptions
         {
         ContentTypeNames = new[]
                {
                 //Enables Page Builder for content types using their generated classes
                Home.CONTENT_TYPE_NAME,
                Issue.CONTENT_TYPE_NAME,
                Article.CONTENT_TYPE_NAME,
                LandingPage.CONTENT_TYPE_NAME,
                Search.CONTENT_TYPE_NAME,
                CategoryPage.CONTENT_TYPE_NAME,
                //NACSShow.Home.CONTENT_TYPE_NAME,
                //AttendeeDirectoryPage.CONTENT_TYPE_NAME
                }
     });
    
     features.UseActivityTracking();
     features.UseWebPageRouting();
     features.UseEmailStatisticsLogging();
     features.UseEmailMarketing();
});

builder.Services.AddIdentity<ApplicationUser, NoOpApplicationRole>(options =>
{
    // Ensures that disabled member accounts cannot sign in
    options.SignIn.RequireConfirmedAccount = true;
    // Ensures unique emails for registered accounts
    options.User.RequireUniqueEmail = true;
})
    .AddUserStore<ApplicationUserStore<ApplicationUser>>()
    .AddRoleStore<NoOpApplicationRoleStore>()
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddSignInManager<SignInManager<ApplicationUser>>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(CategoryPageQuery).Assembly))
    //.AddClosedGenericTypes(typeof(CategoryPageQuery).Assembly, typeof(ICommandHandler<,>), ServiceLifetime.Scoped)
            //.Decorate(typeof(IRequestHandler<,>), typeof(QueryHandlerCacheDecorator<,>))
            //.Decorate(typeof(ICommandHandler<,>), typeof(CommandHandlerLogDecorator<,>))
            //.AddScoped<CacheDependenciesStore>()
            //.AddScoped<ICacheDependenciesStore>(s => s.GetRequiredService<CacheDependenciesStore>())
            //.AddScoped<ICacheDependenciesScope>(s => s.GetRequiredService<CacheDependenciesStore>())
            .AddTransient<WebPageCommandTools>()
            .AddTransient<WebPageQueryTools>()
            .AddTransient<ContentItemQueryTools>()
            .AddTransient<DataItemCommandTools>()
            .AddTransient<DataItemQueryTools>();

//builder.WebHost.UseWebRoot("wwwroot");
//builder.WebHost.UseStaticWebAssets();

var assembly = typeof(NACSMagazine.Features.Home.HomeController).Assembly;
builder.Services.AddControllersWithViews()
    .AddApplicationPart(assembly)
    .AddRazorRuntimeCompilation();

builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
{ options.FileProviders.Add(new EmbeddedFileProvider(assembly)); });

builder.Services.AddRazorPages();//.AddRazorRuntimeCompilation();
//builder.Services.AddControllersWithViews();

builder.Services.AddKenticoTagManager(builder.Configuration);

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationExpanders.Add(new FeatureLocationExpander());
});

var app = builder.Build();
app.InitKentico();

app.UseStaticFiles();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseKentico();
app.UseAuthorization();

app.MapRazorPages();

if (builder.Environment.IsQa() || builder.Environment.IsUat() || builder.Environment.IsProduction())
{
    app.UseKenticoCloud();
}

app.Kentico().MapRoutes();
//app.MapGet("/", () => "The NACS site has not been configured yet.");

app.Run();
