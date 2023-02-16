using Pneuma.DI.Core.BindingContexts;

namespace Pneuma.DI.Core.Binders
{
    public static class SingleBinder
    {
        public static void AsSingle(this BindingBuilder bindingBuilder)
        {
            bindingBuilder.BindingLifeTime = BindingLifeTime.Singular;
        }
    }
}