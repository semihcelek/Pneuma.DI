﻿namespace Pneuma.DI.Tests
{
    public class Bar
    {
        private Foo _foo;

        public Bar(Foo foo)
        {
            _foo = foo;
        }
    }
}