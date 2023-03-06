using Pneuma.DI.Core.Bindings;

namespace Pneuma.DI.Core.Extensions
{
    public static class ContainerExtensions
    {
        public static IBindingBuilder<TConcrete> Bind<TConcrete, TAbstract>(this Container container)
            where TConcrete : TAbstract
        {
            container.SanityCheck();

            IBindingBuilder<TConcrete> bindingBuilder = new BindingBuilder<TConcrete>(container);

            bindingBuilder.AddInterface(typeof(TAbstract));

            return bindingBuilder;
        }

        public static IBindingBuilder<TConcrete> Bind<TConcrete, TAbstract1, TAbstract2>(this Container container)
            where TConcrete : TAbstract1, TAbstract2
        {
            container.SanityCheck();

            IBindingBuilder<TConcrete> bindingBuilder = new BindingBuilder<TConcrete>(container);

            bindingBuilder.AddInterface(typeof(TAbstract1));
            bindingBuilder.AddInterface(typeof(TAbstract2));
        
            return bindingBuilder;
        }
    }
}