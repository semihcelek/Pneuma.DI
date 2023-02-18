using System;

namespace Pneuma.DI.Core.BindingBuilder;

public class BindingLifeTimeBuilder<T> : BindingBuilderBase where T: BindingLifeTimeBuilder<T>
{
    public BindingLifeTimeBuilder(IContainer container, Type buildingType) : base(container, buildingType)
    {
    }

    public T AsSingle()
    {
        return this as T;
    }

    public T AsTransient()
    {
        return this as T;
    }
}