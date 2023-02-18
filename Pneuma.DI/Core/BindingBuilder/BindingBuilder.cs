using System;

namespace Pneuma.DI.Core.BindingBuilder;

public class BindingBuilder: BindingLifeTimeBuilder<BindingBuilder>
{
    public BindingBuilder(IContainer container, Type buildingType) : base(container, buildingType)
    {
    }
}