using System;

namespace Pneuma.DI.Core.Bindings;

public interface IBindingBuilder<TBinding> : ILifeTimeBuilder<TBinding>, IBindingActivator<TBinding>,
    IAbstractBinder<TBinding>, IBindingBuilder
{
     void AddInterface(Type bindingInterface);
}

public interface IBindingBuilder : IEquatable<IBindingBuilder>
{
    Binding BuildBinding();
    
    Type BuildingType { get; }
}