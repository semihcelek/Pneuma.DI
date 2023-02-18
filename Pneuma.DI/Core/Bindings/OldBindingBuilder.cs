using System;
using Pneuma.DI.Core.BindingContexts;
using Pneuma.DI.Core.Injectors;

namespace Pneuma.DI.Core.Bindings
{
    public ref struct OldBindingBuilder
    {
        private readonly IContainer _container;
        
        private readonly Type _buildingType;
        private Type _bindingType;
        private Type _instanceType;

        public BindingLifeTime BindingLifeTime;

        public OldBindingBuilder(Type buildingType, IContainer container)
        {
            _buildingType = buildingType;
            _container = container;
            
            _bindingType = null;
            _instanceType = null;
            BindingLifeTime = BindingLifeTime.Unspecified;
        }

        public static OldBindingBuilder Initialize(Type buildingType, IContainer container)
        {
            return new OldBindingBuilder(buildingType, container);
        }

        private OldBindingBuilder ConcreteTypeBinding()
        {
            using ConstructorInjector constructorInjector = ConstructorInjector.Create(_container);

            _bindingType = constructorInjector.InjectAndActivateType(_buildingType);
            _instanceType = _bindingType;
            
            return this;
        }
        
        private OldBindingBuilder AbstractTypeBinding()
        {
            return this;
        }

        private Binding BuildBinding()
        {
            return new Binding(_bindingType, _buildingType, _bindingType, BindingLifeTime);
        }
    }
}