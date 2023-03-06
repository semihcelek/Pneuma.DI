using Pneuma.DI.Core.Bindings;

namespace Pneuma.DI.Core.Extensions;

public static class ContainerExtensions
{
    public static IBindingBuilder<TConcrete> Bind<TConcrete, TAbstract>(this Container container)
        where TConcrete : TAbstract
    {
        return container.Bind<TConcrete>();
    }

    public static IBindingBuilder<TConcrete> Bind<TConcrete, TAbstract1, TAbstract2>(this Container container)
        where TConcrete : TAbstract1, TAbstract2
    {
        return container.Bind<TConcrete>();
    }
}