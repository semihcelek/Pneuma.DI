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

            diContainer.Bind<Foo>().AsTransient().Lazy();
            diContainer.Bind<IBaz>().To<BazImplementation>().AsSingle().Lazy();
            diContainer.Bind<Qux>().AsTransient().Lazy();

            diContainer.Bind<BazController>().AsSingle().NonLazy();
            
            // diContainer.Bind<Foo>().AsTransient().Lazy();
            // diContainer.Bind<Foo>().AsTransient().Lazy();
            // diContainer.Bind<Foo>().AsTransient().Lazy();
            // diContainer.Bind<Foo>().AsTransient().Lazy();
        }
    }
}