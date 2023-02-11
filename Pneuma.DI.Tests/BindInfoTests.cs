using System;
using NUnit.Framework;
using Pneuma.DI.Core;

namespace Pneuma.DI.Tests
{
    public class BindInfoTests
    {
        [Test]
        public void BindInfo_Retrieves_Type()
        {
            Foo fooInstance = new Foo();
            BindingInfo bindingInfo = new BindingInfo(fooInstance);
            
            Assert.IsTrue(typeof(Foo) == bindingInfo.GetBindingType());
            Assert.IsAssignableFrom<Foo>(bindingInfo.GetBindingInstance());
        }
    }
}