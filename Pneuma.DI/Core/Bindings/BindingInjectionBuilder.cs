using System;

namespace Pneuma.DI.Core.Bindings;

public class BindingInjectionBuilder<T> : BindingBuilderBase where T : BindingInjectionBuilder<T>
{
    public BindingInjectionBuilder(IContainer container, Type buildingType) : base(container, buildingType)
    {
    }

    public T To<T1>()
    {
        return this as T;
    }  
}