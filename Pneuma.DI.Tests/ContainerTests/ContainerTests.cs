using NUnit.Framework;
using Pneuma.DI.Core;
using Pneuma.DI.Core.Extensions;
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
            _container.Bind<Foo>().AsSingle().NonLazy();
        }

        [Test]
        public void Bind_Missing_Dependency_As_Single()
        {
            Assert.Throws<BindingFailedException>(() =>
            {
                _container.Bind<Bar>().AsSingle().NonLazy();
            });
        }
        
        [Test]
        public void Bind_Multiple_Dependencies_As_Single()
        {
            _container.Bind<Foo>().AsSingle().NonLazy();
            _container.Bind<Bar>().AsSingle().NonLazy();
        }

        [Test]
        public void Bind_Same_Dependency_As_Single()
        {
            Assert.Throws<BindingFailedException>(() =>
            {
                _container.Bind<Foo>().AsSingle().NonLazy();
                _container.Bind<Foo>().AsSingle().NonLazy();
            });
        }
        
        [Test]
        public void Bind_Multiple_Dependencies_As_Transient()
        {
            _container.Bind<Foo>().AsTransient().NonLazy();
            _container.Bind<Bar>().AsTransient().NonLazy();

            Assert.AreEqual(2, _container.GetActiveObjectCount());
        }

        [Test]
        public void Bind_Same_Dependency_As_Transient()
        {
            _container.Bind<Foo>().AsTransient().NonLazy();
            _container.Bind<Foo>().AsTransient().NonLazy();
            _container.Bind<Foo>().AsTransient().NonLazy();
            _container.Bind<Foo>().AsTransient().NonLazy();
            
            Assert.AreEqual(4, _container.GetActiveObjectCount());
        }
        
        [Test]
        public void Bind_Same_Dependency_As_Transient_Lazy()
        {
            _container.Bind<Foo>().AsTransient().Lazy();
            _container.Bind<Foo>().AsTransient().Lazy();
            _container.Bind<Foo>().AsTransient().Lazy();
            _container.Bind<Foo>().AsTransient().Lazy();
            
            Assert.AreEqual(0, _container.GetActiveObjectCount());
        }

        [Test]
        public void Bind_Interface_To_Implementation()
        {
            _container.Bind<BazImplementation, IBaz>().AsSingle().NonLazy();
            
            Assert.AreEqual(1, _container.GetActiveObjectCount());
        }

        [Test]
        public void Bind_Interface_To_Implementation_Transient()
        {
            _container.Bind<BazImplementation, IBaz>().AsTransient().NonLazy();
            _container.Bind<BazImplementation, IBaz>().AsTransient().NonLazy();
            _container.Bind<BazImplementation, IBaz>().AsTransient().NonLazy();
            _container.Bind<BazImplementation, IBaz>().AsTransient().NonLazy();
            
            Assert.AreEqual(4, _container.GetActiveObjectCount());
        }

        [TearDown]
        public void TearDown()
        {
            _container.Dispose();
        }
    }
}