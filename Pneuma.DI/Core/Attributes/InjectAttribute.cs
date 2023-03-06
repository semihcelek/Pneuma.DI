using System;

namespace Pneuma.DI.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        public bool IsOptional;

        public InjectAttribute(bool isOptional = false)
        {
            IsOptional = isOptional;
        }
    }
}