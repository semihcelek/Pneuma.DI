using System;
using System.Collections.Generic;
using Pneuma.DI.Core.BindingContexts;
using Pneuma.DI.Core.Bindings;
using Pneuma.DI.Exception;

namespace Pneuma.DI.Core
{
    public sealed class Container : IContainer, IInjector ,IDisposable
    {
        private readonly Dictionary<int, Binding> _singletonRegistrations;
        
        private readonly List<Binding> _transientRegistrations;

        private bool _isValid;

        public Container()
        {
            _singletonRegistrations = new Dictionary<int, Binding>();
            _transientRegistrations = new List<Binding>();
            
            _isValid = true;
        }

        public BindingBuilder Bind<T>()
        {
            SanityCheck();
            Type type = typeof(T);
            return new BindingBuilder(this, type);
        }

        public BindingBuilder BindInterface<T>()
        {
            SanityCheck();
            Type type = typeof(T);
            return new BindingBuilder(this, type);
        }

        public bool ContainerBindingLookup(Type lookupType, out Binding binding)
        {
            binding = default;

            int lookupTypeHashCode = lookupType.GetHashCode();

            if (_singletonRegistrations.ContainsKey(lookupTypeHashCode))
            {
                binding = _singletonRegistrations[lookupTypeHashCode];
                return true;
            }
            
            for (int index = 0; index < _transientRegistrations.Count; index++)
            {
                Binding transientBinding = _transientRegistrations[index];

                int bindingTypeHashCode = transientBinding.BindingType.GetHashCode();
                if (bindingTypeHashCode != lookupTypeHashCode)
                {
                    continue;
                }
                
                binding = transientBinding;
                return true;
            }
            return false;
        }

        public bool RegisterBinding(Binding binding, BindingLifeTime bindingLifeTime)
        {
            switch (bindingLifeTime)
            {
                case BindingLifeTime.Singular:
                    _singletonRegistrations.Add(binding.GetHashCode(), binding);
                    return true;
                case BindingLifeTime.Transient:
                    _transientRegistrations.Add(binding);
                    return true;
                default:
                    _isValid = false;
                    throw new PneumaException("Unable to register binding! Please specify valid lifetime for the binding");
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