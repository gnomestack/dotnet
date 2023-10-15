using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Xunit.Abstractions;

namespace Xunit.Sdk;

public sealed class GnomeStackTestFramework : XunitTestFramework
{
    public GnomeStackTestFramework(IMessageSink messageSink)
        : base(messageSink)
    {
    }

    protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName) =>
        new GnomeStackTestFrameworkExecutor(assemblyName, this.SourceInformationProvider, this.DiagnosticMessageSink);
}