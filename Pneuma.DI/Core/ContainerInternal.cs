using System;
using System.Collections.Generic;
using Pneuma.DI.Core.BindingContexts;
using Pneuma.DI.Core.Bindings;
using Pneuma.DI.Exception;

namespace Pneuma.DI.Core
{
    public sealed partial class Container
    {
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

                binding = lazyBindingBuilder.BuildBinding();
                _lazyBindingBuilderRegistrations.RemoveAt(index);
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
                        throw new BindingFailedException();
                    }

                    Type[] bindingRegistrationBindedInterfaces = bindingRegistration.BindedInterfaces;
                    for (int count = 0; count < bindingRegistrationBindedInterfaces.Length; count++)
                    {
                        Type bindedInterface = bindingRegistrationBindedInterfaces[count];

                        if (TypeIsBindedLookup(bindedInterface, binding))
                        {
                            throw new BindingFailedException();
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
    }
}