using System;

namespace Pneuma.DI.Core.Bindings;

public class BindingBuilder<TBinding> : BindingRegistrationTimeBuilder<TBinding, BindingBuilder<TBinding>>
{
    public BindingBuilder(IContainer container) : base(container)
    {
    }
}