using Architecture2.Common.IoC;
using Autofac;

namespace Architecture2.Logic
{
    public class DependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterTypes(ThisAssembly);
        }

    }
}
