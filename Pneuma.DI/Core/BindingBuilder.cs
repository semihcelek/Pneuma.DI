using System;
using Pneuma.DI.Core.BindingContexts;
using Pneuma.DI.Core.Injectors;

namespace Pneuma.DI.Core
{
    public ref struct BindingBuilder
    {
        private readonly IContainer _container;
        
        private readonly Type _buildingType;
        private Type _bindingType;
        private Type _instanceType;

        public BindingLifeTime BindingLifeTime;

        public BindingBuilder(Type buildingType, IContainer container)
        {
            _buildingType = buildingType;
            _container = container;
            
            _bindingType = null;
            _instanceType = null;
            BindingLifeTime = BindingLifeTime.Unspecified;
        }

        public static BindingBuilder Initialize(Type buildingType, IContainer container)
        {
            return new BindingBuilder(buildingType, container);
        }

        private BindingBuilder ConcreteTypeBinding()
        {
            using ConstructorInjector constructorInjector = ConstructorInjector.Create(_container);

            _bindingType = constructorInjector.InjectAndActivateType(_buildingType);
            _instanceType = _bindingType;
            
            return this;
        }
        
        private BindingBuilder AbstractTypeBinding()
        {
            return this;
        }

        private Binding BuildBinding()
        {
            return new Binding(_bindingType, _buildingType, _bindingType, BindingLifeTime);
        }
    }
}