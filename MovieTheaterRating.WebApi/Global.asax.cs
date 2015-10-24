using MovieTheaterRating.WebApi.CustomMediaFormatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MovieTheaterRating.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ////Remove xml first so that the default will be json
            var xmlType = GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(s => s.MediaType == "application/xml");
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Remove(xmlType);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(
                    new QueryStringMapping("type", "json", new MediaTypeHeaderValue("application/json")));

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.MediaTypeMappings.Add(
                    new QueryStringMapping("type", "xml", new MediaTypeHeaderValue("application/xml")));

            GlobalConfiguration.Configuration.Formatters.Add(new ImageFormatter()); //custom image formatter

        }
    }
}
