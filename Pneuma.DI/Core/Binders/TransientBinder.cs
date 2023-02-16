using Pneuma.DI.Core.BindingContexts;

namespace Pneuma.DI.Core.Binders
{
    public static class TransientBinder
    {
        public static void AsTransient(this BindingBuilder bindingBuilder)
        {
            bindingBuilder.BindingLifeTime = BindingLifeTime.Transient;
        }
    }
}