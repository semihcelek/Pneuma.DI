namespace Pneuma.DI.Core.Binding
{
    public static class TransientBindingContext
    {
        public static void AsTransient(this BindingPrototype bindingPrototype)
        {
            object bindingPrototypeInstance = bindingPrototype.Instance;
            
            BindingInfo bindingInfo = new BindingInfo(bindingPrototypeInstance,
                bindingPrototypeInstance.GetHashCode());
            
            Container container = bindingPrototype.ContainerReference;
            container.RegisterBinding(bindingInfo);
        }
    }
}