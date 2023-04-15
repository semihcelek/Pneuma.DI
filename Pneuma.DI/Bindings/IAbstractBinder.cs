namespace Pneuma.DI.Bindings
{
    public interface IAbstractBinder<TBinding>
    {
        IBindingBuilder<TBinding> To<TConcrete>() where TConcrete : TBinding;
    }
}