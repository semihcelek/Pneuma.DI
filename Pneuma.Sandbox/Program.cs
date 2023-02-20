using Pneuma.DI.Core;
using Pneuma.DI.Tests.Examples;
using Pneuma.Sandbox.BazModule.Controller;

namespace Pneuma.Sandbox
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ComposeObjectGraph();
        }

        private static void ComposeObjectGraph()
        {
            Container diContainer = new Container();

            diContainer.Bind<Foo>().AsTransient();
            diContainer.Bind<IBaz>().To<BazImplementation>().AsSingle();
            diContainer.Bind<Qux>().AsTransient();

            diContainer.Bind<BazController>().AsSingle();
        }
    }
}