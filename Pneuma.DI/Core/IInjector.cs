using Pneuma.DI.Core.Bindings;

namespace Pneuma.DI.Core
{
    public interface IInjector
    {
        BindingBuilder<T> Bind<T>();
        BindingBuilder<T> BindInterface<T>();
    }
}