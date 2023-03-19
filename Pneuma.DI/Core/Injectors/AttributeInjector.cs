using System;
using System.Collections.Generic;
using System.Reflection;
using Pneuma.DI.Core.Attributes;
using Pneuma.DI.Core.Bindings;
using Pneuma.DI.Exception;

namespace Pneuma.DI.Core.Injectors
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

            PropertyInfo[] propertyInfos = buildingType.GetProperties();
            
            for (var index = 0; index < propertyInfos.Length; index++)
            {
                PropertyInfo propertyInfo = propertyInfos[index];
                IEnumerable<Attribute> attributes = propertyInfo.GetCustomAttributes();

                foreach (Attribute attribute in attributes)
                {
                    if (!attribute.Match(RequiredInjectAttribute))
                    {
                        continue;
                    }

                    Type dependedType = propertyInfo.PropertyType;
                    
                    bool isRequiredTypeBinded = _container.ContainerBindingLookup(dependedType, out Binding binding);
                    if (!isRequiredTypeBinded)
                    {
                        throw new BindingFailedException(
                            $"Unable to find {dependedType}. Required dependency for {buildingType} is not registered to the object graph.");
                    }
                    
                    propertyInfo.SetValue(injectObject, binding.Instance);
                }
            }
        }
    }
}