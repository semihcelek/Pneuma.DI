namespace Pneuma.DI.Core.Binding
{
    public interface IBinderContext
    {
        BindingPrototype Bind<T>();
    }
}