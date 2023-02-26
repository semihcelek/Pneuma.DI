namespace Pneuma.DI.Core.Bindings;

public interface ILifeTimeBuilder<TBinding>
{
    IBindingActivator<TBinding> AsSingle();

    IBindingActivator<TBinding> AsTransient();
}