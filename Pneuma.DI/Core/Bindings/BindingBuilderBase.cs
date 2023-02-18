using System;

namespace Pneuma.DI.Core.Bindings;

public abstract class BindingBuilderBase
{
    protected IContainer Container;
    
    protected Type BuildingType;

    protected BindingBuilderBase(IContainer container, Type buildingType)
    {
        Container = container;
        BuildingType = buildingType;
    }

    
    public Binding GetBinding()
    {
        return new Binding();
    }
}