using System;
using Pneuma.DI.Core.BindingContexts;

namespace Pneuma.DI.Core.Bindings;

public class BindingLifeTimeBuilder<TBinding, TBuilder> : BindingInjectionBuilder<TBinding, TBuilder> where TBuilder: BindingLifeTimeBuilder<TBinding, TBuilder>
{
    private BindingLifeTime _bindingLifeTime;
    
    public BindingLifeTimeBuilder(IContainer container) : base(container)
    {
    }

    public BindingBuilder<TBinding> AsSingle()
    {
        InjectDependencies<TBinding>();

        _bindingLifeTime = BindingLifeTime.Singular;
        Binding binding = new Binding(ActivatedObject, typeof(TBinding), ActivatedObject.GetType(),
            BindingLifeTime.Singular);
        Container.RegisterBinding(binding, BindingLifeTime.Singular);
        
        return this as BindingBuilder<TBinding>;
    }

    public BindingBuilder<TBinding> AsTransient()
    {
        InjectDependencies<TBinding>();
        
        _bindingLifeTime = BindingLifeTime.Transient;
        Binding binding = new Binding(ActivatedObject, typeof(TBinding), ActivatedObject.GetType(),
            BindingLifeTime.Transient);
        Container.RegisterBinding(binding, BindingLifeTime.Transient);

        return this as BindingBuilder<TBinding>;
    }
}