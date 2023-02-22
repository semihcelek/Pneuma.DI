using Pneuma.DI.Core.BindingContexts;

namespace Pneuma.DI.Core.Bindings;

public class BindingRegistrationTimeBuilder<TBinding, TBuilder> : BindingLifeTimeBuilder<TBinding, TBuilder>
    where TBuilder : BindingRegistrationTimeBuilder<TBinding, TBuilder>
{
    protected RegistrationTime RegistrationTime;
    
    public BindingRegistrationTimeBuilder(IContainer container) : base(container)
    {
    }

    public void Lazy()
    {
        RegistrationTime = RegistrationTime.LazyRegistration;
    }

    public void NonLazy()
    {
        RegistrationTime = RegistrationTime.InstantRegistration;
    }
}