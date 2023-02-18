using System;
using Pneuma.DI.Core.Injectors;

namespace Pneuma.DI.Core.Bindings;

public class BindingInjectionBuilder<T> : BindingBuilderBase where T : BindingInjectionBuilder<T>
{
    protected object ActivatedObject;
    
    public BindingInjectionBuilder(IContainer container, Type buildingType) : base(container, buildingType)
    {
        using ConstructorInjector constructorInjector = ConstructorInjector.Create(container);

        ActivatedObject = constructorInjector.InjectAndActivateType(buildingType);
    }

    public T To<T1>()
    {
        return this as T;
    }  
}