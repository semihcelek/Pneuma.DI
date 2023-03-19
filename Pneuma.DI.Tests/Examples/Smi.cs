using Pneuma.DI.Core.Attributes;

namespace Pneuma.DI.Tests.Examples
{
    public class Smi
    {
        public IBaz Baz;
        
        [Inject]
        public Foo Foo { get; private set; }

        [Inject] 
        public Bar Bar;

        [Inject]
        public void Construct(IBaz baz)
        {
            Baz = baz;
        }
    }
}