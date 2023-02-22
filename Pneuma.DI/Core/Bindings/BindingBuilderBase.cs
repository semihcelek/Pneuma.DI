using System;

namespace Pneuma.DI.Core.Bindings;

public abstract class BindingBuilderBase
{
    protected IContainer Container;

    protected BindingBuilderBase(IContainer container)
    {
        Container = container;
    }
}