
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Web;

namespace Mikkelhm.Web.Bootstrap
{
    public class StartupHandler:IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            RouteTable.Routes.MapRoute("LegacyImages", "image.axd",
                new { controller = "LegacyFiles", action = "ShowImage", id = UrlParameter.Optional });

            RouteTable.Routes.MapRoute("LegacyFiles", "FILES/{year}/{month}/{filename}.axdx",
                new { controller = "LegacyFiles", action = "DownloadFile", id = UrlParameter.Optional });
        }
    }
}
