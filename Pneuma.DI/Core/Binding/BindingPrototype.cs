namespace Pneuma.DI.Core.Binding
{
    public readonly struct BindingPrototype : IContainerReference
    {
        public readonly object Instance;

        public Container ContainerReference => Container;
        private readonly Container Container;
        
        public BindingPrototype(object instance, Container container)
        {
            Instance = instance;
            Container = container;
        }

    }
}