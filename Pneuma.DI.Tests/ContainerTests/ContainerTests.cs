using NUnit.Framework;
using Pneuma.DI.Core;
using Pneuma.DI.Core.Binding;
using Pneuma.DI.Exception;
using Pneuma.DI.Tests.Examples;

namespace Pneuma.DI.Tests.ContainerTests
{
    [TestFixture]
    public class ContainerTests
    {
        private Container _container;
        
        [SetUp]
        public void Setup()
        {
            Container diContainer = new Container();
            _container = diContainer;
        }

        [Test]
        public void Bind_As_Single()
        {
            _container.Bind<Foo>().AsSingle();
        }

        [Test]
        public void Bind_Missing_Dependency_As_Single()
        {
            Assert.Throws<BindingFailedException>(() =>
            {
                _container.Bind<Bar>().AsSingle();
            });
        }
        
        [Test]
        public void Bind_Multiple_Dependencies_As_Single()
        {
            _container.Bind<Foo>().AsSingle();
            _container.Bind<Bar>().AsSingle();
        }
    }
}