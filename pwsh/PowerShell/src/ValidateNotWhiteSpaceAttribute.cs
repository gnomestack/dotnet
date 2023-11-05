using System.Collections;
using System.Management.Automation;

using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class ValidateNotWhiteSpaceAttribute : NullValidationAttributeBase
{
    protected override void Validate(object arguments, EngineIntrinsics engineIntrinsics)
    {
        if (arguments is null)
        {
            throw new ValidationMetadataException(
                "ArgumentIsEmpty",
                null);
        }

        if (arguments is string value && !value.IsNullOrWhiteSpace())
        {
            throw new ValidationMetadataException(
                "ArgumentIsEmpty",
                null);
        }
        else if (arguments is Array array)
        {
            foreach (var item in array)
            {
                if (item is string itemValue && itemValue.IsNullOrWhiteSpace())
                {
                    throw new ValidationMetadataException(
                        "ArgumentIsEmpty",
                        null);
                }
            }
        }
        else if (arguments is IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                if (item is string itemValue && itemValue.IsNullOrWhiteSpace())
                {
                    throw new ValidationMetadataException(
                        "ArgumentIsEmpty",
                        null);
                }
            }
        }
    }
}