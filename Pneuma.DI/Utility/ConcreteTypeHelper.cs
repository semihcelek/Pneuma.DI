using System;
using Pneuma.DI.Exception;

namespace Pneuma.DI.Utility;

public static class ConcreteTypeHelper
{
    public static Type RetrieveConcreteType<TBinding, TConcrete>() where TConcrete : TBinding
    {
        Type buildingType = typeof(TBinding);

        if (buildingType.IsAbstract || buildingType.IsInterface)
        {
            return typeof(TConcrete);
        }

        return buildingType;
    }

}