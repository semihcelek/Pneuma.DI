using Pneuma.DI.Core.Bindings;

namespace Pneuma.DI.Core
{
    public interface IInjector
    {
        IBindingBuilder<T> Bind<T>();
        
        IBindingBuilder<T> BindInterface<T>();
    }
}