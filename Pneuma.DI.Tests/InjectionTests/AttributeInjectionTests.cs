using NUnit.Framework;
using Pneuma.DI.Core;

namespace Pneuma.DI.Tests.InjectionTests;

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
        
    }
}