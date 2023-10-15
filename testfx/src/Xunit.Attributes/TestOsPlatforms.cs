// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace Xunit;

// ReSharper disable InconsistentNaming
// https://github.com/dotnet/arcade/blob/main/src/Microsoft.DotNet.XUnitExtensions/src/TestPlatforms.cs
[Flags]
public enum TestOsPlatforms
{
    None = 0,

    Windows = 1,

    Linux = 2,

    OSX = 4,

    FreeBSD = 8,

    NetBSD = 16,

    Illumos = 32,

    Solaris = 64,

    IOS = 128,

    TVOS = 256,

    Android = 512,

    Browser = 1024,

    MacCatalyst = 2048,

    AnyUnix = FreeBSD | Linux | NetBSD | OSX | Illumos | Solaris | IOS | TVOS | MacCatalyst | Android | Browser,
}