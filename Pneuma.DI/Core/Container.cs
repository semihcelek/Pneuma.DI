using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public bool RegisterBinding(Binding binding)
        {
            throw new NotImplementedException();
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