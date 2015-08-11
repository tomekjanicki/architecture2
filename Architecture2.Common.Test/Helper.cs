using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using FluentValidation.Results;

namespace Architecture2.Common.Test
{
    public static class Helper
    {
        public static bool HasError(ValidationResult validationResult, string propertyName)
        {
            return validationResult.Errors.Count > 0 && validationResult.Errors.FirstOrDefault(failure => failure.PropertyName == propertyName) != null;
        }

        public static void IgnoreAwaitForNSubstituteAssertion(this Task task)
        {

        }

        public static string GetResolvingErrors(IReadOnlyCollection<Type> types, Action<ContainerBuilder> registerDependenciesAction)
        {
            var errors = new List<string>();

            var builder = new ContainerBuilder();

            registerDependenciesAction(builder);

            var container = builder.Build();

            using (var lifetimeScope = container.BeginLifetimeScope())
            {
                foreach (var type in types)
                {
                    try
                    {
                        lifetimeScope.Resolve(type);
                    }
                    catch (DependencyResolutionException exception)
                    {
                        errors.Add(exception.Message);
                    }
                }
            }
            return string.Join(Environment.NewLine, errors);
        }

        public static IEnumerable<Type> GetTypes<T>(Assembly[] assemblies)
        {
            var types = new List<Type>();

            foreach (var assembly in assemblies)
                GetTypes<T>(assembly, types);
            return types;
        }

        private static void GetTypes<T>(Assembly assembly, ICollection<Type> types)
        {
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var type in assembly.GetTypes())
            {
                if (type.Namespace != null && !type.IsAbstract && type.IsSubclassOf(typeof (T)))
                    types.Add(type);
            }
        }

    }
}
