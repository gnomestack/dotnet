using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Xunit.Sdk;

internal static class Extensions
{
    internal static object? GetDefaultValue(this TypeInfo typeInfo)
    {
        if (typeInfo.IsValueType)
            return Activator.CreateInstance(typeInfo.AsType());

        return null;
    }
}