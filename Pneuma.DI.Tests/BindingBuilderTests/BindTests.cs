using System;
using NUnit.Framework;
using Pneuma.DI.Core.BindingContexts;
using Pneuma.DI.Core.Bindings;
using Pneuma.DI.Tests.Examples;

namespace Pneuma.DI.Tests.BindingBuilderTests
{
    public class BindInfoTests
    {
        [Test]
        public void Binding_Retrieves_Type()
        {
            Foo fooInstance = new Foo();
            Type bindingType = fooInstance.GetType();
            Binding binding = new Binding(fooInstance, bindingType, bindingType, BindingLifeTime.Singular,
                Array.Empty<Type>());
            
            Assert.IsTrue(typeof(Foo) == binding.BindingType);
            Assert.IsAssignableFrom<Foo>(binding.Instance);
        }
    }
}