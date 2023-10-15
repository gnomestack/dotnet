using System;

namespace Xunit;

[Flags]
public enum RuntimeConfigurations
{
    None = 0,
    Checked = 1,
    Debug = 1 << 1,
    Release = 1 << 2,
}