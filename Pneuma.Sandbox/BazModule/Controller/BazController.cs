using System;
using Pneuma.DI.Tests.Examples;

namespace Pneuma.Sandbox.BazModule.Controller
{
    public class BazController
    {
        private readonly IBaz _baz;

        private readonly Qux _qux;

        private readonly Foo _foo;

        private readonly Smi _smi;

        public BazController(IBaz baz, Qux qux, Foo foo, Smi smi)
        {
            _baz = baz;
            _qux = qux;
            _foo = foo;
            _smi = smi;

            DoStuff();
        }

        private void DoStuff()
        {
            Console.WriteLine(_baz.Fizz);
            Console.WriteLine(_foo.MyInt);
            Console.WriteLine(_smi.Foo.MyInt);
            _smi.Bar.DoStuffWithFoo();
            Console.WriteLine(_smi.Baz.Fizz);
        }
    }
}