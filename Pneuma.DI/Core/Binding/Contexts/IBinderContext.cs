namespace Pneuma.DI.Core.Binding.Contexts
{
    public interface IBinderContext
    {
        BindingPrototype Bind<T>();
    }
}