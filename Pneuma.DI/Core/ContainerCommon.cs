using System;
using System.Collections.Generic;
using Pneuma.DI.Core.Bindings;

namespace Pneuma.DI.Core
{
    public sealed partial class DiContainer : IContainer, IInjector, IDisposable
    {
        private readonly List<Binding> _registrations;

        private readonly List<IBindingBuilder> _lazyBindingBuilderRegistrations;

        public DiContainer()
        {
            _registrations = new List<Binding>();
            _lazyBindingBuilderRegistrations = new List<IBindingBuilder>();
        }

        public IBindingBuilder<T> Bind<T>()
        {
            return new BindingBuilder<T>(this);
        }
        
        public bool ContainerBindingLookup(Type lookupType, out Binding binding, bool bindAvailableLazyBindings = true)
        {
            binding = default;

            return lookupType.IsInterface 
                ? InterfaceBindingLookup(lookupType, out binding, bindAvailableLazyBindings) 
                : ConcreteBindingLookup(lookupType, out binding, bindAvailableLazyBindings);
        }

        public bool RegisterBinding(Binding binding)
        {
            return RegisterInternal(binding);
        }

        public bool RegisterLazyBinding<TBinding>(BindingBuilder<TBinding> bindingBuilder)
        {
            _lazyBindingBuilderRegistrations.Add(bindingBuilder);
            return true;
        }
        
        public int GetActiveObjectCount()
        {
            return _registrations.Count;
        }

        public void Dispose()
        {
            _registrations.Clear();
            _lazyBindingBuilderRegistrations.Clear();
        }
    }
}