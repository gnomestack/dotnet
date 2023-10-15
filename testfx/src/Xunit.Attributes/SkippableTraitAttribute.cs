using System;

using Xunit.Abstractions;

namespace Xunit;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
public abstract class SkippableTraitAttribute : Attribute
{
    public abstract string? GetSkipReason(IMessageSink sink, ITestMethod testMethod, IAttributeInfo attributeInfo);
}