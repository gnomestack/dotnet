/*
Copyright (c) .NET Foundation and Contributors
All Rights Reserved

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

namespace Xunit.Sdk;

/// <summary>
/// The dynamic skip token from <see href="https://github.com/xunit/assert.xunit/blob/main/Sdk/DynamicSkipToken.cs" />.
/// </summary>
internal static class DynamicSkipToken
{
    /// <summary>
    /// The contract for exceptions which indicate that something should be skipped rather than
    /// failed is that exception message should start with this, and that any text following this
    /// will be treated as the skip reason (for example,
    /// "$XunitDynamicSkip$This code can only run on Linux") will result in a skipped test with
    /// the reason of "This code can only run on Linux".
    /// </summary>
    public const string Value = "$XunitDynamicSkip$";
}