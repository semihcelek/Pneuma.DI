using System;
using System.Collections.Generic;
using Pneuma.DI.Core.BindingContexts;
using Pneuma.DI.Core.Bindings;
using Pneuma.DI.Exception;

namespace Pneuma.DI.Core
{
    public sealed class Container : IContainer, IInjector, IDisposable
    {
        private readonly List<Binding> _registrations;

        private readonly List<IBindingBuilder> _lazyBindingBuilerRegistrations;

        private bool _isValid;

        public Container()
        {
            _registrations = new List<Binding>();
            _lazyBindingBuilerRegistrations = new List<IBindingBuilder>();

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

        private bool InterfaceBindingLookup(Type lookupType, out Binding binding, bool bindAvailableLazyBindings)
        {
            binding = default;

            for (int index = _registrations.Count - 1; index >= 0; index--)
            {
                Binding registeredBinding = _registrations[index];
                if (registeredBinding.BindedInterfaces.Length <= 0)
                {
                    continue;
                }

                Type[] bindedInterfaces = registeredBinding.BindedInterfaces;
                for (int count = 0; count < bindedInterfaces.Length; count++)
                {
                    Type bindedInterface = bindedInterfaces[index];

                    if (bindedInterface != lookupType)
                    {
                        continue;
                    }

                    binding = registeredBinding;
                    return true;
                }
            }

            if (!bindAvailableLazyBindings)
            {
                return false;
            }

            for (int index = _lazyBindingBuilerRegistrations.Count - 1; index >= 0; index--)
            {
                IBindingBuilder lazyBindingBuilder = _lazyBindingBuilerRegistrations[index];
                
                IReadOnlyList<Type> bindedInterfaces = lazyBindingBuilder.BindedInterfaces;
                if (bindedInterfaces.Count <= 0)
                {
                    continue;
                }
                
                for (int count = 0; count < bindedInterfaces.Count; count++)
                {
                    Type bindedInterface = bindedInterfaces[index];

                    if (bindedInterface != lookupType)
                    {
                        continue;
                    }

                    binding = lazyBindingBuilder.BuildBinding();
                    _lazyBindingBuilerRegistrations.RemoveAt(index);
                    return true;
                }
            }

            return false;
        }

        private bool ConcreteBindingLookup(Type lookupType, out Binding binding, bool bindAvailableLazyBindings)
        {
            binding = default;

            for (int index = _registrations.Count - 1; index >= 0; index--)
            {
                Binding registeredBinding = _registrations[index];
                if (registeredBinding.BindingType != lookupType)
                {
                    continue;
                }

                binding = registeredBinding;
                return true;
            }

            if (!bindAvailableLazyBindings)
            {
                return false;
            }

            for (int index = _lazyBindingBuilerRegistrations.Count - 1; index >= 0; index--)
            {
                IBindingBuilder lazyBindingBuilder = _lazyBindingBuilerRegistrations[index];
                if (lazyBindingBuilder.BuildingType != lookupType)
                {
                    continue;
                }

                binding = lazyBindingBuilder.BuildBinding();
                _lazyBindingBuilerRegistrations.RemoveAt(index);
                return true;
            }

            return false;
        }

        public bool RegisterBinding(Binding binding)
        {
            _registrations.Add(binding);
            return true;
        }

        public bool RegisterLazyBinding<TBinding>(BindingBuilder<TBinding> bindingBuilder)
        {
            _lazyBindingBuilerRegistrations.Add(bindingBuilder);
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
            _lazyBindingBuilerRegistrations.Clear();
        }
    }
}