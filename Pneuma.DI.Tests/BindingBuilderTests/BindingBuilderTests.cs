using NUnit.Framework;
using Pneuma.DI.BindingContexts;
using Pneuma.DI.Bindings;
using Pneuma.DI.Core;
using Pneuma.DI.Exception;
using Pneuma.DI.Tests.Examples;

namespace Pneuma.DI.Tests.BindingBuilderTests
{
    public class BindingBuilderTests
    {
        private DiContainer _diContainer; 
    
        [SetUp]
        public void Setup()
        {
            _diContainer = new DiContainer();
        }
    
        [Test]
        public void BindingBuilder_Binds_Singleton()
        {
            BindingBuilder<Foo> bindingBuilder = new BindingBuilder<Foo>(_diContainer);

            bindingBuilder.AsSingle().Eager();

            _diContainer.ContainerBindingLookup(typeof(Foo), out Binding retrievedBinding);
        
            Assert.AreEqual(typeof(Foo), retrievedBinding.BindingType);
            Assert.AreEqual(typeof(Foo), retrievedBinding.Instance.GetType());
            Assert.AreEqual(typeof(Foo), retrievedBinding.InstanceType);

            Assert.IsTrue(retrievedBinding.BindingLifeTime == BindingLifeTime.Singular);
        }
    
        [Test]
        public void BindingBuilder_Binds_Transient()
        {
            BindingBuilder<Foo> bindingBuilder = new BindingBuilder<Foo>(_diContainer);

            bindingBuilder.AsTransient().Eager();

            _diContainer.ContainerBindingLookup(typeof(Foo), out Binding retrievedBinding);

            Assert.AreEqual(typeof(Foo), retrievedBinding.BindingType);
            Assert.AreEqual(typeof(Foo), retrievedBinding.Instance.GetType());
            Assert.AreEqual(typeof(Foo), retrievedBinding.InstanceType);
        
            Assert.IsTrue(retrievedBinding.BindingLifeTime == BindingLifeTime.Transient);
        }
    
        [Test]
        public void Bind_Same_Dependency_As_Single()
        {
            Assert.Throws<BindingFailedException>(() =>
            {
                BindingBuilder<Foo> bindingBuilderOne = new BindingBuilder<Foo>(_diContainer);
                bindingBuilderOne.AsSingle().Eager();

                BindingBuilder<Foo> bindingBuilderTwo = new BindingBuilder<Foo>(_diContainer);
                bindingBuilderTwo.AsSingle().Eager();
            });
        }
        
        [Test]
        public void BindingBuilder_Binds_Interface_As_Singleton()
        {
            BindingBuilder<BazImplementation> bindingBuilder = new BindingBuilder<BazImplementation>(_diContainer);
            bindingBuilder.AddInterface(typeof(IBaz));
            bindingBuilder.AsSingle().Eager();

            _diContainer.ContainerBindingLookup(typeof(IBaz), out Binding retrievedBinding);
        
            Assert.AreEqual(typeof(BazImplementation), retrievedBinding.Instance.GetType());
            Assert.AreEqual(typeof(BazImplementation), retrievedBinding.InstanceType);

            Assert.IsTrue(retrievedBinding.BindingLifeTime == BindingLifeTime.Singular);
        }
    
        [Test]
        public void BindingBuilder_Binds_Interface_As_Transient()
        {
            BindingBuilder<BazImplementation> bindingBuilder = new BindingBuilder<BazImplementation>(_diContainer);
            bindingBuilder.AddInterface(typeof(IBaz));
        
            bindingBuilder.AsTransient().Eager();

            _diContainer.ContainerBindingLookup(typeof(IBaz), out Binding retrievedBinding);
        
            Assert.AreEqual(typeof(BazImplementation), retrievedBinding.Instance.GetType());
            Assert.AreEqual(typeof(BazImplementation), retrievedBinding.InstanceType);
        
            Assert.IsTrue(retrievedBinding.BindingLifeTime == BindingLifeTime.Transient);
        }
    
        [Test]
        public void BindingBuilder_Binds_Lazy_Not_Registered_Container()
        {
            BindingBuilder<BazImplementation> bindingBuilder = new BindingBuilder<BazImplementation>(_diContainer);
            bindingBuilder.AddInterface(typeof(IBaz));
            bindingBuilder.AsTransient().Lazy();

            bool isRegistered = _diContainer.ContainerBindingLookup(typeof(IBaz), out Binding _, false);
        
            Assert.IsFalse(isRegistered);
        }

        [Test]
        public void BindingBuilder_Binds_Lazy_And_Activates_When_Required()
        {
            BindingBuilder<Foo> bindingBuilder = new BindingBuilder<Foo>(_diContainer);
            bindingBuilder.AsTransient().Lazy();

            _diContainer.Bind<Bar>().AsTransient().Eager();
        
            bool isRegistered = _diContainer.ContainerBindingLookup(typeof(Foo), out Binding _);
        
            Assert.IsTrue(isRegistered);
        }

        [Test]
        public void BindingBuilder_Doesnt_Binds_Lazy()
        {
            BindingBuilder<Foo> bindingBuilder = new BindingBuilder<Foo>(_diContainer);
            bindingBuilder.AsTransient().Lazy();

            _diContainer.Bind<Foo>().AsSingle().Lazy();
        
            bool isFooRegistered = _diContainer.ContainerBindingLookup(typeof(Foo), out Binding _, false);
            bool isBarRegistered = _diContainer.ContainerBindingLookup(typeof(Bar), out Binding _, false);
        
            Assert.IsFalse(isFooRegistered);
            Assert.IsFalse(isBarRegistered);
        }

        [TearDown]
        public void TearDown()
        {
            _diContainer.Dispose();
        }
    }
}