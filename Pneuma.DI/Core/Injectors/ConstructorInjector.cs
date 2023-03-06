using System;
using System.Reflection;
using Pneuma.DI.Core.Bindings;
using Pneuma.DI.Exception;

namespace Pneuma.DI.Core.Injectors
{
    public readonly struct ConstructorInjector : IDisposable
    {
        private readonly IContainer _container;

        public ConstructorInjector(IContainer container)
        {
            _container = container;
        }

        public static ConstructorInjector Create(IContainer container)
        {
            return new ConstructorInjector(container);
        }

        public bool InjectToConstructor<TBinding, TConcrete>(out TConcrete injectedObject) where TConcrete : TBinding
        {
            return InjectToConstructorInternal(out injectedObject);
        }

        public bool InjectToConstructor<TBinding>(out TBinding injectedObject)
        {
            return InjectToConstructorInternal(out injectedObject);
        }

        private bool InjectToConstructorInternal<TBinding>(out TBinding injectedObject)
        {
            injectedObject = default;

            Type buildingType = typeof(TBinding);
            
            ConstructorInfo constructorInfo = GetPublicNonStaticConstructor(buildingType);
            ParameterInfo[] parameterInfos = constructorInfo.GetParameters();
            
            int parameterCount = parameterInfos.Length;
            if (parameterCount <= 0)
            {
                injectedObject = BindParameterlessType<TBinding>();
                return true;
            }
            
            object[] injectParameters = new object[parameterCount];
            
            for (int index = 0; index < parameterCount; index++)
            {
                ParameterInfo parameterInfo = parameterInfos[index];
                
                bool isRequiredTypeBinded = _container.ContainerBindingLookup(parameterInfo.ParameterType, out Binding binding);
                if (!isRequiredTypeBinded)
                {
                    throw new BindingFailedException(
                        $"Unable to find {parameterInfo.ParameterType}. Required dependency for {buildingType} is not registered to the object graph.");
                }
            
                injectParameters[index] = binding.Instance;
            }
            
            injectedObject = (TBinding)Activator.CreateInstance(buildingType, injectParameters);
            return true;
        }
                
        private static ConstructorInfo GetPublicNonStaticConstructor(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();

            if (constructors.Length > 1)
            {
                throw new PneumaException("Multiple constructors found exception!");
            }
            
            ConstructorInfo constructorInfo = constructors[0];
            return constructorInfo;
        }
        
        private T BindParameterlessType<T>()
        {
            T instance = Activator.CreateInstance<T>();
            return instance;
        }

        public void Dispose()
        {
            
        }
    }
}