using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Architecture2.Common.Web
{
    public abstract class WebAppStartup : Startup
    {
        protected virtual void Configure(IAppBuilder app, WebOption webOption)
        {
            HttpConfiguration config = null;

            if (webOption.UseMvc)
            {
                ConfigureMvcRoutes(RouteTable.Routes);
                ConfigureMvcFilters(GlobalFilters.Filters);
                ConfigureMvcViewEngines(ViewEngines.Engines);
            }

            if (webOption.UseApi)
            {
                config = new HttpConfiguration();
                ConfigureApiRoutes(config);
                ConfigureApiFilters(config);
                ConfigureApiConventions(config);

                app.UseWebApi(config);
            }

            ConfigureValidation();
            ConfigureContainer(app, config, webOption);
        }

        protected virtual void ConfigureApiRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional });
        }

        protected virtual void ConfigureApiFilters(HttpConfiguration config)
        {
            config.Filters.Add(new ApiExceptionFilterAttribute());
            config.Services.Add(typeof(IExceptionLogger), new ApiExceptionLogger());
        }

        protected virtual void ConfigureApiConventions(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
        }

        protected virtual void ConfigureMvcRoutes(RouteCollection routes)
        {
            AreaRegistration.RegisterAllAreas();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", "{controller}/{action}/{id}", new
            {
                controller = "Home",
                action = "Index",
                id = RouteParameter.Optional
            });
        }

        protected virtual void ConfigureMvcViewEngines(ViewEngineCollection viewEngines)
        {

        }

        protected virtual void ConfigureContainer(IAppBuilder app, HttpConfiguration config, WebOption webOption)
        {

        }

        protected virtual void ConfigureMvcFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleMvcErrorAttribute());
        }

    }
}
