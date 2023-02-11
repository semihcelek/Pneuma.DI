namespace Pneuma.DI.Exception
{
    public class SanityCheckFailedException : PneumaException
    {
        public SanityCheckFailedException()
        {
        }

        public SanityCheckFailedException(string message) : base(message)
        {
        }
    }
}