using System;

namespace Pneuma.DI.Tests.Examples
{
    public class Bar
    {
        private Foo _foo;

        public Bar(Foo foo)
        {
            _foo = foo;
            
            DoStuffWithFoo();
        }

        public void DoStuffWithFoo()
        {
            Console.WriteLine(_foo.MyInt);
        }
    }
}