using System;
using Pneuma.DI.Core.BindingContexts;

namespace Pneuma.DI.Core.Bindings
{
    public readonly struct Binding : IEquatable<Binding>
    {
        public readonly object Instance;

        public readonly Type BindingType;
        public readonly Type InstanceType;

        public readonly BindingLifeTime BindingLifeTime;

        public Binding(object instance, Type bindingType, Type instanceType, BindingLifeTime bindingLifeTime)
        {
            Instance = instance;
            BindingType = bindingType;
            InstanceType = instanceType;
            BindingLifeTime = bindingLifeTime;
        }

        public Binding(Type bindingType, BindingLifeTime bindingLifeTime) : this()
        {
            BindingType = bindingType;
            BindingLifeTime = bindingLifeTime;
        }

        public override int GetHashCode()
        {
            return GetHashCodeConsideringBindingLifeTime();
        }

        public bool Equals(Binding other)
        {
            return GetHashCodeConsideringBindingLifeTime() == other.GetHashCode();
        }

        private int GetHashCodeConsideringBindingLifeTime()
        {
            return  BindingLifeTime == BindingLifeTime.Singular 
                ? BindingType.GetHashCode() 
                : Instance.GetHashCode();
        }
    }
}