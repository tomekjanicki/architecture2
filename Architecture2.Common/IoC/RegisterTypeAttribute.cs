using System;

namespace Architecture2.Common.IoC
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterTypeAttribute : Attribute
    {
        public RegisterTypeScope Scope { get; set; }
    }
}
