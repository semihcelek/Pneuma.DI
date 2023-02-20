using System;
using Pneuma.DI.Core.BindingContexts;

namespace Pneuma.DI.Core.Bindings;

public class BindingLifeTimeBuilder<T> : BindingInjectionBuilder<T> where T: BindingLifeTimeBuilder<T>
{
    private BindingLifeTime _bindingLifeTime;
    
    public BindingLifeTimeBuilder(IContainer container, Type buildingType) : base(container, buildingType)
    {
    }

    public T AsSingle()
    {
        _bindingLifeTime = BindingLifeTime.Singular;
        Binding binding = new Binding(ActivatedObject, BuildingType, ActivatedObject.GetType(),
            BindingLifeTime.Singular);
        Container.RegisterBinding(binding, BindingLifeTime.Singular);
        
        return this as T;
    }

    public T AsTransient()
    {
        _bindingLifeTime = BindingLifeTime.Transient;
        Binding binding = new Binding(ActivatedObject, BuildingType, ActivatedObject.GetType(),
            BindingLifeTime.Transient);
        Container.RegisterBinding(binding, BindingLifeTime.Transient);

        return this as T;
    }
}