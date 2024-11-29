using Microsoft.AspNet.FriendlyUrls;
using System.Web.Routing;

namespace UI.WebForms
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);

            routes.MapPageRoute(
                "Root",
                "",
                "~/Pages/Home.aspx"
            );

            // Catch-all route for all pages (without 'Pages' in the URL)
            // This should match any route and map to the ~/Pages/{page}.aspx
            routes.MapPageRoute(
                "DefaultRoute",            // Route name
                "{page}",                  // URL pattern without "Pages"
                "~/Pages/{page}.aspx"      // Physical path to the page
            );
        }
    }
}
