using Pneuma.DI.Core;
using Pneuma.DI.Extensions;
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
            DiContainer diContainer = new DiContainer();

            diContainer.Bind<Foo>().AsTransient().Lazy();
            diContainer.Bind<Bar>().AsTransient().Lazy();
            diContainer.Bind<BazImplementation, IBaz>().AsTransient().Eager();
            diContainer.Bind<Qux>().AsTransient().Lazy();
            diContainer.Bind<Smi>().AsTransient().Lazy();

            diContainer.Bind<BazController>().AsSingle().Eager();
        }
    }
}