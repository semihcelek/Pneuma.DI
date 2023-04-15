using System;
using System.Collections.Generic;
using System.Reflection;
using Pneuma.DI.Attributes;
using Pneuma.DI.Bindings;
using Pneuma.DI.Core;
using Pneuma.DI.Exception;

namespace Pneuma.DI.Injectors
{
    public struct AttributeInjector
    {
        private readonly IContainer _container;

        private static readonly InjectAttribute RequiredInjectAttribute = new InjectAttribute(false);
        private static readonly InjectAttribute OptionalInjectAttribute = new InjectAttribute(true);

        public AttributeInjector(IContainer container)
        {
            _container = container;
        }

        public static AttributeInjector Create(IContainer container)
        {
            return new AttributeInjector(container);
        }

        public void InjectToAttributes<TBinding>(ref TBinding injectObject)
        {
            Type buildingType = typeof(TBinding);

            InjectProperties(ref injectObject, buildingType);
            InjectFields(ref injectObject, buildingType);
            InjectMethods(ref injectObject, buildingType);
        }

        private void InjectProperties<TBinding>(ref TBinding injectObject, Type buildingType)
        {
            PropertyInfo[] propertyInfos = buildingType.GetProperties();
            for (var index = 0; index < propertyInfos.Length; index++)
            {
                PropertyInfo propertyInfo = propertyInfos[index];
                IEnumerable<Attribute> attributes = propertyInfo.GetCustomAttributes();

                foreach (Attribute attribute in attributes)
                {
                    bool isRequiredDependency = attribute.Match(RequiredInjectAttribute);
                    bool isOptionalDependency = attribute.Match(OptionalInjectAttribute);

                    if (!isRequiredDependency && !isOptionalDependency)
                    {
                        continue;
                    }

                    Type dependedType = propertyInfo.PropertyType;

                    bool isRequiredTypeBinded = _container.ContainerBindingLookup(dependedType, out Binding binding);
                    if (!isRequiredTypeBinded && isRequiredDependency)
                    {
                        throw new BindingFailedException(
                            $"Unable to find {dependedType}. Required dependency for {buildingType} is not registered to the object graph.");
                    }
                    
                    if (!isRequiredTypeBinded)
                    {
                        continue;
                    }

                    propertyInfo.SetValue(injectObject, binding.Instance);
                }
            }
        }

        private void InjectFields<TBinding>(ref TBinding injectObject, Type buildingType)
        {
            FieldInfo[] fieldInfos = buildingType.GetFields();
            for (var index = 0; index < fieldInfos.Length; index++)
            {
                FieldInfo fieldInfo = fieldInfos[index];
                IEnumerable<Attribute> attributes = fieldInfo.GetCustomAttributes();

                foreach (Attribute attribute in attributes)
                {
                    bool isRequiredDependency = attribute.Match(RequiredInjectAttribute);
                    bool isOptionalDependency = attribute.Match(OptionalInjectAttribute);

                    if (!isRequiredDependency && !isOptionalDependency)
                    {
                        continue;
                    }

                    Type dependedType = fieldInfo.FieldType;

                    bool isRequiredTypeBinded = _container.ContainerBindingLookup(dependedType, out Binding binding);
                    if (!isRequiredTypeBinded && isRequiredDependency)
                    {
                        throw new BindingFailedException(
                            $"Unable to find {dependedType}. Required dependency for {buildingType} is not registered to the object graph.");
                    }

                    if (!isRequiredTypeBinded)
                    {
                        continue;
                    }

                    fieldInfo.SetValue(injectObject, binding.Instance);
                }
            }
        }

        private void InjectMethods<TBinding>(ref TBinding injectObject, Type buildingType)
        {
            MethodInfo[] methodInfos = buildingType.GetMethods();
            for (var index = 0; index < methodInfos.Length; index++)
            {
                MethodInfo methodInfo = methodInfos[index];
                IEnumerable<Attribute> attributes = methodInfo.GetCustomAttributes();

                foreach (Attribute attribute in attributes)
                {
                    bool isRequiredDependency = attribute.Match(RequiredInjectAttribute);
                    bool isOptionalDependency = attribute.Match(OptionalInjectAttribute);

                    if (!isRequiredDependency && !isOptionalDependency)
                    {
                        continue;
                    }

                    ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                    int parameterCount = parameterInfos.Length;
                    object[] parameters = new object[parameterCount];
                    for (int count = 0; count < parameterCount; count++)
                    {
                        Type parameterType = parameterInfos[count].ParameterType;

                        bool isRequiredTypeBinded = _container.ContainerBindingLookup(parameterType, out Binding binding);
                        if (!isRequiredTypeBinded)
                        {
                            throw new BindingFailedException(
                                $"Unable to find {parameterType}. Required dependency for {buildingType} is not registered to the object graph.");
                        }

                        parameters[count] = binding.Instance;
                    }

                    methodInfo.Invoke(injectObject, parameters);
                }
            }
        }
    }
}