# GnomeStack.Xunit.Attributes

Provides useful attributes that extends Xunit for filtering tests or skipping
tests based on traits.

The `FlexAssert.Skip` from `GnomeStack.Xunit.Asserts` is recognized by this
assesmbly and it will skip tests that call `FlexAssert.Skip()`.

## Test Attributes

- `TestAttribute` - is an extension of the `[FactAttribute]` with the properties
  that are created at traits to allow filtering of tests:
  - **Category** - the category.
  - **Tags** - Multiple tags that can be applied to a test.
  - **TicketId** - The ticket id for a test
  - **Unsafe** - Is the test unsafe? (Should it only run in a container or VM)
  - **LongRunning** - Is the test long running?
- `UnitTestAttribute` - inherits `TestAttribute` and applies "unit" to the
  category and adds a unit tag.
- `IntegrationTestAttribute` - inherits `TestAttribute` and applies
  "integration" label to the category and adds a "integration" tag.
- `FunctionalTestAttribute` - inherits `TestAttribute` and applies
  "functional" label to the category and adds a "functional" tag.
- `UITestAttribute` - inherits `TestAttribute` and applies
  "ui" label to the category and adds a "ui" tag.

## Skippable Attributes

- `RequiresOsArchitecturesAttribute` will only run tests that meet a given
  os arch such as `X86`, `X64`, `Arm`, and `Arm64` or the test will be skipped.
- `RequireOsPlatformsAttribute` will only run tests that meet a given os
  such as `Windows`, `Linux`, `OSX`, `FreeBSD`, `NetBSD`, `Illumos`,
  `IOS`, `TVOS`, `Android`, `Browser`, and `MacCatalyst`.
- `RequireTargetFrameworksAttribute` will only run tests that meet a given
  Runtime ID or simple runtime Id Compare such as `net472` or `> net5.0`.

MIT License (project).

Apache License (code from Xunit).
