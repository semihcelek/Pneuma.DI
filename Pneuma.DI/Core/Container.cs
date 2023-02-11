using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pneuma.DI.Core.Binding;
using Pneuma.DI.Core.Binding.Contexts;
using Pneuma.DI.Exception;

namespace Pneuma.DI.Core
{
    public sealed class Container : IBinderContext, IDisposable
    {
        private readonly Dictionary<int, BindingInfo> _container;

        private bool _isValid;

        public Container()
        {
            _container = new Dictionary<int, BindingInfo>();
            _isValid = true;
        }

        public BindingPrototype Bind<T>()
        {
            Type type = typeof(T);

            return BindInternal(type);
        }

        private BindingPrototype BindInternal(Type type)
        {
            SanityCheck();

            ConstructorInfo constructorInfo = GetPublicNonStaticConstructor(type);
            ParameterInfo[] parameterInfos = constructorInfo.GetParameters();

            int parameterCount = parameterInfos.Length;
            if (parameterCount <= 0)
            {
                return BindParameterlessType(type);
            }
            
            object[] injectParameters = new object[parameterCount];

            for (int index = 0; index < parameterCount; index++)
            {
                ParameterInfo parameterInfo = parameterInfos[index];
                
                bool isRequiredTypeBinded = ContainerBindLookup(parameterInfo.ParameterType.GetHashCode(), out object bindedObjectInstance);
                if (!isRequiredTypeBinded)
                {
                    _isValid = false;
                    throw new BindingFailedException(
                        $"Unable to find {parameterInfo.ParameterType}. Required dependency for {type} is not registered to the object graph.");
                }

                injectParameters[index] = bindedObjectInstance;
            }
            
            object instance = constructorInfo.Invoke(injectParameters);
            return new BindingPrototype(instance, this);
        }

        private static ConstructorInfo GetPublicNonStaticConstructor(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();

            constructors.OrderByDescending(c => c.GetParameters().Length);
            ConstructorInfo constructorInfo = constructors.FirstOrDefault();
            return constructorInfo;
        }
        
        private BindingPrototype BindParameterlessType(Type type)
        {
            object instance = Activator.CreateInstance(type);

            return new BindingPrototype(instance, this);
        }

        private bool ContainerBindLookup(int hashCode, out object bindedObjectInstance)
        {
            SanityCheck();
            
            bindedObjectInstance = null;

            if (!_container.ContainsKey(hashCode))
            {
                return CheckTransientInstances(hashCode, out bindedObjectInstance);
            }

            BindingInfo bindingInfo = _container[hashCode];
            bindedObjectInstance = bindingInfo.GetBindingInstance();

            return true;
        }

        private bool CheckTransientInstances(int hashCode, out object bindedObjectInstance)
        {
            bindedObjectInstance = null;
            
            for (int index = 0; index < _container.Count; index++)
            {
                KeyValuePair<int, BindingInfo> bindingInfoKvp = _container.ElementAt(index);

                if (hashCode != bindingInfoKvp.Value.GetBindingType().GetHashCode())
                {
                    continue;
                }

                bindedObjectInstance = bindingInfoKvp.Value.GetBindingInstance();
                return true;
            }
            return false;
        }

        public void RegisterBinding(BindingInfo bindingInfo)
        {
            SanityCheck();
            
            if (_container.ContainsKey(bindingInfo.GetBindingType().GetHashCode()))
            {
                throw new BindingFailedException(
                    $"Unable to bind {bindingInfo.GetBindingType()}. Multiple binding of a singular dependency not allowed!");
            }
            
            _container.Add(bindingInfo.GetHashCode(), bindingInfo);
        }

        public int GetActiveObjectCount()
        {
            return _container.Count;
        }
        
        private void SanityCheck()
        {
            if (!_isValid)
            {
                throw new SanityCheckFailedException("Container validity is interrupted.");
            }
        }

        public void Dispose()
        {
            _container.Clear();
        }
    }
}