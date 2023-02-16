namespace Pneuma.DI.Core
{
    public interface IInjector
    {
        BindingBuilder Bind<T>();
    }
}