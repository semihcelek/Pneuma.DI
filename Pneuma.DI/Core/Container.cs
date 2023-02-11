using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pneuma.DI.Exception;

namespace Pneuma.DI.Core
{
    public sealed class Container
    {
        private readonly Dictionary<int, BindingInfo> _container;

        private bool _isValid;

        public Container()
        {
            _container = new Dictionary<int, BindingInfo>();
            _isValid = true;
        }

        public void BindSingle<T>()
        {
            Type type = typeof(T);
            BindInternal(type);
        }

        private void BindInternal(Type type)
        {
            SanityCheck();
            
            ConstructorInfo[] constructors = type.GetConstructors();

            constructors.OrderByDescending(c => c.GetParameters().Length);
            ConstructorInfo constructorInfo = constructors.FirstOrDefault();
            ParameterInfo[] parameterInfos = constructorInfo.GetParameters();

            int parameterCount = parameterInfos.Length;
            if (parameterCount <= 0)
            {
                BindParameterlessType(type);
                return;
            }
            
            object[] injectParameters = new object[parameterCount];

            for (int index = 0; index < parameterCount; index++)
            {
                ParameterInfo parameterInfo = parameterInfos[index];
                
                bool isRequiredTypeBinded = ContainerBindLookup(parameterInfo.ParameterType, out object bindedObjectInstance);

                if (!isRequiredTypeBinded)
                {
                    _isValid = false;
                    throw new BindingFailedException(
                        $"Unable to find {parameterInfo.ParameterType}. Required dependency for {type} is not registered to the object graph.");
                }

                injectParameters[index] = bindedObjectInstance;
            }

            object instance = constructorInfo.Invoke(injectParameters);

            BindingInfo bindingInfo = new BindingInfo(instance);
            _container.Add(bindingInfo.GetHashCode(), bindingInfo);
        }

        private bool ContainerBindLookup(Type lookupType, out object bindedObjectInstance)
        {
            SanityCheck();
            
            bindedObjectInstance = null;

            if (!_container.ContainsKey(lookupType.GetHashCode()))
            {
                return false;
            }

            BindingInfo bindingInfo = _container[lookupType.GetHashCode()];
            bindedObjectInstance = bindingInfo.GetBindingInstance();

            return true;
        }

        private void BindParameterlessType(Type type)
        {
            object instance = Activator.CreateInstance(type);

            BindingInfo bindingInfo = new BindingInfo(instance);
            _container.Add(bindingInfo.GetHashCode(), bindingInfo);
        }

        private void SanityCheck()
        {
            if (!_isValid)
            {
                throw new SanityCheckFailedException("Container validity is interrupted.");
            }
        }
    }
}