﻿using Architecture2.Common.IoC;
using Autofac;

namespace Architecture2.Web
{
    public class DependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterTypes(ThisAssembly);
        }
    }
}
