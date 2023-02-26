using System;
using Pneuma.DI.Core.BindingContexts;
using Pneuma.DI.Core.Injectors;
using Pneuma.DI.Exception;

namespace Pneuma.DI.Core.Bindings;

public struct BindingBuilder<TBinding> : IBindingBuilder<TBinding>
{
    private readonly IContainer _container;

    private Type _specifiedConcreteType;
    
    private object _activatedObject;
    
    private BindingLifeTime _bindingLifeTime;
    
    private RegistrationTime _registrationTime;
    
    public BindingBuilder(IContainer container)
    {
        _container = container;
    }

    public IBindingBuilder<TBinding> To<TConcrete>() where TConcrete : TBinding
    {
        _specifiedConcreteType = typeof(TConcrete);

        return this;
    }

    public IBindingActivator<TBinding> AsSingle()
    {
        _bindingLifeTime = BindingLifeTime.Singular;

        return this;
    }

    public IBindingActivator<TBinding> AsTransient()
    {
        _bindingLifeTime = BindingLifeTime.Transient;

        return this;
    }

    public void Lazy()
    {
        _container.RegisterLazyBinding(this);
    }

    public void NonLazy()
    {
        Binding binding = ActivateBinding();
        _container.RegisterBinding(binding, BindingLifeTime.Transient);
    }

    private Binding ActivateBinding()
    {
        InjectDependencies<TBinding>();

        Binding binding = new Binding(_activatedObject, typeof(TBinding), _activatedObject.GetType(),
            BindingLifeTime.Transient);
        return binding;
    }

    private void InjectDependencies<T>()
    {
        using ConstructorInjector constructorInjector = ConstructorInjector.Create(_container);

        Type concreteType = RetrieveConcreteType<T>();
        _activatedObject = constructorInjector.InjectAndActivateType(concreteType);
    }
    
    private Type RetrieveConcreteType<T>()
    {
        Type buildingType = typeof(T);
        
        if (buildingType.IsAbstract || buildingType.IsInterface)
        {
            return _specifiedConcreteType == null
                ? throw new BindingFailedException(
                    "Unable to bind abstract/interface type because concrete type is not specified!")
                : _specifiedConcreteType;
        }
        
        return buildingType;
    }

    public Binding BuildBinding()
    {
        return ActivateBinding();
    }
}