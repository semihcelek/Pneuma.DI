using Pneuma.DI.Bindings;
using Pneuma.DI.Core;

namespace Pneuma.DI.Extensions
{
    public static class ContainerExtensions
    {
        public static IBindingBuilder<TConcrete> Bind<TConcrete, TAbstract>(this DiContainer diContainer)
            where TConcrete : TAbstract
        {
            IBindingBuilder<TConcrete> bindingBuilder = new BindingBuilder<TConcrete>(diContainer);

            bindingBuilder.AddInterface(typeof(TAbstract));

            return bindingBuilder;
        }

        public static IBindingBuilder<TConcrete> Bind<TConcrete, TAbstract1, TAbstract2>(this DiContainer diContainer)
            where TConcrete : TAbstract1, TAbstract2
        {
            IBindingBuilder<TConcrete> bindingBuilder = new BindingBuilder<TConcrete>(diContainer);

            bindingBuilder.AddInterface(typeof(TAbstract1));
            bindingBuilder.AddInterface(typeof(TAbstract2));
        
            return bindingBuilder;
        }
    }
}