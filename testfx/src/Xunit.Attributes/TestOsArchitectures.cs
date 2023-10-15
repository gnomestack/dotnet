using System;
using System.Collections.Generic;
using System.Text;

namespace Xunit;

[Flags]
public enum TestOsArchitectures
{
    None = 0,

    X86 = 1,

    X64 = 2,

    Arm = 4,

    Arm64 = 8,
}