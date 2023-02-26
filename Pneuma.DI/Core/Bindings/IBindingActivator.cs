namespace Pneuma.DI.Core.Bindings;

public interface IBindingActivator<TBinding>
{
    void NonLazy();

    void Lazy();
}