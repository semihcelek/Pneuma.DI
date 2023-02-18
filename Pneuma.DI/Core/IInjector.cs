using Pneuma.DI.Core.Bindings;

namespace Pneuma.DI.Core
{
    public interface IInjector
    {
        BindingBuilder Bind<T>();
    }
}