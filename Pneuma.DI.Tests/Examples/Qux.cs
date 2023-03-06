namespace Pneuma.DI.Tests.Examples
{
    public class Qux
    {
        private IBaz _baz;

        public Qux(IBaz baz)
        {
            _baz = baz;
        }
    }
}