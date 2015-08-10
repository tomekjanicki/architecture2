using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;

namespace Architecture2.Common.IoC
{
    public static class Extension
    {
        public static void RegisterTypes(this ContainerBuilder builder, params Assembly[] assemblies)
        {

            var types = GetTypesWithScope(assemblies);

            if (types.ContainsKey(RegisterTypeScope.InstancePerLifetimeScope))
                builder.RegisterTypes(types[RegisterTypeScope.InstancePerLifetimeScope].ToArray()).AsImplementedInterfaces().InstancePerLifetimeScope();

            if (types.ContainsKey(RegisterTypeScope.Singleton))
                builder.RegisterTypes(types[RegisterTypeScope.Singleton].ToArray()).AsImplementedInterfaces().SingleInstance();
        }


        private static IDictionary<RegisterTypeScope, IEnumerable<Type>> GetTypesWithScope(params Assembly[] assemblies)
        {
            var allTypes = assemblies.SelectMany(x => x.GetTypes());

            var servicesTypes = GetTypesWithAttribute<RegisterTypeAttribute>(allTypes);

            return servicesTypes.GroupBy(x => x.Value.Scope).ToDictionary(x => x.Key, x => x.Select(t => t.Key));
        }

        private static IDictionary<Type, TAttribute> GetTypesWithAttribute<TAttribute>(IEnumerable<Type> types) where TAttribute : Attribute
        {
            var res = new Dictionary<Type, TAttribute>();
            foreach (var type in types)
            {
                var attr = type.GetCustomAttributes<TAttribute>().FirstOrDefault();
                if (attr != null)
                    res.Add(type, attr);
            }
            return res;
        }

    }
}
