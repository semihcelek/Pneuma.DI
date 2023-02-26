﻿using System;
using Pneuma.DI.Core.Attributes;

namespace Pneuma.DI.Tests.Examples;

public class Fred
{
    [Inject]
    private IBaz _baz;

    public void DoBaz()
    {
        Console.WriteLine(_baz.Fizz);
    }
}