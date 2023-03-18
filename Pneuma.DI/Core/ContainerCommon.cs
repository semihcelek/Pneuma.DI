using System;
using System.Collections.Generic;
using Pneuma.DI.Core.Bindings;
using Pneuma.DI.Exception;

namespace Pneuma.DI.Core
{
    public sealed partial class Container : IContainer, IInjector, IDisposable
    {
        private readonly List<Binding> _registrations;

        private readonly List<IBindingBuilder> _lazyBindingBuilderRegistrations;

        private bool _isValid;

        public Container()
        {
            _registrations = new List<Binding>();
            _lazyBindingBuilderRegistrations = new List<IBindingBuilder>();

            _isValid = true;
        }

        public IBindingBuilder<T> Bind<T>()
        {
            SanityCheck();
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

        public void SanityCheck()
        {
            if (!_isValid)
            {
                throw new SanityCheckFailedException("Container validity is interrupted.");
            }
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