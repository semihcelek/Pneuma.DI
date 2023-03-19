using Pneuma.DI.Core.Attributes;

namespace Pneuma.DI.Tests.Examples
{
    public class Smi
    {
        [Inject]
        public Foo Foo { get; private set; }

        [Inject] 
        public Bar Bar;
    }
}