using NUnit.Framework;
using Pneuma.DI.Core;
using Pneuma.DI.Tests.Examples;

namespace Pneuma.DI.Tests.InjectionTests
{
    public class AttributeInjectionTests
    {
        private Container _container;
    
        [SetUp]
        public void Setup()
        {
            Container diContainer = new Container();
            _container = diContainer;
        }

        [Test]
        public void AttributeInjector_Process_Inject_On_Fields()
        {
            _container.Bind<Foo>().AsSingle().Lazy();
            _container.Bind<Smi>().AsSingle().NonLazy();
        }
    }
}