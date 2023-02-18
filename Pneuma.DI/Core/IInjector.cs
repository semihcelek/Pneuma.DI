namespace Pneuma.DI.Core
{
    public interface IInjector
    {
        BindingBuilder.BindingBuilder Bind<T>();
    }
}