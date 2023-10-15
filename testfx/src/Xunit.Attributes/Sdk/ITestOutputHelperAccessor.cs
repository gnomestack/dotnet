using System;
using System.Collections.Generic;
using System.Text;

using Xunit.Abstractions;

namespace Xunit.Sdk;

public interface ITestOutputHelperAccessor
{
    public ITestOutputHelper? TestOutputHelper { get; set; }
}