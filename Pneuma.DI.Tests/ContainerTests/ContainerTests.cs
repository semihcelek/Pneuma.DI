using NUnit.Framework;
using Pneuma.DI.Core;
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
        public void Bind_Dependency_To_Container()
        {
            _container.BindSingle<Foo>();
        }

        [Test]
        public void Bind_Missing_Dependency()
        {
            Assert.Throws<BindingFailedException>((() =>
            {
                _container.BindSingle<Bar>();
            }));
        }
        
        [Test]
        public void Bind_Multiple_Dependencies_To_Container()
        {
            _container.BindSingle<Foo>();
            _container.BindSingle<Bar>();
        }
    }
}