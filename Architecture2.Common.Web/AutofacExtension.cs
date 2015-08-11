using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Owin;
using Module = Autofac.Module;

namespace Architecture2.Common.Web
{
    public static class AutofacExtension
    {
        public static IContainer ConfigureContainer(this IAppBuilder app, HttpConfiguration config, IReadOnlyCollection<Module> dependencyModules, WebOption webOption)
        {
            var assemblies = new[] { Assembly.GetCallingAssembly() };

            return app.ConfigureContainer(config, assemblies, dependencyModules, webOption);
        }

        public static void ConfigureContainer(ContainerBuilder builder, WebOption webOption, params Assembly[] applicationAssemblies)
        {
            var assemblies = applicationAssemblies.ToArray();

            if (webOption.UseMvc)
            {
                builder.RegisterControllers(assemblies);
                builder.RegisterFilterProvider();
                builder.RegisterSource(new ViewRegistrationSource());
            }

            if (webOption.UseApi)
                builder.RegisterApiControllers(assemblies);
        }

        public static IContainer ConfigureContainer(this IAppBuilder app, HttpConfiguration config, IReadOnlyCollection<Assembly> applicationAssemblies, IEnumerable<Module> dependencyModules, WebOption webOption)
        {
            var builder = new ContainerBuilder();

            foreach (var module in dependencyModules)
                builder.RegisterModule(module);

            var assemblies = applicationAssemblies.ToArray();

            ConfigureContainer(builder, webOption, assemblies);

            if (webOption.UseApi)
                builder.RegisterWebApiFilterProvider(config);

            var container = builder.Build();

            if (webOption.UseApi)
                config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            if (webOption.UseMvc)
                DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.UseAutofacMiddleware(container);

            if (webOption.UseApi)
                app.UseAutofacWebApi(config);

            return container;
        }

    }
}
