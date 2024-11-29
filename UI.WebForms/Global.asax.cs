using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using UI.WebForms.ApiProxy;

namespace UI.WebForms
{
    public class Global : HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Global));
        public static ServiceProvider ServiceProvider { get; private set; }

        void Application_Start(object sender, EventArgs e)
        {
            XmlConfigurator.Configure();
            log.Info("Application Started");

            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var services = new ServiceCollection();
            // Register callers
            services.AddTransient<CategoryServiceCaller>();
            services.AddTransient<BookServiceCaller>();

            // Register HttpClients for service caller
            var webApiUri = new Uri(ConfigurationManager.AppSettings["BookCatalogServiceUrl"]);            
            services.AddHttpClient<BookServiceCaller>(client =>
            {
                client.BaseAddress = webApiUri;
            });
            services.AddHttpClient<CategoryServiceCaller>(client =>
            {
                client.BaseAddress = webApiUri;
            });

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}