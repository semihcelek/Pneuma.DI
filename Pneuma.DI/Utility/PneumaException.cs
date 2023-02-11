using System;

namespace Pneuma.DI.Utility
{
    public class PneumaException : Exception
    {
        public PneumaException() { }

        public PneumaException(string message) : base(message) { }
    }
}