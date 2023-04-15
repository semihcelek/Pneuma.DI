using System;
using Pneuma.DI.BindingContexts;

namespace Pneuma.DI.Bindings
{
    public readonly struct Binding : IEquatable<Binding>
    {
        public readonly object Instance;

        public readonly Type BindingType;
        public readonly Type InstanceType;

        public readonly Type[] BindedInterfaces;

        public readonly BindingLifeTime BindingLifeTime;

        public Binding(object instance, Type bindingType, Type instanceType, BindingLifeTime bindingLifeTime,
            Type[] bindedInterfaces)
        {
            Instance = instance;
            BindingType = bindingType;
            InstanceType = instanceType;
            BindingLifeTime = bindingLifeTime;
            BindedInterfaces = bindedInterfaces;
        }

        public bool Equals(Binding other)
        {
            return Equals(BindingType, other.BindingType);
        }

        public override bool Equals(object obj)
        {
            return obj is Binding other && Equals(other);
        }
    }
}