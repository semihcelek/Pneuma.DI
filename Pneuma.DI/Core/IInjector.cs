using Pneuma.DI.Bindings;

namespace Pneuma.DI.Core
{
    public interface IInjector
    {
        IBindingBuilder<TConcrete> Bind<TConcrete>();
    }
}