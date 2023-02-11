namespace Pneuma.DI.Utility
{
    public class BindingFailedException : PneumaException
    {
        public BindingFailedException() { }

        public BindingFailedException(string message) : base(message) { }
    }
}