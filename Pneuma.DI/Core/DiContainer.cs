using System;
using System.Collections.Generic;
using Pneuma.DI.BindingContexts;
using Pneuma.DI.Bindings;
using Pneuma.DI.Exception;

namespace Pneuma.DI.Core
{
    public sealed class DiContainer : IContainer
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
        
        public IBindingBuilder<TConcrete> Bind<TConcrete, TAbstract>()
            where TConcrete : TAbstract
        {
            IBindingBuilder<TConcrete> bindingBuilder = new BindingBuilder<TConcrete>(this);

            bindingBuilder.AddInterface(typeof(TAbstract));

            return bindingBuilder;
        }

        public IBindingBuilder<TConcrete> Bind<TConcrete, TAbstract1, TAbstract2>()
            where TConcrete : TAbstract1, TAbstract2
        {
            IBindingBuilder<TConcrete> bindingBuilder = new BindingBuilder<TConcrete>(this);

            bindingBuilder.AddInterface(typeof(TAbstract1));
            bindingBuilder.AddInterface(typeof(TAbstract2));
        
            return bindingBuilder;
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
                    Type bindedInterface = bindedInterfaces[count];
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

            for (int index = _lazyBindingBuilderRegistrations.Count - 1; index >= 0; index--)
            {
                IBindingBuilder lazyBindingBuilder = _lazyBindingBuilderRegistrations[index];
                
                IReadOnlyList<Type> bindedInterfaces = lazyBindingBuilder.BindedInterfaces;
                if (bindedInterfaces.Count <= 0)
                {
                    continue;
                }
                
                for (int count = 0; count < bindedInterfaces.Count; count++)
                {
                    Type bindedInterface = bindedInterfaces[count];
                    if (bindedInterface != lookupType)
                    {
                        continue;
                    }

                    binding = lazyBindingBuilder.BuildBinding();
                    _lazyBindingBuilderRegistrations.RemoveAt(index);
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

            for (int index = _lazyBindingBuilderRegistrations.Count - 1; index >= 0; index--)
            {
                IBindingBuilder lazyBindingBuilder = _lazyBindingBuilderRegistrations[index];
                if (lazyBindingBuilder.BuildingType != lookupType)
                {
                    continue;
                }

                _lazyBindingBuilderRegistrations.RemoveAt(index);
                binding = lazyBindingBuilder.BuildBinding();
                return true;
            }

            return false;
        }

        private bool RegisterInternal(Binding binding)
        {
            if (binding.BindingLifeTime == BindingLifeTime.Singular)
            {
                for (int index = 0; index < _registrations.Count; index++)
                {
                    Binding bindingRegistration = _registrations[index];
                    if (bindingRegistration.BindingLifeTime != BindingLifeTime.Singular)
                    {
                        continue;
                    }

                    if (bindingRegistration.BindingType == binding.BindingType)
                    {
                        throw new BindingFailedException(
                            $"Unable to bind {binding.BindingType}! {binding.BindingType} is already binded!");
                    }

                    Type[] bindingRegistrationBindedInterfaces = bindingRegistration.BindedInterfaces;
                    for (int count = 0; count < bindingRegistrationBindedInterfaces.Length; count++)
                    {
                        Type bindedInterface = bindingRegistrationBindedInterfaces[count];

                        if (TypeIsBindedLookup(bindedInterface, binding))
                        {
                            throw new BindingFailedException(
                                $"Unable to bind {binding.BindingType}! {bindedInterface} is already binded!");
                        }
                    }
                }

                _registrations.Add(binding);
            }
            else
            {
                _registrations.Add(binding);
            }
            
            return true;
        }

        private bool TypeIsBindedLookup(Type lookType, Binding bindingType)
        {
            if (lookType == bindingType.BindingType)
            {
                return true;
            }

            for (int index = 0; index < bindingType.BindedInterfaces.Length; index++)
            {
                Type bindedInterface = bindingType.BindedInterfaces[index];
                if (lookType == bindedInterface)
                {
                    return true;
                }
            }

            return false;
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