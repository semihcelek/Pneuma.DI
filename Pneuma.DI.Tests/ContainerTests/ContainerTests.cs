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
        private DiContainer _diContainer;
        
        [SetUp]
        public void Setup()
        {
            DiContainer diContainer = new DiContainer();
            _diContainer = diContainer;
        }

        [Test]
        public void Bind_As_Single()
        {
            _diContainer.Bind<Foo>().AsSingle().NonLazy();
        }

        [Test]
        public void Bind_Missing_Dependency_As_Single()
        {
            Assert.Throws<BindingFailedException>(() =>
            {
                _diContainer.Bind<Bar>().AsSingle().NonLazy();
            });
        }
        
        [Test]
        public void Bind_Multiple_Dependencies_As_Single()
        {
            _diContainer.Bind<Foo>().AsSingle().NonLazy();
            _diContainer.Bind<Bar>().AsSingle().NonLazy();
        }

        [Test]
        public void Bind_Same_Dependency_As_Single()
        {
            Assert.Throws<BindingFailedException>(() =>
            {
                _diContainer.Bind<Foo>().AsSingle().NonLazy();
                _diContainer.Bind<Foo>().AsSingle().NonLazy();
            });
        }
        
        [Test]
        public void Bind_Multiple_Dependencies_As_Transient()
        {
            _diContainer.Bind<Foo>().AsTransient().NonLazy();
            _diContainer.Bind<Bar>().AsTransient().NonLazy();

            Assert.AreEqual(2, _diContainer.GetActiveObjectCount());
        }

        [Test]
        public void Bind_Same_Dependency_As_Transient()
        {
            _diContainer.Bind<Foo>().AsTransient().NonLazy();
            _diContainer.Bind<Foo>().AsTransient().NonLazy();
            _diContainer.Bind<Foo>().AsTransient().NonLazy();
            _diContainer.Bind<Foo>().AsTransient().NonLazy();
            
            Assert.AreEqual(4, _diContainer.GetActiveObjectCount());
        }
        
        [Test]
        public void Bind_Same_Dependency_As_Transient_Lazy()
        {
            _diContainer.Bind<Foo>().AsTransient().Lazy();
            _diContainer.Bind<Foo>().AsTransient().Lazy();
            _diContainer.Bind<Foo>().AsTransient().Lazy();
            _diContainer.Bind<Foo>().AsTransient().Lazy();
            
            Assert.AreEqual(0, _diContainer.GetActiveObjectCount());
        }

        [Test]
        public void Bind_Interface_To_Implementation()
        {
            _diContainer.Bind<BazImplementation, IBaz>().AsSingle().NonLazy();
            
            Assert.AreEqual(1, _diContainer.GetActiveObjectCount());
        }

        [Test]
        public void Bind_Interface_To_Implementation_Transient()
        {
            _diContainer.Bind<BazImplementation, IBaz>().AsTransient().NonLazy();
            _diContainer.Bind<BazImplementation, IBaz>().AsTransient().NonLazy();
            _diContainer.Bind<BazImplementation, IBaz>().AsTransient().NonLazy();
            _diContainer.Bind<BazImplementation, IBaz>().AsTransient().NonLazy();
            
            Assert.AreEqual(4, _diContainer.GetActiveObjectCount());
        }

        [TearDown]
        public void TearDown()
        {
            _diContainer.Dispose();
        }
    }
}