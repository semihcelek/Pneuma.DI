using System;
using NUnit.Framework;
using Pneuma.DI.Core;
using Pneuma.DI.Core.BindingContexts;
using Pneuma.DI.Core.Bindings;
using Pneuma.DI.Tests.Examples;

namespace Pneuma.DI.Tests.BindingBuilderTests;

public class BindingBuilderTests
{
    private Container _container; 
    
    [SetUp]
    public void Setup()
    {
        _container = new Container();
    }
    
    [Test]
    public void BindingBuilder_Binds_Singleton()
    {
        BindingBuilder<Foo> bindingBuilder = new BindingBuilder<Foo>(_container);

        bindingBuilder.AsSingle().NonLazy();

        _container.ContainerBindingLookup(typeof(Foo), out Binding retrievedBinding);
        
        Assert.AreEqual(typeof(Foo), retrievedBinding.BindingType);
        Assert.AreEqual(typeof(Foo), retrievedBinding.Instance.GetType());
        Assert.AreEqual(typeof(Foo), retrievedBinding.InstanceType);

        Assert.AreEqual(typeof(Foo).GetHashCode(), retrievedBinding.GetHashCode());
        Assert.IsTrue(retrievedBinding.BindingLifeTime == BindingLifeTime.Singular);
    }
    
    [Test]
    public void BindingBuilder_Binds_Transient()
    {
        
        BindingBuilder<Foo> bindingBuilder = new BindingBuilder<Foo>(_container);

        bindingBuilder.AsTransient().NonLazy();

        _container.ContainerBindingLookup(typeof(Foo), out Binding retrievedBinding);

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
            BindingBuilder<Foo> bindingBuilderOne = new BindingBuilder<Foo>(_container);
            bindingBuilderOne.AsSingle().NonLazy();

            BindingBuilder<Foo> bindingBuilderTwo = new BindingBuilder<Foo>(_container);
            bindingBuilderTwo.AsSingle().NonLazy();
        });
    }
        
    [Test]
    public void BindingBuilder_Binds_Interface_As_Singleton()
    {
        BindingBuilder<IBaz> bindingBuilder = new BindingBuilder<IBaz>(_container);
        bindingBuilder.To<BazImplementation>().AsSingle().NonLazy();

        _container.ContainerBindingLookup(typeof(IBaz), out Binding retrievedBinding);
        
        Assert.AreEqual(typeof(IBaz), retrievedBinding.BindingType);
        Assert.AreEqual(typeof(BazImplementation), retrievedBinding.Instance.GetType());
        Assert.AreEqual(typeof(BazImplementation), retrievedBinding.InstanceType);

        Assert.AreEqual(typeof(IBaz).GetHashCode(), retrievedBinding.GetHashCode());
        Assert.IsTrue(retrievedBinding.BindingLifeTime == BindingLifeTime.Singular);
    }
    
    [Test]
    public void BindingBuilder_Binds_Interface_As_Transient()
    {
        var bindingBuilder = new BindingBuilder<IBaz>(_container);
        bindingBuilder.To<BazImplementation>().AsTransient().NonLazy();

        _container.ContainerBindingLookup(typeof(IBaz), out Binding retrievedBinding);
        
        Assert.AreEqual(typeof(IBaz), retrievedBinding.BindingType);
        Assert.AreEqual(typeof(BazImplementation), retrievedBinding.Instance.GetType());
        Assert.AreEqual(typeof(BazImplementation), retrievedBinding.InstanceType);
        
        Assert.AreNotEqual(typeof(IBaz).GetHashCode(), retrievedBinding.GetHashCode());
        Assert.IsTrue(retrievedBinding.BindingLifeTime == BindingLifeTime.Transient);
    }
    
    [Test]
    public void BindingBuilder_Binds_Lazy_Not_Registered_Container()
    {
        var bindingBuilder = new BindingBuilder<IBaz>(_container);
        bindingBuilder.To<BazImplementation>().AsTransient().Lazy();

        bool isRegistered = _container.ContainerBindingLookup(typeof(IBaz), out Binding _, false);
        
        Assert.IsFalse(isRegistered);
    }

    [Test]
    public void BindingBuilder_Binds_Lazy_And_Activates_When_Required()
    {
        var bindingBuilder = new BindingBuilder<Foo>(_container);
        bindingBuilder.AsTransient().Lazy();

        _container.Bind<Bar>().AsTransient().NonLazy();
        
        bool isRegistered = _container.ContainerBindingLookup(typeof(Foo), out Binding _);
        
        Assert.IsTrue(isRegistered);
    }

    [Test]
    public void BindingBuilder_Doesnt_Binds_Lazy()
    {
        var bindingBuilder = new BindingBuilder<Foo>(_container);
        bindingBuilder.AsTransient().Lazy();

        _container.Bind<Foo>().AsSingle().Lazy();
        
        bool isFooRegistered = _container.ContainerBindingLookup(typeof(Foo), out Binding _, false);
        bool isBarRegistered = _container.ContainerBindingLookup(typeof(Bar), out Binding _, false);
        
        Assert.IsFalse(isFooRegistered);
        Assert.IsFalse(isBarRegistered);
    }

    [TearDown]
    public void TearDown()
    {
        _container.Dispose();
    }
}