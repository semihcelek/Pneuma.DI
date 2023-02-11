namespace Pneuma.DI.Exception
{
    public class BindingFailedException : PneumaException
    {
        public BindingFailedException() { }

        public BindingFailedException(string message) : base(message) { }
    }
}