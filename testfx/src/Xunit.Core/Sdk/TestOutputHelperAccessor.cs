using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Xunit.Abstractions;

namespace Xunit.Sdk;

public class TestOutputHelperAccessor : ITestOutputHelperAccessor
{
    private readonly AsyncLocal<ITestOutputHelper?> outputHelper = new();

    public ITestOutputHelper? TestOutputHelper
    {
        get => this.outputHelper.Value;
        set => this.outputHelper.Value = value;
    }
}