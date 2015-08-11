using System.Collections.Generic;
using System.Web.Http;
using Architecture2.Common;
using Architecture2.Common.Web;
using Autofac;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Architecture2.Web
{
    
    public class Startup : WebAppStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var option = new WebOption
            {
                UseMvc = true,
                UseApi = true
            };

            Configure(app, option);
        }

        protected override void ConfigureContainer(IAppBuilder app, HttpConfiguration config, WebOption webOption)
        {
            var modules = GetModules();

            app.ConfigureContainer(config, modules, webOption);
        }

        public IReadOnlyCollection<Module> GetModules()
        {
            return new Module[]
            {
                new Logic.DependencyModule(),
                new DependencyModule()
            };
        }

    }
}
