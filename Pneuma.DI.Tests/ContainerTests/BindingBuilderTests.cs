using System;
using NUnit.Framework;
using Pneuma.DI.Core.BindingContexts;
using Pneuma.DI.Core.Bindings;
using Pneuma.DI.Tests.Examples;

namespace Pneuma.DI.Tests.ContainerTests;

public class BindingBuilderTests
{
    private MockContainer _mockContainer; 
    
    [SetUp]
    public void Setup()
    {
        _mockContainer = new MockContainer();
    }
    
    [Test]
    public void BindingBuilder_Binds_Singleton()
    {
        BindingBuilder bindingBuilder = new BindingBuilder(_mockContainer, typeof(Foo));

        bindingBuilder.AsSingle();

        Assert.AreEqual(typeof(Foo), _mockContainer.RegisteredBinding.BindingType);
        Assert.AreEqual(typeof(Foo), _mockContainer.RegisteredBinding.Instance.GetType());
        Assert.AreEqual(typeof(Foo), _mockContainer.RegisteredBinding.InstanceType);

        Assert.AreEqual(typeof(Foo).GetHashCode(), _mockContainer.RegisteredBinding.GetHashCode());
        Assert.IsTrue(_mockContainer.RegisteredBinding.BindingLifeTime == BindingLifeTime.Singular);
    }
    
    [Test]
    public void BindingBuilder_Binds_Transient()
    {
        BindingBuilder bindingBuilder = new BindingBuilder(_mockContainer, typeof(Foo));

        bindingBuilder.AsTransient();

        Assert.AreEqual(typeof(Foo), _mockContainer.RegisteredBinding.BindingType);
        Assert.AreEqual(typeof(Foo), _mockContainer.RegisteredBinding.Instance.GetType());
        Assert.AreEqual(typeof(Foo), _mockContainer.RegisteredBinding.InstanceType);
        
        Assert.AreNotEqual(typeof(Foo).GetHashCode(), _mockContainer.RegisteredBinding.GetHashCode());
        Assert.IsTrue(_mockContainer.RegisteredBinding.BindingLifeTime == BindingLifeTime.Transient);
    }
    
    [Test]
    public void Bind_Same_Dependency_As_Single()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            BindingBuilder bindingBuilderOne = new BindingBuilder(_mockContainer, typeof(Foo));
            bindingBuilderOne.AsSingle();

            BindingBuilder bindingBuilderTwo = new BindingBuilder(_mockContainer, typeof(Foo));
            bindingBuilderTwo.AsSingle();
        });
    }
        
    [Test]
    public void BindingBuilder_Binds_Interface_As_Singleton()
    {
        BindingBuilder bindingBuilder = new BindingBuilder(_mockContainer, typeof(IBaz));
        bindingBuilder.To<BazImplementation>().AsSingle();

        Assert.AreEqual(typeof(IBaz), _mockContainer.RegisteredBinding.BindingType);
        Assert.AreEqual(typeof(BazImplementation), _mockContainer.RegisteredBinding.Instance.GetType());
        Assert.AreEqual(typeof(BazImplementation), _mockContainer.RegisteredBinding.InstanceType);

        Assert.AreEqual(typeof(IBaz).GetHashCode(), _mockContainer.RegisteredBinding.GetHashCode());
        Assert.IsTrue(_mockContainer.RegisteredBinding.BindingLifeTime == BindingLifeTime.Singular);
    }
    
    [Test]
    public void BindingBuilder_Binds_Interface_As_Transient()
    {
        BindingBuilder bindingBuilder = new BindingBuilder(_mockContainer, typeof(IBaz));
        bindingBuilder.To<BazImplementation>().AsTransient();

        Assert.AreEqual(typeof(IBaz), _mockContainer.RegisteredBinding.BindingType);
        Assert.AreEqual(typeof(BazImplementation), _mockContainer.RegisteredBinding.Instance.GetType());
        Assert.AreEqual(typeof(BazImplementation), _mockContainer.RegisteredBinding.InstanceType);

        Assert.AreNotEqual(typeof(IBaz).GetHashCode(), _mockContainer.RegisteredBinding.GetHashCode());
        Assert.IsTrue(_mockContainer.RegisteredBinding.BindingLifeTime == BindingLifeTime.Transient);
    }

}