using System;

namespace Pneuma.DI.Core.Bindings;

public interface IBindingBuilder<TBinding> : ILifeTimeBuilder<TBinding>, IBindingActivator<TBinding>,
    IAbstractBinder<TBinding>, IBindingBuilder
{
}

public interface IBindingBuilder
{
    Binding BuildBinding();
}