using System;
using NUnit.Framework;
using Pneuma.DI.Core;
using Pneuma.DI.Core.BindingContexts;
using Pneuma.DI.Core.Bindings;
using Pneuma.DI.Tests.Examples;

namespace Pneuma.DI.Tests.BindingBuilderTests;

public class BindingBuilderTests
{
    private IContainer _mockContainer; 
    
    [SetUp]
    public void Setup()
    {
        _mockContainer = new Container();
    }
    
    [Test]
    public void BindingBuilder_Binds_Singleton()
    {
        BindingBuilder<Foo> bindingBuilder = new BindingBuilder<Foo>(_mockContainer);

        bindingBuilder.AsSingle().NonLazy();

        _mockContainer.ContainerBindingLookup(typeof(Foo), out Binding retrievedBinding);
        
        Assert.AreEqual(typeof(Foo), retrievedBinding.BindingType);
        Assert.AreEqual(typeof(Foo), retrievedBinding.Instance.GetType());
        Assert.AreEqual(typeof(Foo), retrievedBinding.InstanceType);

        Assert.AreEqual(typeof(Foo).GetHashCode(), retrievedBinding.GetHashCode());
        Assert.IsTrue(retrievedBinding.BindingLifeTime == BindingLifeTime.Singular);
    }
    
    [Test]
    public void BindingBuilder_Binds_Transient()
    {
        
        BindingBuilder<Foo> bindingBuilder = new BindingBuilder<Foo>(_mockContainer);

        bindingBuilder.AsTransient().NonLazy();

        _mockContainer.ContainerBindingLookup(typeof(Foo), out Binding retrievedBinding);

        Assert.AreEqual(typeof(Foo), retrievedBinding.BindingType);
        Assert.AreEqual(typeof(Foo), retrievedBinding.Instance.GetType());
        Assert.AreEqual(typeof(Foo), retrievedBinding.InstanceType);
        
        Assert.AreNotEqual(typeof(Foo).GetHashCode(), retrievedBinding.GetHashCode());
        Assert.IsTrue(retrievedBinding.BindingLifeTime == BindingLifeTime.Transient);
    }
    
    [Test]
    public void Bind_Same_Dependency_As_Single()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            BindingBuilder<Foo> bindingBuilderOne = new BindingBuilder<Foo>(_mockContainer);
            bindingBuilderOne.AsSingle().NonLazy();

            BindingBuilder<Foo> bindingBuilderTwo = new BindingBuilder<Foo>(_mockContainer);
            bindingBuilderTwo.AsSingle().NonLazy();
        });
    }
        
    [Test]
    public void BindingBuilder_Binds_Interface_As_Singleton()
    {
        BindingBuilder<IBaz> bindingBuilder = new BindingBuilder<IBaz>(_mockContainer);
        bindingBuilder.To<BazImplementation>().AsSingle().NonLazy();

        _mockContainer.ContainerBindingLookup(typeof(IBaz), out Binding retrievedBinding);
        
        Assert.AreEqual(typeof(IBaz), retrievedBinding.BindingType);
        Assert.AreEqual(typeof(BazImplementation), retrievedBinding.Instance.GetType());
        Assert.AreEqual(typeof(BazImplementation), retrievedBinding.InstanceType);

        Assert.AreEqual(typeof(IBaz).GetHashCode(), retrievedBinding.GetHashCode());
        Assert.IsTrue(retrievedBinding.BindingLifeTime == BindingLifeTime.Singular);
    }
    
    [Test]
    public void BindingBuilder_Binds_Interface_As_Transient()
    {
        var bindingBuilder = new BindingBuilder<IBaz>(_mockContainer);
        bindingBuilder.To<BazImplementation>().AsTransient().NonLazy();

        _mockContainer.ContainerBindingLookup(typeof(IBaz), out Binding retrievedBinding);
        
        Assert.AreEqual(typeof(IBaz), retrievedBinding.BindingType);
        Assert.AreEqual(typeof(BazImplementation), retrievedBinding.Instance.GetType());
        Assert.AreEqual(typeof(BazImplementation), retrievedBinding.InstanceType);
        
        Assert.AreNotEqual(typeof(IBaz).GetHashCode(), retrievedBinding.GetHashCode());
        Assert.IsTrue(retrievedBinding.BindingLifeTime == BindingLifeTime.Transient);
    }
    
    [Test]
    public void BindingBuilder_Binds_Lazy()
    {
        var bindingBuilder = new BindingBuilder<IBaz>(_mockContainer);
        bindingBuilder.To<BazImplementation>().AsTransient().NonLazy();

        _mockContainer.ContainerBindingLookup(typeof(IBaz), out Binding retrievedBinding);
        
        Assert.AreEqual(typeof(IBaz), retrievedBinding.BindingType);
        Assert.AreEqual(typeof(BazImplementation), retrievedBinding.Instance.GetType());
        Assert.AreEqual(typeof(BazImplementation), retrievedBinding.InstanceType);
        
        Assert.AreNotEqual(typeof(IBaz).GetHashCode(), retrievedBinding.GetHashCode());
        Assert.IsTrue(retrievedBinding.BindingLifeTime == BindingLifeTime.Transient);
    }

    [TearDown]
    public void TearDown()
    {
        _mockContainer.Dispose();
    }
}