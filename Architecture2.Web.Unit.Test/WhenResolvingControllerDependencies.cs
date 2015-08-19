using System.Linq;
using System.Reflection;
using System.Web.Http;
using Architecture2.Common.Test;
using Architecture2.Common.Web;
using Autofac;
using NUnit.Framework;

namespace Architecture2.Web.Unit.Test
{
    public class WhenResolvingControllerDependencies : BaseTest
    {
        [Test]
        public void ShouldReturnNoError_IfDependenciesAreSet()
        {
            var webAssembly = typeof(Startup).Assembly;

            var mvcControllerTypes = Helper.GetTypes<System.Web.Mvc.Controller>(new[] { webAssembly });

            var apiControllerTypes = Helper.GetTypes<ApiController>(new[] { webAssembly });

            var types = apiControllerTypes.Union(mvcControllerTypes).ToList();

            var result = Helper.GetResolvingErrors(types, builder => ConfigureContainer(builder, webAssembly));

            Assert.IsTrue(string.IsNullOrEmpty(result), result);

        }

        private static void ConfigureContainer(ContainerBuilder containerBuilder, Assembly assembly)
        {
            var modules = new Startup().GetModules();

            foreach (var module in modules)
                containerBuilder.RegisterModule(module);

            AutofacExtension.ConfigureContainer(containerBuilder, new WebOption { UseApi = true, UseMvc = true }, assembly);
        }

    }
}
