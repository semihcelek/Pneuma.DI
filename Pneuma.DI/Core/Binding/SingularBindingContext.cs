namespace Pneuma.DI.Core.Binding
{
    public static class SingularBindingContext
    {
        public static void AsSingle(this BindingPrototype bindingPrototype)
        {
            object bindingPrototypeInstance = bindingPrototype.Instance;
            
            BindingInfo bindingInfo = new BindingInfo(bindingPrototypeInstance,
                bindingPrototypeInstance.GetType().GetHashCode());
            
            Container container = bindingPrototype.ContainerReference;
            container.RegisterBinding(bindingInfo);
        }
    }
}