using System;
using Pneuma.DI.Core.Injectors;
using Pneuma.DI.Exception;

namespace Pneuma.DI.Core.Bindings;

public class BindingInjectionBuilder<T> : BindingBuilderBase where T : BindingInjectionBuilder<T>
{
    protected object ActivatedObject;

    protected Type SpecifiedConcreteType;
    
    public BindingInjectionBuilder(IContainer container, Type buildingType) : base(container, buildingType)
    {
    }

    protected void InjectDependencies()
    {
        using ConstructorInjector constructorInjector = ConstructorInjector.Create(Container);

        Type concreteType = RetrieveConcreteType();
        ActivatedObject = constructorInjector.InjectAndActivateType(concreteType);
    }

    protected Type RetrieveConcreteType()
    {
        if (BuildingType.IsAbstract || BuildingType.IsInterface)
        {
            return SpecifiedConcreteType == null
                ? throw new BindingFailedException(
                    "Unable to bind abstract/interface type because concrete type is not specified!")
                : SpecifiedConcreteType;
        }
        
        return BuildingType;
    }

    public T To<T1>()
    {
        SpecifiedConcreteType = typeof(T1);
        
        return this as T;
    }  
}