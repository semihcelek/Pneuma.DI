namespace Pneuma.DI.Bindings
{
    public interface ILifeTimeBuilder<TBinding>
    {
        IBindingActivator<TBinding> AsSingle();

        IBindingActivator<TBinding> AsTransient();
    }
}