using NUnit.Framework;
using Pneuma.DI.Core;
using Pneuma.DI.Injectors;

namespace Pneuma.DI.Tests.InjectionTests
{
    public class ConstructorInjectionTests
    {
        private ConstructorInjector _constructorInjector;

        private DiContainer _diContainer;
        
        [SetUp]
        public void Setup()
        {
            _diContainer = new DiContainer();
            _constructorInjector = ConstructorInjector.Create(_diContainer);
        }
    }
}