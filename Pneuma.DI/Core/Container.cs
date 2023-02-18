using System;
using System.Collections.Generic;
using Pneuma.DI.Core.BindingContexts;
using Pneuma.DI.Core.Bindings;
using Pneuma.DI.Exception;

namespace Pneuma.DI.Core
{
    public sealed class Container : IContainer, IInjector ,IDisposable
    {
        private readonly Dictionary<Type, Binding> _singletonRegistrations;
        
        private readonly HashSet<Binding> _transientRegistrations;

        private bool _isValid;

        public Container()
        {
            _singletonRegistrations = new Dictionary<Type, Binding>();
            _transientRegistrations = new HashSet<Binding>();
            
            _isValid = true;
        }

        public BindingBuilder Bind<T>()
        {
            Type type = typeof(T);

            return BindInternal(type);
        }

        private BindingBuilder BindInternal(Type type)
        {
            SanityCheck();
            
            return new BindingBuilder(this, type);
        }
        
        public bool ContainerBindingLookup(Type lookupType, out Binding binding)
        {
            binding = default;
            
            if (_singletonRegistrations.ContainsKey(lookupType))
            {
                binding = _singletonRegistrations[lookupType];
                return true;
            }

            Binding placeHolderBinding = new Binding(lookupType, BindingLifeTime.Transient);
            if (_transientRegistrations.TryGetValue(placeHolderBinding, out Binding registeredBinding))
            {
                binding = registeredBinding;
                return true;
            }

            return false;
        }

        public bool RegisterBinding(Binding binding, BindingLifeTime bindingLifeTime)
        {
            switch (bindingLifeTime)
            {
                case BindingLifeTime.Singular:
                    _singletonRegistrations.Add(binding.BindingType, binding);
                    return true;
                case BindingLifeTime.Transient:
                    _transientRegistrations.Add(binding);
                    return true;
                default:
                    _isValid = false;
                    throw new PneumaException("Unable to register binding! Please specify the lifetime of the binding");
            }
        }

        private void SanityCheck()
        {
            if (!_isValid)
            {
                throw new SanityCheckFailedException("Container validity is interrupted.");
            }
        }

        public int GetActiveObjectCount()
        {
            return _singletonRegistrations.Count + _transientRegistrations.Count;
        }

        public void Dispose()
        {
            _singletonRegistrations.Clear();
            _transientRegistrations.Clear();
        }
    }
}