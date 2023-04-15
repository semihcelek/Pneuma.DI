namespace Pneuma.DI.Bindings
{
    public interface IBindingActivator<TBinding>
    {
        void Eager();

        void Lazy();
    }
}