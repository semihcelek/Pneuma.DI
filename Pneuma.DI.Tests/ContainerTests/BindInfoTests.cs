using NUnit.Framework;
using Pneuma.DI.Core;
using Pneuma.DI.Tests.Examples;

namespace Pneuma.DI.Tests.ContainerTests
{
    public class BindInfoTests
    {
        [Test]
        public void Binding_Retrieves_Type()
        {
            Foo fooInstance = new Foo();
            Binding binding = new Binding(fooInstance, fooInstance.GetType());
            
            Assert.IsTrue(typeof(Foo) == binding.GetBindingType());
            Assert.IsAssignableFrom<Foo>(binding.GetBindingInstance());
        }

        [Test]
        public void Binding_HashCode_Same_As_Injected_Objects_HashCode()
        {
            Foo fooInstance = new Foo();
            Binding binding = new Binding(fooInstance, fooInstance.GetType());

            Assert.AreEqual(fooInstance.GetHashCode(), binding.GetHashCode());
        }
    }
}