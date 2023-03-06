using System;
using System.Collections.Generic;
using Pneuma.DI.Core.BindingContexts;
using Pneuma.DI.Core.Injectors;

namespace Pneuma.DI.Core.Bindings;

public struct BindingBuilder<TBinding> : IBindingBuilder<TBinding>
{
    private readonly IContainer _container;

    public  Type BuildingType => typeof(TBinding);
    
    private TBinding _specifiedConcreteType;

    private object _activatedObject;

    public BindingLifeTime BindingLifeTime;

    private RegistrationTime _registrationTime;

    private readonly List<Type> _bindingInterfaces;

    public BindingBuilder(IContainer container)
    {
        _container = container;
        _bindingInterfaces = new List<Type>();
        _activatedObject = null;
        BindingLifeTime = BindingLifeTime.Unspecified;
        _registrationTime = RegistrationTime.Unspecified;
    }

    public IBindingBuilder<TBinding> To<TConcrete>() where TConcrete : TBinding
    {
        _specifiedConcreteType = default(TConcrete);

        return this;
    }

    public IBindingActivator<TBinding> AsSingle()
    {
        BindingLifeTime = BindingLifeTime.Singular;

        return this;
    }

    public IBindingActivator<TBinding> AsTransient()
    {
        BindingLifeTime = BindingLifeTime.Transient;

        return this;
    }

    public void Lazy()
    {
        _container.RegisterLazyBinding(this);
    }

    public void NonLazy()
    {
        Binding binding = ActivateBinding();
        _container.RegisterBinding(binding, BindingLifeTime);
    }

    private Binding ActivateBinding()
    {
        InjectDependencies<TBinding>();

        Binding binding = new Binding(_activatedObject,
            BuildingType, _activatedObject.GetType(),
            BindingLifeTime, Array.Empty<Type>());
        return binding;
    }

    private void InjectDependencies<T>()
    {
        using ConstructorInjector constructorInjector = ConstructorInjector.Create(_container);
        
        constructorInjector.InjectToConstructor<T>(out T activatedObject);
        _activatedObject = activatedObject;
    }


    public Binding BuildBinding()
    {
        Binding activatedBinding = ActivateBinding();
        _container.RegisterBinding(activatedBinding, activatedBinding.BindingLifeTime);
        
        return activatedBinding;
    }

    public bool Equals(BindingBuilder<TBinding> other)
    {
        return Equals(_container, other._container) && Equals(_specifiedConcreteType, other._specifiedConcreteType) &&
               Equals(_activatedObject, other._activatedObject) && BindingLifeTime == other.BindingLifeTime &&
               _registrationTime == other._registrationTime;
    }

    public bool Equals(IBindingBuilder other)
    {
        return Equals(this, other);
    }

    public void AddInterface(Type bindingInterface)
    {
        if (_bindingInterfaces.Contains(bindingInterface))
        {
            return;
        }
        
        _bindingInterfaces.Add(bindingInterface);
    }

    public override bool Equals(object obj)
    {
        return obj is BindingBuilder<TBinding> other && Equals(other);
    }
}