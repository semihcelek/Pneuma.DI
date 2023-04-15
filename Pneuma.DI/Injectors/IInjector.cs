namespace Pneuma.DI.Injectors
{
    public interface IInjector
    {
        void Inject<T>(ref T injectedObject);
    }
}