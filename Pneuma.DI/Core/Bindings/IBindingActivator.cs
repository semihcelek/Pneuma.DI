namespace Pneuma.DI.Core.Bindings
{
    public interface IBindingActivator<TBinding>
    {
        void Eager();

        void Lazy();
    }
}