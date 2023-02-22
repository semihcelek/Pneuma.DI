using System;
using Pneuma.DI.Core.BindingContexts;

namespace Pneuma.DI.Core.Bindings;

public class BindingLifeTimeBuilder<TBinding, TBuilder> : BindingInjectionBuilder<TBinding, TBuilder>
    where TBuilder : BindingLifeTimeBuilder<TBinding, TBuilder>
{
    protected BindingLifeTime BindingLifeTime;
    
    public BindingLifeTimeBuilder(IContainer container) : base(container)
    {
    }

    public BindingBuilder<TBinding> AsSingle()
    {
        InjectDependencies<TBinding>();

        BindingLifeTime = BindingLifeTime.Singular;
        Binding binding = new Binding(ActivatedObject, typeof(TBinding), ActivatedObject.GetType(),
            BindingLifeTime.Singular);
        Container.RegisterBinding(binding, BindingLifeTime.Singular);
        
        return this as BindingBuilder<TBinding>;
    }

    public BindingBuilder<TBinding> AsTransient()
    {
        InjectDependencies<TBinding>();
        
        BindingLifeTime = BindingLifeTime.Transient;
        Binding binding = new Binding(ActivatedObject, typeof(TBinding), ActivatedObject.GetType(),
            BindingLifeTime.Transient);
        Container.RegisterBinding(binding, BindingLifeTime.Transient);

        return this as BindingBuilder<TBinding>;
    }
}