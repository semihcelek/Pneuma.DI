using System;

namespace Pneuma.DI.Core
{
    public readonly struct BindingInfo
    {
        private readonly object _instance;
        
        private readonly Type _bindingType;

        private readonly int _hashCode;

        public BindingInfo(object instance)
        {
            _instance = instance;
            _bindingType = instance.GetType();
            _hashCode = instance.GetHashCode();
        }

        public Type GetBindingType()
        {
            Console.WriteLine(_bindingType);
            return _bindingType;
        }

        public object GetBindingInstance()
        {
            return _instance;
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }
    }
}