using System;
using Pneuma.DI.Core.Injectors;
using Pneuma.DI.Exception;

namespace Pneuma.DI.Core.Bindings;

public class BindingInjectionBuilder<TBinding, TBuilder> : BindingBuilderBase where TBuilder : BindingInjectionBuilder<TBinding, TBuilder>
{
    protected object ActivatedObject;

    protected Type SpecifiedConcreteType;
    
    public BindingInjectionBuilder(IContainer container) : base(container)
    {
    }

    protected void InjectDependencies<T>()
    {
        using ConstructorInjector constructorInjector = ConstructorInjector.Create(Container);

        Type concreteType = RetrieveConcreteType<T>();
        ActivatedObject = constructorInjector.InjectAndActivateType(concreteType);
    }

    protected Type RetrieveConcreteType<T>()
    {
        Type buildingType = typeof(T);
        
        if (buildingType.IsAbstract || buildingType.IsInterface)
        {
            return SpecifiedConcreteType == null
                ? throw new BindingFailedException(
                    "Unable to bind abstract/interface type because concrete type is not specified!")
                : SpecifiedConcreteType;
        }
        
        return buildingType;
    }

    public BindingBuilder<TBinding> To<T1>()
    {
        SpecifiedConcreteType = typeof(T1);
        
        return this as BindingBuilder<TBinding>;
    }  
}