#if NETLEGACY

using System.Globalization;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;

using GnomeStack.Extras.Strings;

// ReSharper disable once CheckNamespace
namespace GnomeStack.PowerShell;

/// <summary>
/// An implementation of semantic versioning (https://semver.org)
/// that can be converted to/from <see cref="System.Version"/>.
///
/// When converting to <see cref="Version"/>, a PSNoteProperty is
/// added to the instance to store the semantic version label so
/// that it can be recovered when creating a new SemanticVersion.
/// </summary>
// ReSharper disable ParameterHidesMember
public sealed class SemanticVersion : IComparable, IComparable<SemanticVersion>, IEquatable<SemanticVersion>
{
    private const string VersionSansRegEx = @"^(?<major>\d+)(\.(?<minor>\d+))?(\.(?<patch>\d+))?$";
    private const string LabelRegEx = @"^((?<preLabel>[0-9A-Za-z][0-9A-Za-z\-\.]*))?(\+(?<buildLabel>[0-9A-Za-z][0-9A-Za-z\-\.]*))?$";
    private const string LabelUnitRegEx = @"^[0-9A-Za-z][0-9A-Za-z\-\.]*$";
    private const string PreLabelPropertyName = "PSSemVerPreReleaseLabel";
    private const string BuildLabelPropertyName = "PSSemVerBuildLabel";
    private const string TypeNameForVersionWithLabel = "System.Version#IncludeLabel";

