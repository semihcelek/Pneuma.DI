using System;
using System.Collections.Generic;
using System.Reflection;

namespace Pneuma.DI.Core
{
    public sealed class Container
    {
        private readonly Dictionary<int, BindingInfo> _container = new Dictionary<int, BindingInfo>();

        public void BindSingle<T>()
        {
            Type type = typeof(T);
            
            if (_container.ContainsKey(type.GetHashCode()))
            {
                return;
            }

            ConstructorInfo constructors = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, );

            foreach (ConstructorInfo constructor in constructors)
            {
                if (constructor.IsPrivate || constructor.IsStatic)
                {
                    continue;
                }

                MethodBody methodBody = constructor.GetMethodBody();
                
                methodBody.
            }
            

            T instance = Activator.CreateInstance<T>();
            BindingInfo bindingInfo = new BindingInfo(instance);
            _container.Add(instance.GetHashCode(), bindingInfo);
        }
    }
}