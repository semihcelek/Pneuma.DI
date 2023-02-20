using Pneuma.DI.Core;
using Pneuma.DI.Tests.Examples;

namespace Pneuma.Sandbox
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Container diContainer = new Container();

            diContainer.Bind<Foo>().AsTransient();
            diContainer.Bind<Bar>().AsTransient();
        }
    }
}