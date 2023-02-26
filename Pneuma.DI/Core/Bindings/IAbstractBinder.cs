namespace Pneuma.DI.Core.Bindings;

public interface IAbstractBinder<TBinding>
{
    IBindingBuilder<TBinding> To<TConcrete>() where TConcrete : TBinding;
}