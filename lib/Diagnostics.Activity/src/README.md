# GnomeStack.Diagnostics.Activity

Provides extension methods for `System.Diagnostics.Activity` to make it easier
to leverage `Activity` in lower level libraries as OpenTelemetry.Api includes
logging and other things one may not want in a base class library.

The semantic convention tags for traces and resources are included.  The are
pulled from the Apache 2.0 source code in OpenTelemetry.SemanticConventions package.

## Usage

```csharp

using GnomeStack.Diagnostics.Activity;

// generally you would create from ActivitySource, this is for example purposes.
var activity = new Activity("MyActivity");
activity.WithTag("foo", "bar")
    .WithTag("baz", 10);


try {
    activity
        .WithName("UpdateName")
        .SetTagSet(new Dictionary<string, object> {
            { "foo2", "bar2" },
            { "baz2", 10 }
        })
        .Start();

    // do stuff
    activity.Ok();
    // or
    activity.WithStatus(ActivityStatus.Ok);
} catch (Exception ex) {
    activity.RecordException(ex);
    activity.Error();
    // or just call exception to set status and record exception in one call.
    activity.Error(ex);
    
    // there is also a WithStatu
    throw;
} finally {
    activity.Stop();
}

```

To simplify creating an activity source there is a factory.

```csharp
using GnomeStack.Diagnostics.Activity;

// this will look for an ActivitySourceAttribute or use the information from the calling assembly.
public static ActivitySource LibSource { get; } = ActivitySourceFactory.CreateFromCallingAssembly();

// this will look for an ActivitySourceAttribute or use the information from the assembly.
public static ActivitySource LibSource { get; } = ActivitySourceFactory.CreateFromAssembly(typeof(MyClass).Assembly);

// this will look for an ActivitySourceAttribute or use the information from assembly retrieved from type MyClass.
public static ActivitySource LibSource { get; } = ActivitySourceFactory.CreateFromAssembly<MyClass>();
```

MIT License

Source from OpenTelemetry.SemanticConventions is Apache 2.0