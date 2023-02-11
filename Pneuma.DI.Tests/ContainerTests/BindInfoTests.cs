﻿using NUnit.Framework;
using Pneuma.DI.Core;
using Pneuma.DI.Tests.Examples;

namespace Pneuma.DI.Tests.ContainerTests
{
    public class BindInfoTests
    {
        [Test]
        public void BindingInfo_Retrieves_Type()
        {
            Foo fooInstance = new Foo();
            BindingInfo bindingInfo = new BindingInfo(fooInstance);
            
            Assert.IsTrue(typeof(Foo) == bindingInfo.GetBindingType());
            Assert.IsAssignableFrom<Foo>(bindingInfo.GetBindingInstance());
        }

        [Test]
        public void BindingInfo_HashCode_Same_As_Injected_Object_HashCode()
        {
            Foo fooInstance = new Foo();
            BindingInfo bindingInfo = new BindingInfo(fooInstance);

            Assert.AreEqual(fooInstance.GetHashCode(), bindingInfo.GetHashCode());
        }
    }
}