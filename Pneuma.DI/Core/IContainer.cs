using System;
using Pneuma.DI.Core.BindingContexts;
using Pneuma.DI.Core.Bindings;

namespace Pneuma.DI.Core
{
    public interface IContainer
    {
        bool ContainerBindingLookup(Type lookupType, out Binding binding);
        
        bool RegisterBinding(Binding binding, BindingLifeTime bindingLifeTime);
    }
}