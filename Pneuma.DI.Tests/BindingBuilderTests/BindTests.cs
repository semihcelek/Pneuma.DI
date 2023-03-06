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
        
        [Test]
        public void Binding_Retrieves_Binded_Interface_Types()
        {
            IBaz bazImplementation = new BazImplementation();
            Type bindingType = bazImplementation.GetType().GetInterface(nameof(IBaz));

            Type[] interfaces = new[] { typeof(IBaz) };
            
            Binding binding = new Binding(bazImplementation, bindingType, bazImplementation.GetType(), BindingLifeTime.Singular,
                interfaces);
            
            Assert.IsTrue(typeof(IBaz) == binding.BindingType);
        }
    }
}