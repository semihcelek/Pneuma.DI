namespace Pneuma.DI.Exception
{
    public class PneumaException : System.Exception
    {
        public PneumaException() { }

        public PneumaException(string message) : base(message) { }
    }
}