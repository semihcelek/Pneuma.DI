using NUnit.Framework;
using Pneuma.DI.Core;
using Pneuma.DI.Tests.Examples;

namespace Pneuma.DI.Tests.InjectionTests
{
    public class AttributeInjectionTests
    {
        private DiContainer _diContainer;
    
        [SetUp]
        public void Setup()
        {
            DiContainer diContainer = new DiContainer();
            _diContainer = diContainer;
        }

        [Test]
        public void AttributeInjector_Process_Inject_On_Fields()
        {
            _diContainer.Bind<Foo>().AsSingle().Lazy();
            _diContainer.Bind<Smi>().AsSingle().NonLazy();
        }
    }
}