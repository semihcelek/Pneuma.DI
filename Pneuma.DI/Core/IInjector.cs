using Pneuma.DI.Core.Bindings;

namespace Pneuma.DI.Core
{
    public interface IInjector
    {
        IBindingBuilder<TConcrete> Bind<TConcrete>();
        
        // IBindingBuilder<T> BindInterface<T>();
    }
}