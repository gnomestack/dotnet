# GnomeStack.Xunit.Asserts

## Description

Provides a new `FlexAssert` class with all the methods from Xunit and heavily
uses Xunit's code. It must be instantiated or use `FlexAssert.Default` as the
class inherits the `IAssert` to provide dependency injection for
 `GnomeStack.Xunit.Core`.

The asserts include methods to test `Span<T>` and `Memory<T>`
data and `FlexAssert.Skip` to dynamically skip a test in addition to the
default asserts provided by Xunit.

Examples can be found in the project's test source code.

## Features

- Enables extension methods to be added to the `IAssert` interface.
- Supports all xunit2 asserts.
- Adds asserts from xunit3 around span and memory.
- Adds `Skip()` for skipping tests.
- Enables GnomeStack.Xunit.Core to inject `IAssert` into test methods.

MIT License (project).

Apache License (code from Xunit).
