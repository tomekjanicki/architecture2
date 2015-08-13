using System;
using Architecture2.Common.IoC;
using Autofac;

namespace Architecture2.Common
{
    public class DependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterTypes(ThisAssembly);

            builder.Register<Func<Type, object>>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

        }

    }
}