    private string? versionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticVersion"/> class.
    /// </summary>
    /// <param name="version">The version to parse.</param>
    /// <exception cref="FormatException">Thrown when version string cannot be parsed.</exception>
    /// <exception cref="OverflowException">Thrown when a stack overflow occurs.</exception>
    public SemanticVersion(string version)
    {
        var v = SemanticVersion.Parse(version);

        this.Major = v.Major;
        this.Minor = v.Minor;
        this.Patch = v.Patch < 0 ? 0 : v.Patch;
        this.PreReleaseLabel = v.PreReleaseLabel;
        this.BuildLabel = v.BuildLabel;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticVersion"/> class.
    /// </summary>
    /// <param name="major">The major version.</param>
    /// <param name="minor">The minor version.</param>
    /// <param name="patch">The patch version.</param>
    /// <param name="preReleaseLabel">The pre-release label for the version.</param>
    /// <param name="buildLabel">The build metadata for the version.</param>
    /// <exception cref="FormatException">
    /// If <paramref name="preReleaseLabel"/> don't match 'LabelUnitRegEx'.
    /// If <paramref name="buildLabel"/> don't match 'LabelUnitRegEx'.
    /// </exception>
    public SemanticVersion(int major, int minor, int patch, string? preReleaseLabel, string? buildLabel)
        : this(major, minor, patch)
    {
        if (!string.IsNullOrEmpty(preReleaseLabel))
        {
            if (!Regex.IsMatch(preReleaseLabel, LabelUnitRegEx)) throw new FormatException(nameof(preReleaseLabel));

            this.PreReleaseLabel = preReleaseLabel;
        }

        if (!string.IsNullOrEmpty(buildLabel))
        {
            if (!Regex.IsMatch(buildLabel, LabelUnitRegEx)) throw new FormatException(nameof(buildLabel));

            this.BuildLabel = buildLabel;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticVersion"/> class.
    /// </summary>
    /// <param name="major">The major version.</param>
    /// <param name="minor">The minor version.</param>
    /// <param name="patch">The patch version.</param>
    /// <param name="label">The label for the version.</param>
    /// <exception cref="PSArgumentException">Thrown when major, minor, or patch versions is less than 0.</exception>
    /// <exception cref="FormatException" >
    /// If <paramref name="label"/> don't match 'LabelRegEx'.
    /// </exception>
    public SemanticVersion(int major, int minor, int patch, string? label)
        : this(major, minor, patch)
    {
        // We presume the SymVer :
        // 1) major.minor.patch-label
        // 2) 'label' starts with letter or digit.
        if (!string.IsNullOrEmpty(label))
        {
            var match = Regex.Match(label, LabelRegEx);
            if (!match.Success) throw new FormatException(nameof(label));

            this.PreReleaseLabel = match.Groups["preLabel"].Value;
            this.BuildLabel = match.Groups["buildLabel"].Value;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticVersion"/> class.
    /// </summary>
    /// <param name="major">The major version.</param>
    /// <param name="minor">The minor version.</param>
    /// <param name="patch">The patch version.</param>
    /// <exception cref="PSArgumentException">
    /// If <paramref name="major"/>, <paramref name="minor"/>, or <paramref name="patch"/> is less than 0.
    /// </exception>
    public SemanticVersion(int major, int minor, int patch)
    {
        if (major < 0)
            throw new PSArgumentException("major version must be zero or greater.", nameof(major));

        if (minor < 0)
            throw new PSArgumentException("major version must be zero or greater.", nameof(minor));

        if (patch < 0)
            throw new PSArgumentException("patch version must be zero or greater.", nameof(patch));

        this.Major = major;
        this.Minor = minor;
        this.Patch = patch;

        // We presume:
        // PreReleaseLabel = null;
        // BuildLabel = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticVersion"/> class.
    /// </summary>
    /// <param name="major">The major version.</param>
    /// <param name="minor">The minor version.</param>
    /// <exception cref="PSArgumentException">
    /// If <paramref name="major"/> or <paramref name="minor"/> is less than 0.
    /// </exception>
    public SemanticVersion(int major, int minor)
        : this(major, minor, 0)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticVersion"/> class.
    /// </summary>
    /// <param name="major">The major version.</param>
    /// <exception cref="PSArgumentException">
    /// If <paramref name="major"/> is less than 0.
    /// </exception>
    public SemanticVersion(int major)
        : this(major, 0, 0)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticVersion"/> class.
    /// Construct a <see cref="SemanticVersion"/> from a <see cref="Version"/>,
    /// copying the NoteProperty storing the label if the expected property exists.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="version"/> is null.
    /// </exception>
    /// <exception cref="PSArgumentException">
    /// If <paramref name="version.Revision"/> is more than 0.
    /// </exception>
    public SemanticVersion(Version version)
    {
        if (version == null)
            throw new PSArgumentNullException(nameof(version));

        if (version.Revision > 0)
            throw new PSArgumentException("version.Revision must never be greater than zero.", nameof(version));

        this.Major = version.Major;
        this.Minor = version.Minor;
        this.Patch = version.Build == -1 ? 0 : version.Build;

        var psobj = new PSObject(version);
        var preLabelNote = psobj.Properties[PreLabelPropertyName];
        if (preLabelNote != null)
        {
            this.PreReleaseLabel = preLabelNote.Value as string;
        }

        var buildLabelNote = psobj.Properties[BuildLabelPropertyName];
        if (buildLabelNote != null)
        {
            this.BuildLabel = buildLabelNote.Value as string;
        }
    }

    internal enum ParseFailureKind
    {
        ArgumentException,
        ArgumentOutOfRangeException,
        FormatException,
    }

    /// <summary>
    /// Gets the major version number, never negative.
    /// </summary>
    public int Major { get; }

    /// <summary>
    /// Gets the minor version number, never negative.
    /// </summary>
    public int Minor { get; }

    /// <summary>
    /// Gets the patch version, -1 if not specified.
    /// </summary>
    public int Patch { get; }

    /// <summary>
    /// Gets the pre release label.
    /// PreReleaseLabel position in the SymVer string 'major.minor.patch-PreReleaseLabel+BuildLabel'.
    /// </summary>
    public string? PreReleaseLabel { get; }

    /// <summary>
    /// Gets the build label.
    /// BuildLabel position in the SymVer string 'major.minor.patch-PreReleaseLabel+BuildLabel'.
    /// </summary>
    public string? BuildLabel { get; }

    /// <summary>
    /// Convert a <see cref="SemanticVersion"/> to a <see cref="Version"/>.
    /// If there is a <see cref="PreReleaseLabel"/> or/and a <see cref="BuildLabel"/>,
    /// it is added as a NoteProperty to the result so that you can round trip
    /// back to a <see cref="SemanticVersion"/> without losing the label.
    /// </summary>
    /// <param name="semver">The semantic version to convert.</param>
    public static implicit operator Version(SemanticVersion semver)
    {
        PSObject psobj;

        var result = new Version(semver.Major, semver.Minor, semver.Patch);

        if (!string.IsNullOrEmpty(semver.PreReleaseLabel) || !string.IsNullOrEmpty(semver.BuildLabel))
        {
            psobj = new PSObject(result);

            if (!string.IsNullOrEmpty(semver.PreReleaseLabel))
            {
                psobj.Properties.Add(new PSNoteProperty(PreLabelPropertyName, semver.PreReleaseLabel));
            }

            if (!string.IsNullOrEmpty(semver.BuildLabel))
            {
                psobj.Properties.Add(new PSNoteProperty(BuildLabelPropertyName, semver.BuildLabel));
            }

            psobj.TypeNames.Insert(0, TypeNameForVersionWithLabel);
        }

        return result;
    }

    public static bool operator ==(SemanticVersion? v1, SemanticVersion? v2)
    {
        if (v1 is null)
        {
            return v2 is null;
        }

        return v1.Equals(v2);
    }

    public static bool operator !=(SemanticVersion? v1, SemanticVersion? v2)
    {
        return !(v1 == v2);
    }

    public static bool operator <(SemanticVersion? v1, SemanticVersion? v2)
    {
        return Compare(v1, v2) < 0;
    }

    public static bool operator <=(SemanticVersion v1, SemanticVersion v2)
    {
        return Compare(v1, v2) <= 0;
    }

    public static bool operator >(SemanticVersion v1, SemanticVersion v2)
    {
        return Compare(v1, v2) > 0;
    }

    public static bool operator >=(SemanticVersion v1, SemanticVersion v2)
    {
        return Compare(v1, v2) >= 0;
    }

    public static int Compare(SemanticVersion? versionA, SemanticVersion? versionB)
    {
        if (versionA is null)
            return versionB is null ? 0 : -1;

        return versionA.CompareTo(versionB);
    }

    /// <summary>
    /// Parse <paramref name="version"/> and return the result if it is a valid <see cref="SemanticVersion"/>, otherwise throws an exception.
    /// </summary>
    /// <param name="version">The string to parse.</param>
    /// <returns>A semantic version.</returns>
    /// <exception cref="PSArgumentException">Thrown when version is null.</exception>
    /// <exception cref="FormatException">Thrown when the version is an empty string.</exception>
    /// <exception cref="OverflowException">Thrown when there is an overflow.</exception>
    public static SemanticVersion Parse(string version)
    {
        if (version == null)
            throw new PSArgumentNullException(nameof(version));

        if (version == string.Empty)
            throw new FormatException(nameof(version));

#pragma warning disable SA1129
        var r = new VersionResult();
#pragma warning restore SA1129
        r.Init(true);
        TryParseVersion(version, ref r);

        return r.ParsedVersion;
    }

    /// <summary>
    /// Parse <paramref name="version"/> and return true if it is a valid <see cref="SemanticVersion"/>, otherwise return false.
    /// No exceptions are raised.
    /// </summary>
    /// <param name="version">The string to parse.</param>
    /// <param name="result">The return value when the string is a valid <see cref="SemanticVersion"/>.</param>
    /// <returns>True when the value is parsed; otherwise, false.</returns>
    public static bool TryParse(string? version, out SemanticVersion? result)
    {
        if (version != null)
        {
            var r = default(VersionResult);
            r.Init(false);

            if (TryParseVersion(version, ref r))
            {
                result = r.ParsedVersion;
                return true;
            }
        }

        result = null;
        return false;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        if (this.versionString is not null)
            return this.versionString;

        StringBuilder result = new StringBuilder();

        result.Append(this.Major)
            .Append('.')
            .Append(this.Minor)
            .Append('.')
            .Append(this.Patch);

        if (!string.IsNullOrEmpty(this.PreReleaseLabel))
        {
            result.Append('-').Append(this.PreReleaseLabel);
        }

        if (!string.IsNullOrEmpty(this.BuildLabel))
        {
            result.Append('+').Append(this.BuildLabel);
        }

        this.versionString = result.ToString();

        return this.versionString;
    }

    public int CompareTo(object? obj)
    {
        if (obj == null)
        {
            return 1;
        }

        if (!(obj is SemanticVersion v))
        {
            throw new PSArgumentException("version must be type of SemanticVersion", nameof(obj));
        }

        return this.CompareTo(v);
    }

    public int CompareTo(SemanticVersion? value)
    {
        if (value is null)
            return 1;

        if (this.Major != value.Major)
            return this.Major > value.Major ? 1 : -1;

        if (this.Minor != value.Minor)
            return this.Minor > value.Minor ? 1 : -1;

        if (this.Patch != value.Patch)
            return this.Patch > value.Patch ? 1 : -1;

        // SymVer 2.0 standard requires to ignore 'BuildLabel' (Build metadata).
        return ComparePreLabel(this.PreReleaseLabel, value.PreReleaseLabel);
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        if (obj is SemanticVersion semver)
            return this.Equals(semver);

        return false;
    }

    /// <inheritdoc />
    public bool Equals(SemanticVersion? other)
    {
        if (other is null)
            return false;

        // SymVer 2.0 standard requires to ignore 'BuildLabel' (Build metadata).
        return (this.Major == other.Major) &&
               (this.Minor == other.Minor) &&
               (this.Patch == other.Patch) &&
               string.Equals(this.PreReleaseLabel, other.PreReleaseLabel, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return this.ToString().GetHashCode();
    }

    private static bool TryParseVersion(string version, ref VersionResult result)
    {
        if (version.EndsWith("-") || version.EndsWith("+") || version.EndsWith("."))
        {
            result.SetFailure(ParseFailureKind.FormatException);
            return false;
        }

        string? versionSansLabel;
        var minor = 0;
        var patch = 0;
        string? preLabel = null;
        string? buildLabel = null;

        // We parse the SymVer 'version' string 'major.minor.patch-PreReleaseLabel+BuildLabel'.
        var dashIndex = version.IndexOf('-');
        var plusIndex = version.IndexOf('+');

        if (dashIndex > plusIndex)
        {
            // 'PreReleaseLabel' can contains dashes.
            if (plusIndex == -1)
            {
                // No buildLabel: buildLabel == null
                // Format is 'major.minor.patch-PreReleaseLabel'
                preLabel = version.Substring(dashIndex + 1);
                versionSansLabel = version.Substring(0, dashIndex);
            }
            else
            {
                // No PreReleaseLabel: preLabel == null
                // Format is 'major.minor.patch+BuildLabel'
                buildLabel = version.Substring(plusIndex + 1);
                versionSansLabel = version.Substring(0, plusIndex);
                dashIndex = -1;
            }
        }
        else
        {
            if (plusIndex == -1)
            {
                // Here dashIndex == plusIndex == -1
                // No preLabel - preLabel == null;
                // No buildLabel - buildLabel == null;
                // Format is 'major.minor.patch'
                versionSansLabel = version;
            }
            else if (dashIndex == -1)
            {
                // No PreReleaseLabel: preLabel == null
                // Format is 'major.minor.patch+BuildLabel'
                buildLabel = version.Substring(plusIndex + 1);
                versionSansLabel = version.Substring(0, plusIndex);
            }
            else
            {
                // Format is 'major.minor.patch-PreReleaseLabel+BuildLabel'
                preLabel = version.Substring(dashIndex + 1, plusIndex - dashIndex - 1);
                buildLabel = version.Substring(plusIndex + 1);
                versionSansLabel = version.Substring(0, dashIndex);
            }
        }

        if ((dashIndex != -1 && string.IsNullOrEmpty(preLabel)) ||
            (plusIndex != -1 && string.IsNullOrEmpty(buildLabel)) ||
            string.IsNullOrEmpty(versionSansLabel))
        {
            // We have dash and no preReleaseLabel  or
            // we have plus and no buildLabel or
            // we have no main version part (versionSansLabel==null)
            result.SetFailure(ParseFailureKind.FormatException);
            return false;
        }

        var match = Regex.Match(versionSansLabel, VersionSansRegEx);
        if (!match.Success)
        {
            result.SetFailure(ParseFailureKind.FormatException);
            return false;
        }

        if (!int.TryParse(match.Groups["major"].Value, out int major))
        {
            result.SetFailure(ParseFailureKind.FormatException);
            return false;
        }

        if (match.Groups["minor"].Success && !int.TryParse(match.Groups["minor"].Value, out minor))
        {
            result.SetFailure(ParseFailureKind.FormatException);
            return false;
        }

        if (match.Groups["patch"].Success && !int.TryParse(match.Groups["patch"].Value, out patch))
        {
            result.SetFailure(ParseFailureKind.FormatException);
            return false;
        }

        if ((preLabel != null && !Regex.IsMatch(preLabel, LabelUnitRegEx)) ||
           (buildLabel != null && !Regex.IsMatch(buildLabel, LabelUnitRegEx)))
        {
            result.SetFailure(ParseFailureKind.FormatException);
            return false;
        }

        result.ParsedVersion = new SemanticVersion(major, minor, patch, preLabel, buildLabel);
        return true;
    }

    private static int ComparePreLabel(string? preLabel1, string? preLabel2)
    {
        // Semver 2.0 standard p.9
        // Pre-release versions have a lower precedence than the associated normal version.
        // Comparing each dot separated identifier from left to right
        // until a difference is found as follows:
        //     identifiers consisting of only digits are compared numerically
        //     and identifiers with letters or hyphens are compared lexically in ASCII sort order.
        // Numeric identifiers always have lower precedence than non-numeric identifiers.
        // A larger set of pre-release fields has a higher precedence than a smaller set,
        // if all of the preceding identifiers are equal.
        if (preLabel1.IsNullOrEmpty())
            return string.IsNullOrEmpty(preLabel2) ? 0 : 1;

        if (preLabel2.IsNullOrEmpty())
            return -1;

        var units1 = preLabel1.Split('.');
        var units2 = preLabel2.Split('.');

        var minLength = units1.Length < units2.Length ? units1.Length : units2.Length;

        for (int i = 0; i < minLength; i++)
        {
            var ac = units1[i];
            var bc = units2[i];
            var isNumber1 = int.TryParse(ac, out var number1);
            var isNumber2 = int.TryParse(bc, out var number2);

            if (isNumber1 && isNumber2)
            {
                if (number1 != number2)
                    return number1 < number2 ? -1 : 1;
            }
            else
            {
                if (isNumber1)
                    return -1;

                if (isNumber2)
                    return -1;

                int result = string.CompareOrdinal(ac, bc);
                if (result != 0)
                    return result;
            }
        }

        return units1.Length.CompareTo(units2.Length);
    }

    internal struct VersionResult
    {
        internal SemanticVersion ParsedVersion;
        private ParseFailureKind failure;
        private string exceptionArgument;
        private bool canThrow;

        internal void Init(bool canThrow)
        {
            this.canThrow = canThrow;
        }

        internal void SetFailure(ParseFailureKind failure)
        {
            this.SetFailure(failure, string.Empty);
        }

        internal void SetFailure(ParseFailureKind failure, string argument)
        {
            this.failure = failure;
            this.exceptionArgument = argument;
            if (this.canThrow)
            {
                throw this.GetVersionParseException();
            }
        }

        internal Exception GetVersionParseException()
        {
            switch (this.failure)
            {
                case ParseFailureKind.ArgumentException:
                    return new PSArgumentException("version");
                case ParseFailureKind.FormatException:
                    // Regenerate the FormatException as would be thrown by Int32.Parse()
                    try
                    {
                        _ = int.Parse(this.exceptionArgument, CultureInfo.InvariantCulture);
                    }
                    catch (FormatException e)
                    {
                        return e;
                    }
                    catch (OverflowException e)
                    {
                        return e;
                    }

                    break;
            }

            return new PSArgumentException("version");
        }
    }
}

#endif