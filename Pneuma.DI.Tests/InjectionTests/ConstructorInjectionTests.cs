using NUnit.Framework;
using Pneuma.DI.Core;
using Pneuma.DI.Core.Injectors;

namespace Pneuma.DI.Tests.InjectionTests
{
    public class ConstructorInjectionTests
    {
        private ConstructorInjector _constructorInjector;

        private Container _container;
        
        [SetUp]
        public void Setup()
        {
            _container = new Container();
            _constructorInjector = ConstructorInjector.Create(_container);
        }
    }
}