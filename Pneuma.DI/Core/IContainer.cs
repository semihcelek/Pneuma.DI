using System;

namespace Pneuma.DI.Core
{
    public interface IContainer
    {
        bool ContainerBindingLookup(Type lookupType, out Binding binding);
        
        bool RegisterBinding(Binding binding);
    }
}