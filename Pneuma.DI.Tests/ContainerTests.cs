using NUnit.Framework;
using Pneuma.DI.Core;

namespace Pneuma.DI.Tests
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
        public void Bind_Multiple_Dependencies_To_Container()
        {
            _container.BindSingle<Foo>();
            _container.BindSingle<Bar>();
        }
    }
}