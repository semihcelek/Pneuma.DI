using Pneuma.DI.Core;
using Pneuma.DI.Core.Bindings;
using Pneuma.DI.Tests.Examples;

namespace Pneuma.Sandbox
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Container diContainer = new Container();

            BindingBuilder bindingBuilder = new BindingBuilder(diContainer, typeof(Foo)).AsSingle();
        }
    }
}