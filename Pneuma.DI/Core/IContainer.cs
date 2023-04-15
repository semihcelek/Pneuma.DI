using System;
using Pneuma.DI.Bindings;

namespace Pneuma.DI.Core
{
    public interface IContainer : IDisposable
    {
        bool ContainerBindingLookup(Type lookupType, out Binding binding, bool bindAvailableLazyBindings = true);

        bool RegisterBinding(Binding binding);

        bool RegisterLazyBinding<TBinding>(BindingBuilder<TBinding> bindingBuilder);
    }
}