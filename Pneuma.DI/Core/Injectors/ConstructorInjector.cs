using System;
using System.Linq;
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

        public Type InjectAndActivateType(Type buildingType)
        {
            ConstructorInfo constructorInfo = GetPublicNonStaticConstructor(buildingType);
            
            ParameterInfo[] parameterInfos = constructorInfo.GetParameters();
            
            int parameterCount = parameterInfos.Length;
            if (parameterCount <= 0)
            {
                return BindParameterlessType(buildingType);
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
            
                injectParameters[index] = binding.BindingType;
            }

            Type instance = (Type)Activator.CreateInstance(buildingType, injectParameters);
            return instance;
        }
                
        private static ConstructorInfo GetPublicNonStaticConstructor(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();

            constructors.OrderByDescending(c => c.GetParameters().Length);
            ConstructorInfo constructorInfo = constructors.FirstOrDefault();
            return constructorInfo;
        }
        
        private Type BindParameterlessType(Type type)
        {
            Type instance = (Type)Activator.CreateInstance(type);

            return instance;
        }

        public void Dispose()
        {
            
        }
    }
}