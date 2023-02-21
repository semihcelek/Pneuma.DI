using System;
using Pneuma.DI.Core;
using Pneuma.DI.Core.BindingContexts;
using Pneuma.DI.Core.Bindings;
using Pneuma.DI.Exception;
using Pneuma.DI.Tests.Examples;

namespace Pneuma.DI.Tests.ContainerTests;

public class MockContainer : IContainer
{
    public Binding RegisteredBinding; 
    
    public bool ContainerBindingLookup(Type lookupType, out Binding binding)
    {
        binding = default;

        if (lookupType == typeof(Foo))
        {
            CreateMockFooBinding(out binding);
            return true;
        }
        
        return false;
    }

    public bool RegisterBinding(Binding binding, BindingLifeTime bindingLifeTime)
    {
        if (bindingLifeTime != BindingLifeTime.Singular && bindingLifeTime != BindingLifeTime.Transient)
        {
            throw new PneumaException("Unable to register binding! Please specify valid lifetime for the binding");
        }
        
        if (Equals(binding, RegisteredBinding) && binding.BindingLifeTime == BindingLifeTime.Singular &&
            RegisteredBinding.BindingLifeTime == BindingLifeTime.Singular)
        {
            throw new ArgumentException();
        }

        RegisteredBinding = binding;
        return true;
    }

    private static void CreateMockFooBinding(out Binding binding)
    {
        object foo = Activator.CreateInstance<Foo>();
        binding = new Binding(foo, typeof(Foo), typeof(Foo), BindingLifeTime.Singular);
    }
}